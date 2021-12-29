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

        while (q.Count > 0) {
            maze = q.Dequeue();

            if (maze.Finished()) {
                return cost[maze];
            }

            foreach (var n in Neighbours(maze)) {
                if (cost[maze] + n.cost < cost.GetValueOrDefault(n.maze, int.MaxValue)) {
                    cost[n.maze] = cost[maze] + n.cost;
                    q.Enqueue(n.maze, cost[n.maze]);
                }
            }
        }

        throw new Exception();
    }

    Maze GetMaze(string input) {
        var map = input.Split("\n");
        var maze = new Maze(map.Length > 6);
        foreach (var irow in Enumerable.Range(0, map.Length)) {
            foreach (var icol in Enumerable.Range(0, map[0].Length)) {
                maze = maze.SetItem(
                    new Point(irow, icol), irow < map.Length && icol < map[irow].Length ? map[irow][icol] : '#');
            }
        }
        return maze;
    }

    int stepCost(char actor) {
        return actor == 'A' ? 1 : actor == 'B' ? 10 : actor == 'C' ? 100 : 1000;
    }

    int getIcolDst(char ch) {
        return
            ch == 'A' ? 3 :
            ch == 'B' ? 5 :
            ch == 'C' ? 7 :
            ch == 'D' ? 9 :
            throw new Exception();
    }

    (Maze maze, int cost) HallwayToRoom(Maze maze) {

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

                var l = HallwayToRoom(maze.SetItem(new Point(1, icol), '.').SetItem(pt, ch));
                return (l.maze, l.cost + steps * stepCost(ch));
            }
        }
        return (maze, 0);
    }

    IEnumerable<(Maze maze, int cost)> RoomToHallway(Maze maze) {
        var allowedHColumns = new int[] { 1, 2, 4, 6, 8, 10, 11 };

        // fel lehet-e menni
        foreach (var icol in new[] { 3, 5, 7, 9 }) {

            if (maze.FinishedColumn(icol)) {
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
                        var mazeT = maze.SetItem(ptSrc, '.').SetItem(ptDst, ch);
                        var c = (stepsV + stepsH) * stepCost(ch);
                        // (mazeT, c) = (Lemegy(mazeT).maze, c + Lemegy(mazeT).cost);

                        yield return (mazeT, c);
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

    IEnumerable<(Maze maze, int cost)> Neighbours(Maze maze) {
        var hallwayToRoom = HallwayToRoom(maze);
        return hallwayToRoom.cost != 0 ? new[] { hallwayToRoom } : RoomToHallway(maze);
    }

}

record Point(int irow, int icol) {
    public Point Below => new Point(irow + 1, icol);
    public Point Above => new Point(irow - 1, icol);
    public Point Left => new Point(irow, icol - 1);
    public Point Right => new Point(irow, icol + 1);
}

record Maze {

    bool big;
    int a, b, c, d;

    public Maze(bool big) : this(0, 0, 0, 0, big) {
    }

    private Maze(int a, int b, int c, int d, bool big) {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
        this.big = big;
    }

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

    private int ColumnMask(char ch) {
        if (big) {
            switch (ch) {
                case 'A': return (1 << 11) | (1 << 15) | (1 << 19) | (1 << 23);
                case 'B': return (1 << 12) | (1 << 16) | (1 << 20) | (1 << 24);
                case 'C': return (1 << 13) | (1 << 17) | (1 << 21) | (1 << 25);
                case 'D': return (1 << 14) | (1 << 18) | (1 << 22) | (1 << 26);
            }
        } else {
            switch (ch) {
                case 'A': return (1 << 11) | (1 << 15);
                case 'B': return (1 << 12) | (1 << 16);
                case 'C': return (1 << 13) | (1 << 17);
                case 'D': return (1 << 14) | (1 << 18);
            }
        }
        throw new Exception();
    }

    public bool FinishedColumn(int icol) =>
        icol switch {
            3 => (a & ColumnMask('A')) == ColumnMask('A'),
            5 => (b & ColumnMask('B')) == ColumnMask('B'),
            7 => (c & ColumnMask('C')) == ColumnMask('C'),
            9 => (d & ColumnMask('D')) == ColumnMask('D'),
            _ => throw new Exception()
        };

    public bool Finished() =>
        FinishedColumn(3) && FinishedColumn(5) && FinishedColumn(7) && FinishedColumn(9);

    public char ItemAt(Point pt) {
        var bit = BitFromPoint(pt);
        return
            bit == 1 << 31 ? '#' :
            (a & bit) != 0 ? 'A' :
            (b & bit) != 0 ? 'B' :
            (c & bit) != 0 ? 'C' :
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
            '.' => new Maze(
                a & ~bit,
                b & ~bit,
                c & ~bit,
                d & ~bit,
                big
            ),
            'A' => new Maze(a | bit, b, c, d, big),
            'B' => new Maze(a, b | bit, c, d, big),
            'C' => new Maze(a, b, c | bit, d, big),
            'D' => new Maze(a, b, c, d | bit, big),
            _ => throw new Exception()
        };
    }
}