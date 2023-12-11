using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day11;

[ProblemName("Cosmic Expansion")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, 1);
    public object PartTwo(string input) => Solve(input, 999999);

    long Solve(string input, int expansion) {
        var map = input.Split("\n");

        Func<int, bool> emptyRow = EmptyRows(map).ToHashSet().Contains;
        Func<int, bool> emptyCol = EmptyCols(map).ToHashSet().Contains;

        var stars = FindAll(map, '#');
        return (
            from star1 in stars
            from star2 in stars
            select
                Distance(star1.irow, star2.irow, expansion, emptyRow) +
                Distance(star1.icol, star2.icol, expansion, emptyCol)
        ).Sum() / 2;
    }

    long Distance(int i1, int i2, int expansion, Func<int, bool> empty) {
        var a = Math.Min(i1, i2);
        var d = Math.Abs(i1 - i2);
        return d + expansion * Enumerable.Range(a, d).Count(empty);
    }

    IEnumerable<int> EmptyRows(string[] map) =>
        from irow in Enumerable.Range(0, map.Length)
        where map[irow].All(ch => ch == '.')
        select irow;

    IEnumerable<int> EmptyCols(string[] map) =>
        from icol in Enumerable.Range(0, map[0].Length)
        where map.All(row => row[icol] == '.')
        select icol;

    IEnumerable<Position> FindAll(string[] map, char ch) =>
        from irow in Enumerable.Range(0, map.Length)
        from icol in Enumerable.Range(0, map[0].Length)
        where map[irow][icol] == ch
        select new Position(irow, icol);
}

record Position(int irow, int icol);