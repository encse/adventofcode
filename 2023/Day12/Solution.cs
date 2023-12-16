using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day12;
using Cache = Dictionary<(string, ImmutableStack<int>), long>;

[ProblemName("Hot Springs")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, 1);
    public object PartTwo(string input) => Solve(input, 5);

    // After unfolding the input we process it line by line computing the possible 
    // combinations for each. We use memoized recursion to speed up PartTwo.
    // 
    // The computation is recursive by nature, and goes over the pattern and numbers
    // in tandem branching on '?' symbols and consuming as much of dead springs
    // as dictated by the next number when a '#' is found. The symbol that follows 
    // a dead range needs special treatment: it cannot be a '#', and if it was a '?'
    // we should consider it as a '.' according to the problem statement.
    // 
    // I like to use immutable datastructures when dealing with problems that
    // involves backtracking, it's not immediately obvious from the solution below
    // but using a mutable stack or list would cause a lot of headache.

    long Solve(string input, int repeat) => (
        from line in input.Split("\n")
        let parts = line.Split(" ")
        let pattern = Unfold(parts[0], '?', repeat)
        let numString = Unfold(parts[1], ',', repeat)
        let nums = numString.Split(',').Select(int.Parse)
        select
            Compute(pattern, ImmutableStack.CreateRange(nums.Reverse()), new Cache())
    ).Sum();

    string Unfold(string st, char join, int unfold) =>
        string.Join(join, Enumerable.Repeat(st, unfold));

    long Compute(string pattern, ImmutableStack<int> nums, Cache cache) {
        if (!cache.ContainsKey((pattern, nums))) {
            cache[(pattern, nums)] = Dispatch(pattern, nums, cache);
        }
        return cache[(pattern, nums)];
    }

    long Dispatch(string pattern, ImmutableStack<int> nums, Cache cache) {
        return pattern.FirstOrDefault() switch {
            '.' => ProcessDot(pattern, nums, cache),
            '?' => ProcessQuestion(pattern, nums, cache),
            '#' => ProcessHash(pattern, nums, cache),
            _ => ProcessEnd(pattern, nums, cache),
        };
    }

    long ProcessEnd(string _, ImmutableStack<int> nums, Cache __) {
        // the good case is when there are no numbers left at the end of the pattern
        return nums.Any() ? 0 : 1;
    }

    long ProcessDot(string pattern, ImmutableStack<int> nums, Cache cache) {
        // consume one spring and recurse
        return Compute(pattern[1..], nums, cache);
    }

    long ProcessQuestion(string pattern, ImmutableStack<int> nums, Cache cache) {
        // recurse both ways
        return Compute("." + pattern[1..], nums, cache) + Compute("#" + pattern[1..], nums, cache);
    }

    long ProcessHash(string pattern, ImmutableStack<int> nums, Cache cache) {
        // take the first number and consume that many dead springs, recurse

        if (!nums.Any()) {
            return 0; // no more numbers left, this is no good
        }

        var n = nums.Peek();
        nums = nums.Pop();

        var potentiallyDead = pattern.TakeWhile(s => s == '#' || s == '?').Count();

        if (potentiallyDead < n) {
            return 0; // not enough dead springs 
        } else if (pattern.Length == n) {
            return Compute("", nums, cache);
        } else if (pattern[n] == '#') {
            return 0; // dead spring follows the range -> not good
        } else {
            return Compute(pattern[(n + 1)..], nums, cache);
        }
    }
}
