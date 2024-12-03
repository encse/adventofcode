namespace AdventOfCode.Y2024.Day03;

using System.Linq;
using System.Text.RegularExpressions;

[ProblemName("Mull It Over")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, @"mul\((\d{1,3}),(\d{1,3})\)");

    public object PartTwo(string input) => Solve(input, @"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)");

    long Solve(string input, string rx) {
        // overly functionaly approach...
        var matches = Regex.Matches(input, rx, RegexOptions.Multiline);
        return matches.Aggregate(
            (enabled: true, res: 0L), 
            (acc, m) => 
                (m.Value, acc.res, acc.enabled) switch {
                    ("don't()", _, _)  => (false, acc.res),
                    ("do()", _, _)     => (true, acc.res),
                    (_, var res, true) => 
                        (true, res + int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)),
                    _ => acc
                },
            acc => acc.res
        );
    }
}