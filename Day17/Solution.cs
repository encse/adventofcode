using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode2017.Day17 {

    class Solution : Solver {

        public string GetName() => "Spinlock";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var step = int.Parse(input);
            var nums = new List<int>() { 0 };
            var pos = 0;
            for (int i = 1; i < 2018; i++) {
                pos = (pos + step) % nums.Count + 1;
                nums.Insert(pos, i);
            }
            return nums[(pos + 1) % nums.Count];
        }

        int PartTwo(string input) {
            var step = int.Parse(input);
            var pos = 0;
            var numsCount = 1;
            var res = 0;
            for (int i = 1; i < 50000001; i++) {
                pos = (pos + step) % numsCount + 1;
                if (pos == 1) {
                    res = i;
                }
                numsCount++;
            }
            return res;
        }
    }
}
