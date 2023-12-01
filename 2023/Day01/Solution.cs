using System;
using System.Linq;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]
class Solution : Solver {

    public object PartOne(string input) => 
        Solve(input, Digit);

    public object PartTwo(string input) => 
        Solve(input, st => Digit(st) ?? SpelledOutNumber(st));

    int? Digit(string st) => char.IsDigit(st[0]) ? st[0] - '0' : null;

    int? SpelledOutNumber(string st) =>
        st.StartsWith("one") ? 1 :
        st.StartsWith("two") ? 2 :
        st.StartsWith("three") ? 3 :
        st.StartsWith("four") ? 4 :
        st.StartsWith("five") ? 5 :
        st.StartsWith("six") ? 6 :
        st.StartsWith("seven") ? 7 :
        st.StartsWith("eight") ? 8 :
        st.StartsWith("nine") ? 9 :
        null;

    int Solve(string input, Func<string, int?> parser) =>
        input.Split("\n").Select(line => GetNumber(line, parser)).Sum();

    // Go over the line from both ends using the parser and find the first numbers 
    // in both directions, these will be the "first" and "last" digits.
    // Returns 0 if no numbers found.
    int GetNumber(string line, Func<string, int?> parse) {
        int? first = null;
        int? last = null;

        for (var i =0; i < line.Length; i++) {
            var prefix = line[i..]; 
            first = first ?? parse(prefix);

            var suffix = line[(line.Length - i - 1)..];
            last = last ?? parse(suffix);
            
            if (first.HasValue && last.HasValue) {
                return first.Value * 10 + last.Value;
            }
        }
        return 0;
    }
}
