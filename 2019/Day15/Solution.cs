using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2019.Day15 {

    class Solution : Solver {

        public string GetName() => "Oxygen System";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var icm = new IntCodeMachine(input);
            return Traverse(icm, ImmutableList<int>.Empty).Count;
        }

        int PartTwo(string input) {
            var icm = new IntCodeMachine(input);
            var o2 = Traverse(icm, ImmutableList<int>.Empty);
            return Traverse(icm, o2).Count - o2.Count;
        }

        int walk(IntCodeMachine icm, ImmutableList<int> path) {
            icm.Reset();
            var res = 0;
            foreach (var step in path) {
                res = (int)icm.Run(step).Single();
            }
            return res;
        }

        ImmutableList<int> Traverse(IntCodeMachine icm,  ImmutableList<int> startPath) {
            var q = new Queue<(int x, int y, ImmutableList<int> path)>();
            var seen = new HashSet<(int x, int y)>();

            (int dx, int dy)[] dirs = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) };

            var e = new[] { -1, 2, 1, 4, 3 };

            q.Enqueue((0, 0, startPath));
            seen.Add((0, 0));
            ImmutableList<int> lastPath = null;
            var stop = false;
            while (!stop && q.Any()) {
                var (x, y, path) = q.Dequeue();
                lastPath = path;

                for (var i = 0; i < dirs.Length; i++) {
                    var (xT, yT) = (x + dirs[i].dx, y + dirs[i].dy);

                    if (!seen.Contains((xT, yT))) {
                        seen.Add((xT, yT));

                        var pathT = path.Add(i + 1);
                        var res = walk(icm, pathT);
                        switch (res) {
                            case 0: break;
                            case 1: q.Enqueue((xT, yT, pathT)); break;
                            case 2: lastPath = pathT; stop = true; break;
                            default: throw new Exception();
                        }
                    }
                }

            }
            return lastPath;
        }
    }
}