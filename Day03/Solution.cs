using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2017.Day03 {

    class Solution : Solver {

        public void Solve(string input) {
            var num = int.Parse(input);
            Console.WriteLine(PartOne(num));
            Console.WriteLine(PartTwo(num));
        }

        int PartOne(int num) {
            var (x, y) = SpiralCoordinates().ElementAt(num - 1);
            return Math.Abs(x) + Math.Abs(y);
        }

        int PartTwo(int num) {
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

            foreach (var (x, y) in SpiralCoordinates()) {
                var v = (
                    from dx in new[] { -1, 0, 1 }
                    from dy in new[] { -1, 0, 1 }
                    let key = (x + dx, y + dy)
                    where mem.ContainsKey(key)
                    select mem[key]
                ).Sum();

                mem[(x, y)] = v;
                yield return v;
            }
        }
    }
}
