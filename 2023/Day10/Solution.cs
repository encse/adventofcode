using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Y2023.Day10;

using Map = Dictionary<Complex, char>;

[ProblemName("Pipe Maze")]
class Solution : Solver {
    static readonly Complex Up = -Complex.ImaginaryOne;
    static readonly Complex Down = Complex.ImaginaryOne;
    static readonly Complex Left = -Complex.One;
    static readonly Complex Right = Complex.One;
    static readonly Complex[] Dirs = [Up, Right, Down, Left];

    static readonly Dictionary<char, Complex[]>  OutputDirs = new Dictionary<char, Complex[]>{
        {'7', [Left, Down] },
        {'F', [Right, Down]},
        {'L', [Up, Right]},
        {'J', [Up, Left]},
        {'|', [Up, Down]},
        {'-', [Left, Right]},
        {'S', [Up, Down, Left, Right]},
    };

    public object PartOne(string input) {
        var map = ParseMap(input);
        var loop = LoopPositions(map);
        return loop.Count / 2;
    }

    public object PartTwo(string input) {
        var map = ParseMap(input);
        var loop = LoopPositions(map);

        // remove pipes not in the loop:
        map = (
            from kvp in map
            let position = kvp.Key
            let cell = loop.Contains(position) ? kvp.Value : '.'
            select (position, cell)
        ).ToDictionary();

        return map.Keys.Count(position => Inside(map, position));
    }

    // Returns the positions that make up the loop starting at 'S'
    HashSet<Complex> LoopPositions(Map map) {
        var position = map.Keys.Single(k => map[k] == 'S');
        var positions = new HashSet<Complex>();

        // pick one direction that leads out from S and connected to the neighbour
        var dir = Dirs.First(dir => OutputDirs[map[position + dir]].Contains(-dir));

        for (; ; ) {
            positions.Add(position);
            position += dir;
            if (map[position] == 'S') {
                break;
            }
            dir = OutputDirs[map[position]].Single(dirOut => dirOut != -dir);
        }
        return positions;
    }

    // Check if position is inside the loop using ray casting algorithm
    bool Inside(Map map, Complex position) {
        var cell = map[position];
        if (cell != '.') {
            return false;
        }

        // Imagine a small elf starting from the top right segment of a cell 
        // and moving to the left jumping over the pipes it encounters.
        // Every jump flips the "inside" variable.
        var inside = false;
        position--;
        while (map.ContainsKey(position)) {
            if ("SJL|".Contains(map[position])) {
                inside = !inside;
            }
            position--;
        }
        return inside;
    }

    Map ParseMap(string input) {
        var rows = input.Split("\n");
        var crow = rows.Length;
        var ccol = rows[0].Length;
        var res = new Map();
        for (var irow = 0; irow < crow; irow++) {
            for (var icol = 0; icol < ccol; icol++) {
                res[new Complex(icol, irow)] = rows[irow][icol];
            }
        }
        return res;
    }
}
