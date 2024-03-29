﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Day11;

[ProblemName("Hex Ed")]
class Solution : Solver {

    public object PartOne(string input) => Distances(input).Last();

    public object PartTwo(string input) => Distances(input).Max();

    IEnumerable<int> Distances(string input) => 
        from w in Wander(input) select (Math.Abs(w.x) + Math.Abs(w.y) + Math.Abs(w.z))/2;

    IEnumerable<(int x, int y, int z)> Wander(string input) {
        var (x, y, z) = (0, 0, 0);
        foreach (var dir in input.Split(',')) {
            switch (dir) {
                case "n":  (x, y, z) = (x + 0, y + 1, z - 1); break;
                case "ne": (x, y, z) = (x + 1, y + 0, z - 1); break;
                case "se": (x, y, z) = (x + 1, y - 1, z + 0); break;
                case "s":  (x, y, z) = (x + 0, y - 1, z + 1); break;
                case "sw": (x, y, z) = (x - 1, y + 0, z + 1); break;
                case "nw": (x, y, z) = (x - 1, y + 1, z + 0); break;
                default: throw new ArgumentException(dir);
            }
            yield return (x, y, z);
        }
    }
}
