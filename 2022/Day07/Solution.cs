using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day07;

[ProblemName("No Space Left On Device")]
class Solution : Solver {

    public object PartOne(string input) {
        return GetDirectorySizes(input).Where(size => size < 100_000).Sum();
    }

    public object PartTwo(string input) {
        var directorySizes = GetDirectorySizes(input);
        var freeSpace = 70_000_000 - directorySizes.Max();
        return directorySizes.Where(size => size + freeSpace >= 30_000_000).Min();
    }

    private List<int> GetDirectorySizes(string input) {
        var path = new Stack<string>();
        var sizes = new Dictionary<string, int>();
        foreach (var line in input.Split("\n")) {
            if (line == "$ cd ..") {
                path.Pop();
            } else if (line.StartsWith("$ cd")) {
                path.Push(string.Join("", path)+line.Split(" ")[2]);
            } else if (Regex.Match(line, @"\d+").Success) {
                var size = int.Parse(line.Split(" ")[0]);
                foreach (var dir in path) {
                    sizes[dir] = sizes.GetValueOrDefault(dir) + size;
                }
            }
        }
        return sizes.Values.ToList();
    }
}
