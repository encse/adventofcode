namespace AdventOfCode.Y2023.Day22;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

record Range(int begin, int end);

record Cube(Range x, Range y, Range z) {
    public int Bottom => z.begin;
    public int Top => z.end;
}

record Connections(
    Dictionary<Cube, HashSet<Cube>> cubesAbove,
    Dictionary<Cube, HashSet<Cube>> cubesBelow
);

[ProblemName("Sand Slabs")]
class Solution : Solver {

    public object PartOne(string input) {
        var cubes = ParseCubes(input);
        cubes = Fall(cubes);
        var conn = Analyze(cubes);
        return cubes.Count(cube => Boom(cube, conn) == 0);
    }

    public object PartTwo(string input) {
        var cubes = ParseCubes(input);
        cubes = Fall(cubes);
        var conn = Analyze(cubes);
        return cubes.Select(cube => Boom(cube, conn)).Sum();
    }

    // returns the number of other bricks that would fall when disintegrating 'cube'
    int Boom(Cube cube, Connections conn) {
        var q = new Queue<Cube>();
        q.Enqueue(cube);

        var fallingCubes = new HashSet<Cube>();
        while (q.Any()) {
            cube = q.Dequeue();
            fallingCubes.Add(cube);

            var startsFalling = from cubeT in conn.cubesAbove[cube]
                                let supports = conn.cubesBelow[cubeT]
                                where supports.IsSubsetOf(fallingCubes)
                                select cubeT;

            foreach (var cubeT in startsFalling) {
                q.Enqueue(cubeT);
            }
        }
        return fallingCubes.Count - 1;
    }

    Cube[] Fall(Cube[] cubes) {
        cubes = cubes.OrderBy(cube => cube.Bottom).ToArray();

        for (var i = 0; i < cubes.Length; i++) {
            var cubeI = cubes[i];
            var bottom = 1;
            for (var j = 0; j < i; j++) {
                var cubeJ = cubes[j];
                if (IntersectsXY(cubeI, cubeJ)) {
                    bottom = Math.Max(bottom, cubeJ.Top + 1);
                }
            }

            var fall = cubeI.Bottom - bottom;
            cubes[i] = cubeI with { z = new Range(cubeI.Bottom - fall, cubeI.Top - fall) };
        }
        return cubes;
    }

    Connections Analyze(Cube[] cubes) {
        var cubesAbove = cubes.ToDictionary(cube => cube, _ => new HashSet<Cube>());
        var cubesBelow = cubes.ToDictionary(cube => cube, _ => new HashSet<Cube>());
        for (var i = 0; i < cubes.Length; i++) {
            var cubeLo = cubes[i];
            for (var j = i + 1; j < cubes.Length; j++) {
                var cubeHi = cubes[j];
                if (cubeLo.Top == cubeHi.Bottom - 1 && IntersectsXY(cubeHi, cubeLo)) {
                    cubesBelow[cubeHi].Add(cubeLo);
                    cubesAbove[cubeLo].Add(cubeHi);
                }
            }
        }
        return new Connections(cubesAbove, cubesBelow);
    }

    bool IntersectsXY(Cube cubeA, Cube cubeB) =>
        Intersects(cubeA.x, cubeB.x) && Intersects(cubeA.y, cubeB.y);

    // see https://stackoverflow.com/a/3269471
    bool Intersects(Range r1, Range r2) =>
        r1.begin <= r2.end && r2.begin <= r1.end;

    Cube[] ParseCubes(string input) => (
        from p in Enumerable.Zip(input.Split('\n'), Enumerable.Range(1, 100000))
        let line = p.First
        let id = p.Second
        let v = (from m in Regex.Matches(line, @"\d+") select int.Parse(m.Value)).ToArray()
        select new Cube(new Range(v[0], v[3]), new Range(v[1], v[4]), new Range(v[2], v[5]))
    ).ToArray();
}