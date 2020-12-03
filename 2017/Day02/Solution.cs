using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Y2017.Day02 {

    [ProblemName("Corruption Checksum")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            return (
                from line in input.Split('\n')
                let numbers = line.Split('\t').Select(int.Parse)
                select numbers.Max() - numbers.Min()
            ).Sum();
        }

        int PartTwo(string input) {
            return (
                from line in input.Split('\n')
                let numbers = line.Split('\t').Select(int.Parse)
                from a in numbers
                from b in numbers
                where a > b && a % b == 0
                select a / b
            ).Sum();
        }
    }
}
