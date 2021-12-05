using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day05;

[ProblemName("Hydrothermal Venture")]
class Solution : Solver {

    public object PartOne(string input) => GetIntersections(ParseLines(input, skipDiagonals: true)).Count();
    public object PartTwo(string input) => GetIntersections(ParseLines(input, skipDiagonals: false)).Count();

    IEnumerable<Vec2> GetIntersections(IEnumerable<IEnumerable<Vec2>> lines) => 
        // group all the points and return the intersections:
        lines.SelectMany(pt => pt).GroupBy(pt => pt).Where(g => g.Count() > 1).Select(g => g.Key);
    
    IEnumerable<IEnumerable<Vec2>> ParseLines(string input, bool skipDiagonals) =>
        from line in input.Split("\n")
        // parse out numbers first:
        let ns = (
            from st in line.Split(", ->".ToArray(), StringSplitOptions.RemoveEmptyEntries)
            select int.Parse(st)
        ).ToArray()

        // line properties:
        let start = new Vec2(ns[0], ns[1])
        let end = new Vec2(ns[2], ns[3])
        let displacement = new Vec2(end.x - start.x, end.y - start.y)
        let length = 1 + Math.Max(Math.Abs(displacement.x), Math.Abs(displacement.y))
        let dir = new Vec2(Math.Sign(displacement.x), Math.Sign(displacement.y))

        // represent lines with a set of points:
        let points =
            from i in Enumerable.Range(0, length)
            select new Vec2(start.x + i * dir.x, start.y + i * dir.y)

        // skip diagonals in part 1:
        where !skipDiagonals || dir.x == 0 || dir.y == 0  

        select points;
}

record Vec2(int x, int y);
