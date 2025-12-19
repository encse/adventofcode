namespace AdventOfCode.Y2025.Day12;

using System;
using System.Linq;
using System.Text.RegularExpressions;

[ProblemName("Christmas Tree Farm")]
class Solution : Solver {

    public object PartOne(string input) {
        // 🎄 🎄 🎄 This problem was a joke by Eric. The solution is input specific.
        return input.Split("\n\n").Last()
            .Split("\n")
            .Select(line => Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value)).ToArray())
            .Count(nums => nums[0] * nums[1] >= 9 * nums[2..].Sum());
    }
}