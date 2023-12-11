using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2023.Day11;

[ProblemName("Cosmic Expansion")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, 2);
    public object PartTwo(string input) => Solve(input, 1_000_000);

    long Solve(string input, int e) {
        var map = input.Split("\n");
        var crow = map.Length;
        var ccol = map[0].Length;
        var emptyRows = Enumerable.Range(0, crow).Where(irow => EmptyRow(map, irow)).ToHashSet();
        var emptyCols = Enumerable.Range(0, ccol).Where(icol => EmptyCol(map, icol)).ToHashSet();
        var stars = (
            from irow in Enumerable.Range(0, crow)
            from icol in Enumerable.Range(0, ccol)
            where map[irow][icol] == '#'
            select (irow, icol)
        ).ToArray();

        return (
            from star1 in stars
            from star2 in stars
            select Distance(star1, star2, e, emptyRows, emptyCols)
        ).Sum() / 2;
    }

    long Distance((int irow, int icol)p1, (int irow, int icol)p2, long e, HashSet<int> emptyRows, HashSet<int> emptyCols) {
        var (irowSrc, irowDst) = (Math.Min(p1.irow, p2.irow), Math.Max(p1.irow, p2.irow));
        var (icolSrc, icolDst) = (Math.Min(p1.icol, p2.icol), Math.Max(p1.icol, p2.icol));

        return (
            irowDst - irowSrc +
            icolDst - icolSrc +
            (e-1) * Enumerable.Range(icolSrc, icolDst - icolSrc).Count(emptyCols.Contains) +
            (e-1) * Enumerable.Range(irowSrc, irowDst - irowSrc).Count(emptyRows.Contains)
        );
    }

    bool EmptyRow(string[] map, int irow) {
        var crow = map.Length;
        var ccol = map[0].Length;
        return Enumerable.Range(0, ccol).All(icol => map[irow][icol] == '.');
    }

    bool EmptyCol(string[] map, int icol) {
        var crow = map.Length;
        var ccol = map[0].Length;
        return Enumerable.Range(0, crow).All(irow => map[irow][icol] == '.');
    }
}
