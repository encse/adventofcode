using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2015.Day16 {

    [ProblemName("Aunt Sue")]
    class Solution : Solver {

        private Dictionary<string, int> target = new Dictionary<string, int> {
            ["children"] = 3,
            ["cats"] = 7,
            ["samoyeds"] = 2,
            ["pomeranians"] = 3,
            ["akitas"] = 0,
            ["vizslas"] = 0,
            ["goldfish"] = 5,
            ["trees"] = 3,
            ["cars"] = 2,
            ["perfumes"] = 1,
        };

        public object PartOne(string input) =>
            Parse(input).FindIndex(p => p.Keys.All(k => p[k] == target[k])) + 1;

        public object PartTwo(string input) =>
            Parse(input).FindIndex(p => p.Keys.All(k => {
                if (k == "cats" || k == "trees") {
                    return p[k] > target[k];
                } else if (k == "pomeranians" || k == "goldfish") {
                    return p[k] < target[k];
                } else {
                    return p[k] == target[k];
                }
            })) + 1;

        List<Dictionary<string, int>> Parse(string input) => (
                from line in input.Split('\n')
                let parts = Regex.Matches(line, @"(\w+): (\d+)")
                select parts.ToDictionary(
                    part => part.Groups[1].Value,
                    part => int.Parse(part.Groups[2].Value))
             ).ToList();
    }
}