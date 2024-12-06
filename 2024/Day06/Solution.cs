namespace AdventOfCode.Y2024.Day06;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;

[ProblemName("Guard Gallivant")]
class Solution : Solver {

    Complex Up = Complex.ImaginaryOne;
    Complex TurnRight = -Complex.ImaginaryOne;

    public object PartOne(string input) {
        var (map, start) = Parse(input);
        return Walk(map, start).positions.Count();
    }

    public object PartTwo(string input) {
        var (map, start) = Parse(input);
        var positions = Walk(map, start).positions;
        var loops = 0;
        // simply try a blocker in each locations visited by the guard and count the loops
        foreach (var block in positions.Where(pos => map[pos] == '.')) {
            map[block] = '#';
            if (Walk(map, start).isLoop) {
                loops++;
            }
            map[block] = '.';
        }
        return loops;
    }

    // returns the positions visited when starting from 'pos', isLoop is set if the 
    // guard enters a cycle.
    (IEnumerable<Complex> positions, bool isLoop) Walk(Map map, Complex pos) {
        var seen = new HashSet<(Complex pos, Complex dir)>();
        var dir = Up;
        while (map.ContainsKey(pos) && !seen.Contains((pos, dir))) {
            seen.Add((pos, dir));
            if (map.GetValueOrDefault(pos + dir) == '#') {
                dir *= TurnRight;
            } else {
                pos += dir;
            }
        }
        return (
            positions: seen.Select(s => s.pos).Distinct(),
            isLoop: seen.Contains((pos, dir))
        );
    }

    // store the grid in a dictionary, to make bounds checks and navigation simple
    // start represents the starting postion of the guard
    (Map map, Complex start) Parse(string input) {
        var lines = input.Split("\n");
        var map = (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(-Up * y + x, lines[y][x])
        ).ToDictionary();

        var start = map.First(x => x.Value == '^').Key;
        
        return (map, start);
    }
}