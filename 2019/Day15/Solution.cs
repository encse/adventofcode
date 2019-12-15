using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2019.Day15 {

    class Solution : Solver {

        enum Tile {
            Wall = 0,
            Empty = 1,
            O2 = 2,
        }

        public string GetName() => "Oxygen System";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var iicm = new ImmutableIntCodeMachine(input);
            return Bfs(iicm).First(s => s.tile == Tile.O2).path.Count;
        }

        int PartTwo(string input) {
            var iicm = Bfs(new ImmutableIntCodeMachine(input)).First(s => s.tile == Tile.O2).iicm;
            return Bfs(iicm).Last().path.Count;
        }

        IEnumerable<(Tile tile, ImmutableList<int> path, ImmutableIntCodeMachine iicm)> Bfs(ImmutableIntCodeMachine startIicm) {

            (int dx, int dy)[] dirs = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) };

            var seen = new HashSet<(int x, int y)>{(0,0)};
            var q = new Queue<(int x, int y, ImmutableList<int> path, ImmutableIntCodeMachine iicm)>();
            q.Enqueue((0, 0, ImmutableList<int>.Empty, startIicm));
            while (q.Any()) {
                var (x, y, path, iicm) = q.Dequeue();

                for (var i = 0; i < dirs.Length; i++) {
                    var (xT, yT) = (x + dirs[i].dx, y + dirs[i].dy);

                    if (!seen.Contains((xT, yT))) {
                        seen.Add((xT, yT));

                        var pathT = path.Add(i + 1);
                        var (iicmT, output) = iicm.Run(i+1); 
                        var tile = (Tile)output.Single();
                        if (tile != Tile.Wall) {
                            yield return (tile, pathT, iicmT);
                            q.Enqueue((xT, yT, pathT, iicmT));
                        }
                    }
                }
            }
        }

        Tile Walk(IntCodeMachine icm, ImmutableList<int> path) {
            icm.Reset();
            Tile tile = 0;
            foreach (var step in path) {
                tile = (Tile)icm.Run(step).Single();
            }
            return tile;
        }
    }
}