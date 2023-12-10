using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Y2023.Day10;

using Cell = char;
using Map = Dictionary<Complex, char>;

[ProblemName("Pipe Maze")]
class Solution : Solver {
    static readonly Complex Up = -Complex.ImaginaryOne;
    static readonly Complex Down = Complex.ImaginaryOne;
    static readonly Complex Left = -Complex.One;
    static readonly Complex Right = Complex.One;
    static readonly Complex[] Dirs = [Up, Right, Down, Left];

    public object PartOne(string input) {
        var map = ParseMap(input);
        var loop = LoopPositions(map);
        return loop.Count / 2;
    }

    // We pretty much want to flood fill from a corner that is outside the loop.
    // Unfortunately we need to deal with loops not part of the relevant loop and 
    // the narrow tunnels where 'paint' should flow in but not represented as 
    // an individual cell in the input, such as two vertical pipes next to 
    // each other like ||.
    // 
    // The trick is to find the loop first, then remove everything that is not part
    // of it. Now scale up the map so that these narrow tunnels become real cells. 
    // I map each cell to a 3x3 pattern. 
    // 
    // Then run the the flood fill algoithm in the scaled up map and finally 
    // calculate the cells that were not 'painted', i.e. inside the original loop.
    public object PartTwo(string input) {
        var map = ParseMap(input);
        map = Filter(map, LoopPositions(map));
        map = ScaleUp(map);
        map = Fill(map, 0, ".", ' '); // top left corner is always '.'
        map = ScaleDown(map);
        return map.Values.Count(v => v == '.');
    }

    // Returns the positions that make up the loop starting at 'S'
    HashSet<Complex> LoopPositions(Map map) {
        var position = map.Keys.Single(k => map[k] == 'S');
        var positions = new HashSet<Complex>();

        // pick one direction that leads out from S and connected to the neighbour
        var dir = Dirs.First(dir => OutputDirs(map[position + dir]).Contains(-dir));

        for (; ; ) {
            positions.Add(position);
            position += dir;
            if (map[position] == 'S') {
                break;
            }
            dir = OutputDirs(map[position]).Single(dirOut => dirOut != -dir);
        }
        return positions;
    }

    // Fills the map using flood fill. Replaces cells of cellsToFill with the
    // given filler. Every other cell of the map is considered as wall.
    Map Fill(Map map, Complex start, string cellsToFill, Cell filler) {
        var q = new Queue<Complex>();
        q.Enqueue(start);
        while (q.Any()) {
            var p = q.Dequeue();
            if (!cellsToFill.Contains(map[p])) {
                continue;
            }
            map[p] = filler;
            foreach (var d in Dirs) {
                if (map.ContainsKey(p + d)) {
                    q.Enqueue(p + d);
                }
            }
        }
        return map;
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

    // Clears cells not in the provided hashset.
    Map Filter(Map map, HashSet<Complex> keep) =>
        map.Keys.ToDictionary(k => k, k => keep.Contains(k) ? map[k] : '.');

    // Creates a 3x scaled up map applying the patterns of ScaleUpCell
    Map ScaleUp(Map map) {
        var newMap = new Map();
        foreach (var pos in map.Keys) {
            var cell = map[pos];
            var pattern = ScaleUpCell(cell);
            for (var v = 0; v < 9; v++) {
                var d = v % 3 + v / 3 * Complex.ImaginaryOne;
                newMap[3 * pos + d] = pattern[v];
            }
        }
        return newMap;
    }

    // Invert ScaleUp, keeping the center of each 3x3 cell
    Map ScaleDown(Map map) =>
        map.Keys
            .Where(key => key.Imaginary % 3 == 1 && key.Real % 3 == 1)
            .ToDictionary(
                k => (k - new Complex(1, 1)) / 3,
                k => map[k]
            );

    // Directions leading out from a cell
    Complex[] OutputDirs(Cell cell) => cell switch {
        '7' => [Left, Down],
        'F' => [Right, Down],
        'L' => [Up, Right],
        'J' => [Up, Left],
        '|' => [Up, Down],
        '-' => [Left, Right],
        'S' => [Up, Down, Left, Right],
        _ => []
    };

    //  Returns a 3x3 pattern for a cell
    string ScaleUpCell(Cell cell) => cell switch {
        '7' => "...-7..|.",
        'F' => "....F-.|.",
        'L' => ".|..L-...",
        'J' => ".|.-J....",
        '|' => ".|..|..|.",
        '-' => "...---...",
        'S' => ".|.-S-.|.",
        _ => "...." + cell + "....", // just leave it in the middle
    };
}
