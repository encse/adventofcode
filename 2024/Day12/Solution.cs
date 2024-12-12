namespace AdventOfCode.Y2024.Day12;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

using Region = System.Collections.Generic.HashSet<System.Numerics.Complex>;
using System;

[ProblemName("Garden Groups")]
class Solution : Solver {

    Complex Up = Complex.ImaginaryOne;
    Complex Down = -Complex.ImaginaryOne;
    Complex Left = -1;
    Complex Right = 1;

    public object PartOne(string input) => CalculateFencePrice(input, discounted: false);
    public object PartTwo(string input) => CalculateFencePrice(input, discounted: true);

    int CalculateFencePrice(string input, bool discounted) {
        var regions = GetRegions(input);
        var perimeters = GetPerimeters(regions, discounted);
        return regions.Values.Distinct().Sum(region => region.Count * perimeters[region]);
    }

    // Maps the positions of plants in a garden to their corresponding regions, grouping plants 
    // of the same type into contiguous regions.
    Dictionary<Complex, Region> GetRegions(string input) {
        var lines = input.Split("\n");

        var garden = (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Down, lines[y][x])
        ).ToDictionary();

        var res = new Dictionary<Complex, Region>();

        // go over the positions of the garden and use a floodfill to determine the region
        var positions = garden.Keys.ToHashSet();
        while (positions.Any()) {
            var pivot = positions.First();
            var region = new Region { pivot };

            var q = new Queue<Complex>();
            q.Enqueue(pivot);

            var plant = garden[pivot];

            while (q.Any()) {
                var point = q.Dequeue();
                res[point] = region;
                positions.Remove(point);
                foreach (var dir in new[] { Up, Down, Left, Right }) {
                    if (!region.Contains(point + dir) && garden.GetValueOrDefault(point + dir) == plant) {
                        region.Add(point + dir);
                        q.Enqueue(point + dir);
                    }
                }
            }
        }
        return res;
    }

    // Calculates the perimeters of all regions at once (in segments or regular units)
    Dictionary<Region, int> GetPerimeters(Dictionary<Complex, Region> map, bool segemented) {
        var perimeters = new Dictionary<Region, int>();
        foreach (var fence in GetFenceSegements(map)) {
            var length = segemented ? 1 : fence.Count();
            var region = map[fence.First()];
            perimeters[region] = perimeters.GetValueOrDefault(region) + length;
        }
        return perimeters;
    }

    // Finds the positions of the straight fence segments
    IEnumerable<IEnumerable<Complex>> GetFenceSegements(Dictionary<Complex, Region> map) {
        // the loop scans the garden four times checking if the given position has a fence up, down, 
        // left or right maintaining when a new fence segment is to be started

        foreach (var (dir, look) in new[] { (Right, Up), (Right, Down), (Down, Left), (Down, Right) }) {
            var fence = new List<Complex>();

            foreach (var line in Scan(map, dir == Right)) {
                foreach (var position in line) {
                    // fence ends when:
                    // - we step into a different region
                    // - the position we are looking at belongs to the same region as our current region.
                    if (map[position] != map.GetValueOrDefault(position - dir) ||
                        map[position] == map.GetValueOrDefault(position + look)
                    ) {
                        if (fence.Any()) {
                            yield return fence;
                        }
                        fence = new List<Complex>();
                    }

                    if (map[position] != map.GetValueOrDefault(position + look)) {
                        fence.Add(position);
                    }
                }
            }

            if (fence.Any()) {
                yield return fence;
            }
        }
    }

    // Returns horizontal or vertical scanlines of a map
    IEnumerable<IEnumerable<Complex>> Scan<T>(Dictionary<Complex, T> map, bool horiz) {
        var du = horiz ? Right : Down;
        var dv = horiz ? Down : Right;

        var pt0 = Complex.Zero;
        while (map.ContainsKey(pt0)) {
            var line = new List<Complex>();
            var pt = pt0;
            while (map.ContainsKey(pt)) {
                line.Add(pt);
                pt += du;
            }

            yield return line;
            pt0 += dv;
        }
    }

}