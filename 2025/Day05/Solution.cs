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
        var (ranges, _) = Parse(input);

        // Merge overlapping ranges into the disjoint intervals.
        // First sort ranges by start so that any range that can extend
        // the current one appears at a higher index. Then we walk the list
        // once extending ranges[i] with any later range that overlaps with it.
        ranges = ranges.OrderBy(x => x.start).ToList();
        for (var i = 0; i < ranges.Count; i++) {
            int j = i + 1;
            while (j < ranges.Count) {
                var rangeI = ranges[i];
                var rangeJ = ranges[j];

                if (rangeJ.start <= rangeI.end) {
                    ranges[i] = new Range(rangeI.start, Math.Max(rangeI.end, rangeJ.end));
                    ranges.RemoveAt(j);
                } else {
                    j++;
                }
            }
        }

        return ranges.Sum(range => range.end - range.start + 1);
    }

    (List<Range> ranges, long[] nums) Parse(string input) {
        var blocks = input.Split("\n\n");
        var ranges = (
            from line in blocks[0].Split("\n")
            let limits = line.Split("-").Select(long.Parse).ToArray()
            select new Range(limits[0], limits[1])
        ).ToList();

        var nums = blocks[1].Split("\n").Select(long.Parse).ToArray();
        return (ranges, nums);
    }
}