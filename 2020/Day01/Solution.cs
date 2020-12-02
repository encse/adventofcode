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
            return (
                from x in numbers 
                from y in numbers 
                where x + y == 2020 
                select x * y
            ).First();
        }

        long PartTwo(string input) {
            var numbers = Numbers(input);
            return (
                from x in numbers 
                from y in numbers 
                from z in numbers 
                where x + y + z == 2020 
                select x * y * z
            ).First();
        }

        int[] Numbers(string input) {
            return input.Split('\n').Select(int.Parse).ToArray();
        }
    }
}