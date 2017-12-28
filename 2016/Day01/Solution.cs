using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2016.Day01 {

    class Solution : Solver {

        public string GetName() => "No Time for a Taxicab";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var (irow, icol) = Travel(input).Last();
            return irow + icol;
        }

        int PartTwo(string input) {
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
                switch (stm[0]) {
                    case 'R': (drow, dcol) = (dcol, -drow); break;
                    case 'L': (drow, dcol) = (-dcol, drow); break;
                }
                for (int i = 0; i < d; i++) {
                    (irow, icol) = (irow + drow, icol + dcol);
                    yield return (irow, icol);
                }
            }
        }
    }
}