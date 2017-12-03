using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2017.Day03 {

    class Solution : Solver {

        public void Solve(string input) {
            var targetValue = int.Parse(input);
            Console.WriteLine(PartOne(targetValue));
            Console.WriteLine(PartTwo(targetValue));
        }

        int PartOne(int input) {
            var (x, y) = SpiralCoordinates().ElementAt(input - 1);
            return Math.Abs(x) + Math.Abs(y);
        }

        int PartTwo(int input) {
            return SpiralSums().First(v => v > input);
        }

        IEnumerable<int> SpiralSums() {
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
                yield return v;
            }
        }

        IEnumerable<(int, int)> SpiralCoordinates() {
            var (x, y) = (0, 0);
            var (dx, dy) = (1, 0);

            for (var edgeLength = 1; ; edgeLength++) {
                for (var run = 0; run < 2; run++) {
                    for (var step = 0; step < edgeLength; step++) {
                        yield return (x, y);

                        x += dx;
                        y -= dy;
                    }
                    (dx, dy) = (-dy, dx);
                }
            }
        }
    }
}
