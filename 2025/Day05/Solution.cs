namespace AdventOfCode.Y2025.Day05;

using System;
using System.Collections.Generic;
using System.Linq;

record Range(long start, long end);

[ProblemName("Cafeteria")]
class Solution : Solver {

    public object PartOne(string input) {
        var (ranges, nums) = Parse(input);
        return nums.Count(num => ranges.Any(range => range.start <= num && num <= range.end));
    }

    public object PartTwo(string input) {
        // Sort ranges by start so that potentionally overlapping ranges come after 
        // each other. Then walk the list and make them disjoint:
        var ranges = Parse(input).ranges.OrderBy(x => x.start).ToArray();
        for (var i = 0; i < ranges.Length - 1; i++) {
            if (ranges[i+1].start <= ranges[i].end) {
                var end = Math.Max(ranges[i].end, ranges[i + 1].end);
                ranges[i] = new Range(ranges[i].start, ranges[i+1].start - 1);
                ranges[i+1] = new Range(ranges[i+1].start, end);
            }
        }
        return ranges.Sum(range => range.end - range.start + 1);
    }

    (Range[] ranges, long[] nums) Parse(string input) {
        var blocks = input.Split("\n\n");
        var ranges = (
            from line in blocks[0].Split("\n")
            let limits = line.Split("-").Select(long.Parse).ToArray()
            select new Range(limits[0], limits[1])
        ).ToArray();

        var nums = blocks[1].Split("\n").Select(long.Parse).ToArray();
        return (ranges, nums);
    }
}