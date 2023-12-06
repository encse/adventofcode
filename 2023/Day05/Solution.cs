using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day05;

[ProblemName("If You Give A Seed A Fertilizer")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, PartOneRanges);

    public object PartTwo(string input) => Solve(input, PartTwoRanges);

    long Solve(string input, Func<long[], IEnumerable<Range>> parseSeeds) {
        var blocks = input.Split("\n\n");
        var seedRanges = parseSeeds(ParseNumbers(blocks[0])).ToArray();
        var maps = blocks.Skip(1).Select(ParseMap).ToArray();

        // Project each range through the series of maps, this will result some
        // new ranges. Return the leftmost value (minimum) of these.
        return maps.Aggregate(seedRanges, Project).Select(r => r.from).Min();
    }

    // When a single 'range' is mapped  it can generate multiple 'ranges' in the 
    // output. Since we are dealing with this anyways, we can just as well solve 
    // the more general problem which gets an array of ranges instead.
    //
    // This has the proper signature for Aggregate(), so we can push the input 
    // through multiple maps in one call.
    Range[] Project(Range[] inputRanges, Dictionary<Range,Range> map) {
        var todo = new Queue<Range>();
        foreach (var range in inputRanges) {
            todo.Enqueue(range);
        }

        var outputRanges = new List<Range>();
        while (todo.Any()) {
            var range = todo.Dequeue();
            // If no entry intersects our range -> just add it to the output. 
            // If an entry completely contains the range -> add after mapping.
            // Otherwise, some entry partly covers the range. In this case 'chop' 
            // the range into two halfs getting rid of the intersection. The new 
            // pieces are added back to the queue for further processing and will be 
            // ultimately consumed by the first two cases.
            var src = map.Keys.FirstOrDefault(src => Intersects(src, range));
            if (src == null) {
                outputRanges.Add(range);
            } else if (src.from <= range.from && range.to <= src.to) {
                var dst = map[src];
                var shift = dst.from - src.from;
                outputRanges.Add(new Range(range.from + shift, range.to + shift));
            } else if (range.from < src.from) {
                todo.Enqueue(new Range(range.from, src.from - 1));
                todo.Enqueue(new Range(src.from, range.to));
            } else {
                todo.Enqueue(new Range(range.from, src.to));
                todo.Enqueue(new Range(src.to + 1, range.to));
            }
        }
        return [..outputRanges];
    }

    // see https://stackoverflow.com/a/3269471
    bool Intersects(Range r1, Range r2) => r1.from <= r2.to && r2.from <= r1.to;

    // consider each number as a range of 1 length
    IEnumerable<Range> PartOneRanges(long[] numbers) =>
        from n in numbers select new Range(n, n);

    // chunk is a great way to iterate over the pairs of numbers
    IEnumerable<Range> PartTwoRanges(long[] numbers) =>
        from c in numbers.Chunk(2) select new Range(c[0], c[0] + c[1] - 1);

    long[] ParseNumbers(string input) =>
        [.. from m in Regex.Matches(input, @"\d+") select long.Parse(m.Value)];

    Dictionary<Range, Range> ParseMap(string input) => (
        from line in input.Split("\n").Skip(1)
        let parts =  line.Split(" ").Select(long.Parse).ToArray()
        let src = new Range(parts[1], parts[2] + parts[1] - 1)
        let dst = new Range(parts[0], parts[2] + parts[0] - 1)
        select new KeyValuePair<Range, Range>(src, dst)
    ).ToDictionary();
}

record Range(long from, long to);
