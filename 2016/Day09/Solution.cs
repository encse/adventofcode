using System.Text.RegularExpressions;

namespace AdventOfCode.Y2016.Day09;

[ProblemName("Explosives in Cyberspace")]
class Solution : Solver {

    public object PartOne(string input) {
        return Expand(input, 0, input.Length, false);
    }

    public object PartTwo(string input) {
        return Expand(input, 0, input.Length, true);
    }

    long Expand(string input, int i, int lim, bool recursive) {
        var res = 0L;
        while (i < lim) {
            if (input[i] == '(') {
                var j = input.IndexOf(')', i + 1);
                var m = Regex.Match(input.Substring(i + 1, j - i - 1), @"(\d+)x(\d+)");
                var length = int.Parse(m.Groups[1].Value);
                var mul = int.Parse(m.Groups[2].Value);
                res += recursive ? Expand(input, j + 1, j + length + 1, recursive) * mul : length * mul;
                i = j + length + 1;
            } else {
                res++;
                i++;
            }
        }
        return res;
    }
}
