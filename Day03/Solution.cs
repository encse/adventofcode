using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2017.Day03 {

    class Solution : Solver {

        public void Solve(string input) {
            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
        }

        int PartOne(string input) {
            var targetValue = int.Parse(input);
            var (x, y) = SpiralCoordinates().ElementAt(targetValue - 1);
            return Math.Abs(x) + Math.Abs(y);
        }

        int PartTwo(string input) {
            var targetValue = int.Parse(input);
            var mem = new int[1000, 1000];
            Action<int, int, int> setMem = (x, y, t) => mem[x + 500, y + 500] = t;
            Func<int, int, int> getMem = (x, y) => mem[x + 500, y + 500];

            setMem(0, 0, 1);

            foreach (var (x, y) in SpiralCoordinates()) {
                var v = (
                    from dx in new[] { -1, 0, 1 }
                    from dy in new[] { -1, 0, 1 }
                    select getMem(x + dx, y + dy)
                ).Sum();

                setMem(x, y, v);

                if (v > targetValue) {
                    return v;
                }
            }
            throw new Exception("never");
        }

        IEnumerable<(int, int)> SpiralCoordinates() {
            var (edgeLength, nextEdgeLength) = (1, 1);
            var (x, y) = (0, 0);
            var (dx, dy) = (1, 0);
            var stepsOnEdge = 0;

            while (true) {
                yield return (x, y);

                if (stepsOnEdge == edgeLength) {
                    (dx, dy) = (-dy, dx);
                    (edgeLength, nextEdgeLength) = (nextEdgeLength, nextEdgeLength + (nextEdgeLength == edgeLength ? 1 : 0));
                    stepsOnEdge = 0;
                }
                x += dx;
                y -= dy;
                stepsOnEdge += 1;
            }
        }
    }
}
