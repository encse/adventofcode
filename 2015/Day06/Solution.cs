using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2015.Day06 {

    class Solution : Solver {

        public string GetName() => "Probably a Fire Hazard";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Run(input, _ => 1, _ => 0, v => 1 - v);
        int PartTwo(string input) => Run(input, v => v + 1, v => v > 0 ? v - 1 : 0, v => v + 2);

        int Run(string input, Func<int, int> turnOn, Func<int, int> turnOff, Func<int, int> toggle) {
            int[] apply(int[] grid, string line, string pattern, Func<int, int> dg) {
                var match = Regex.Match(line, pattern);
                if (match.Success) {
                    var rect = match.Groups.Cast<Group>().Skip(1).Select(g => int.Parse(g.Value)).ToArray();
                    for (int irow = rect[0]; irow <= rect[2]; irow++) {
                        for (int icol = rect[1]; icol <= rect[3]; icol++) {
                            grid[irow * 1000 + icol] = dg(grid[irow * 1000 + icol]);
                        }
                    }
                    return grid;
                } else {
                    return null;
                }
            }
            return input.Split('\n')
                .Aggregate(new int[1000 * 1000], (grid, line) =>
                    apply(grid, line, @"turn on (\d+),(\d+) through (\d+),(\d+)", turnOn) ??
                    apply(grid, line, @"turn off (\d+),(\d+) through (\d+),(\d+)", turnOff) ??
                    apply(grid, line, @"toggle (\d+),(\d+) through (\d+),(\d+)", toggle) ??
                    throw new Exception(line))
                .Sum();
        }
    }
}