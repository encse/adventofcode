using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day15 {

    [ProblemName("Rambunctious Recitation")]
    class Solution : Solver {

        public object PartOne(string input) => NumberAt(input, 2020);
        public object PartTwo(string input) => NumberAt(input, 30000000);

        public int NumberAt(string input, int count) {
            var nums = input.Split(",").Select(int.Parse).ToArray();
            var spoken = new Dictionary<int, int>();
            var prev = -1;
            for (var i = 0; i < count; i++) {
                var cur = 
                    i < nums.Length          ? nums[i] :
                    spoken.ContainsKey(prev) ? i - spoken[prev] :
                    /*otherwise*/            0;

                spoken[prev] = i; 
                prev = cur;
            }
            return prev;
        } 
    }
}