using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day10;

[ProblemName("Syntax Scoring")]
class Solution : Solver {

    public object PartOne(string input) => GetScores(input, syntaxError: true).Sum();
    public object PartTwo(string input) => Median(GetScores(input, syntaxError: false));

    public long Median(IEnumerable<long> items) =>
        items.OrderBy(x => x).ElementAt(items.Count() / 2);

    IEnumerable<long> GetScores(string input, bool syntaxError) =>
        input.Split("\n").Select(line => GetScore(line, syntaxError)).Where(score => score > 0);

    long GetScore(string line, bool syntaxErrorScore) {
        // standard stack based approach
        var stack = new Stack<char>();

        foreach (var ch in line) {
            switch ((stack.FirstOrDefault(), ch)) {
                // matching closing parenthesis:
                case ('(', ')'): stack.Pop(); break;
                case ('[', ']'): stack.Pop(); break;
                case ('{', '}'): stack.Pop(); break;
                case ('<', '>'): stack.Pop(); break;
                // return early if syntax error found:
                case (_, ')'): return syntaxErrorScore ? 3     : 0;
                case (_, ']'): return syntaxErrorScore ? 57    : 0;
                case (_, '}'): return syntaxErrorScore ? 1197  : 0;
                case (_, '>'): return syntaxErrorScore ? 25137 : 0;
                // otherwise, it's an opening parenthesis:
                case (_, _): stack.Push(ch); break;
            }
        }

        if (syntaxErrorScore) {
            return 0;
        } else {
            return stack
                .Select(item => 1 + "([{<".IndexOf(item)) // convert chars to digits
                .Aggregate(0L, (acc, item) => acc * 5 + item); // get base 5 number
        }
    }
}
