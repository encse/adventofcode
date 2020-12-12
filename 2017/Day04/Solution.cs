using System;
using System.Linq;

namespace AdventOfCode.Y2017.Day04 {

    [ProblemName("High-Entropy Passphrases")]
    class Solution : Solver {

        public object PartOne(string lines) =>
            ValidLineCount(lines, word => word);

        public object PartTwo(string lines) =>
            ValidLineCount(lines, word => string.Concat(word.OrderBy(ch => ch)));

        int ValidLineCount(string lines, Func<string, string> normalizer) =>
            lines.Split('\n').Where(line => IsValidLine(line.Split(' '), normalizer)).Count();

        bool IsValidLine(string[] words, Func<string, string> normalizer) =>
            words.Select(normalizer).Distinct().Count() == words.Count();
    }
}
