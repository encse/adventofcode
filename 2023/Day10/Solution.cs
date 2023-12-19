namespace AdventOfCode.Y2023.Day10;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;

[ProblemName("Pipe Maze")]
class Solution : Solver {
    static readonly Complex Up = -Complex.ImaginaryOne;
    static readonly Complex Down = Complex.ImaginaryOne;
    static readonly Complex Left = -Complex.One;
    static readonly Complex Right = Complex.One;
    static readonly Complex[] Dirs = [Up, Right, Down, Left];

    static readonly Dictionary<char, Complex[]> Exits = new Dictionary<char, Complex[]>{
        {'7', [Left, Down] },
        {'F', [Right, Down]},
        {'L', [Up, Right]},
        {'J', [Up, Left]},
        {'|', [Up, Down]},
        {'-', [Left, Right]},
        {'S', [Up, Down, Left, Right]},
        {'.', []},
    };

    public object PartOne(string input) {
        var map = ParseMap(input);
        var loop = LoopPositions(map);
        return loop.Count / 2;
    }

    public object PartTwo(string input) {
        var map = ParseMap(input);
        var loop = LoopPositions(map);
        return map.Keys.Count(position => Inside(position, map, loop));
    }

    // Returns the positions that make up the loop containing 'S'
    HashSet<Complex> LoopPositions(Map map) {
        var position = map.Keys.Single(k => map[k] == 'S');
        var positions = new HashSet<Complex>();

        // pick a direction connected to a neighbour
        var dir = Dirs.First(dir => Exits[map[position + dir]].Contains(-dir));

        for (; ; ) {
            positions.Add(position);
            position += dir;
            if (map[position] == 'S') {
                break;
            }
            dir = Exits[map[position]].Single(exit => exit != -dir);
        }
        return positions;
    }

    // Check if position is inside the loop using ray casting algorithm
    bool Inside(Complex position, Map map, HashSet<Complex> loop) {
        // Imagine a small elf starting from the top half of a cell and moving 
        // to the left jumping over the pipes it encounters. It needs to jump 
        // over only 'vertically' oriented pipes leading upwards, since it runs 
        // in the top of the row. Each jump flips the "inside" variable.

        if (loop.Contains(position)) {
            return false;
        }

        var inside = false;
        position += Left;
        while (map.ContainsKey(position)) {
            if (loop.Contains(position) && Exits[map[position]].Contains(Up)) {
                inside = !inside;
            }
            position += Left;
        }
        return inside;
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
