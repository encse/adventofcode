using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2016.Day01;

[ProblemName("No Time for a Taxicab")]
class Solution : Solver {

    public object PartOne(string input) {
        var (irow, icol) = Travel(input).Last();
        return irow + icol;
    }

    public object PartTwo(string input) {
        var seen = new HashSet<(int, int)>();
        foreach (var pos in Travel(input)) {
            if (seen.Contains(pos)) {
                return (pos.icol + pos.irow);
            }
            seen.Add(pos);
        }
        throw new Exception();
    }

    IEnumerable<(int irow, int icol)> Travel(string input) {
        var (irow, icol) = (0, 0);
        var (drow, dcol) = (-1, 0);
        yield return (irow, icol);

        foreach (var stm in Regex.Split(input, ", ")) {
            var d = int.Parse(stm.Substring(1));

            (drow, dcol) = stm[0] switch {
                'R' => (dcol, -drow),
                'L' => (-dcol, drow),
                _ => throw new ArgumentException()
            };
           
            for (int i = 0; i < d; i++) {
                (irow, icol) = (irow + drow, icol + dcol);
                yield return (irow, icol);
            }
        }
    }
}
