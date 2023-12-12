using System;
using System.Collections.Immutable;
using System.Linq;

using Cache = System.Collections.Generic.Dictionary<string, long>;
namespace AdventOfCode.Y2023.Day12;

[ProblemName("Hot Springs")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, 1);
    public object PartTwo(string input) => Solve(input, 5);

    long Solve(string input, int repeat) => (
        from line in input.Split("\n")
        let parts = line.Split(" ")
        let pattern = Unfold(parts[0], '?', repeat)
        let numString = Unfold(parts[1], ',', repeat)
        let nums = numString.Split(',').Select(int.Parse)
        select
            Solve(pattern, ImmutableStack.CreateRange(nums.Reverse()), new Cache())
    ).Sum();

    string Unfold(string st, char join, int unfold) =>
        string.Join(join, Enumerable.Repeat(st, unfold));

    // Classic memoized recursion.
    // I like to use immutable datastructures here because of easy backtracking.
    long Solve(string pattern, ImmutableStack<int> nums, Cache cache) {

        // The 'good enough for x-mas' hash key
        var key = pattern + "," + string.Join(",", nums.Select(n => n.ToString()));

        if (!cache.ContainsKey(key)) {
            cache[key] = Compute(pattern, nums, cache);
        }
        return cache[key];
    }

    long Compute(string pattern, ImmutableStack<int> nums, Cache cache) {
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
        return Solve(pattern[1..], nums, cache);
    }

    long ProcessQuestion(string pattern, ImmutableStack<int> nums, Cache cache) {
        // recurse both ways
        return Solve("." + pattern[1..], nums, cache) + Solve("#" + pattern[1..], nums, cache);
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
            return Solve("", nums, cache);
        } else if (pattern[n] == '#') {
            return 0; // dead spring follows the range -> not good
        } else {
            return Solve(pattern[(n + 1)..], nums, cache);
        }
    }
}
