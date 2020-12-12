using System;
using System.Collections.Generic;

namespace AdventOfCode.Y2020.Day12 {

    record Point(int x, int y);
    record State(Point p, Point wp);

    [ProblemName("Rain Risk")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => MoveShip(input, new Point(1, 0), false);
        int PartTwo(string input) => MoveShip(input, new Point(10, 1), true);

        int MoveShip(string input, Point wp, bool part2) {
            var state = new State(new Point(0, 0), wp);

            foreach (var line in input.Split("\n")) {
                var (ch, arg) = (line[0], int.Parse(line.Substring(1)));

                Point NSEW(Point p) => ch switch {
                    'N' => new Point(p.x, p.y + arg),
                    'S' => new Point(p.x, p.y - arg),
                    'E' => new Point(p.x + arg, p.y),
                    'W' => new Point(p.x - arg, p.y),
                    _ => p
                };

                if (part2) {
                    state = state with {wp = NSEW(state.wp)};
                } else {
                    state = state with {p = NSEW(state.p)};
                }
                
                state = (ch, arg) switch {
                    ('F',   _) => state with { p = new Point( state.p.x + state.wp.x * arg, state.p.y + state.wp.y * arg)},
                    ('R',  90) => state with {wp = new Point( state.wp.y, -state.wp.x)},
                    ('R', 180) => state with {wp = new Point(-state.wp.x, -state.wp.y)},
                    ('R', 270) => state with {wp = new Point(-state.wp.y, state.wp.x)},
                    ('L',  90) => state with {wp = new Point(-state.wp.y, state.wp.x)},
                    ('L', 180) => state with {wp = new Point(-state.wp.x, -state.wp.y)},
                    ('L', 270) => state with {wp = new Point( state.wp.y, -state.wp.x)},
                    _ => state
                };
            }
            return Math.Abs(state.p.x) + Math.Abs(state.p.y);
        }
    }
}