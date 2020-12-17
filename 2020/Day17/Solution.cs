using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day17 {

    [ProblemName("Conway Cubes")]
    class Solution : Solver {
        public object PartOne(string input) {
            var ds = (from dx in new[] { -1, 0, 1 }
                      from dy in new[] { -1, 0, 1 }
                      from dz in new[] { -1, 0, 1 }
                      where dx != 0 || dy != 0 || dz != 0
                      select (dx, dy, dz)).ToArray();
            return Solve(
                input,
                (x, y) => (x: x, y: y, z: 0),
                (p) => ds.Select(d => (p.x + d.dx, p.y + d.dy, p.z + d.dz)));
        }

        public object PartTwo(string input) {
            var ds = (from dx in new[] { -1, 0, 1 }
                      from dy in new[] { -1, 0, 1 }
                      from dz in new[] { -1, 0, 1 }
                      from dw in new[] { -1, 0, 1 }
                      where dx != 0 || dy != 0 || dz != 0 || dw != 0
                      select (dx, dy, dz, dw)).ToArray();

            return Solve(
                input,
                (x, y) => (x: x, y: y, z: 0, w: 0),
                (p) => ds.Select(d => (p.x + d.dx, p.y + d.dy, p.z + d.dz, p.w + d.dw)));
        }

        private int Solve<T>(string input, Func<int, int, T> create, Func<T, IEnumerable<T>> neighbours) {
            var activePoints = new HashSet<T>();
            var lines = input.Split("\n");
            var (height, width) = (lines.Length, lines[0].Length);
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    if (lines[y][x] == '#') {
                        activePoints.Add(create(x, y));
                    }
                }
            }
            for (var i = 0; i < 6; i++) {
                var newActivePoints = new HashSet<T>();
                var inactivePoints = new Dictionary<T, int>();

                foreach (var point in activePoints) {
                    var activeNeighbours = 0;
                    foreach (var neighbour in neighbours(point)) {
                        if (activePoints.Contains(neighbour)) {
                            activeNeighbours++;
                        } else {
                            inactivePoints[neighbour] = inactivePoints.GetValueOrDefault(neighbour) + 1;
                        }
                    }

                    if (activeNeighbours == 2 || activeNeighbours == 3) {
                        newActivePoints.Add(point);
                    }
                }

                foreach (var (point, activeNeighbours) in inactivePoints) {
                    if (activeNeighbours == 3) {
                        newActivePoints.Add(point);
                    }
                }
                activePoints = newActivePoints;
            }
            return activePoints.Count();
        }
    }
}