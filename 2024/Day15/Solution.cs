namespace AdventOfCode.Y2024.Day15;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

using Map = System.Collections.Immutable.IImmutableDictionary<System.Numerics.Complex, char>;

[ProblemName("Warehouse Woes")]
class Solution : Solver {

    static Complex Up = Complex.ImaginaryOne;
    static Complex Down = -Complex.ImaginaryOne;
    static Complex Left = -1;
    static Complex Right = 1;

    static Dictionary<char, Complex> move = new Dictionary<char, Complex>{
        {'^', Up},
        {'<', Left},
        {'>', Right},
        {'v', Down},
    };

    public object PartOne(string input) => Solve(input, false);
    public object PartTwo(string input) => Solve(input, true);

    public int Solve(string input, bool scaleUp) {
        var (map, movements) = Parse(input);
        if (scaleUp) {
            map = ScaleUp(map);
        }
        var robot = map.Keys.Single(k => map[k] == '@');
        foreach (var m in movements) {
            var dir = move[m];
            if (Step(map, robot, dir, out map)) {
                robot += dir;
            }
        }
        var res = 0.0;
        foreach (var crate in map.Keys.Where(k => map[k] == '[' || map[k] == 'O')) {
            res += 100 * Math.Abs(crate.Imaginary) + crate.Real;
        }
        return (int)res;
    }

    bool Step(Map map, Complex pos, Complex dir, out Map mapAfter) {
        // var mapOrig = map;
        
        if (map[pos] == '.') {
            mapAfter = map;
            return true;
        } else if (map[pos] == 'O' || map[pos] == '@') {
            if (Step(map, pos + dir, dir, out mapAfter)) {
                mapAfter = mapAfter.SetItem(pos + dir, map[pos]);
                mapAfter = mapAfter.SetItem(pos, '.');
                return true;
            }
        } else if (map[pos] == ']') {
            return Step(map, pos + Left, dir, out mapAfter);
        } else if (map[pos] == '[') {
            if (dir == Left) {
                if (Step(map, pos + Left, Left, out mapAfter)) {
                    mapAfter = mapAfter.SetItem(pos + Left, '[');
                    mapAfter = mapAfter.SetItem(pos, ']');
                    mapAfter = mapAfter.SetItem(pos + Right, '.');
                    return true;
                }
            }
            if (dir == Right) {
                if (Step(map, pos + 2 * Right, dir, out mapAfter)) {
                    mapAfter = mapAfter.SetItem(pos, '.');
                    mapAfter = mapAfter.SetItem(pos + Right, '[');
                    mapAfter = mapAfter.SetItem(pos + 2 * Right, ']');
                    return true;
                }
            }
            if (dir == Up || dir == Down) {
                if (Step(map, pos + dir, dir, out mapAfter) && Step(mapAfter, pos + Right + dir, dir, out mapAfter)) {
                    mapAfter = mapAfter.SetItem(pos, '.');
                    mapAfter = mapAfter.SetItem(pos + Right, '.');
                    mapAfter = mapAfter.SetItem(pos + dir, '[');
                    mapAfter = mapAfter.SetItem(pos + dir + Right, ']');
                    return true;
                }
            }
        }
        mapAfter = map;
        return false;
    }

    Map ScaleUp(Map map) {
        var height = map.Keys.Max(k => (int)Math.Abs(k.Imaginary)) + 1;
        var width = map.Keys.Max(k => (int)Math.Abs(k.Real)) + 1;
        var res = new Dictionary<Complex, char>();

        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                var c = map[x + y * Down];
                res[2 * x + y * Down] = c == 'O' ? '[' : c;
                res[2 * x + 1 + y * Down] = c == '@' ? '.' : c == 'O' ? ']' : c;
            }
        }
        return res.ToImmutableDictionary();
    }
    (Map, string) Parse(string input) {
        var blocks = input.Split("\n\n");
        var lines = blocks[0].Split("\n");
        var map = (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Down, lines[y][x])
        ).ToImmutableDictionary();
        var movements = blocks[1].ReplaceLineEndings("");
        return (map, movements);
    }
}
