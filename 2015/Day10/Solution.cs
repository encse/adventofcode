using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Y2015.Day10 {

    [ProblemName("Elves Look, Elves Say")]
    class Solution : Solver {

        public object PartOne(string input) => LookAndSay(input).Skip(39).First().Length;
        public object PartTwo(string input) => LookAndSay(input).Skip(49).First().Length;

        IEnumerable<string> LookAndSay(string input) {
            while (true) {
                var sb = new StringBuilder();
                var ich = 0;
                while (ich < input.Length) {
                    if (ich < input.Length - 2 && input[ich] == input[ich + 1] && input[ich] == input[ich + 2]) {
                        sb.Append("3");
                        sb.Append(input[ich]);
                        ich += 3;
                    } else if (ich < input.Length - 1 && input[ich] == input[ich + 1]) {
                        sb.Append("2");
                        sb.Append(input[ich]);
                        ich += 2;
                    } else {
                        sb.Append("1");
                        sb.Append(input[ich]);
                        ich += 1;
                    }
                }
                input = sb.ToString();
                yield return input;
            }
        }
    }
}