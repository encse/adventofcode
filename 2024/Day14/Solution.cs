namespace AdventOfCode.Y2024.Day14;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
record struct Vec2(int x, int y);
record struct Robot(Vec2 pos, Vec2 vel);

[ProblemName("Restroom Redoubt")]
class Solution : Solver {
    const int width = 101;
    const int height = 103;

    // run the simulation for 100 steps and count the robots in the different quadrants.
    public object PartOne(string input) {
        var quadrants = Simulate(input)
            .ElementAt(100)
            .CountBy(GetQuadrant)
            .Where(group =>  group.Key.x != 0 && group.Key.y != 0)
            .Select(group => group.Value)
            .ToArray();
        return quadrants[0] * quadrants[1] * quadrants[2] * quadrants[3];
    }

    // i figured that the xmas tree pattern has a long horizontal ### pattern in it
    public object PartTwo(string input) =>
       Simulate(input)
        .TakeWhile(robots => !Plot(robots).Contains("#################"))
        .Count();

    // an infinite simulation of robot movement
    IEnumerable<Robot[]> Simulate(string input) {
        var robots = Parse(input).ToArray();
        while (true) {
            yield return robots;
            robots = robots.Select(Step).ToArray();
        }
    }

    // advance a robot by its velocity taking care of the 'teleportation'
    Robot Step(Robot robot) => robot with {pos = AddWithWrapAround(robot.pos, robot.vel) };

    // returns the direction (-1/0/1) of the robot to the center of the room
    Vec2 GetQuadrant(Robot robot) =>
        new Vec2(Math.Sign(robot.pos.x - width / 2), Math.Sign(robot.pos.y - height / 2));

    Vec2 AddWithWrapAround(Vec2 a, Vec2 b) =>
        new Vec2((a.x + b.x + width) % width, (a.y + b.y + height) % height);

    // shows the robot locations in the room 
    string Plot(IEnumerable<Robot> robots) {
        var res = new char[height, width];
        foreach (var robot in robots) {
            res[robot.pos.y, robot.pos.x] = '#';
        }
        var sb = new StringBuilder();
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                sb.Append(res[y, x] == '#' ? "#" : " ");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    IEnumerable<Robot> Parse(string input) =>
        from line in input.Split("\n")
        let nums = Regex.Matches(line, @"-?\d+").Select(m => int.Parse(m.Value)).ToArray()
        select new Robot(new Vec2(nums[0], nums[1]), new Vec2(nums[2], nums[3]));
}