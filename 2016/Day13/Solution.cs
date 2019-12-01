using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2016.Day13 {

    class Solution : Solver {

        public string GetName() => "A Maze of Twisty Little Cubicles";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => 
            Steps(int.Parse(input))
            .First(s => s.icol == 31 && s.irow == 39)
            .steps;

        int PartTwo(string input) =>
            Steps(int.Parse(input))
            .TakeWhile(s => s.steps <= 50)
            .Count();

        IEnumerable<(int steps, int irow, int icol)> Steps(int input) {
            var q = new Queue<(int steps, int irow, int icol)>();
            q.Enqueue((0, 1, 1));
            var seen = new HashSet<(int, int)>();
            seen.Add((1, 1));
            var n = (
                    from drow in new[] { -1, 0, 1 }
                    from dcol in new[] { -1, 0, 1 }
                    where Math.Abs(drow) + Math.Abs(dcol) == 1
                    select (drow, dcol)
                ).ToArray();

            while (q.Any()) {
                var (steps, irow, icol) = q.Dequeue();

                yield return (steps, irow, icol);

                foreach (var (drow, dcol) in n) {
                    var (irowT, icolT) = (irow + drow, icol + dcol);
                    if (irowT >= 0 && icolT >= 0) {
                        var w = icolT * icolT + 3 * icolT + 2 * icolT * irowT + irowT + irowT * irowT + input;
                        if (Convert.ToString(w, 2).Count(ch => ch == '1') % 2 == 0) {
                            if (!seen.Contains((irowT, icolT))) {
                                q.Enqueue((steps + 1, irowT, icolT));
                                seen.Add((irowT, icolT));
                            }
                        }
                    }
                }
            }
        }
    }
}