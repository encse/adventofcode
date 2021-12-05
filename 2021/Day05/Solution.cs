using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2021.Day05;

[ProblemName("Hydrothermal Venture")]
class Solution : Solver {

    public object PartOne(string input) => GetIntersectionCount(ParseLines(input, skipDiagonal: true));
    public object PartTwo(string input) => GetIntersectionCount(ParseLines(input, skipDiagonal: false));

    int GetIntersectionCount(IEnumerable<Line> lines) =>
        lines.Aggregate(
            new Dictionary<Vec2, int>(),
            DrawLine,
            grid => grid.Count(kvp => kvp.Value > 1)
        );

    Dictionary<Vec2, int> DrawLine(Dictionary<Vec2, int> grid, Line line) {
        var dx = Math.Sign(line.to.x - line.from.x);
        var dy = Math.Sign(line.to.y - line.from.y);
        var pt = line.from; 

        while (true) {
            grid[pt] = grid.GetValueOrDefault(pt, 0) + 1;
            if (pt == line.to) {
                break;
            }
            pt = pt with {x = pt.x + dx, y = pt.y + dy};
        }
        return grid;
    }

    IEnumerable<Line> ParseLines(string input, bool skipDiagonal) =>
        from line in input.Split("\n")
        let num = ParseNumbers(line).ToArray()
        where !skipDiagonal || num[0] == num[2] || num[1] == num[3]
        select new Line(new Vec2(num[0], num[1]), new Vec2(num[2], num[3]));

    IEnumerable<int> ParseNumbers(string stLine) =>
        from st in stLine.Split(", ->".ToArray(), StringSplitOptions.RemoveEmptyEntries)
        select int.Parse(st);
}

record Vec2(int x, int y);
record Line(Vec2 from, Vec2 to);
