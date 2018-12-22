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
            var lines = input.Split("\n");
            var depth = Regex.Matches(lines[0], @"\d+").Select(x => int.Parse(x.Value)).Single();
            var target = Regex.Matches(lines[1], @"\d+").Select(x => int.Parse(x.Value)).ToArray();
            var (targetX, targetY) = (target[0], target[1]);
            var m = 20183;

            var erosionLevel = new int[targetX + 1, targetY + 1];
            for (var x = 0; x <= targetX; x++) {
                if (x * 16807 < 0) {
                    throw new Exception();
                }
                erosionLevel[x, 0] = ((x * 16807) + depth) % m;
            }

            for (var y = 0; y <= targetY; y++) {
                if (y * 48271 < 0) {
                    throw new Exception();
                }
                erosionLevel[0, y] = ((y * 48271) + depth) % m;
            }

            for (var y = 1; y <= targetY; y++) {
                for (var x = 1; x <= targetX; x++) {

                    if (erosionLevel[x, y - 1] * erosionLevel[x - 1, y] < 0) {
                        throw new Exception();
                    }
                    erosionLevel[x, y] = ((erosionLevel[x, y - 1] * erosionLevel[x - 1, y]) + depth) % m;
                }
            }

            erosionLevel[targetX, targetY] = depth;

            var regionType = new int[targetX + 1, targetY + 1];
            for (var y = 0; y <= targetY; y++) {
                for (var x = 0; x <= targetX; x++) {
                    regionType[x, y] = erosionLevel[x, y] % 3;
                }
            }

            var riskLevel = 0;

            for (var y = 0; y <= targetY; y++) {
                for (var x = 0; x <= targetX; x++) {
                    riskLevel += regionType[x, y];
                }
            }
            return riskLevel;
        }

        string Tsto(int[,] m) {
            var sb = new StringBuilder();
            foreach (var irow in Enumerable.Range(0, m.GetLength(1))) {
                foreach (var icol in Enumerable.Range(0, m.GetLength(0))) {
                    sb.Append(".=|"[m[icol, irow]]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        int PartTwo(string input) {
            return 0;
        }
    }
}