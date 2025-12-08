namespace AdventOfCode.Y2025.Day08;

using System.Collections.Generic;
using System.Data;
using System.Linq;

record Point(decimal x, decimal y, decimal z);

[ProblemName("Playground")]
class Solution : Solver {

    public object PartOne(string input) {
        // Apply 1000 steps of Kruskal's algorithm to the points using the specified
        // metric, then return the product of the sizes of the three largest components.
        var points = Parse(input);
        var setOf = points.ToDictionary(p => p, p => new HashSet<Point>([p]));
        foreach (var (a, b) in GetOrderedPairs(points).Take(1000)) {
            if (setOf[a] != setOf[b]) {
                Connect(a, b, setOf);
            }
        }
        return setOf.Values.Distinct()
            .OrderByDescending(set => set.Count)
            .Take(3)
            .Aggregate(1, (a, b) => a * b.Count);
    }

    public object PartTwo(string input) {
        // Run Kruskal's algorithm on all points and return the product of the
        // x-coordinates of the last edge added to the spanning tree.
        var points = Parse(input);
        var componentCount = points.Length;
        var setOf = points.ToDictionary(p => p, p => new HashSet<Point>([p]));
        var res = 0m;
        foreach (var (a, b) in GetOrderedPairs(points).TakeWhile(_ => componentCount > 1)) {
            if (setOf[a] != setOf[b]) {
                Connect(a, b, setOf);
                res = a.x * b.x;
                componentCount--;
            }
        }
        return res;
    }

    void Connect(Point a, Point b, Dictionary<Point, HashSet<Point>> setOf) {
        setOf[a].UnionWith(setOf[b]);
        foreach (var p in setOf[b]) {
            setOf[p] = setOf[a];
        }
    }

    IEnumerable<(Point a, Point b)> GetOrderedPairs(Point[] points) =>
        from a in points
        from b in points
        where (a.x, a.y, a.z).CompareTo((b.x, b.y, b.z)) < 0
        orderby Metric(a,b)
        select (a, b);

    decimal Metric(Point a, Point b) =>
        (a.x - b.x) * (a.x - b.x) +
        (a.y - b.y) * (a.y - b.y) +
        (a.z - b.z) * (a.z - b.z);

    Point[] Parse(string input) => (
        from line in input.Split("\n")
        let parts = line.Split(",").Select(int.Parse).ToArray()
        select new Point(parts[0], parts[1], parts[2])
    ).ToArray();
}