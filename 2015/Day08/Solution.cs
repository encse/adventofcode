using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2015.Day08;

[ProblemName("Matchsticks")]
class Solution : Solver {

    public object PartOne(string input) =>
        (from line in input.Split('\n')
         let u = Regex.Unescape(line.Substring(1, line.Length - 2))
         select line.Length - u.Length).Sum();


    public object PartTwo(string input) =>
        (from line in input.Split('\n')
        let u = "\"" + line.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\""
        select u.Length - line.Length).Sum();

}
