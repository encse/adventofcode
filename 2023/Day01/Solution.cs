using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]
class Solution : Solver {

    public object PartOne(string input) => 
        Solve(input, @"\d");

    public object PartTwo(string input) => 
        Solve(input, @"\d|one|two|three|four|five|six|seven|eight|nine");

    int Solve(string input, string rx) =>
        input.Split("\n").Select(line => GetNumber(line, rx)).Sum();
  
    int GetNumber(string line, string rx) {
        var first = Regex.Match(line, rx);
        var last = Regex.Match(line, rx, RegexOptions.RightToLeft);

        return ParseMatch(first) * 10 + ParseMatch(last);
    }

    int ParseMatch(Match m) => 
        !m.Success ? 0 :
        m.Groups[0].Value == "one" ? 1 :
        m.Groups[0].Value == "two" ? 2 :
        m.Groups[0].Value == "three" ? 3 :
        m.Groups[0].Value == "four" ? 4 :
        m.Groups[0].Value == "five" ? 5 :
        m.Groups[0].Value == "six" ? 6 :
        m.Groups[0].Value == "seven" ? 7 :
        m.Groups[0].Value == "eight" ? 8 :
        m.Groups[0].Value == "nine" ? 9 :
        int.Parse(m.Groups[0].Value);
}
