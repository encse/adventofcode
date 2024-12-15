namespace AdventOfCode.Y2024.Day15;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
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

    public object PartOne(string input) {
        var (map, movements) = Parse(input);
        var robot = map.Keys.Single(k => map[k] == '@');
        // Console.WriteLine(Show(map));
        foreach (var m in movements) {
            var dir = move[m];
            var moved = Move(map, robot, dir);
            if (moved.ok) {
                map = moved.map;
                robot += dir;
            }
            // Console.WriteLine(Show(map));
        }
        var res = 0.0;
        foreach (var crate in map.Keys.Where(k => map[k] == 'O')) {
            res += 100 * Math.Abs(crate.Imaginary) + crate.Real;
        }
        return res;
    }

    (bool ok, Map map) Move(Map map, Complex pos, Complex dir) {
        if (map[pos] == '.') {
            return (true, map);
        } else if (map[pos] == '#') {
            return (false, map);
        } else if (map[pos] == 'O' || map[pos] == '@') {
            var (ok, mapT) = Move(map, pos + dir, dir);
            if (ok) {
                map = mapT;
                map = map.SetItem(pos + dir, map[pos]);
                map = map.SetItem(pos, '.');
                return (true, map);
            }
        } else if (map[pos] == ']') {
            return Move(map, pos+Left, dir);
        } else if (map[pos] == '[') {
            if (dir == Left && Move(map, pos + Left, Left).ok)  {
                map = Move(map, pos + Left, Left).map;
                map = map.SetItem(pos+Left, '[');
                map = map.SetItem(pos, ']');
                map = map.SetItem(pos+Right, '.');
                return (true, map);
            }
            if (dir == Right && Move(map, pos + 2*Right, dir).ok)  {
                map = Move(map, pos + 2*Right, dir).map;
                map = map.SetItem(pos, '.');
                map = map.SetItem(pos+Right, '[');
                map = map.SetItem(pos+2*Right, ']');
                return (true, map);
            }
            if (dir == Up || dir == Down) {
                 if (Move(map, pos + dir, dir).ok && Move(map, pos + Right + dir, dir).ok){
                    map = Move(map, pos + dir, dir).map;
                    map = Move(map, pos + Right + dir, dir).map;
                    map = map.SetItem(pos, '.');
                    map = map.SetItem(pos+Right, '.');
                    map = map.SetItem(pos+dir, '[');
                    map = map.SetItem(pos+dir+Right, ']');
                    return (true, map);
                 }
            }
        }
        
        return (false, map);
    }



    public object PartTwo(string input) {
        var (map, movements) = Parse(input);
        map = ScaleUp(map);
        var robot = map.Keys.Single(k => map[k] == '@');
        foreach (var m in movements) {
            var dir = move[m];
            var moved = Move(map, robot, dir);
            if (moved.ok) {
                map = moved.map;
                robot += dir;
            }
        }
        var res = 0.0;
        foreach (var crate in map.Keys.Where(k => map[k] == '[')) {
            res += 100 * Math.Abs(crate.Imaginary) + crate.Real;
        }
        return res;
    }

    string Show(Map map) {
        var height = map.Keys.Max(k => (int)Math.Abs(k.Imaginary)) + 1;
        var width = map.Keys.Max(k => (int)Math.Abs(k.Real)) + 1;
        var res = new char[height, width];
        var sb = new StringBuilder();
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                sb.Append(map[x + y * Down]);
            }
            sb.AppendLine();
        }
        return sb.ToString();
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
