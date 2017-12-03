using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2017.Day01 {

    class Solution : Solver {

        public void Solve(string input) {
            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
        }

        int InverseCaptcha(string input, int skip) {
            return (
                from i in Enumerable.Range(0, input.Length)
                where input[i] == input[(i + skip) % input.Length]
                select int.Parse(input[i].ToString())
            ).Sum();
        }

        int PartOne(string input) {
            return InverseCaptcha(input, 1);
        }

        int PartTwo(string input) {
            return InverseCaptcha(input, input.Length / 2);
        }
    }
}
