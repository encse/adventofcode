using System.Collections.Generic;
using System.Linq;
using System;
using Parser = System.Func<string, System.Collections.Generic.IEnumerable<string>>;

namespace AdventOfCode.Y2020.Day19 {

    [ProblemName("Monster Messages")]
    class Solution : Solver {

        public object PartOne(string input) => Solve(input, true);
        public object PartTwo(string input) => Solve(input, false);

        private int Solve(string input, bool part1) {
            var blocks = (
                from block in input.Split("\n\n") 
                select block.Split("\n")
            ).ToArray();

            var rules = new Dictionary<int, string>(
                from line in blocks[0]
                    let parts = line.Split(": ")
                    let index = int.Parse(parts[0])
                    let rule = parts[1]
                select new KeyValuePair<int, string>(index, rule)
            );

            if (!part1) {
                rules[8] = "42 | 42 8";
                rules[11] = "42 31 | 42 11 31";
            }

            Dictionary<int, Parser> parsers = new Dictionary<int, Parser>();

            Parser parser(int i) {
                if (!parsers.ContainsKey(i)) {
                    parsers[i] = alt(
                        from alternative in rules[i].Split(" | ") 
                        select seq(
                            from item in alternative.Split(" ")
                            select
                                item.StartsWith("\"") ?
                                    literal(item.Substring(1, item.Length - 2)) :
                                    int.Parse(item) switch { var i => (input) => parser(i)(input)}
                        ));
                }
                return parsers[i];
            }

            var parse = parser(0);
            return blocks[1].Count(data => parse(data).Any(st => st.Length == 0));
        }

        private Parser literal(string st) =>
            input => input.StartsWith(st) ? new[] { input.Substring(st.Length) } : new string[0];

        private static Func<IEnumerable<Parser>, Parser> seq = combine(parsers => {
            var head = parsers[0];
            var tail = seq(parsers.Skip(1));
            return input => 
                from suffix in head(input)
                from suffixT in tail(suffix)
                select suffixT;
        });

        private Func<IEnumerable<Parser>, Parser> alt = 
            combine(parsers => input =>
                from parser in parsers
                from suffix in parser(input)
                select suffix
            );

        private static Func<IEnumerable<Parser>, Parser> combine(Func<Parser[], Parser> c) {
            return parsers => {
                var parserArray = parsers.ToArray();
                return parserArray.Length == 1 ? parserArray[0]: c(parserArray);
            };
        }
    }
}