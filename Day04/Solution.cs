using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Day04 {

    class Solution : Solver {

        public void Solve(string input) {
            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
        }

        int PartOne(string input) =>
            ValidLineCount(input, word => word);

        int PartTwo(string input) =>
            ValidLineCount(input, word => string.Concat(word.OrderBy(ch => ch)));

        int ValidLineCount(string input, Func<string, string> normalizer) =>
            input.Split('\n').Where(IsValid(normalizer)).Count();

        Func<string, bool> IsValid(Func<string, string> normalizer) {
            return (string line) => {
                var seen = new HashSet<string>();
                foreach (var word in line.Split(' ')) {
                    var normalized = normalizer(word);
                    if (seen.Contains(normalized)) {
                        return false;
                    }
                    seen.Add(normalized);
                }
                return true;
            };
        }
    }
}
