using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day15 {

    [ProblemName("Rambunctious Recitation")]
    class Solution : Solver {

        public object PartOne(string input) => Solve(input, 2020);

        public object PartTwo(string input) => Solve(input, 30000000);

        public int Solve(string input, int count) {
            var nums = input.Split(",").Select(int.Parse).ToArray();
            var seen = new Dictionary<int, (int, int)>();
            var prev = -1;
            for (var i = 0; i< count ; i++) {
                var cur = -1;
                if (i < nums.Length) {
                    seen[nums[i]] = (-1, i);
                    cur = nums[i];
                } else {
                    var (pp, p) = seen.ContainsKey(prev) ?  seen[prev] : (-1,-1);
                    cur = pp == -1 ? 0 : p - pp;
                }

                var (_, pCur) = seen.ContainsKey(cur) ? seen[cur] : (-1,-1);
                seen[cur] = (pCur, i);
                prev = cur;
            }
            return prev;
        } 
    }
}