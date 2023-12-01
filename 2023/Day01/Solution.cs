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
        return ParseMatch(first.Value) * 10 + ParseMatch(last.Value);
    }

    int ParseMatch(string st) => st switch {
        "" => 0, // no match
        "one" => 1,
        "two" => 2,
        "three" => 3,
        "four" => 4,
        "five" => 5,
        "six" => 6,
        "seven" => 7,
        "eight" => 8,
        "nine" => 9,
        var d => int.Parse(d)
    };
}
