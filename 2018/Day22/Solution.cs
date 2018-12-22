using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day22 {

    class Solution : Solver {

        public string GetName() => "Mode Maze";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var (targetX, targetY, regionType) = Parse(input);
            var riskLevel = 0;
            for (var y = 0; y <= targetY; y++) {
                for (var x = 0; x <= targetX; x++) {
                    riskLevel += (int)regionType(x, y);
                }
            }
            return riskLevel;
        }

        int PartTwo(string input) {
            var (targetX, targetY, regionType) = Parse(input);
            var q = new PQueue<((int x, int y) pos, Tool tool)>();
            var seen = new HashSet<((int x, int y), Tool tool)>();

            q.Enqueue(0, ((0, 0), Tool.Torch));

            while (q.Any()) {
                var (t, state) = q.Dequeue();
                var (pos, tool) = state;
                if (pos.x == targetX && pos.y == targetY && tool == Tool.Torch) {
                    return t;
                }

                if (seen.Contains(state)) {
                    continue;
                }

                seen.Add(state);

                switch (regionType(pos.x, pos.y)) {
                    case RegionType.Rocky:
                        q.Enqueue(t + 7, (pos, tool == Tool.ClimbingGear ? Tool.Torch : Tool.ClimbingGear));
                        break;
                    case RegionType.Narrow:
                        q.Enqueue(t + 7, (pos, tool == Tool.Torch ? Tool.Nothing : Tool.Torch));
                        break;
                    case RegionType.Wet:
                        q.Enqueue(t + 7, (pos, tool == Tool.ClimbingGear ? Tool.Nothing : Tool.ClimbingGear));
                        break;
                }

                foreach (var dx in new[] { -1, 0, 1 }) {
                    foreach (var dy in new[] { -1, 0, 1 }) {
                        if (Math.Abs(dx) + Math.Abs(dy) != 1) {
                            continue;
                        }

                        var posNew = (x: pos.x + dx, y: pos.y + dy);
                        if (posNew.x < 0 || posNew.y < 0) {
                            continue;
                        }

                        switch (regionType(posNew.x, posNew.y)) {
                            case RegionType.Rocky:
                                if (tool == Tool.ClimbingGear || tool == Tool.Torch) {
                                    q.Enqueue(t + 1, (posNew, tool));
                                }
                                break;
                            case RegionType.Narrow:
                                if (tool == Tool.Torch || tool == Tool.Nothing) {
                                    q.Enqueue(t + 1, (posNew, tool));
                                }
                                break;
                            case RegionType.Wet:

                                if (tool == Tool.ClimbingGear || tool == Tool.Nothing) {
                                    q.Enqueue(t + 1, (posNew, tool));
                                }

                                break;
                        }
                    }
                }
            }

            throw new Exception();
        }

        (int targetX, int targetY, Func<int, int, RegionType> regionType) Parse(string input) {
            var lines = input.Split("\n");
            var depth = Regex.Matches(lines[0], @"\d+").Select(x => int.Parse(x.Value)).Single();
            var target = Regex.Matches(lines[1], @"\d+").Select(x => int.Parse(x.Value)).ToArray();
            var (targetX, targetY) = (target[0], target[1]);

            var m = 20183;

            var erosionLevelCache = new Dictionary<(int, int), int>();
            int erosionLevel(int x, int y) {
                var key = (x, y);
                if (!erosionLevelCache.ContainsKey(key)) {
                    if (x == targetX && y == targetY) {
                        erosionLevelCache[key] = depth;
                    } else if (x == 0 && y == 0) {
                        erosionLevelCache[key] = depth;
                    } else if (x == 0) {
                        erosionLevelCache[key] = ((y * 48271) + depth) % m;
                    } else if (y == 0) {
                        erosionLevelCache[key] = ((x * 16807) + depth) % m;
                    } else {
                        erosionLevelCache[key] = ((erosionLevel(x, y - 1) * erosionLevel(x - 1, y)) + depth) % m;
                    }
                }
                return erosionLevelCache[key];
            }

            RegionType regionType(int x, int y) {
                return (RegionType)(erosionLevel(x, y) % 3);
            }

            return (targetX, targetY, regionType);
        }
    }
    enum RegionType {
        Rocky = 0,
        Wet = 1,
        Narrow = 2
    }

    enum Tool {
        Nothing,
        Torch,
        ClimbingGear
    }

    class PQueue<T> {
        SortedDictionary<int, Queue<T>> d = new SortedDictionary<int, Queue<T>>();
        public bool Any() {
            return d.Any();
        }

        public void Enqueue(int p, T t) {
            if (!d.ContainsKey(p)) {
                d[p] = new Queue<T>();
            }
            d[p].Enqueue(t);
        }

        public (int p, T t) Dequeue() {
            var p = d.Keys.First();
            var items = d[p];
            var t = items.Dequeue();
            if (!items.Any()) {
                d.Remove(p);
            }
            return (p, t);
        }
    }
}