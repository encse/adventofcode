using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Day05 {

    [ProblemName("A Maze of Twisty Trampolines, All Alike")]
    class Solution : Solver {
        
        public object PartOne(string input) => GetStepCount(input, x => x + 1);

        public object PartTwo(string input) => GetStepCount(input, x => x < 3 ? x + 1 : x - 1);

        int GetStepCount(string input, Func<int, int> update) {
            var numbers = input.Split('\n').Select(int.Parse).ToArray();
            var i = 0;
            var stepCount = 0;
            while (i < numbers.Length && i >= 0) {
                var jmp = numbers[i];
                numbers[i] = update(numbers[i]);
                i += jmp;
                stepCount++;
            }
            return stepCount;
        }
    }
}
