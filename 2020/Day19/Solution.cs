using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Parser = System.Func<string, (bool, string)>;

namespace AdventOfCode.Y2020.Day19 {

    [ProblemName("Monster Messages")]
    class Solution : Solver {

        public object PartOne(string input) {
            return  Solve(input, false);
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

        public object PartTwo(string input) {
            return Solve(input, true);
        }


        private int Solve(string input, bool part2){
            var rules = input.Split("\n\n")[0].Split("\n").OrderBy(rule => int.Parse(rule.Split(":")[0]))
                .ToDictionary(line => int.Parse(line.Split(":")[0]), line => line);

            Dictionary<int, Parser> parsers = new Dictionary<int, Parser>();

            Func<string, bool> buildParser() {
                Parser buildParserPart(int i) {
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
                                    seqParsers.Add((st) => buildParserPart(int.Parse(item))(st));
                                }
                            }

                            orParsers.Add(seqParsers.Count == 1 ? seqParsers.Single() : seq(seqParsers.ToArray()));
                        }
                        parsers[i] = orParsers.Count == 1 ? orParsers.Single() : or(orParsers.ToArray());

                    }
                    return parsers[i];
                }


                parsers[0] = parsers[8] = parsers[11] = (input) => {
                    throw new Exception();
                };


                return (input) => {
                    
                    var orig = input;
                    var p42 = buildParserPart(42);
                    var p31 = buildParserPart(31);

                    var res = p42(input);
                    var n = 0;
                    while (res.Item1){
                        n++;
                        input = res.Item2;
                        res = p42(input);
                    }

                    res = p31(input);
                    var m = 0;
                    while (res.Item1){
                        m++;
                        input = res.Item2;
                        res = p31(input);
                    }

                    if (part2 && n >= 2 && m >= 1 && m < n){
                        return input == "";
                    } else if (!part2 && n == 2 && m == 1){
                        return input == "";
                    } else {
                        return false;
                    }
                };
            }

            var parser = buildParser();

            var c = 0;
            var data = input.Split("\n\n")[1].Split("\n");

            foreach (var line in data) {
                if (parser(line)) {
                   // Console.WriteLine(line);
                    c++;
                }
            }
            return c;
        }
    }
}