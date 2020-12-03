using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2018.Day10 {

    [ProblemName("The Stars Align")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        string PartOne(string input) => OCR(Solver(input).mx);

        int PartTwo(string input) => Solver(input).seconds;

        (bool[,] mx, int seconds) Solver(string input) {
            // position=< 21992, -10766> velocity=<-2,  1>
            var rx = new Regex(@"position=\<\s*(?<x>-?\d+),\s*(?<y>-?\d+)\> velocity=\<\s*(?<vx>-?\d+),\s*(?<vy>-?\d+)\>");
            var points = (
                from line in input.Split("\n")
                let m = rx.Match(line)
                select new Point {
                    x = int.Parse(m.Groups["x"].Value),
                    y = int.Parse(m.Groups["y"].Value),
                    vx = int.Parse(m.Groups["vx"].Value),
                    vy = int.Parse(m.Groups["vy"].Value)
                }
            ).ToArray();

            var seconds = 0;
            Func<bool, (int left, int top, long width, long height)> step = (bool forward) => {
                foreach (var point in points) {
                    if (forward) {
                        point.x += point.vx;
                        point.y += point.vy;
                    } else {
                        point.x -= point.vx;
                        point.y -= point.vy;
                    }
                }
                seconds += forward ? 1 : -1;

                var minX = points.Min(pt => pt.x);
                var maxX = points.Max(pt => pt.x);
                var minY = points.Min(pt => pt.y);
                var maxY = points.Max(pt => pt.y);
                return (minX, minY, maxX - minX + 1, maxY - minY + 1);
            };

            var area = long.MaxValue;
            while (true) {

                var rect = step(true);
                var areaNew = (rect.width) * (rect.height);

                if (areaNew > area) {
                    rect = step(false);
                    var mx = new bool[rect.height, rect.width];
                    foreach (var point in points) {
                        mx[point.y - rect.top, point.x - rect.left] = true;
                    }

                    return (mx, seconds);
                }
                area = areaNew;
            }
        }

        string OCR(bool[,] mx) {
            var dict = new Dictionary<long, string>{
                {0x384104104145138, "J"},
                {0xF4304104F0C31BA, "G"},
                {0x1F430C3F430C30FC, "B"},
                {0xF430410410410BC, "C"},
                {0x1F8208421084107E, "Z"},
                {0x114517D145144, "H"},
                {0x1841041041060, "I"},
            };
            var res = "";
            for (var ch = 0; ch < Math.Ceiling(mx.GetLength(1) / 8.0); ch++) {
                var hash = 0L;
                var st = "";
                for (var irow = 0; irow < mx.GetLength(0); irow++) {
                    for (var i = 0; i < 6; i++) {
                        var icol = (ch * 8) + i;

                        if (icol < mx.GetLength(1) && mx[irow, icol]) {
                            hash += 1;
                            st += "#";
                        } else {
                            st += ".";
                        }
                        hash <<= 1;
                    }
                    st += "\n";
                }
                if (!dict.ContainsKey(hash)) {
                    throw new Exception($"Unrecognized letter with hash: 0x{hash.ToString("X")}\n{st}");
                }
                res += dict[hash];
            }
            return res;
        }
    }

    class Point {
        public int x;
        public int y;
        public int vx;
        public int vy;
    }
}