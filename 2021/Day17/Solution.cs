using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day17;

[ProblemName("Trick Shot")]
class Solution : Solver {

    public object PartOne(string input) {
        // target area: x=265..287, y=-103..-58

        return Solve(265, 287, -103, -58);
        // return Solve(20, 30, -10, -5);
    }

    int Solve(int xMin, int xMax, int yMin, int yMax) {
        var maxY = 0;

        var q = 0;
        foreach (var vx0 in Enumerable.Range(-1000, 2000)) {
            foreach (var vy0 in Enumerable.Range(-1000, 2000)) {

                var vx = vx0;
                var vy = vy0;
                var x = 0;
                var y = 0;
                var maxYR = 0;
                while (x < 1000 && y > -1000) {
                    x += vx;
                    y += vy;
                    if (x >= xMin && x <= xMax && y >= yMin && y <= yMax) {
                        maxY = Math.Max(maxY, maxYR);
                        q++;
                        break;
                    }
                    maxYR = Math.Max(y, maxYR);
                    vy--;
                    vx = Math.Max(0, vx - 1);
                }
            }
        }
        return q;
    }

    public object PartTwo(string input) {
        return 0;
    }
}
