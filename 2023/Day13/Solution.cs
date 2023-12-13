using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
using AngleSharp.Common;

namespace AdventOfCode.Y2023.Day13;

[ProblemName("Point of Incidence")]
class Solution : Solver {

    Complex Right = 1;
    Complex Down = Complex.ImaginaryOne;

    public object PartOne(string input) => Solve(input, UnpatchedScore);
    public object PartTwo(string input) => Solve(input, PatchedScore);

    int Solve(string input, Func<Map, int> score) =>  (
        from block in input.Split("\n\n")
        let map = ParseMap(block)
        select score(map)
    ).Sum();

    int UnpatchedScore(Map map) => GetScores(map).Single();

    int PatchedScore(Map map) {
        var unpatchedScore = GetScores(map).Single();
        return (
            from patchedMap in Patches(map)
            from patchedScore in GetScores(patchedMap)
            where patchedScore != unpatchedScore
            select patchedScore
        ).First();
    }

    // returns all possible patched versions of a given map 
    IEnumerable<Map> Patches(Map map) {
        foreach (var key in map.Keys) {
            var res = new Map(map);
            res[key] = map[key] == '.' ? '#' : '.';
            yield return res;
        }
    }
    
    // Find all possible mirrors horiontally and vertically and returns the score 
    // associated with them
    int[] GetScores(Map map) {
        return [
            ..GetMirrorPositionsByDir(map, Down), 
            ..GetMirrorPositionsByDir(map, Right).Select(r => r * 100),
        ];
    }

    // Starting from (0,0) go over the map in the given direction and check 
    // each row/column for possible mirror locations. The result is the common 
    // intersection of these candidates.
    int[] GetMirrorPositionsByDir(Map map, Complex dir) {
        var ortho = dir == Right ? Down : Right;
        var res = MirrorCandidates(map, Complex.Zero, ortho).ToHashSet();
        for (var pos = dir; map.ContainsKey(pos); pos += dir) {
            res.IntersectWith(MirrorCandidates(map, pos, ortho));
        }
        return res.ToArray();
    }

    // Collects the possible mirror locations in a single row/column
    IEnumerable<int> MirrorCandidates(Map map, Complex pos, Complex dir) {
        for(var i=1; map.ContainsKey(pos); i++, pos += dir) {
            if (IsMirrorCandidate(map, pos, dir)) {
                yield return i;
            }
        }
    }

    // go in both directions from pos and validate the mirror property
    bool IsMirrorCandidate(Map map, Complex pos, Complex dir) {
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

    Map ParseMap(string input) {
        var rows = input.Split("\n");
        return (
            from irow in Enumerable.Range(0, rows.Length)
            from icol in Enumerable.Range(0, rows[0].Length)
            let pos = new Complex(icol, irow)
            let cell = rows[irow][icol]
            select new KeyValuePair<Complex, char>(pos, cell)
        ).ToDictionary();
    }
}
