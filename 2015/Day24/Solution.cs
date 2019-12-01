using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2015.Day24 {

    class Solution : Solver {

        public string GetName() => "It Hangs in the Balance";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => Solve(Parse(input), 3);

        long PartTwo(string input) => Solve(Parse(input), 4);

        int[] Parse(string input) =>
            input.Split("\n").Select(int.Parse).ToArray();

        long Solve(int[] nums, int groups) {
            var minLength = int.MaxValue;
            var min = long.MaxValue;
            foreach (var part in Pick(nums, 0, nums.Sum() / groups)) {
                if (part.c < minLength) {
                    minLength = part.c;
                    min = part.mul;
                } else if (part.c == minLength) {
                    min = Math.Min(min, part.mul);
                }
            }
            return min;
        }
        
        IEnumerable<(int c, long mul)> Pick(int[] nums, int i, int sum) {
            if (sum == 0) {
                yield return (0, 1);
                yield break;
            }

            if (sum < 0 || i >= nums.Length){
                yield break;
            }
            
            if (nums[i] <= sum) {
                foreach (var x in Pick(nums, i + 1, sum - nums[i])) {
                    yield return (x.c + 1, x.mul * nums[i]);
                }
            }

            foreach (var x in Pick(nums, i + 1, sum)) {
                yield return x;
            }
        }
    }
}