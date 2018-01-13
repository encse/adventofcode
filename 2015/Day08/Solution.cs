using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day08 {

    class Solution : Solver {

        public string GetName() => "Matchsticks";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) =>
            (from line in input.Split('\n')
             let u = Regex.Unescape(line.Substring(1, line.Length - 2))
             select line.Length - u.Length).Sum();


        int PartTwo(string input) =>
            (from line in input.Split('\n')
            let u = "\"" + line.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\""
            select u.Length - line.Length).Sum();

    }
}