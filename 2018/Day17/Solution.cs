using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day17 {

    class Solution : Solver {

        public string GetName() => "Reservoir Research";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Regex.Matches(Fill(input), "[~|]").Count;
        int PartTwo(string input) => Regex.Matches(Fill(input), "[~]").Count;

        string Fill(string input) {
            var (width, height) = (2000, 2000);
            var mtx = new char[width, height];

            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    mtx[x, y] = '.';
                }
            }

            foreach (var line in input.Split("\n")) {
                var nums = Regex.Matches(line, @"\d+").Select(g => int.Parse(g.Value)).ToArray();
                for (var i = nums[1]; i <= nums[2]; i++) {
                    if (line.StartsWith("x")) {
                        mtx[nums[0], i] = '#';
                    } else {
                        mtx[i, nums[0]] = '#';
                    }
                }
            }
            FillRecursive(mtx, 500, 0);

            var (minY, maxY) = (int.MaxValue, int.MinValue);
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    if (mtx[x, y] == '#') {
                        minY = Math.Min(minY, y);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }
            var sb = new StringBuilder();
            for (var y = minY; y <= maxY; y++) {
                for (var x = 0; x < width; x++) {
                    sb.Append(mtx[x, y]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        void FillRecursive(char[,] mtx, int x, int y) {
            var width = mtx.GetLength(0);
            var height = mtx.GetLength(1);
            if (mtx[x, y] != '.') {
                return;
            }
            mtx[x, y] = '|';
            if (y == height - 1) {
                return ;
            }
            FillRecursive(mtx, x, y + 1);

            if (mtx[x, y + 1] == '#' || mtx[x, y + 1] == '~') {
                if (x > 0) {
                    FillRecursive(mtx, x - 1, y);
                }
                if (x < width - 1) {
                    FillRecursive(mtx, x + 1, y);
                }
            }

            if (IsStill(mtx, x, y)) {
                foreach (var dx in new[] { -1, 1 }) {
                    for (var xT = x; xT >= 0 && xT < width && mtx[xT, y] == '|'; xT += dx) {
                        mtx[xT, y] = '~';
                    }
                }
            }
        }

        bool IsStill(char[,] mtx, int x, int y) {
            var width = mtx.GetLength(0);
            foreach (var dx in new[] { -1, 1 }) {
                for (var xT = x; xT >= 0 && xT < width && mtx[xT, y] != '#'; xT += dx) {
                    if (mtx[xT, y] == '.' || mtx[xT, y + 1] == '|') {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}