using System;
using System.Linq;

namespace AdventOfCode2017.Day02 {

    class Solution : Solver {

        public void Solve(string input) {
            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
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
