namespace AdventOfCode.Y2024.Day13;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Machine = (Vec2 a, Vec2 b, Vec2 p);

record struct Vec2(long x, long y);

[ProblemName("Claw Contraption")]
class Solution : Solver {

    public object PartOne(string input) => Parse(input).Sum(GetPrize);
    public object PartTwo(string input) => Parse(input, shift: 10000000000000).Sum(GetPrize);

    long GetPrize(Machine m) {
        var (a, b, p) = m;

        // solve a * i + b * j = p for i and j using Cramer's rule
        var i = Det(p, b) / Det(a, b);
        var j = Det(a, p) / Det(a, b);

        // return the prize when an _integer_ solution is found
        if (a.x * i + b.x * j == p.x && a.y * i + b.y * j == p.y) {
            return 3 * i + j;
        } else {
            return 0;
        }
    }

    long Det(Vec2 a, Vec2 b) => a.x * b.y - a.y * b.x;

    IEnumerable<Machine> Parse(string input, long shift=0) {
        var blocks = input.Split("\n\n");
        foreach (var block in blocks) {
            var nums =
                Regex.Matches(block, @"\d+", RegexOptions.Multiline)
                    .Select(m => int.Parse(m.Value))
                    .Chunk(2).Select(p => new Vec2(p[0], p[1]))
                    .ToArray();

            nums[2] = new Vec2(nums[2].x + shift, nums[2].y + shift);
            yield return (nums[0], nums[1], nums[2]);
        }
    }
}