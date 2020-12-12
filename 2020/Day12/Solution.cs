using System;
using System.Collections.Generic;

namespace AdventOfCode.Y2020.Day12 {

    [ProblemName("Rain Risk")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => MoveShip(input, new State(new Vec2(0, 0), new Vec2(1, 0)), false);
        int PartTwo(string input) => MoveShip(input, new State(new Vec2(0, 0), new Vec2(10, 1)), true);

        int MoveShip(string input, State state, bool part2) {
            foreach (var line in input.Split("\n")) {
                var (ch, arg) = (line[0], int.Parse(line.Substring(1)));

                state = ch switch {
                    'N' when part2 => state with { dir = state.dir + new Vec2(0, arg) },
                    'N'            => state with { pos = state.pos + new Vec2(0, arg) },
                    'S' when part2 => state with { dir = state.dir + new Vec2(0, -arg) },
                    'S'            => state with { pos = state.pos + new Vec2(0, -arg) },
                    'E' when part2 => state with { dir = state.dir + new Vec2(arg, 0) }, 
                    'E'            => state with { pos = state.pos + new Vec2(arg, 0) },
                    'W' when part2 => state with { dir = state.dir + new Vec2(-arg, 0) }, 
                    'W'            => state with { pos = state.pos + new Vec2(-arg, 0) },
                    'F'            => state with { pos = state.pos + arg * state.dir },
                    'L'            => state with { dir = state.dir.Rotate(arg) },
                    'R'            => state with { dir = state.dir.Rotate(360 - arg) },
                    _ => throw new Exception()
                };
            }
            return Math.Abs(state.pos.x) + Math.Abs(state.pos.y);
        }
    }
    
    record Vec2(int x, int y) {

        public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.x + b.x, a.y + b.y);

        public static Vec2 operator *(int m, Vec2 v) => new Vec2(m * v.x, m * v.y);

        public Vec2 Rotate(int arg) => arg switch {
            90 => new Vec2(-y, x),
            180 => new Vec2(-x, -y),
            270 => new Vec2(y, -x),
            _ => throw new Exception()
        };
    }

    record State(Vec2 pos, Vec2 dir);
}