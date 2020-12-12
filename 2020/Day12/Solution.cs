using System;
using System.Collections.Generic;

namespace AdventOfCode.Y2020.Day12 {

    record Point(int x, int y);

    [ProblemName("Rain Risk")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Ship(input, new Point(1, 0), false);
        int PartTwo(string input) => Ship(input, new Point(10, 1), true);

        int Ship(string input, Point wp, bool part2) {
            var p = new Point(0, 0);
            foreach (var line in input.Split("\n")) {
                var (ch, arg) = (line[0], int.Parse(line.Substring(1)));

                Point NSEW(Point p) =>
                    ch == 'N' ? new Point(p.x, p.y + arg) :
                    ch == 'S' ? new Point(p.x, p.y - arg) :
                    ch == 'E' ? new Point(p.x + arg, p.y) :
                    ch == 'W' ? new Point(p.x - arg, p.y) :
                    p;

                if (part2) {
                    wp = NSEW(wp);
                } else {
                    p = NSEW(p);
                }

                p = ch == 'F' ? new Point(p.x + wp.x * arg, p.y + wp.y * arg) : p;

                wp = (ch, arg) switch {
                    (_, 180) => new Point(-wp.x, -wp.y),
                    ('R', 90) => new Point(wp.y, -wp.x),
                    ('L', 270) => new Point(wp.y, -wp.x),
                    ('L', 90) => new Point(-wp.y, wp.x),
                    ('R', 270) => new Point(-wp.y, wp.x),
                    _ => wp
                };

            }
            return Math.Abs(p.x) + Math.Abs(p.y);
        }
    }
}