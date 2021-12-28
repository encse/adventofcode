using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2021.Day23;

[ProblemName("Amphipod")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input);
    public object PartTwo(string input) => Solve(Upscale(input));

    string Upscale(string input) {
        var lines = input.Split("\n").ToList();
        lines.Insert(3, "  #D#C#B#A#");
        lines.Insert(4, "  #D#B#A#C#");
        return string.Join("\n", lines);
    }

    int Solve(string input) {
        var maze = GetMaze(input);

        var q = new PriorityQueue<Maze, int>();
        var cost = new Dictionary<Maze, int>();

        q.Enqueue(maze, 0);
        cost.Add(maze, 0);

        bool finished(Maze maze) {
            bool colMatch(int icol, char c) {
                for (var pt = new Point(2, icol); maze.ItemAt(pt) != '#'; pt = pt.Below) {
                    if (maze.ItemAt(pt) != c) {
                        return false;
                    }
                }
                return true;
            }
            return colMatch(3, 'A') && colMatch(5, 'B') && colMatch(7, 'C') && colMatch(9, 'D');
        }

        while (q.Count > 0) {
            while (cost[q.UnorderedItems.First().Element] != q.UnorderedItems.First().Priority) {
                q.Dequeue();
            }
            
            maze =  q.Dequeue();;

            if (finished(maze)) {
                return cost[maze];
            }

            foreach (var n in Neighbours(maze)) {
                if (n.cost + cost[maze] < cost.GetValueOrDefault(n.maze, int.MaxValue)) {
                    cost[n.maze] = n.cost + cost[maze];
                    q.Enqueue(n.maze, cost[n.maze]);
                }
            }
        }

        throw new Exception();
    }

    Maze GetMaze(string input) {
        var map = input.Split("\n");
        var maze = new Maze(0, 0, 0, 0, map.Length > 6);
        foreach (var y in Enumerable.Range(0, map.Length)) {
            foreach (var x in Enumerable.Range(0, map[0].Length)) {
                maze = maze.SetItem(new Point(y, x), y < map.Length && x < map[y].Length ? map[y][x] : '#');
            }
        }
        return maze;
    }

    int stepCost(char actor) {
        return actor == 'A' ? 1 : actor == 'B' ? 10 : actor == 'C' ? 100 : 1000;
    }

    char getChDst(int icol) {
        return
            icol == 3 ? 'A' :
            icol == 5 ? 'B' :
            icol == 7 ? 'C' :
            icol == 9 ? 'D' :
            throw new Exception();
    }

    int getIcolDst(char ch) {
        return
            ch == 'A' ? 3 :
            ch == 'B' ? 5 :
            ch == 'C' ? 7 :
            ch == 'D' ? 9 :
            throw new Exception();
    }

    bool finishedColumn(Maze maze, int icol) {
        var pt = new Point(2, icol);
        var chDst = getChDst(icol);
        while (maze.ItemAt(pt) != '#') {
            if (maze.ItemAt(pt) != chDst) {
                return false;
            }
            pt = pt.Below;
        }
        return true;
    }

    IEnumerable<(Maze maze, int cost)> Neighbours(Maze maze) {

        bool columnIsClean(int icol, char ch) {
            var pt = new Point(2, icol);
            while (maze.ItemAt(pt) != '#') {
                if (maze.ItemAt(pt) != ch && maze.ItemAt(pt) != '.') {
                    return false;
                }
                pt = pt.Below;
            }
            return true;
        }

        bool rowIsClean(int icolFrom, int icolTo) {
            Point step(Point pt) {
                return icolFrom < icolTo ? pt.Right : pt.Left;
            }
            var pt = step(new Point(1, icolFrom));
            while (pt.icol != icolTo) {
                if (maze.ItemAt(pt) != '.') {
                    return false;
                }
                pt = step(pt);
            }
            return true;
        }

        // le-e lehet menni?
        (Maze maze, int cost) Lemegy(Maze maze) {
            for (var icol = 1; icol < 12; icol++) {
                var ch = maze.ItemAt(new Point(1, icol));

                if (ch == '.') {
                    continue;
                }

                var icolDst = getIcolDst(ch);

                if (rowIsClean(icol, icolDst) && columnIsClean(icolDst, ch)) {
                    var steps = Math.Abs(icolDst - icol);
                    var pt = new Point(1, icolDst);

                    while (maze.ItemAt(pt.Below) == '.') {
                        pt = pt.Below;
                        steps++;
                    }

                    var l = Lemegy(maze.SetItem(new Point(1, icol), '.').SetItem(pt, ch));
                    return (l.maze, l.cost + steps * stepCost(ch));
                }
            }
            return (maze, 0);
        }

        var lemegy = Lemegy(maze);
        if (lemegy.cost != 0) {
            yield return lemegy;
            yield break;
        }
        var allowedHColumns = new int[] { 1, 2, 4, 6, 8, 10, 11 };

        // fel lehet-e menni
        foreach (var icol in new[] { 3, 5, 7, 9 }) {

            if (finishedColumn(maze, icol)) {
                continue;
            }

            var stepsV = 0;
            var ptSrc = new Point(1, icol);
            while (maze.ItemAt(ptSrc) == '.') {
                ptSrc = ptSrc.Below;
                stepsV++;
            }

            var ch = maze.ItemAt(ptSrc);
            if (ch == '#') {
                continue;
            }

            foreach (var dj in new[] { -1, 1 }) {
                var stepsH = 0;
                var ptDst = new Point(1, icol);
                while (maze.ItemAt(ptDst) == '.') {

                    if (allowedHColumns.Contains(ptDst.icol)) {
                        yield return (
                            maze.SetItem(ptSrc, '.').SetItem(ptDst, ch),
                            (stepsV + stepsH) * stepCost(ch)
                        );
                    }

                    if (dj == -1) {
                        ptDst = ptDst.Left;
                    } else {
                        ptDst = ptDst.Right;
                    }
                    stepsH++;
                }
            }
        }
    }

}

