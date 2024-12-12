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

    public object PartOne(string input) => CalculateFencePrice(input, FindEdges);

    public object PartTwo(string input) => CalculateFencePrice(input, FindCorners);

    int CalculateFencePrice(string input, MeasurePerimeter measure){
        var regions = GetRegions(input);
        var res = 0;
        foreach (var region in regions.Values.Distinct()) {
            var perimeter = 0;
            foreach (var pt in region) {
                perimeter += measure(regions, pt);
            }
            res += region.Count() * perimeter;
        }
        return res;
    }    
   
    delegate int MeasurePerimeter(Dictionary<Complex, Region> map, Complex pt);

    int FindEdges(Dictionary<Complex, Region> map, Complex pt) {
        var res = 0;
        var region = map[pt];
        foreach (var du in new[] { Right, Down, Left, Up}) {
            // xy 
            if (map.GetValueOrDefault(pt + du) != region) {
                res++;
            }
        }
        return res;
    }

    int FindCorners(Dictionary<Complex, Region> map, Complex pt) {
        var res = 0;
        var region = map[pt];
        
        // rotate du and dv and check for the 4 corner types
        foreach (var (du, dv) in new[] { (Up, Right), (Right, Down), (Down, Left), (Left, Up) }) {
            //  yy
            //  xy
            if (map.GetValueOrDefault(pt + du) != region &&  map.GetValueOrDefault(pt + dv) != region) {
                res++; 
            }

            //  xy
            //  xx
            if (map.GetValueOrDefault(pt + du) == region && map.GetValueOrDefault(pt + dv) == region &&  map.GetValueOrDefault(pt + du + dv) != region) {
                res++;
            }
        }
        return res;
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

        // go over the positions of the garden and use a floodfill to determine the region
        var res = new Dictionary<Complex, Region>();
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

}