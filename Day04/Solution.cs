using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Day04 {

    class Solution : Solver {

        public string GetName() => "High-Entropy Passphrases"; 

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string lines) =>
            ValidLineCount(lines, word => word);

        int PartTwo(string lines) =>
            ValidLineCount(lines, word => string.Concat(word.OrderBy(ch => ch)));

        int ValidLineCount(string lines, Func<string, string> normalizer) =>
            lines.Split('\n').Where(line => IsValidLine(line.Split(' '), normalizer)).Count();

        bool IsValidLine(string[] words, Func<string, string> normalizer) =>
            words.Select(normalizer).Distinct().Count() == words.Count();
    }
}
