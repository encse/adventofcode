using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day22;

[ProblemName("Monkey Map")]
class Solution : Solver {
    /*
        The cube is unfolded like this. Each letter identifies an 50x50 square 
        in the input:
                 AB
                 C 
                DE
                F 
        A topology map tells us how cube sides are connected. For example in 
        case of part 1 the line "A -> B0 C0 B0 E0" means that if we go to the 
        right from A we get to B, C is down, moving to the left we find B again, 
        and moving up from A we get to E. The order of directions is always 
        right, down, left and up.

        The number next to the letter tells us how many 90 degrees we need to 
        rotate the destination square to point upwards. In case of part 1 we 
        don't need to rotate so the number is always zero. In part 2 there is 
        "A -> B0 C0 D2 F1" which means that if we are about to move up from A we 
        get to F, but F is rotated to the right once, likewise D2 means that D 
        is on the left of A and it is up side down.

        This mapping was generated from a paper model.
    */

    public object PartOne(string input) => Solve(
        input,
        """"
        A -> B0 C0 B0 E0
        B -> A0 B0 A0 B0
        C -> C0 E0 C0 A0
        D -> E0 F0 E0 F0
        E -> D0 A0 D0 C0
        F -> F0 D0 F0 D0
        """"
    );
    public object PartTwo(string input) => Solve(
        input,
        """
        A -> B0 C0 D2 F1
        B -> E2 C1 A0 F0
        C -> B3 E0 D3 A0
        D -> E0 F0 A2 C1
        E -> B2 F1 D0 C0
        F -> E3 B0 A3 D0
        """
    );

    const int blockSize = 50;
    const int right = 0;
    const int down = 1;
    const int left = 2;
    const int up = 3;

    int Solve(string input, string topology) {
        var (map, cmds) = Parse(input);
        var state = new State("A", new Coord(0, 0), right);

        foreach (var cmd in cmds) {
            switch (cmd) {
                case Left:
                    state = state with { dir = (state.dir + 3) % 4 };
                    break;
                case Right:
                    state = state with { dir = (state.dir + 1) % 4 };
                    break;
                case Forward(var n):
                    for (var i = 0; i < n; i++) {
                        var stateNext = Step(topology, state);
                        var global = ToGlobal(stateNext);
                        if (map[global.irow][global.icol] == '.') {
                            state = stateNext;
                        } else {
                            break;
                        }
                    }
                    break;
            }
        }

        return 1000 * (ToGlobal(state).irow + 1) + 
                  4 * (ToGlobal(state).icol + 1) + 
                      state.dir;
    }

    Coord ToGlobal(State state) => 
        state.block switch {
            "A" => state.coord + new Coord(0, blockSize),
            "B" => state.coord + new Coord(0, 2 * blockSize),
            "C" => state.coord + new Coord(blockSize, blockSize),
            "D" => state.coord + new Coord(2 * blockSize, 0),
            "E" => state.coord + new Coord(2 * blockSize, blockSize),
            "F" => state.coord + new Coord(3 * blockSize, 0),
            _ => throw new Exception()
        };

    State Step(string topology, State state) {

        bool wrapsAround(Coord coord) =>
            coord.icol < 0 || coord.icol >= blockSize || 
            coord.irow < 0 || coord.irow >= blockSize;

        var (srcBlock, coord, dir) = state;
        var dstBlock = srcBlock;

        // take one step, if there is no wrap around we are all right
        coord = dir switch {
            left => coord with { icol = coord.icol - 1 },
            down => coord with { irow = coord.irow + 1 },
            right => coord with { icol = coord.icol + 1 },
            up => coord with { irow = coord.irow - 1 },
            _ => throw new Exception()
        };

        if (wrapsAround(coord)) {
            // check the topology, select the dstBlock and rotate coord and dir 
            // as much as needed this is easier to follow through an example
            // if srcBlock: "C", dir: 2

            var line = topology.Split('\n').Single(x => x.StartsWith(srcBlock));
            // line: C -> B3 E0 D3 A0

            var mapping = line.Split(" -> ")[1].Split(" ");
            // mapping: B3 E0 D3 A0

            var neighbour = mapping[dir];
            // neighbour: D3

            dstBlock = neighbour.Substring(0, 1);
            // dstBlock: D

            var rotate = int.Parse(neighbour.Substring(1));
            // rotate: 3

            // go back to the 0..49 range first, then rotate as much as needed
            coord = coord with {
                irow = (coord.irow + blockSize) % blockSize,
                icol = (coord.icol + blockSize) % blockSize,
            };

            for (var i = 0; i < rotate; i++) {
                coord = coord with { 
                    irow = coord.icol, 
                    icol = blockSize - coord.irow - 1 
                };
                dir = (dir + 1) % 4;
            }
        }

        return new State(dstBlock, coord, dir);
    }

    (string[] map, Cmd[] path) Parse(string input) {
        var blocks = input.Split("\n\n");

        var map = blocks[0].Split("\n");
        var commands = Regex
            .Matches(blocks[1], @"(\d+)|L|R")
            .Select<Match, Cmd>(m =>
                m.Value switch {
                    "L" => new Left(),
                    "R" => new Right(),
                    string n => new Forward(int.Parse(n)),
                })
            .ToArray();

        return (map, commands);
    }

    record State(string block, Coord coord, int dir);

    record Coord(int irow, int icol) {
        public static Coord operator +(Coord a, Coord b) =>
            new Coord(a.irow + b.irow, a.icol + b.icol);

        public static Coord operator -(Coord a, Coord b) =>
            new Coord(a.irow - b.irow, a.icol - b.icol);
    }

    interface Cmd { }
    record Forward(int n) : Cmd;
    record Right() : Cmd;
    record Left() : Cmd;
}
