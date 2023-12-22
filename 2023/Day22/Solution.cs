namespace AdventOfCode.Y2023.Day22;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

record Range(int begin, int end) {
    public bool Containts(int x) {
        return begin <= x && x <= end;
    }
}
record Cube(int id, Range x, Range y, Range z) {
    public int Front => y.begin;
    public int Back => y.end;
    public int Left => x.begin;
    public int Right => x.end;
    public int Bottom => z.begin;
    public int Top => z.end;
    public int Height => Top - Bottom + 1;
    public bool Containts(int x, int y, int z) {
        return this.x.Containts(x) && this.y.Containts(y) && this.z.Containts(z);
    }
}

[ProblemName("Sand Slabs")]
class Solution : Solver {

    public object PartTwo(string input) {
        var cubes = ParseCubes(input);
        (cubes, var _) = Fall(cubes);

        var supportList = new List<(Cube lo, Cube hi)>();
        for (var i = 0; i < cubes.Length; i++) {
            var cubeLo = cubes[i];
            for (var j = 0; j < cubes.Length; j++) {
                var cubeHi = cubes[j];
                if (cubeLo.Top == cubeHi.Bottom - 1 && Above(cubeHi, cubeLo)) {
                    supportList.Add((cubeLo, cubeHi));
                }
            }
        }

        Tsto(cubes);
        var supportedBy = (Cube c) => supportList.Where(s => s.lo == c).Select(s => s.hi);
        var supportsOf = (Cube c) => supportList.Where(s => s.hi == c).Select(s => s.lo);

        var blowsup = new Dictionary<Cube, int>();

        int Boom(Cube cube) {
            if (!blowsup.ContainsKey(cube)) {
                Console.WriteLine(blowsup.Count);
                var (_, falls) = Fall(cubes.Where(c => c != cube).ToArray());
                blowsup[cube] = falls;
                // Console.WriteLine(cube.id + " -> " + blowsup[cube]);
            }
            return blowsup[cube];
        }

        foreach (var cube in cubes) {
            Boom(cube);
        }
        return blowsup.Values.Sum();
    }

    (Cube[], int) Fall(Cube[] cubes) {
        var falls = 0;
        cubes = cubes.OrderBy(cube => cube.Bottom).ToArray();

        for (var i = 0; i < cubes.Length; i++) {
            var cubeI = cubes[i];
            var newBottom = 1;
            for (var j = 0; j < i; j++) {
                var cubeJ = cubes[j];
                if (Above(cubeI, cubeJ)) {
                    newBottom = Math.Max(newBottom, cubeJ.Top + 1);
                }

            }
            var fall = cubeI.Bottom - newBottom;
            if (fall > 0) {
                falls++;
                cubes[i] = cubeI with { z = new Range(cubeI.Bottom - fall, cubeI.Top - fall) };
            }
        }
        return (cubes, falls);
    }

    public object PartOne(string input) {
        var cubes = ParseCubes(input);
        cubes = cubes.OrderBy(cube => cube.Bottom).ToArray();

        for (var i = 0; i < cubes.Length; i++) {
            var cubeI = cubes[i];
            var newBottom = 1;
            for (var j = 0; j < i; j++) {
                var cubeJ = cubes[j];
                if (Above(cubeI, cubeJ)) {
                    newBottom = Math.Max(newBottom, cubeJ.Top + 1);
                }

            }
            var fall = cubeI.Bottom - newBottom;
            cubes[i] = cubeI with { z = new Range(cubeI.Bottom - fall, cubeI.Top - fall) };
        }

        var supportList = new List<(Cube lo, Cube hi)>();
        for (var i = 0; i < cubes.Length; i++) {
            var cubeLo = cubes[i];
            for (var j = 0; j < cubes.Length; j++) {
                var cubeHi = cubes[j];
                if (cubeLo.Top == cubeHi.Bottom - 1 && Above(cubeHi, cubeLo)) {
                    supportList.Add((cubeLo, cubeHi));
                }
            }
        }

        var supportedBy = (Cube c) => supportList.Where(s => s.lo == c).Select(s => s.hi);
        var supportsOf = (Cube c) => supportList.Where(s => s.hi == c).Select(s => s.lo);

        return cubes.Count(c => supportedBy(c).All(c2 => supportsOf(c2).Count() > 1));
    }

    int Tsto(Cube[] cubes) {
        var minX = cubes.Select(c => c.Left).Min();
        var maxX = cubes.Select(c => c.Right).Max();
        var maxZ = cubes.Select(c => c.Top).Max();

        var volume = 0;
        var lines = new List<string>();
        for (var z = 1; z <= maxZ; z++) {
            var line = "";
            for (var x = minX; x <= maxX; x++) {
                var m = cubes.Count(c => c.Containts(x, c.Front, z));
                if (m == 0) {
                    line += ".";
                } else if (m == 1) {
                    var cube = cubes.Single(c => c.Containts(x, c.Front, z));
                    if (cube.id < 10) {
                        line += cube.id;
                    } else {
                        line += "#";
                    }
                } else {
                    line += "?";
                }
                volume += m;
            }
            lines.Insert(0, line);
        }

        Console.WriteLine(string.Join("\n", lines));
        Console.WriteLine();
        return volume;
    }

    bool Above(Cube cubeA, Cube cubeB) {
        return cubeA.Bottom > cubeB.Top &&
          Intersects(cubeA.x, cubeB.x) &&
          Intersects(cubeA.y, cubeB.y);
    }

    // see https://stackoverflow.com/a/3269471
    bool Intersects(Range r1, Range r2) =>
        r1.begin <= r2.end && r2.begin <= r1.end;



    Cube[] ParseCubes(string input) => (
        from p in Enumerable.Zip(input.Split('\n'), Enumerable.Range(1, 100000))
        let line = p.First
        let id = p.Second
        let v = (from m in Regex.Matches(line, @"\d+") select int.Parse(m.Value)).ToArray()
        select new Cube(id, new Range(v[0], v[3]), new Range(v[1], v[4]), new Range(v[2], v[5]))
    ).ToArray();
}