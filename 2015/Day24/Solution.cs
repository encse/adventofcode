using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2015.Day24;

[ProblemName("It Hangs in the Balance")]
class Solution : Solver {

    public object PartOne(string input) => Solve(Parse(input), 3);

    public object PartTwo(string input) => Solve(Parse(input), 4);

    int[] Parse(string input) =>
        input.Split("\n").Select(int.Parse).ToArray();

    long Solve(int[] nums, int groups) {
        var mul = (ImmutableList<int> l) => l.Aggregate(1L, (m, x) => m*x);

        for(var i =0;i<nums.Length;i++) {
            var parts = Pick(nums, i, 0, nums.Sum() / groups);
            if (parts.Any()){
                return parts.Select(mul).Min();
            }
        }
        throw new Exception();
    }
    
    IEnumerable<ImmutableList<int>> Pick(int[] nums, int count, int i, int sum) {
        if (sum == 0) {
            yield return ImmutableList.Create<int>();
            yield break;
        }

        if (count < 0 || sum < 0 || i >= nums.Length) {
            yield break;
        }
        
        if (nums[i] <= sum) {
            foreach (var x in Pick(nums, count-1, i + 1, sum - nums[i])) {
                yield return x.Add(nums[i]);
            }
        }

        foreach (var x in Pick(nums, count, i + 1, sum)) {
            yield return x;
        }
    }
}
