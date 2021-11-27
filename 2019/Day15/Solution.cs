using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2019.Day15;

[ProblemName("Oxygen System")]
class Solution : Solver {

    enum Tile {
        Wall = 0,
        Empty = 1,
        O2 = 2,
    }

    public object PartOne(string input) {
        var iicm = new ImmutableIntCodeMachine(input);
        return Bfs(iicm).First(s => s.tile == Tile.O2).path.Count;
    }

    public object PartTwo(string input) {
        var iicm = Bfs(new ImmutableIntCodeMachine(input)).First(s => s.tile == Tile.O2).iicm;
        return Bfs(iicm).Last().path.Count;
    }

    IEnumerable<(ImmutableIntCodeMachine iicm, ImmutableList<int> path, Tile tile)> Bfs(ImmutableIntCodeMachine startIicm) {

        (int dx, int dy)[] dirs = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) };

        var seen = new HashSet<(int x, int y)> { (0, 0) };
        var q = new Queue<(ImmutableIntCodeMachine iicm, ImmutableList<int> path, int x, int y)>();
        q.Enqueue((startIicm, ImmutableList<int>.Empty, 0, 0));
        while (q.Any()) {
            var current = q.Dequeue();

            for (var i = 0; i < dirs.Length; i++) {
                var (nextX, nextY) = (current.x + dirs[i].dx, current.y + dirs[i].dy);

                if (!seen.Contains((nextX, nextY))) {
                    seen.Add((nextX, nextY));
                    var nextPath = current.path.Add(i + 1);
                    var (nextIicm, output) = current.iicm.Run(i + 1);

                    var tile = (Tile)output.Single();
                    if (tile != Tile.Wall) {
                        yield return (nextIicm, nextPath, tile);
                        q.Enqueue((nextIicm, nextPath, nextX, nextY));
                    }
                }
            }
        }
    }
}
