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
            var q = new Queue<(int x, int y, ImmutableList<int> path)>();
            var seen = new HashSet<(int x, int y)>();

            (int dx, int dy)[] dirs = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) };

            var e = new []{-1, 2,1,4,3};
           
            int walk(ImmutableList<int> path){
                icm.Reset();
                var res = 0;
                foreach(var step in path){
                    res= (int)icm.Run(step).Single();
                }
                return res;
            }

            q.Enqueue((0, 0, ImmutableList<int>.Empty));
            seen.Add((0, 0));
            while (true) {
                var (x, y, path) = q.Dequeue();

                for (var i = 0; i < dirs.Length; i++) {
                    var (xT, yT) = (x + dirs[i].dx, y + dirs[i].dy);

                    if (!seen.Contains((xT, yT))) {
                        seen.Add((xT, yT));

                        var pathT = path.Add(i + 1);

                        var res = walk(pathT);
                        switch (res) {
                            case 0: break;
                            case 1: q.Enqueue((xT, yT, pathT)); break;
                            case 2: return pathT.Count;
                            default: throw new Exception();
                        }
                    }
                }

            }
        }

        int PartTwo(string input) {
            return 0;
        }
    }
}