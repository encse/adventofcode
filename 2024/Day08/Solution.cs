namespace AdventOfCode.Y2024.Day08;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

[ProblemName("Resonant Collinearity")]
class Solution : Solver {
    public object PartOne(string input) => GetUniquePositions(input, GetAntinodes1).Count();
    public object PartTwo(string input) => GetUniquePositions(input, GetAntinodes2).Count();

    HashSet<Complex> GetUniquePositions(string input, GetAntinodes getAntinodes) {
        var map = GetMap(input);

        var antennaLocations = (
            from pos in map.Keys 
            where char.IsAsciiLetterOrDigit(map[pos])
            select pos
        ).ToArray();

        return (
             from srcAntenna in antennaLocations
             from dstAntenna in antennaLocations
             where srcAntenna != dstAntenna && map[srcAntenna] == map[dstAntenna]
             from antinode in getAntinodes(srcAntenna, dstAntenna, map)
             select antinode
         ).ToHashSet();
    }

    // returns the antinode positions of srcAntenna on the dstAntenna side
    delegate IEnumerable<Complex> GetAntinodes(Complex srcAntenna, Complex dstAntenna, Map map);

    // in part 1 we just look at the immediate neighbour
    IEnumerable<Complex> GetAntinodes1(Complex srcAntenna, Complex dstAntenna, Map map) {
        var dir = dstAntenna - srcAntenna;
        var antinote = dstAntenna + dir;
        if (map.Keys.Contains(antinote)) {
            yield return antinote;
        }
    }

    // in part 2 this becomes a cycle, plus srcAntenna is also a valid position now
    IEnumerable<Complex> GetAntinodes2(Complex srcAntenna, Complex dstAntenna, Map map) {
        var dir = dstAntenna - srcAntenna;
        var antinote = dstAntenna;
        while (map.Keys.Contains(antinote)) {
            yield return antinote;
            antinote += dir;
        }
    }

    // store the points in a dictionary so that we can iterate over them and 
    // to easily deal with points outside the area using GetValueOrDefault
    Map GetMap(string input) {
        var map = input.Split("\n");
        return (
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Complex, char>(x - y * Complex.ImaginaryOne, map[y][x])
        ).ToImmutableDictionary();
    }
}