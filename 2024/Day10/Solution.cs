namespace AdventOfCode.Y2024.Day10;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

using Map = System.Collections.Immutable.ImmutableDictionary<System.Numerics.Complex, char>;

[ProblemName("Hoof It")]
class Solution : Solver {

    Complex Up = Complex.ImaginaryOne;
    Complex Down = -Complex.ImaginaryOne;
    Complex Left = -1;
    Complex Right = 1;

    public object PartOne(string input) => GetAllTrails(input).Sum(x => x.Value.Distinct().Count());
    public object PartTwo(string input) => GetAllTrails(input).Sum(x => x.Value.Count());

    Dictionary<Complex, List<Complex>> GetAllTrails(string input) {
        var map = GetMap(input);
        var trailHeads = map.Keys.Where(k => map[k] == '0');
        return GetTrailHeads(map).ToDictionary(x => x, trailHead => GetTrailsFrom(map, trailHead));
    }

    IEnumerable<Complex> GetTrailHeads(Map map) => map.Keys.Where(k => map[k] == '0');

    List<Complex> GetTrailsFrom(Map map, Complex trailHead) {
        // standard floodfill algorithm with a queue
        var points = new Queue<Complex>();
        points.Enqueue(trailHead);
        var trails = new List<Complex>();
        while (points.Any()) {
            var point = points.Dequeue();
            if (map[point] == '9') {
                trails.Add(point);
            } else {
                foreach (var dir in new[] { Up, Down, Left, Right }) {
                    if (map.GetValueOrDefault(point + dir) == map[point] + 1) {
                        points.Enqueue(point + dir);
                    }
                }
            }
        }
        return trails;
    }

    // store the points in a dictionary so that we can iterate over them and 
    // to easily deal with points outside the area using GetValueOrDefault
    Map GetMap(string input) {
        var map = input.Split("\n");
        return (
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Down, map[y][x])
        ).ToImmutableDictionary();
    }
}