using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2019.Day04 {

    [ProblemName("Secure Container")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Solve(input, true);
        int PartTwo(string input) => Solve(input, false);
        private int Solve(string input, bool trippletsAllowed) {

            var args = input.Split("-").Select(int.Parse).ToArray();
            return (
                from i in Enumerable.Range(args[0], args[1] - args[0] + 1)
                where OK(i.ToString(), trippletsAllowed)
                select i
            ).Count();
        }

        private bool OK(string password, bool trippletsAllowed) {

            if (string.Join("", password.OrderBy(ch => ch)) != password) {
                return false;
            }

            return (
                from sequence in Split(password)
                where sequence.Length >= 2 && (trippletsAllowed || sequence.Length == 2)
                select sequence
            ).Any();
        }

        private IEnumerable<string> Split(string st) {
            var ich = 0;
            while (ich < st.Length) {
                var sequence = Regex.Match(st.Substring(ich), @$"[{st[ich]}]+").Value;
                yield return sequence;
                ich += sequence.Length;
            }
        }
    }
}