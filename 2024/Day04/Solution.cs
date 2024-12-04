namespace AdventOfCode.Y2024.Day04;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using System.Linq;

using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

[ProblemName("Ceres Search")]
class Solution : Solver {

    Complex Up = -Complex.ImaginaryOne;
    Complex Down = Complex.ImaginaryOne;
    Complex Left = -1;
    Complex Right = 1;

    public object PartOne(string input) {
        var mat = GetMap(input);
        return (
            from pt in mat.Keys
            from dir in  new[] { Right, Right + Down, Down + Left, Down}
            where Matches(mat, pt, dir, "XMAS")
            select 1
        ).Count();
    }

    public object PartTwo(string input) {
        var mat = GetMap(input);
        return (
            from pt in mat.Keys
            where 
                Matches(mat, pt + Up + Left, Down + Right, "MAS") && 
                Matches(mat, pt + Down + Left, Up + Right, "MAS")
            select 1
        ).Count();
    }

    // check if the pattern (or its reverse) can be read in the given direction 
    // starting from pt
    bool Matches(Map map, Complex pt, Complex dir, string pattern) {
        var chars = Enumerable.Range(0, pattern.Length)
            .Select(i => map.GetValueOrDefault(pt + i * dir))
            .ToArray();
        return
            Enumerable.SequenceEqual(chars, pattern) ||
            Enumerable.SequenceEqual(chars, pattern.Reverse());
    }

    // store the points in a dictionary so that we can iterate over them and 
    // to easily deal with points outside the area using GetValueOrDefault
    Map GetMap(string input) {
        var map = input.Split("\n");
        return (
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Complex, char>(Complex.ImaginaryOne * y + x, map[y][x])
        ).ToImmutableDictionary();
    }
}