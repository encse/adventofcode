using System;
using System.Linq;

namespace AdventOfCode.Y2022.Day04;

[ProblemName("Camp Cleanup")]
class Solution : Solver {
    public object PartOne(string input) => DuplicatedWorkCount(input, Contains);
    public object PartTwo(string input) => DuplicatedWorkCount(input, Overlaps);

    record struct Range(int from, int to);

    // True if r1 contains r2 [ { } ] 
    bool Contains(Range r1, Range r2) => r1.from <= r2.from && r2.to <= r1.to; 
    
    // True if r1 overlaps r2 { [ } ], the other direction is not checked.
    bool Overlaps(Range r1, Range r2) => r1.to >= r2.from && r1.from <= r2.to; 

    // DuplicatedWorkCount goes over the lines in the input, converts them to 
    // ranges A and B, and counts how many times rangeCheck(A,B) or 
    // rangeCheck(B, A) is true. The check is applied in both ways so that the 
    // Contains and Overlaps functions don't have to check each directions.
    private int DuplicatedWorkCount(
        string input, 
        Func<Range, Range, bool> rangeCheck
    ) {
        // '36-41,35-40' becomes [Range(36, 41), Range(35, 40)]
        var parseRanges = (string line) => 
            from range in line.Split(',') 
            let fromTo = range.Split('-').Select(int.Parse)
            select new Range(fromTo.First(), fromTo.Last());

        return input
            .Split("\n")
            .Select(parseRanges)
            .Count(ranges => 
                rangeCheck(ranges.First(), ranges.Last()) || 
                rangeCheck(ranges.Last(), ranges.First())
            );
    }
}
