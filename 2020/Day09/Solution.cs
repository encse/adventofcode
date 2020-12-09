using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day09 {

    [ProblemName("Encoding Error")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        IEnumerable<int> Range(int min, int lim) => Enumerable.Range(min, lim - min);

        long PartOne(string input) {
            var numbers = input.Split("\n").Select(long.Parse).ToArray();

            bool Mismatch(int i) => (
                from j in Range(i - 25, i)
                from k in Range(j + 1, i)
                select numbers[j] + numbers[k]
            ).All(sum => sum != numbers[i]);

            return numbers[Range(25, input.Length).First(Mismatch)];
        }

        long PartTwo(string input) {
            var d = PartOne(input);
            var lines = input.Split("\n").Select(long.Parse).ToList();

            foreach (var j in Range(0, lines.Count)) {
                var s = lines[j];
                foreach (var k in Range(j + 1, lines.Count)) {
                    s += lines[k];
                    if (s > d) {
                        break;
                    } else if (s == d) {
                        var range = lines.GetRange(j, k - j + 1);
                        return range.Min() + range.Max();
                    }
                }
            }
            throw new Exception();
        }
    }
}