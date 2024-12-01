namespace AdventOfCode.Y2024.Day01;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

[ProblemName("Historian Hysteria")]
class Solution : Solver {

    public object PartOne(string input) => 
        // go over the sorted columns pairwise and sum the difference of the pairs
        Enumerable.Zip(Column(input, 0), Column(input, 1))
            .Select(p =>  Math.Abs(p.First - p.Second))
            .Sum();

    public object PartTwo(string input) {
        // sum the left column weighted by the number of occurrences in the right
        // ⭐ .Net 9 comes with a new CountBy function
        var numberCount = Column(input, 1).CountBy(x=>x).ToDictionary();
        return Column(input, 0).Select(num => numberCount.GetValueOrDefault(num) * num).Sum();
    }

    IEnumerable<int> Column(string input, int column) =>
        from line in input.Split("\n")
        let nums = line.Split("   ").Select(int.Parse).ToArray()
        orderby nums[column]
        select nums[column];
}