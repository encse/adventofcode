using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Y2020.Day12 {

    [ProblemName("Rain Risk")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => MoveShip(input, (1, 0), false);
        int PartTwo(string input) => MoveShip(input, (10, 1), true);

        int MoveShip(string input, Vec2 dir, bool part2) =>
            input.Split("\n")
                .Select(line => ((ch: line[0], arg: int.Parse(line.Substring(1)))))
                .Aggregate(
                    (pos: new Vec2(0,0), dir: dir), 
                    (state, line) => 
                        line.ch switch {
                            'N' when part2 => (state.pos, state.dir + (0,  line.arg)),
                            'N'            => (state.pos + (0,  line.arg), state.dir),
                            'S' when part2 => (state.pos, state.dir + (0, -line.arg)),
                            'S'            => (state.pos + (0, -line.arg), state.dir),
                            'E' when part2 => (state.pos, state.dir + ( line.arg, 0)), 
                            'E'            => (state.pos + ( line.arg, 0), state.dir),
                            'W' when part2 => (state.pos, state.dir + (-line.arg, 0)), 
                            'W'            => (state.pos + (-line.arg, 0), state.dir),
                            'F'            => (state.pos + line.arg * state.dir, state.dir),
                            'L'            => (state.pos, state.dir.Rotate(line.arg)),
                            'R'            => (state.pos, state.dir.Rotate(360 - line.arg)),
                            _ => throw new Exception()
                        }, 
                    state => Math.Abs(state.pos.x) + Math.Abs(state.pos.y));
    }

    record Vec2(int x, int y) {

        public static implicit operator Vec2((int x, int y) p) => new Vec2(p.x, p.y);
        public static Vec2 operator +(Vec2 a, Vec2 b) => (a.x + b.x, a.y + b.y);
        public static Vec2 operator *(int m, Vec2 v) => (m * v.x, m * v.y);

        public Vec2 Rotate(int arg) => arg switch {
            90 => (-y, x),
            180 => (-x, -y),
            270 => (y, -x),
            _ => throw new Exception()
        };
    }
}