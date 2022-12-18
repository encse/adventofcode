using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day18;

[ProblemName("Boiling Boulders")]
class Solution : Solver {

    record class Point(int x, int y, int z);
    record class Bounds(Point min, Point max);

    public object PartOne(string input) {
        var lavaLocations = GetLavaLocations(input).ToHashSet();
        return lavaLocations.SelectMany(Neighbours).Count(p => !lavaLocations.Contains(p));
    }

    public object PartTwo(string input) {
        var lavaLocations = GetLavaLocations(input).ToHashSet();
        var bounds = GetBounds(lavaLocations);
        var waterLocations = FillWithWater(bounds.min, bounds, lavaLocations);
        return lavaLocations.SelectMany(Neighbours).Count(p => waterLocations.Contains(p));
    }

    // fills a region with water starting from the given point and avoiding lavalLocations
    // standard flood fill algorith,
    HashSet<Point> FillWithWater(Point from, Bounds bounds, HashSet<Point> lavaLocations) {
        var result = new HashSet<Point>();
        var q = new Queue<Point>();

        result.Add(from);
        q.Enqueue(from);
        while (q.Any()) {
            var water = q.Dequeue();
            foreach (var neighbour in Neighbours(water)) {
                if (!result.Contains(neighbour) && 
                    Within(bounds, neighbour) && 
                    !lavaLocations.Contains(neighbour)
                ) {
                    result.Add(neighbour);
                    q.Enqueue(neighbour);
                }
            }
        }

        return result;
    }

    IEnumerable<Point> GetLavaLocations(string input) =>
        from line in input.Split("\n")
        let coords = line.Split(",").Select(int.Parse).ToArray()
        select new Point(coords[0], coords[1], coords[2]);

    // returns the enclosing box of a point set, the min and max values are padded by one
    Bounds GetBounds(IEnumerable<Point> points) {
        var minX = points.Select(p => p.x).Min() - 1;
        var maxX = points.Select(p => p.x).Max() + 1;

        var minY = points.Select(p => p.y).Min() - 1;
        var maxY = points.Select(p => p.y).Max() + 1;

        var minZ = points.Select(p => p.z).Min() - 1;
        var maxZ = points.Select(p => p.z).Max() + 1;

        return new Bounds(new Point(minX, minY, minZ), new Point(maxX, maxY, maxZ));
    }

    bool Within(Bounds bounds, Point point) =>
        bounds.min.x <= point.x && point.x <= bounds.max.x &&
        bounds.min.y <= point.y && point.y <= bounds.max.y &&
        bounds.min.z <= point.z && point.z <= bounds.max.z;

    IEnumerable<Point> Neighbours(Point point) =>
        new[]{
           point with { x = point.x - 1 },
           point with { x = point.x + 1 },
           point with { y = point.y - 1 },
           point with { y = point.y + 1 },
           point with { z = point.z - 1 },
           point with { z = point.z + 1 }
        };
}
