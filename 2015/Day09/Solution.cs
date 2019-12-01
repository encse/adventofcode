using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2015.Day09 {

    class Solution : Solver {

        public string GetName() => "All in a Single Night";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Routes(input).Min();
        int PartTwo(string input) => Routes(input).Max();

        IEnumerable<int> Routes(string input) {
            var distances = input.Split('\n').SelectMany(line => {
                var m = Regex.Match(line, @"(.*) to (.*) = (.*)");
                var (a, b) = (m.Groups[1].Value, m.Groups[2].Value);
                var d = int.Parse(m.Groups[3].Value);
                return new[] {
                    (k: (a, b), d),
                    (k: (b, a), d)
                };
            }).ToDictionary(p => p.k, p => p.d);
           
            var cities = distances.Keys.Select(k => k.Item1).Distinct().ToArray();
            return Permutations(cities).Select(route =>
                route.Zip(route.Skip(1), (a, b) => distances[(a, b)]).Sum()
            );
        }

        IEnumerable<ImmutableList<T>> Permutations<T>(IList<T> st) {

            IEnumerable<ImmutableList<T>> PermutationsRec(ImmutableList<T> prefix, bool[] fseen) {
                if (prefix.Count == st.Count()) {
                    yield return prefix;
                } else {
                    for (int i = 0; i < st.Count(); i++) {
                        if (!fseen[i]) {
                            fseen[i] = true;
                            var prefixT = prefix.Add(st[i]);
                            foreach (var res in PermutationsRec(prefixT, fseen)) {
                                yield return res;
                            }
                            fseen[i] = false;
                        }
                    }
                }
            }

            return PermutationsRec(ImmutableList<T>.Empty, new bool[st.Count()]);
        }
    }
}