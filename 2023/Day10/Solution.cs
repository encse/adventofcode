using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using AngleSharp.Common;

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
        return GetLoopPositions(ParseMap(input)).Count / 2;
    }

    public object PartTwo(string input) {

        var map = ParseMap(input);
        var positions = GetLoopPositions(map);
        var ccol = map.Keys.Select(k => k.Real).Max() + 1;
        var crow = map.Keys.Select(k => k.Imaginary).Max();

        map = map.Keys.ToDictionary(k => k, k => positions.Contains(k) ? map[k] : '.');

        var newMap = new Map();
        for (var irow = 0; irow < crow; irow++) {
            for (var icol = 0; icol < ccol; icol++) {
                var ch = map[new Complex(icol, irow)];
                var m = Magnify(ch);
                for (var v = 0; v < 9; v++) {
                    newMap[new Complex(3 * icol + (v % 3), 3 * irow + v / 3)] = m[v];
                }
            }
        }
        ccol = 3 * ccol + 2;
        crow = 3 * crow + 2;
        Fill(newMap);
        return newMap.Values.Count(v => v == '.');
    }

    HashSet<Complex> GetLoopPositions(Map map) {
        var curr = map.Keys.Single(k => map[k] == 'S');
        var positions = new HashSet<Complex>() { };
        var dir = Dirs.First(dir => CanStep(curr, dir, map));

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

    void Fill(Map map) {
        var ccol = map.Keys.Select(k => k.Real).Max();
        var crow = map.Keys.Select(k => k.Imaginary).Max();

        var q = new Queue<Complex>();
        q.Enqueue(new Complex(0, 0));
        while (q.Any()) {
            var p = q.Dequeue();
            if (map[p] != ',' && map[p] != '.') {
                continue;
            }
            map[p] = 'O';
            foreach (var d in Dirs) {
                if (map.ContainsKey(p + d)) {
                    q.Enqueue(p + d);
                }
            }
        }
    }
    Complex Rotate(Complex d) => d * Complex.ImaginaryOne;

    bool CanStep(Complex p, Complex d, Map map) {
        var curr = map[p];
        var next = map.GetValueOrDefault(p + d, '.');
        return DirsOut(curr).Contains(d) && DirsIn(next).Contains(d);
    }

    string Magnify(char ch) => ch switch {
        '7' => ",,,-7,,|,",
        'F' => ",,,,F-,|,",
        'L' => ",|,,L-,,,",
        'J' => ",|,-J,,,,",
        '|' => ",|,,|,,|,",
        '-' => ",,,---,,,",
        'S' => ",|,-S-,|,",
        '.' => ",,,,.,,,,",
        _ => throw new ArgumentException()
    };


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

    Complex[] DirsIn(char ch) => DirsOut(ch).Select(ch => -ch).ToArray();

    Map ParseMap(string input) {
        var rows = input.Split("\n");
        var crow = rows.Length;
        var ccol = rows[0].Length;
        var res = new Map();
        for (var irow = 0; irow < crow; irow++) {
            for (var icol = 0; icol < ccol; icol++) {
                res[new Complex(icol + 1, irow + 1)] = rows[irow][icol];
            }
        }
        for (var irow = -1; irow <= crow; irow++) {
            res[new Complex(0, irow)] = '.';
            res[new Complex(ccol + 1, irow)] = '.';
        }
        for (var icol = -1; icol <= ccol; icol++) {
            res[new Complex(icol, 0)] = '.';
            res[new Complex(icol, crow + 1)] = '.';
        }
        return res;
    }

}
