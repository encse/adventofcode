using System.Linq;

namespace AdventOfCode.Y2017.Day02 {

    [ProblemName("Corruption Checksum")]
    class Solution : Solver {

        public object PartOne(string input) {
            return (
                from line in input.Split('\n')
                let numbers = line.Split('\t').Select(int.Parse)
                select numbers.Max() - numbers.Min()
            ).Sum();
        }

        public object PartTwo(string input) {
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
