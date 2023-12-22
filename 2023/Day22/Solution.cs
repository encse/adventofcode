namespace AdventOfCode.Y2023.Day22;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

// WIP

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
        var cubes = Fall(ParseCubes(input));
        var (supportedBy, supportsOf) = Analyze(cubes);
        var res = 0;
        foreach (var cube in cubes) {
            res += Boom(cube, supportedBy, supportsOf);
        }
        return res;
    }

    public object PartOne(string input) {
        var cubes = ParseCubes(input);
        cubes = cubes.OrderBy(cube => cube.Bottom).ToArray();
        var (supportedBy, supportsOf) = Analyze(cubes);
        return cubes.Count(c => supportedBy(c).All(c2 => supportsOf(c2).Count() > 1));
    }

    int Boom(Cube cube, Func<Cube, IEnumerable<Cube>> supportedBy, Func<Cube, IEnumerable<Cube>> supportsOf) {
        var q = new Queue<Cube>();
        q.Enqueue(cube);
        var affected = new HashSet<Cube>();
        while (q.Any()) {
            cube = q.Dequeue();
            affected.Add(cube);
            var falls = supportedBy(cube).
                Where(c => supportsOf(c).All(support => affected.Contains(support)));
            foreach (var cubeT in falls) {
                q.Enqueue(cubeT);
            }
        }
        return affected.Count - 1;
    }

    Cube[] Fall(Cube[] cubes) {
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
        return cubes;
    }

    (Func<Cube, IEnumerable<Cube>> supportedBy, Func<Cube, IEnumerable<Cube>> supportsOf) Analyze(Cube[] cubes) {

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

        var supportedByT = cubes.ToDictionary(
            cube => cube,
            cube => supportList.Where(s => s.lo == cube).Select(s => s.hi).ToArray()
        );

        var supportsOfT = cubes.ToDictionary(
            cube => cube,
            cube => supportList.Where(s => s.hi == cube).Select(s => s.lo).ToArray()
        );


        var supportedBy = (Cube c) => supportedByT[c];
        var supportsOf = (Cube c) => supportsOfT[c];
        return (supportedBy, supportsOf);
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