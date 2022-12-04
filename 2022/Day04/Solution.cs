using System;
using System.Linq;

namespace AdventOfCode.Y2022.Day04;

[ProblemName("Camp Cleanup")]
class Solution : Solver {
    public object PartOne(string input) => DuplicateWorkCount(input, Contains);
    public object PartTwo(string input) => DuplicateWorkCount(input, Overlaps);

    record struct Range(int from, int to);
    bool Contains(Range r1, Range r2) => r1.from <= r2.from && r2.to <= r1.to; 
    bool Overlaps(Range r1, Range r2) => r1.to >= r2.from && r1.from <= r2.to; 

    private int DuplicateWorkCount(string input, Func<Range, Range, bool> rangeCheck) {
        var parseRanges = (string line) => 
            from range in line.Split(',') 
            let fromTo = range.Split('-').Select(int.Parse)
            select new Range(fromTo.First(), fromTo.Last());

        return input.Split("\n").Select(parseRanges).Count(ranges => 
            rangeCheck(ranges.First(), ranges.Last()) || 
            rangeCheck(ranges.Last(), ranges.First())
        );
    }
}
