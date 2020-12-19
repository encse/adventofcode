using System.Collections.Generic;
using System.Linq;
using System;
using Parser = System.Func<string, System.Collections.Generic.IEnumerable<string>>;

namespace AdventOfCode.Y2020.Day19 {

    [ProblemName("Monster Messages")]
    class Solution : Solver {

        public object PartOne(string input) => Solve(input, true);
        public object PartTwo(string input) => Solve(input, false);

        int Solve(string input, bool part1) {
            var blocks = (
                from block in input.Split("\n\n") 
                select block.Split("\n")
            ).ToArray();

            var rules = new Dictionary<int, string>(
                from line in blocks[0]
                    let parts = line.Split(": ")
                    let index = int.Parse(parts[0])
                    let rule = parts[1]
                select 
                    new KeyValuePair<int, string>(index, rule)
            );

            if (!part1) {
                rules[8] = "42 | 42 8";
                rules[11] = "42 31 | 42 11 31";
            }

            var parsers = new Dictionary<int, Parser>();

            Parser getParser(int index) {
                if (!parsers.ContainsKey(index)) {
                    parsers[index] = (input) => getParser(index)(input); 

                    parsers[index] = 
                        alt(
                            from sequence in rules[index].Split(" | ") 
                            select 
                                seq(
                                    from item in sequence.Split(" ") 
                                    select
                                        item[0] == '"'                ? literal(item.Trim('"')) :
                                        int.TryParse(item, out var i) ? getParser(i) :
                                        throw new Exception()
                                )
                        );
                }
                return parsers[index];
            }

            return blocks[1].Count(data => getParser(0)(data).Any(st => st == ""));
        }

        static Parser literal(string st) =>
            input => input.StartsWith(st) ? new[] { input.Substring(st.Length) } : new string[0];

        static Parser seq(IEnumerable<Parser> parsers) {
            if (parsers.Count() == 1) {
                return parsers.Single();
            }

            var parseHead = parsers.First();
            var parseTail = seq(parsers.Skip(1));

            return input => 
                from tail in parseHead(input)
                from rest in parseTail(tail)
                select rest;
        }

        static Parser alt(IEnumerable<Parser> parsers) {
            if (parsers.Count() == 1) {
                return parsers.Single();
            }
            
            var arr = parsers.ToArray();
            return input => 
                from parser in arr
                from rest in parser(input)
                select rest;
        }
    }
}