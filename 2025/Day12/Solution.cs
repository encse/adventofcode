namespace AdventOfCode.Y2025.Day12;

using System;
using System.Linq;
using System.Text.RegularExpressions;
record Todo(int w, int h, int[] counts);

[ProblemName("Christmas Tree Farm")]
class Solution : Solver {

    public object PartOne(string input) {
        // 🎄 🎄 🎄 This problem was a joke by Eric. The solution is specific to the input.
        
        var blocks = input.Split("\n\n");

        var areas = (
            from block in blocks[..^1]
            let area = block.Count(ch => ch == '#')
            select area
        ).ToArray();

        var todos = (
            from line in blocks.Last().Split("\n")
            let nums = Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value)).ToArray()
            select new Todo(nums[0], nums[1], nums[2..])
        ).ToArray();
       
        var res = 0;
        foreach(var todo in todos) {
            var areaNeeded = Enumerable.Zip(todo.counts, areas).Sum(p => p.First * p.Second);
            if (areaNeeded <= todo.w * todo.h) {
                res++;
            }
        }
        return res;
    }
}