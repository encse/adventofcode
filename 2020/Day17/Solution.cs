using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day17 {

    [ProblemName("Conway Cubes")]
    class Solution : Solver {

        private IEnumerable<(int, int, int)> Neighbours((int x, int y, int z) p) {
            foreach (var dx in new[] { -1, 0, 1 }) {
                foreach (var dy in new[] { -1, 0, 1 }) {
                    foreach (var dz in new[] { -1, 0, 1 }) {
                        if (dx != 0 || dy != 0 || dz != 0) {
                            yield return (p.x + dx, p.y + dy, p.z + dz);
                        }
                    }
                }
            }
        }

        private IEnumerable<(int, int, int, int)> Neighbours2((int x, int y, int z, int w) p) {
            foreach (var dx in new[] { -1, 0, 1 }) {
                foreach (var dy in new[] { -1, 0, 1 }) {
                    foreach (var dz in new[] { -1, 0, 1 }) {
                        foreach (var dw in new[] { -1, 0, 1 }) {
                            if (dx != 0 || dy != 0 || dz != 0 || dw != 0) {
                                yield return (p.x + dx, p.y + dy, p.z + dz, p.w + dw);
                            }
                        }
                    }
                }
            }
        }
        public object PartOne(string input) {
            var m = new HashSet<(int x, int y, int z)>();
            var lines = input.Split("\n");
            var (height, width) = (lines.Length, lines[0].Length);
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    if (lines[y][x] == '#') {
                        m.Add((x, y, 0));
                    }
                }
            }
            for (var i = 0; i < 6; i++) {
                var newM = new HashSet<(int x, int y, int z)>();
                foreach (var p in m) {
                    var activeNeighbours = Neighbours(p).Count(n => m.Contains(n));
                    if (activeNeighbours == 2 || activeNeighbours == 3) {
                        newM.Add(p);
                    }
                    foreach (var pT in Neighbours(p)) {
                        if (!m.Contains(pT)) {
                            activeNeighbours = Neighbours(pT).Count(n => m.Contains(n));
                            if (activeNeighbours == 3) {
                                newM.Add(pT);
                            }
                        }
                    }
                }
                m = newM;
            }
            return m.Count();
        }

        public object PartTwo(string input) {
             var m = new HashSet<(int x, int y, int z, int w)>();
            var lines = input.Split("\n");
            var (height, width) = (lines.Length, lines[0].Length);
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    if (lines[y][x] == '#') {
                        m.Add((x, y, 0, 0));
                    }
                }
            }
            for (var i = 0; i < 6; i++) {
                var newM = new HashSet<(int x, int y, int z, int w)>();
                foreach (var p in m) {
                    var activeNeighbours = Neighbours2(p).Count(n => m.Contains(n));
                    if (activeNeighbours == 2 || activeNeighbours == 3) {
                        newM.Add(p);
                    }
                    foreach (var pT in Neighbours2(p)) {
                        if (!m.Contains(pT)) {
                            activeNeighbours = Neighbours2(pT).Count(n => m.Contains(n));
                            if (activeNeighbours == 3) {
                                newM.Add(pT);
                            }
                        }
                    }
                }
                m = newM;
            }
            return m.Count();
        }
    }
}