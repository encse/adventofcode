using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day01 {

    class Solution : Solver {

        public string GetName() => "Report Repair";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) {
            var numbers = Numbers(input);
            foreach (var x in numbers)
            foreach (var y in numbers) {
                if (x + y == 2020) {
                    return x * y;
                }
            }
            throw new Exception();
        }

        long PartTwo(string input) {
            var numbers = Numbers(input);
            foreach (var x in numbers)
            foreach (var y in numbers) 
            foreach (var z in numbers) {
                if (x + y + z == 2020) {
                    return x * y * z;
                }
            }
            throw new Exception();
        }

        IEnumerable<int> Numbers(string input) {
            return input.Split('\n').Select(int.Parse);
        }
    }
}