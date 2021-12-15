using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day09;

[ProblemName("Smoke Basin")]
class Solution : Solver {

    public object PartOne(string input) {
        var map = GetMap(input);

        // find the 'low points' and return a hash computed from their heights:
        return GetLowPoints(map).Select(point => 1 + map[point]).Sum();
    }

    public object PartTwo(string input) {
        var map = GetMap(input);

        // find the 3 biggest basins and return a hash computed from their size:
        return GetLowPoints(map)
            .Select(p => BasinSize(map, p))
            .OrderByDescending(basinSize => basinSize)
            .Take(3)
            .Aggregate(1, (m, basinSize) => m * basinSize);
    }

    // store the points in a dictionary so that we can iterate over them and 
    // to easily deal with points outside the area using GetValueOrDefault
    ImmutableDictionary<Point, int> GetMap(string input) {
        var map = input.Split("\n");
        return (
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Point, int>(new Point(x, y), map[y][x] - '0')
        ).ToImmutableDictionary();
    }

    IEnumerable<Point> Neighbours(Point point) => 
        new [] {
           point with {y = point.y + 1},
           point with {y = point.y - 1},
           point with {x = point.x + 1},
           point with {x = point.x - 1},
        };

    public IEnumerable<Point> GetLowPoints(ImmutableDictionary<Point, int>  map) =>
        from point in map.Keys 
        // point is low if each of its neighbours is higher:
        where Neighbours(point).All(neighbour => map[point] < map.GetValueOrDefault(neighbour, 9))
        select point;

    public int BasinSize(ImmutableDictionary<Point, int> map, Point point) {
        // flood fill algorithm
        var filled = new HashSet<Point>{point};
        var queue = new Queue<Point>(filled);

        while (queue.Any()) {
            foreach (var neighbour in Neighbours(queue.Dequeue()).Except(filled)) {
                if (map.GetValueOrDefault(neighbour, 9) != 9) {
                    queue.Enqueue(neighbour);
                    filled.Add(neighbour);
                }
            }
        }
        return filled.Count;
    }
}

record Point(int x, int y);
