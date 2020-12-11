using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day11 {

    [ProblemName("Seating System")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Solve(input, 4, _ => true);
        int PartTwo(string input) => Solve(input, 5, ch => ch != '.');

        int Solve(string input, int occupiedLimit, Func<char, bool> stop) =>
            FixPoint(
                input.Replace("\n", "").Replace("L", "#"),
                Step(input.Split("\n").Length, input.IndexOf('\n'), occupiedLimit, stop)
            ).Count(ch => ch == '#');

        string FixPoint(string st, Func<string, string> f) {
            var (prev, curr) = (st, f(st));
            while (prev != curr) {
                (prev, curr) = (curr, f(curr));
            }
            return curr;
        }

        Func<string, string> Step(int crow, int ccol, int occupiedLimit, Func<char, bool> stop) {

            int OccupiedPlaces(string st, int irow, int icol) {
                var n = 0;
                foreach (var (drow, dcol) in new[] { (0, -1), (0, 1), (-1, 0), (1, 0), (-1, -1), (-1, 1), (1, -1), (1, 1) }) {
                    var (irowT, icolT) = (irow, icol);
                    while (true) {
                        (irowT, icolT) = (irowT + drow, icolT + dcol);
                        var place =
                            irowT < 0 || irowT >= crow ? 'L' :
                            icolT < 0 || icolT >= ccol ? 'L' :
                            st[irowT * ccol + icolT];

                        if (stop(place)) {
                            if (place == '#')
                                n++;
                            break;
                        }
                    }
                }
                return n;
            }

            return st =>
                string.Join("",
                    from irow in Enumerable.Range(0, crow)
                    from icol in Enumerable.Range(0, ccol)
                    select
                        st[irow * ccol + icol] switch {
                            'L' => OccupiedPlaces(st, irow, icol) == 0 ? '#' : 'L',
                            '#' => OccupiedPlaces(st, irow, icol) >= occupiedLimit ? 'L' : '#',
                            var ch => ch
                        }
                );
        }
    }
}