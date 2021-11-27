using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Day03;

[ProblemName("Spiral Memory")]
class Solution : Solver {

    public object PartOne(string input) {
        var (x, y) = SpiralCoordinates().ElementAt(int.Parse(input) - 1);
        return Math.Abs(x) + Math.Abs(y);
    }

    public object PartTwo(string input) {
        var num = int.Parse(input);
       return SpiralSums().First(v => v > num);
    }

    IEnumerable<(int, int)> SpiralCoordinates() {
        var (x, y) = (0, 0);
        var (dx, dy) = (1, 0);

        for (var edgeLength = 1; ; edgeLength++) {
            for (var run = 0; run < 2; run++) {
                for (var step = 0; step < edgeLength; step++) {
                    yield return (x, y);
                    (x, y) = (x + dx, y - dy);
                }
                (dx, dy) = (-dy, dx);
            }
        }
    }

    IEnumerable<int> SpiralSums() {
        var mem = new Dictionary<(int, int), int>();
        mem[(0, 0)] = 1;

        foreach (var coord in SpiralCoordinates()) {
            var sum = (from coordT in Window(coord) where mem.ContainsKey(coordT) select mem[coordT]).Sum();
            mem[coord] = sum;
            yield return sum;
        }
    }

    IEnumerable<(int, int)> Window((int x, int y) coord) =>
         from dx in new[] { -1, 0, 1 }
         from dy in new[] { -1, 0, 1 }
         select (coord.x + dx, coord.y + dy);
}
