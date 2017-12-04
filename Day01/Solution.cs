using System;
using System.Linq;

namespace AdventOfCode2017.Day01 {

    class Solution : Solver {

        public void Solve(string input) {
            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
        }

        int PartOne(string input) => InverseCaptcha(input, 1);

        int PartTwo(string input) => InverseCaptcha(input, input.Length / 2);

        int InverseCaptcha(string input, int skip) {
            return (
                from i in Enumerable.Range(0, input.Length)
                where input[i] == input[(i + skip) % input.Length]
                select int.Parse(input[i].ToString())
            ).Sum();
        }
    }
}
