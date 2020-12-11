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

            char PlaceInDirection(string st, int irow, int icol, int drow, int dcol) {
                while (true) {
                    (irow, icol) = (irow + drow, icol + dcol);
                    var place =
                        irow < 0 || irow >= crow ? 'L' :
                        icol < 0 || icol >= ccol ? 'L' :
                        st[irow * ccol + icol];
                    if (stop(place)) {
                        return place;
                    }
                }
            }

            int OccupiedPlaces(string st, int idx) {
                var directions = new[] { (0, -1), (0, 1), (-1, 0), (1, 0), (-1, -1), (-1, 1), (1, -1), (1, 1) };
                var n = 0;
                foreach (var (drow, dcol) in directions) {
                    if (PlaceInDirection(st, idx / ccol, idx % ccol, drow, dcol) == '#') {
                        n++;
                    }
                }
                return n;
            }

            return st =>
                string.Join("",
                    st.Select((ch, i) =>
                        ch switch {
                            'L' => OccupiedPlaces(st, i) == 0 ? '#' : 'L',
                            '#' => OccupiedPlaces(st, i) >= occupiedLimit ? 'L' : '#',
                            _ => ch
                        }
                    )
                );
        }
    }
}