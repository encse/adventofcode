using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day03 {

    class Solution : Solver {

        public string GetName() => "Crossed Wires";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Solve(input, (x) => Math.Abs(x.irow) + Math.Abs(x.icol));

        int PartTwo(string input) => Solve(input, (x) => x.distance1 + x.distance2);

        int Solve(string input, Func<(int irow, int icol, int distance1, int distance2), int> distance) {
            var paths = input.Split("\n");
            var trace1 = Trace(paths[0]);
            var trace2 = Trace(paths[1]);

            var distances =
                from pos in trace1.Keys
                where trace2.ContainsKey(pos)
                select distance((pos.irow, pos.icol, trace1[pos], trace2[pos]));
            return distances.Min();
        }

        Dictionary<(int irow, int icol), int> Trace(string path) {
            var res = new Dictionary<(int irow, int icol), int>();

            var (irow, icol, distance) = (0, 0, 0);
            foreach (var step in path.Split(",")) {
                var (drow, dcol) = step[0] switch {
                    'U' => (-1, 0),
                    'D' => (1, 0),
                    'R' => (0, -1),
                    'L' => (0, 1),
                    _ => throw new ArgumentException()
                };

                for (var i = 0; i < int.Parse(step.Substring(1)); i++) {
                    (irow, icol, distance) = (irow + drow, icol + dcol, distance + 1);

                    if (!res.ContainsKey((irow, icol))) {
                        res[(irow, icol)] = distance;
                    }
                }
            }

            return res;
        }
    }
}