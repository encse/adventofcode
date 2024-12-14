namespace AdventOfCode.Y2024.Day14;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Robot = (Vec2 pos, Vec2 vel);
record struct Vec2(int x, int y);

[ProblemName("Restroom Redoubt")]
class Solution : Solver {
    const int steps = 100;
    const int width = 101;
    const int height = 103;

    public object PartOne(string input) {
        var quadrantCounts = (
            from pos in Simulate(input).ElementAt(steps)
            let quadrant = ToQuadrant(pos)
            where quadrant.x != 0 && quadrant.y != 0
            group quadrant by quadrant into g
            select g.Count()
        ).ToArray();
        return quadrantCounts[0] * quadrantCounts[1] * quadrantCounts[2] * quadrantCounts[3];
    }

    public object PartTwo(string input) {
        return Simulate(input).Select(Plot).Select(st => st.Contains("#################")).TakeWhile(x=> !x).Count();
    }

    IEnumerable<Vec2[]> Simulate(string input) {
        var robots = Parse(input).ToArray();
        while (true) {
            yield return robots.Select(r => r.pos).ToArray();
            robots = robots.Select(r => 
                r with {pos = new Vec2(
                    (r.pos.x + r.vel.x) % width,
                    (r.pos.y + r.vel.y) % height
                )}
            ).ToArray();
        }
    }

    Vec2 ToQuadrant(Vec2 pos) {
        return new Vec2(
            Math.Sign(pos.x - width / 2),
            Math.Sign(pos.y - height / 2)
        );
    }

    string Plot(IEnumerable<Vec2> positions) {
        var res = new char[height, width];
        foreach (var pos in positions) {
            res[pos.y, pos.x] = '#';
        }
        var sb = new StringBuilder();
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                sb.Append(res[y, x] == 0 ? " " : res[y, x]);
            }
            sb.Append("\n");
        }
        return sb.ToString();
    }

    IEnumerable<Robot> Parse(string input) =>
        from line in input.Split("\n")
        let nums = Regex.Matches(line, @"-?\d+").Select(m => int.Parse(m.Value)).ToArray()
        let pos = new Vec2(nums[0], nums[1])
        let vel = new Vec2(nums[2] < 0 ? nums[2] + width : nums[2], nums[3] < 0 ? nums[3] + height : nums[3])
        select new Robot(pos, vel);
}