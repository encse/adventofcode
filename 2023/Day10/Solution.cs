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
        var loop = LoopPositions(map);
        map = Filter(map, loop);
        map = Replace(map, '.', 'I');
        map = ScaleUp(map);
        // start from the top left corner, works for the input
        map = Fill(map, Complex.Zero, ".I", 'O'); 
        return map.Values.Count(v => v == 'I');
    }

    // Finds 'S' in the map and returns the coordinates that make up the loop
    HashSet<Complex> LoopPositions(Map map) {
        var curr = map.Keys.Single(k => map[k] == 'S');
        var positions = new HashSet<Complex>() { };
        var dir = Dirs.First(dir => DirsIn(map[curr + dir]).Contains(dir));

        for (; !positions.Contains(curr);) {
            positions.Add(curr);
            curr += dir;
            if (map[curr] == 'S') {
                break;
            }
            var outs = DirsOut(map[curr]);
            dir = DirsOut(map[curr]).Single(dirOut => dirOut != -dir);
        }
        return positions;
    }

    // Fills the map using flood fill replacing chars of charsToFill with the
    // given fillChar. Every other character of the map is considered as a wall.
    Map Fill(Map map, Complex start, string charsToFill, char fillChar) {
        var q = new Queue<Complex>();
        q.Enqueue(start);
        while (q.Any()) {
            var p = q.Dequeue();
            if (!charsToFill.Contains(map[p])) {
                continue;
            }
            map[p] = fillChar;
            foreach (var d in Dirs) {
                if (map.ContainsKey(p + d)) {
                    q.Enqueue(p + d);
                }
            }
        }
        return map;
    }

    // In which directions can a cell left
    Complex[] DirsOut(char ch) => ch switch {
        '7' => [Left, Down],
        'F' => [Right, Down],
        'L' => [Up, Right],
        'J' => [Up, Left],
        '|' => [Up, Down],
        '-' => [Left, Right],
        'S' => [Up, Down, Left, Right],
        _ => []
    };

    // In which directions can a cell entered
    Complex[] DirsIn(char ch) => 
        DirsOut(ch).Select(ch => -ch).ToArray();

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

    Map Filter(Map map, HashSet<Complex> keep) => 
        map.Keys.ToDictionary(k => k, k => keep.Contains(k) ? map[k] : '.');

    Map Replace(Map map, char chSrc, char chDst) => 
        map.Keys.ToDictionary(k => k, k => map[k] == chSrc ? chDst : map[k]);

    // Creates a 3x scaled up map applying the patterns of MagnifyCh
    Map ScaleUp(Map map) {
        var ccol = map.Keys.Select(k => k.Real).Max();
        var crow = map.Keys.Select(k => k.Imaginary).Max();
        var newMap = new Map();
        for (var irow = 0; irow < crow; irow++) {
            for (var icol = 0; icol < ccol; icol++) {
                var ch = map[new Complex(icol, irow)];
                var m = MagnifyCh(ch);
                for (var v = 0; v < 9; v++) {
                    newMap[new Complex(3 * icol + (v % 3), 3 * irow + v / 3)] = m[v];
                }
            }
        }
        return newMap;
    }

    // Defines a 3x3 magnifiying pattern for a char
    string MagnifyCh(char ch) => ch switch {
        '7' => "...-7..|.",
        'F' => "....F-.|.",
        'L' => ".|..L-...",
        'J' => ".|.-J....",
        '|' => ".|..|..|.",
        '-' => "...---...",
        'S' => ".|.-S-.|.",
        _ => "...." + ch + "....", // just leave it in the middle
    };
}
