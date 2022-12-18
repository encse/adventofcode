using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2022.Day18;

[ProblemName("Boiling Boulders")]
class Solution : Solver {

    record class Point(int x, int y, int z);
    public object PartOne(string input) {
        var points = input.Split("\n").Select(line =>
            line.Split(",").Select(int.Parse).ToArray() switch {
                [var x, var y, var z] => new Point(x, y, z),
                _ => throw new ArgumentException()
            }
        ).ToArray();

        var res = 6 * points.Length;
        foreach (var ptA in points) {
            foreach (var ptB in points) {
                var d = Math.Abs(ptA.x - ptB.x) + Math.Abs(ptA.y - ptB.y) + Math.Abs(ptA.z - ptB.z);
                if (d == 1) {
                    res--;
                }
            }
        }

        return res;
    }

    public object PartTwo(string input) {

        var points = input.Split("\n").Select(line =>
            line.Split(",").Select(int.Parse).ToArray() switch {
                [var x, var y, var z] => new Point(x, y, z),
                _ => throw new ArgumentException()
            }
        ).ToHashSet();

        var minX = points.Select(p => p.x).Min() - 2;
        var maxX = points.Select(p => p.x).Max() + 2;

        var minY = points.Select(p => p.y).Min() - 2;
        var maxY = points.Select(p => p.y).Max() + 2;

        var minZ = points.Select(p => p.z).Min() - 2;
        var maxZ = points.Select(p => p.z).Max() + 2;

        var boundsA = new Point(minX, minY, minZ);
        var boundsB = new Point(maxX, maxY, maxZ);

        var q = new Queue<Point>();
        var seen = new HashSet<Point>();
        q.Enqueue(boundsA);
        seen.Add(boundsA);

        while (q.Any()) {
            var water = q.Dequeue();
            foreach (var direction in Directions(water)) {
                if (!seen.Contains(direction) && Within(boundsA, boundsB, direction) && !points.Contains(direction) ) {
                    seen.Add(direction);
                    if (seen.Count % 100 == 0) {
                        Console.WriteLine(seen.Count);
                    }

                    q.Enqueue(direction);
                }
            }

        }

        var res = 0;
        foreach (var pt in points) {
            foreach (var direction in Directions(pt)) {
                if (seen.Contains(direction)) {
                    res++;
                }
            }
        }
        return res;
    }

    bool Within(Point boundsA, Point boundsB, Point point) {
        return
            boundsA.x <= point.x && point.x <= boundsB.x &&
            boundsA.y <= point.y && point.y <= boundsB.y &&
            boundsA.z <= point.z && point.z <= boundsB.z;
    }
    IEnumerable<Point> Directions(Point point) {
        yield return point with { x = point.x - 1 };
        yield return point with { x = point.x + 1 };
        yield return point with { y = point.y - 1 };
        yield return point with { y = point.y + 1 };
        yield return point with { z = point.z - 1 };
        yield return point with { z = point.z + 1 };
    }
}
