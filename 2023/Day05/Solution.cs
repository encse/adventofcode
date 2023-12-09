using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day05;

[ProblemName("If You Give A Seed A Fertilizer")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, PartOneRanges);
    public object PartTwo(string input) => Solve(input, PartTwoRanges);

    long Solve(string input, Func<IEnumerable<long>, IEnumerable<Range>> parseSeeds) {
        var blocks = input.Split("\n\n");
        var seedRanges = parseSeeds(ParseNumbers(blocks[0])).ToList();
        var maps = blocks.Skip(1).Select(ParseMap).ToArray();

        // Project each range through the series of maps, this will result some
        // new ranges. Return the leftmost value (minimum) of these.
        return maps.Aggregate(seedRanges, Project).Select(r => r.begin).Min();
    }

    List<Range> Project(List<Range> inputRanges, Dictionary<Range, Range> map) {
        var input = new Queue<Range>(inputRanges);
        var output = new List<Range>();

        while (input.Any()) {
            var range = input.Dequeue();
            // If no entry intersects our range -> just add it to the output. 
            // If an entry completely contains the range -> add after mapping.
            // Otherwise, some entry partly covers the range. In this case 'chop' 
            // the range into two halfs getting rid of the intersection. The new 
            // pieces are added back to the queue for further processing and will be 
            // ultimately consumed by the first two cases.
            var src = map.Keys.FirstOrDefault(src => Intersects(src, range));
            if (src == null) {
                output.Add(range);
            } else if (src.begin <= range.begin && range.end <= src.end) {
                var dst = map[src];
                var shift = dst.begin - src.begin;
                output.Add(new Range(range.begin + shift, range.end + shift));
            } else if (range.begin < src.begin) {
                input.Enqueue(new Range(range.begin, src.begin - 1));
                input.Enqueue(new Range(src.begin, range.end));
            } else {
                input.Enqueue(new Range(range.begin, src.end));
                input.Enqueue(new Range(src.end + 1, range.end));
            }
        }
        return output;
    }

    // see https://stackoverflow.com/a/3269471
    bool Intersects(Range r1, Range r2) => r1.begin <= r2.end && r2.begin <= r1.end;

    // consider each number as a range of 1 length
    IEnumerable<Range> PartOneRanges(IEnumerable<long> numbers) =>
        from n in numbers select new Range(n, n);

    // chunk is a great way to iterate over the pairs of numbers
    IEnumerable<Range> PartTwoRanges(IEnumerable<long> numbers) =>
        from n in numbers.Chunk(2) select new Range(n[0], n[0] + n[1] - 1);

    IEnumerable<long> ParseNumbers(string input) =>
        from m in Regex.Matches(input, @"\d+") select long.Parse(m.Value);

    Dictionary<Range, Range> ParseMap(string input) => (
        from line in input.Split("\n").Skip(1)
        let parts = ParseNumbers(line).ToArray()
        select new KeyValuePair<Range, Range>(
            new Range(parts[1], parts[2] + parts[1] - 1),
            new Range(parts[0], parts[2] + parts[0] - 1))
    ).ToDictionary();
}

record Range(long begin, long end);
