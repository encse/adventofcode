namespace AdventOfCode.Y2024.Day15;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;

using Map = System.Collections.Immutable.IImmutableDictionary<System.Numerics.Complex, char>;

[ProblemName("Warehouse Woes")]
class Solution : Solver {

    static Complex Up = -Complex.ImaginaryOne;
    static Complex Down = Complex.ImaginaryOne;
    static Complex Left = -1;
    static Complex Right = 1;

    public object PartOne(string input) => Solve(input);
    public object PartTwo(string input) => Solve(ScaleUp(input));

    public double Solve(string input) {
        var (map, steps) = Parse(input);

        var robot = map.Keys.Single(k => map[k] == '@');
        foreach (var dir in steps) {
            if (TryToStep(ref map, robot, dir)) {
                robot += dir;
            }
        }

        return map.Keys
            .Where(k => map[k] == '[' || map[k] == 'O')
            .Sum(box => box.Real + 100 * box.Imaginary);
    }

    // Attempts to move the robot in the given direction on the map, pushing boxes as necessary.
    // If the move is successful, the map is updated to reflect the new positions and the function returns true.
    // Otherwise, the map remains unchanged and the function returns false.
    bool TryToStep(ref Map map, Complex pos, Complex dir) {
        var mapOrig = map;

        if (map[pos] == '.') {
            return true;
        } else if (map[pos] == 'O' || map[pos] == '@') {
            if (TryToStep(ref map, pos + dir, dir)) {
                map = map
                    .SetItem(pos + dir, map[pos])
                    .SetItem(pos, '.');
                return true;
            }
        } else if (map[pos] == ']') {
            return TryToStep(ref map, pos + Left, dir);
        } else if (map[pos] == '[') {
            if (dir == Left) {
                if (TryToStep(ref map, pos + Left, dir)) {
                    map = map
                        .SetItem(pos + Left, '[')
                        .SetItem(pos, ']')
                        .SetItem(pos + Right, '.');
                    return true;
                }
            } else if (dir == Right) {
                if (TryToStep(ref map, pos + 2 * Right, dir)) {
                    map = map
                        .SetItem(pos, '.')
                        .SetItem(pos + Right, '[')
                        .SetItem(pos + 2 * Right, ']');
                    return true;
                }
            } else {
                if (TryToStep(ref map, pos + dir, dir) && TryToStep(ref map, pos + Right + dir, dir)) {
                    map = map
                        .SetItem(pos, '.')
                        .SetItem(pos + Right, '.')
                        .SetItem(pos + dir, '[')
                        .SetItem(pos + dir + Right, ']');
                    return true;
                }
            }
        }

        map = mapOrig;
        return false;
    }

    string ScaleUp(string input) =>
        input.Replace("#", "##").Replace(".", "..").Replace("O", "[]").Replace("@", "@.");

    (Map, Complex[]) Parse(string input) {
        var blocks = input.Split("\n\n");
        var lines = blocks[0].Split("\n");
        var map = (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Down, lines[y][x])
        ).ToImmutableDictionary();

        var steps = blocks[1].ReplaceLineEndings("").Select(ch =>
            ch switch {
                '^' => Up,
                '<' => Left,
                '>' => Right,
                'v' => Down,
                _ => throw new Exception()
            });

        return (map, steps.ToArray());
    }
}
