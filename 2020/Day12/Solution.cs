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

        int PartOne(string input) => MoveShip(input, true);
        int PartTwo(string input) => MoveShip(input, false);

        int MoveShip(string input, bool part1) =>
            input
                .Split("\n")
                .Select(line => (line[0], int.Parse(line.Substring(1))))
                .Aggregate(
                    (pos: (x: 0, y: 0), dir: part1 ? (x: 1, y: 0) : (x: 10, y: 1)), 
                    (state, line) => 
                        line switch {
                            ('N', var arg) when part1 => ((state.pos.x,  state.pos.y + arg), state.dir),
                            ('N', var arg)            => (state.pos, (state.dir.x, state.dir.y + arg)),
                            ('S', var arg) when part1 => ((state.pos.x, state.pos.y - arg), state.dir),
                            ('S', var arg)            => (state.pos, (state.dir.x, state.dir.y - arg)),
                            ('E', var arg) when part1 => ((state.pos.x + arg,  state.pos.y), state.dir),
                            ('E', var arg)            => (state.pos, (state.dir.x + arg, state.dir.y)),
                            ('W', var arg) when part1 => ((state.pos.x - arg, state.pos.y), state.dir),
                            ('W', var arg)            => (state.pos, (state.dir.x - arg, state.dir.y)), 
                            ('F', var arg)            => ((state.pos.x + arg * state.dir.x, state.pos.y + arg * state.dir.y), state.dir),
                            ('L', 90)  or ('R', 270)  => (state.pos, (-state.dir.y,  state.dir.x)),
                            ('L', 270) or ('R', 90)   => (state.pos, ( state.dir.y, -state.dir.x)),
                            ('L', 180) or ('R', 180)  => (state.pos, (-state.dir.x, -state.dir.y)),
                            _ => throw new Exception()
                        }, 
                    state => Math.Abs(state.pos.x) + Math.Abs(state.pos.y));
    }
}