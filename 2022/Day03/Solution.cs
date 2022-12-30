using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day03;

[ProblemName("Rucksack Reorganization")]
class Solution : Solver {

    public object PartOne(string input) =>
        // A line can be divided to two 'compartments' of equal length. We need 
        // to find the common item (letter) in them, and convert it to a number 
        // called 'priority'. Do this for each line and sum the priorities. 
        // We use 'chunk' to split a line in half.
        input.Split("\n")
            .Select(line => line.Chunk(line.Length/2)) // 🥩 
            .Select(GetCommonItemPriority)
            .Sum();

    public object PartTwo(string input) =>
        // Here we need to find the common item in three consecutive lines, 
        // convert it to priority as before, and sum it up along the whole 
        // input. 
        // This is again conveniently done using the chunk function.
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
