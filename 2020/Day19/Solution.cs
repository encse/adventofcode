using System.Collections.Generic;
using System.Linq;
using Parser = System.Func<string, System.Collections.Generic.IEnumerable<string>>;

namespace AdventOfCode.Y2020.Day19 {

    [ProblemName("Monster Messages")]
    class Solution : Solver {

        public object PartOne(string input) => Solve(input, true);
        public object PartTwo(string input) => Solve(input, false);

        private int Solve(string input, bool part1) {
            var rules = input.Split("\n\n")[0]
                .Split("\n")
                .ToDictionary(line => int.Parse(line.Split(":")[0]), line => line);

            if (!part1) {
                rules[8] = "8: 42 | 42 8";
                rules[11] = "11: 42 31 | 42 11 31";
            }

            Dictionary<int, Parser> parsers = new Dictionary<int, Parser>();

            Parser parser(int i) {
                if (!parsers.ContainsKey(i)) {
                    var rule = rules[i].Trim();
                    var right = rule.Split(": ")[1];
                    var ors = right.Split("|");
                    var orParsers = new List<Parser>();
                    foreach (var or in ors) {
                        var seqParsers = new List<Parser>();
                        foreach (var item in or.Trim().Split(" ")) {
                            if (item.StartsWith("\"")) {
                                seqParsers.Add(literal(item.Substring(1, item.Length - 2)));
                            } else {
                                seqParsers.Add((st) => parser(int.Parse(item))(st));
                            }
                        }

                        orParsers.Add(seqParsers.Count == 1 ? seqParsers.Single() : seq(seqParsers.ToArray()));
                    }

                    parsers[i] = orParsers.Count == 1 ? orParsers.Single() : or(orParsers.ToArray());

                }
                return parsers[i];
            }

            bool valid(string st) {
                return parser(0)(st).Any(st => st.Length == 0);
            }

            return input.Split("\n\n")[1].Split("\n").Count(valid);
        }

        private Parser literal(string st) {
            IEnumerable<string> p(string input) {
                if (input.StartsWith(st)) {
                    yield return input.Substring(st.Length);
                }
            }
            return p;
        }

        private Parser seq(Parser[] parsers) {
            IEnumerable<string> p(int i, string input) {
                if (i == parsers.Length) {
                    yield return input;
                } else {
                    foreach (var inputT in parsers[i](input)) {
                        foreach (var inputTT in p(i + 1, inputT)) {
                            yield return inputTT;
                        }
                    }
                }
            };
            return (string input) => p(0, input);
        }

        private Parser or(Parser[] parsers) {
            IEnumerable<string> p(string input) {
                foreach (var parser in parsers) {
                    foreach (var res in parser(input)) {
                        yield return res;
                    }
                }
            };
            return p;
        }
    }
}