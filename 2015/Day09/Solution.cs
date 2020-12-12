using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2015.Day09 {

    [ProblemName("All in a Single Night")]
    class Solution : Solver {

        public object PartOne(string input) => Routes(input).Min();
        public object PartTwo(string input) => Routes(input).Max();

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

        IEnumerable<T[]> Permutations<T>(T[] rgt) {
            IEnumerable<T[]> PermutationsRec(int i) {
                if (i == rgt.Length) {
                    yield return rgt.ToArray();
                }

                for (var j = i; j < rgt.Length; j++) {
                    (rgt[i], rgt[j]) = (rgt[j], rgt[i]);
                    foreach (var perm in PermutationsRec(i + 1)) {
                        yield return perm;
                    }
                    (rgt[i], rgt[j]) = (rgt[j], rgt[i]);
                }
            }

            return PermutationsRec(0);
        }
    }
}