record Point(int irow, int icol) {
    public Point Below => new Point(irow + 1, icol);
    public Point Above => new Point(irow - 1, icol);
    public Point Left => new Point(irow, icol - 1);
    public Point Right => new Point(irow, icol + 1);
}
record Maze(int a, int b, int c, int d, bool big) {

    int BitFromPoint(Point pt) =>
        (big, pt.irow, pt.icol) switch {
            (_, 1, 1) => 1 << 0,
            (_, 1, 2) => 1 << 1,
            (_, 1, 3) => 1 << 2,
            (_, 1, 4) => 1 << 3,
            (_, 1, 5) => 1 << 4,
            (_, 1, 6) => 1 << 5,
            (_, 1, 7) => 1 << 6,
            (_, 1, 8) => 1 << 7,
            (_, 1, 9) => 1 << 8,
            (_, 1, 10) => 1 << 9,
            (_, 1, 11) => 1 << 10,

            (_, 2, 3) => 1 << 11,
            (_, 2, 5) => 1 << 12,
            (_, 2, 7) => 1 << 13,
            (_, 2, 9) => 1 << 14,

            (_, 3, 3) => 1 << 15,
            (_, 3, 5) => 1 << 16,
            (_, 3, 7) => 1 << 17,
            (_, 3, 9) => 1 << 18,

            (true, 4, 3) => 1 << 19,
            (true, 4, 5) => 1 << 20,
            (true, 4, 7) => 1 << 21,
            (true, 4, 9) => 1 << 22,

            (true, 5, 3) => 1 << 23,
            (true, 5, 5) => 1 << 24,
            (true, 5, 7) => 1 << 25,
            (true, 5, 9) => 1 << 26,

            _ => 1 << 31,
        };

    public char ItemAt(Point pt) {
        var bit = BitFromPoint(pt);
        return
            bit == 1 << 31 ? '#' :
            (a & bit) != 0 ? 'A' :
            (b & bit) != 0 ? 'B' :
            (c & bit) != 0 ? 'C' :
            (d & bit) != 0 ? 'D' :
            (d & bit) != 0 ? 'D' :
            '.';
    }

    public Maze SetItem(Point pt, char ch) {
        var bit = BitFromPoint(pt);
        if (bit == 1 << 31) {
            if (ch != '#' && ch != ' ')
                throw new Exception();
            return this;
        }

        return ch switch {
            '.' =>
                this with {
                    a = a & ~bit,
                    b = b & ~bit,
                    c = c & ~bit,
                    d = d & ~bit
                },
            'A' => this with { a = a | bit },
            'B' => this with { b = b | bit },
            'C' => this with { c = c | bit },
            'D' => this with { d = d | bit },
            _ => throw new Exception()
        };
    }
}