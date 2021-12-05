using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2021.Day05;

[ProblemName("Hydrothermal Venture")]
class Solution : Solver {

    public object PartOne(string input) => GetIntersectionCount(input, true);
    public object PartTwo(string input) => GetIntersectionCount(input, false);

    int GetIntersectionCount(string input, bool skipDiagonal) =>
        ParseLines(input).Aggregate(
            new Dictionary<Vec2, int>(),
            (grid, line) => DrawLine(grid, line, skipDiagonal),
            grid => grid.Count(kvp => kvp.Value > 1)
        );

    Dictionary<Vec2, int> DrawLine(Dictionary<Vec2, int> grid, Line line, bool skipDiagonal)  {
        var dx = Math.Sign(line.to.x - line.from.x);
        var dy = Math.Sign(line.to.y - line.from.y);

        if (!skipDiagonal || dx == 0 || dy == 0) {
            var pt = line.from;
            grid[pt] = grid.GetValueOrDefault(pt, 0) + 1;
            while (pt != line.to) {
                pt = new Vec2(pt.x + dx, pt.y + dy);
                grid[pt] = grid.GetValueOrDefault(pt, 0) + 1;
            }
        }
        return grid;
    }

    IEnumerable<Line> ParseLines(string input) => 
        from line in input.Split("\n") 
        let num = ParseNumbers(line).ToArray()
        select new Line(new Vec2(num[0], num[1]), new Vec2(num[2], num[3]));

    IEnumerable<int> ParseNumbers(string stLine) =>
        from st in stLine.Split(", ->".ToArray(), StringSplitOptions.RemoveEmptyEntries) 
        select int.Parse(st);
}

record Vec2(int x, int y);
record Line(Vec2 from, Vec2 to);
