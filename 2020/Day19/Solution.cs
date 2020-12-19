using System;
using System.Collections.Generic;
using System.Linq;
using Parser = System.Func<string, (bool, string)>;

namespace AdventOfCode.Y2020.Day19 {

    [ProblemName("Monster Messages")]
    class Solution : Solver {

        public object PartOne(string input) => Solve(input, true);
        public object PartTwo(string input) => Solve(input, false);

        private int Solve(string input, bool part1) {
            var rules = input.Split("\n\n")[0]
                .Split("\n")
                .ToDictionary(line => int.Parse(line.Split(":")[0]), line => line);

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

            Func<string, bool> valid = (input) => {
                var orig = input;
                var parse42 = parser(42);
                var parse31 = parser(31);

                var res = parse42(input);
                var n = 0;
                while (res.Item1) {
                    n++;
                    input = res.Item2;
                    res = parse42(input);
                }

                res = parse31(input);
                var m = 0;
                while (res.Item1) {
                    m++;
                    input = res.Item2;
                    res = parse31(input);
                }

                return input == "" && 
                    n >= 2 && m >= 1 && m < n && 
                    (!part1 || n == 2 && m == 1);
            };

            return input.Split("\n\n")[1].Split("\n").Count(valid);
        }

        private Parser seq(Parser[] parsers) =>
          (string input) => {
              var inputT = input;
              foreach (var parser in parsers) {
                  var res = parser(inputT);
                  if (!res.Item1) {
                      return (false, input);
                  } else {
                      inputT = res.Item2;
                  }
              }
              return (true, inputT);
          };

        private Parser literal(string st) =>
            (string input) => {
                return input.StartsWith(st) ? (true, input.Substring(st.Length)) : (false, input);
            };

        private Parser or(Parser[] parsers) {
            if (parsers.Length < 2) {
                throw new Exception();
            }
            return (string input) => {
                foreach (var parser in parsers) {
                    var res = parser(input);
                    if (res.Item1) {
                        return res;
                    }
                }
                return (false, input);
            };
        }
    }
}