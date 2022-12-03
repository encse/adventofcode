using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day03;

[ProblemName("Rucksack Reorganization")]
class Solution : Solver {

    public object PartOne(string input) =>
        input.Split("\n")
            .Select(line => line.Chunk(line.Length/2)) // 🥩 
            .Select(GetCommonItemPriority)
            .Sum();

    public object PartTwo(string input) =>
        input.Split("\n")
            .Chunk(3)
            .Select(GetCommonItemPriority)
            .Sum();

    private int GetCommonItemPriority(IEnumerable<IEnumerable<char>> texts) => (
        from ch in texts.First()
        where texts.All(text => text.Contains(ch))
        select ch < 'a' ? ch - 'A' + 27 : ch - 'a' + 1
    ).First();

}
