using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Collections.Immutable;

using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

namespace AdventOfCode.Y2023.Day13;

[ProblemName("Point of Incidence")]
class Solution : Solver {

    Complex Right = 1;
    Complex Down = Complex.ImaginaryOne;

    public object PartOne(string input) => Solve(input, Part1Score);
    public object PartTwo(string input) => Solve(input, Part2Score);

    int Solve(string input, Func<Map, int> score) =>
        input.Split("\n\n").Select(ParseMap).Select(score).Sum();

    int Part1Score(Map map) => GetScores(map).Single();

    // try patching the map in all possible ways, and return the first score that differs from part1
    int Part2Score(Map map) {
        var part1Score = Part1Score(map);
        return (
            from patched in Patches(map)
            from score in GetScores(patched)
            where score != part1Score
            select score
        ).First();
    }

    // Find all possible mirrors and return the score associated with them
    int[] GetScores(Map map) {
        return [
            ..GetMirrors(map, Down),
            ..GetMirrors(map, Right).Select(r => r * 100),
        ];
    }

    // Starting from (0,0) go over the map in the given direction and check 
    // each row/column for possible mirror locations. Return the common 
    // intersection of these candidates.
    int[] GetMirrors(Map map, Complex step) {
        var ortho = step == Right ? Down : Right;
        var res = Get1DMirrors(map, Complex.Zero, ortho).ToHashSet();
        for (var pos = step; map.ContainsKey(pos); pos += step) {
            res.IntersectWith(Get1DMirrors(map, pos, ortho));
        }
        return res.ToArray();
    }

    // Collects the possible mirror locations in a single row/column
    IEnumerable<int> Get1DMirrors(Map map, Complex pos, Complex dir) {
        for (var i = 1; map.ContainsKey(pos); i++, pos += dir) {
            if (Is1DMirror(map, pos, dir)) {
                yield return i;
            }
        }
    }

    // Validate the mirror property for a single row/column starting at pos.
    bool Is1DMirror(Map map, Complex pos, Complex dir) {
        if (!map.ContainsKey(pos + dir)) {
            return false; // cannot a place a mirror at the edge of the map
        }

        for (var i = Complex.Zero; ; i += dir) {
            if (!map.ContainsKey(pos - i) || !map.ContainsKey(pos + i + dir)) {
                return true; // we reached the end of the map, it's a mirror
            } else if (map[pos - i] != map[pos + i + dir]) {
                return false; // not a mirror image
            }
        }
    }

    // All possible versions of a given map when changing a single key
    IEnumerable<Map> Patches(Map map) =>
        from key in map.Keys
        select map.SetItem(key, map[key] == '.' ? '#' : '.');

    Map ParseMap(string input) {
        var rows = input.Split("\n");
        return (
            from irow in Enumerable.Range(0, rows.Length)
            from icol in Enumerable.Range(0, rows[0].Length)
            let pos = new Complex(icol, irow)
            let cell = rows[irow][icol]
            select new KeyValuePair<Complex, char>(pos, cell)
        ).ToImmutableDictionary();
    }
}
