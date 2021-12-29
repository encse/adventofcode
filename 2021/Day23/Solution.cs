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
        var maze = new Maze();
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
        for (var icol = 1; icol < 12; icol++) {
            var ch = maze.ItemAt(new Point(1, icol));

            if (ch == '.') {
                continue;
            }

            var icolDst = getIcolDst(ch);

            if (maze.CanMoveToDoor(icol, icolDst) && maze.CanEnterRoom(ch)) {
                var steps = Math.Abs(icolDst - icol);
                var pt = new Point(1, icolDst);

                while (maze.ItemAt(pt.Below) == '.') {
                    pt = pt.Below;
                    steps++;
                }

                var l = HallwayToRoom(maze.Move(new Point(1, icol), pt));
                return (l.maze, l.cost + steps * stepCost(ch));
            }
        }
        return (maze, 0);
    }

    IEnumerable<(Maze maze, int cost)> RoomToHallway(Maze maze) {
        var allowedHColumns = new int[] { 1, 2, 4, 6, 8, 10, 11 };

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
                        yield return (maze.Move(ptSrc, ptDst), (stepsV + stepsH) * stepCost(ch));
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

    int a, b, c, d;

    public Maze() {
        a = ColumnMask('A');
        b = ColumnMask('B');
        c = ColumnMask('C');
        d = ColumnMask('D');
    }

    private Maze(int a, int b, int c, int d) {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
    }

    int BitFromPoint(Point pt) =>
        (pt.irow, pt.icol) switch {
            (1, 1) => 1 << 0,
            (1, 2) => 1 << 1,
            (1, 3) => 1 << 2,
            (1, 4) => 1 << 3,
            (1, 5) => 1 << 4,
            (1, 6) => 1 << 5,
            (1, 7) => 1 << 6,
            (1, 8) => 1 << 7,
            (1, 9) => 1 << 8,
            (1, 10) => 1 << 9,
            (1, 11) => 1 << 10,

            (2, 3) => 1 << 11,
            (2, 5) => 1 << 12,
            (2, 7) => 1 << 13,
            (2, 9) => 1 << 14,

            (3, 3) => 1 << 15,
            (3, 5) => 1 << 16,
            (3, 7) => 1 << 17,
            (3, 9) => 1 << 18,

            (4, 3) => 1 << 19,
            (4, 5) => 1 << 20,
            (4, 7) => 1 << 21,
            (4, 9) => 1 << 22,

            (5, 3) => 1 << 23,
            (5, 5) => 1 << 24,
            (5, 7) => 1 << 25,
            (5, 9) => 1 << 26,

            _ => 1 << 31,
        };

    private int ColumnMask(char ch) =>
        ch switch {
            'A' => (1 << 11) | (1 << 15) | (1 << 19) | (1 << 23),
            'B' => (1 << 12) | (1 << 16) | (1 << 20) | (1 << 24),
            'C' => (1 << 13) | (1 << 17) | (1 << 21) | (1 << 25),
            'D' => (1 << 14) | (1 << 18) | (1 << 22) | (1 << 26),
            _ => throw new Exception()
        };

    public bool CanEnterRoom(char ch) {
        var mask = ColumnMask(ch);
        return ch switch {
            'A' => (b & mask) == 0 && (c & mask) == 0 && (d & mask) == 0,
            'B' => (a & mask) == 0 && (c & mask) == 0 && (d & mask) == 0,
            'C' => (a & mask) == 0 && (b & mask) == 0 && (d & mask) == 0,
            'D' => (a & mask) == 0 && (b & mask) == 0 && (c & mask) == 0,
            _ => throw new Exception()
        };
    }

    public bool CanMoveToDoor(int icolFrom, int icolTo) {
        Point step(Point pt) {
            return icolFrom < icolTo ? pt.Right : pt.Left;
        }
        var pt = step(new Point(1, icolFrom));
        while (pt.icol != icolTo) {
            if (this.ItemAt(pt) != '.') {
                return false;
            }
            pt = step(pt);
        }
        return true;
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

    public Maze Move(Point from, Point to) =>
        SetItem(to, ItemAt(from)).SetItem(from, '.');

    public Maze SetItem(Point pt, char ch) {
        if (ch == '#') {
            return this;
        }
        var bit = BitFromPoint(pt);
        if (bit == 1 << 31) {
            return this;
        }

        return ch switch {
            '.' => new Maze(
                a & ~bit,
                b & ~bit,
                c & ~bit,
                d & ~bit
            ),
            'A' => new Maze(a | bit, b & ~bit, c & ~bit, d & ~bit),
            'B' => new Maze(a & ~bit, b | bit, c & ~bit, d & ~bit),
            'C' => new Maze(a & ~bit, b & ~bit, c | bit, d & ~bit),
            'D' => new Maze(a & ~bit, b & ~bit, c & ~bit, d | bit),
            _ => throw new Exception()
        };
    }
}