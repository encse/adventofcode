using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2021.Day23;

[ProblemName("Amphipod")]
class Solution : Solver {

    public object PartOne(string input)  => Solve(input).Min();
    public object PartTwo(string input)  => Solve(Upscale(input)).Min();

    string Upscale(string input){
        var lines = input.Split("\n").ToList();
        lines.Insert(3, "  #D#C#B#A#");
        lines.Insert(4, "  #D#B#A#C#");
        return string.Join("\n", lines);
    }

    IEnumerable<int> Solve(string input) {
        var maze = GetMaze(input);

        var q = new PriorityQueue<Maze, int>();
        var seen = new Dictionary<string, Maze>();
        q.Enqueue(maze, maze.cost);
        seen.Add(maze.Tsto(), maze);

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
            maze = q.Dequeue();

            foreach (var mazeT in Neighbours(maze)) {
                if (finished(mazeT)) {
                    yield return mazeT.cost;
                } else if (!seen.ContainsKey(mazeT.Tsto()) || seen[mazeT.Tsto()].cost > mazeT.cost) {
                    seen[mazeT.Tsto()] = mazeT;
                    q.Enqueue(mazeT with { prev = maze }, mazeT.cost);
                }
            }
        }
    }

    Maze GetMaze(string input) {
        var map = input.Split("\n");
        return new Maze((
                from y in Enumerable.Range(0, map.Length)
                from x in Enumerable.Range(0, map[0].Length)
                select new KeyValuePair<Point, char>(new Point(y, x), y < map.Length && x < map[y].Length ? map[y][x] : ' ')
            ).ToImmutableDictionary(),
            0
        );
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

    IEnumerable<Maze> Neighbours(Maze maze) {

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
        Maze Lemegy(Maze maze) {
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

                    return Lemegy(maze with {
                        map = maze.map.SetItem(new Point(1, icol), '.').SetItem(pt, ch),
                        cost = maze.cost + steps * stepCost(ch)
                    });
                }
            }
            return maze;
        }

        if (Lemegy(maze).Tsto() != maze.Tsto()) {
            yield return Lemegy(maze);
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
                        yield return maze with {
                            map = maze.map.SetItem(ptSrc, '.').SetItem(ptDst, ch),
                            cost = maze.cost + (stepsV + stepsH) * stepCost(ch)
                        };
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
record Maze(ImmutableDictionary<Point, char> map, int cost, Maze prev = null) {

    public char ItemAt(Point pt) => map.GetValueOrDefault(pt, '#');

    public string Tsto() {
        var st = "";
        for (var irow = 0; irow < 7; irow++) {
            for (var icol = 0; icol < 13; icol++) {
                st += map.GetValueOrDefault(new Point(irow, icol), ' ');
            }
            st += "\n";
        }
        st += "---\n";
        return st;
    }
}