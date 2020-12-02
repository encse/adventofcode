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
                let y = 2020 - x
                where numbers.Contains(y)
                select x * y
            ).First();
        }

        long PartTwo(string input) {
            var numbers = Numbers(input);
            return (
                from x in numbers 
                from y in numbers 
                let z = 2020 - x - y
                where numbers.Contains(z)
                select x * y * z
            ).First();
        }

        HashSet<int> Numbers(string input) {
            return input.Split('\n').Select(int.Parse).ToHashSet<int>();
        }
    }
}