namespace AdventOfCode.Y2025.Day04;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Data;

[ProblemName("Printing Department")]
class Solution : Solver {

    public object PartOne(string input) => Cleanup1(Parse(input));
    public object PartTwo(string input) => Cleanup2(Parse(input));
    
    HashSet<Complex> Parse(string input) {
        var lines = input.Split("\n");
        var (crow, ccol) = (lines.Length, lines[0].Length);
        return (
            from irow in Enumerable.Range(0, crow)
            from icol in Enumerable.Range(0, ccol)
            where lines[irow][icol] == '@'
            select new Complex(irow, icol)
        ).ToHashSet();
    }

    int Cleanup1(HashSet<Complex> rolls) {
        var reachable = rolls.Where(roll => Reachable(rolls, roll)).ToHashSet();
        return rolls.RemoveWhere(reachable.Contains);
    }

    int Cleanup2(HashSet<Complex> rolls) {
        var initialCount = rolls.Count;
        while (Cleanup1(rolls) != 0);
        return initialCount - rolls.Count;
    }

    bool Reachable(HashSet<Complex> rolls, Complex roll) => (
        from dcol in new[] { -1, 0, 1 }
        from drow in new[] { -1, 0, 1 }
        let neighbour = roll + drow + dcol * Complex.ImaginaryOne
        where neighbour != roll && rolls.Contains(neighbour)
        select neighbour
    ).Count() < 4;
}