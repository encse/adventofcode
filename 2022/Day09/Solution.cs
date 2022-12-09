using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day09;

[ProblemName("Rope Bridge")]
class Solution : Solver {

    public object PartOne(string input) => Tails(input, 2).ToHashSet().Count;
    public object PartTwo(string input) => Tails(input, 10).ToHashSet().Count;

    private IEnumerable<Knot> Tails(string input, int ropeLength) {
        var rope = Enumerable.Repeat(new Knot(0, 0), ropeLength).ToArray();
        yield return rope.Last();

        foreach (var line in input.Split("\n")) {
            var parts = line.Split(' ');
            var dir = parts[0];
            var dist = int.Parse(parts[1]);

            for (var i = 0; i < dist; i++) {
                MoveHead(rope, dir);
                yield return rope.Last();
            }
        }
    }

    record struct Knot(int irow, int icol);

    // moves the head in the given direction, inplace update of the rope
    void MoveHead(Knot[] rope, string dir) {
        rope[0] = dir switch {
            "U" => rope[0] with { irow = rope[0].irow - 1 },
            "D" => rope[0] with { irow = rope[0].irow + 1 },
            "L" => rope[0] with { icol = rope[0].icol - 1 },
            "R" => rope[0] with { icol = rope[0].icol + 1 },
            _ => throw new ArgumentException(dir)
        };

        // move knots which are not adjacent to their previous sibling in the rope:
        for (var i = 1; i < rope.Length; i++) {
            var drow = rope[i - 1].irow - rope[i].irow; 
            var dcol = rope[i - 1].icol - rope[i].icol;

            if (Math.Abs(drow) > 1 || Math.Abs(dcol) > 1) {
                rope[i] = new Knot(
                    rope[i].irow + Math.Sign(drow), 
                    rope[i].icol + Math.Sign(dcol)
                );
            }
        }
    }
}
