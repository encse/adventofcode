using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day05;

[ProblemName("If You Give A Seed A Fertilizer")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, PartOneRanges);

    public object PartTwo(string input) => Solve(input, PartTwoRanges);

    long Solve(string input, Func<long[], IEnumerable<Range>> parseRanges) {
        var blocks = input.Split("\n\n");

        // Parse the 'seeds' in the input into range of numbers; in part 1 these 
        // are just individual numbers, but in part 2 we have real ranges
        var ranges = parseRanges(ParseNumbers(blocks[0])).ToArray();
   
        var maps = blocks.Skip(1).Select(ParseMap).ToArray();
        // Project each range through the series of maps, this will result some
        // new ranges. Return the leftmost value (minimum) of these.
        return maps.Aggregate(ranges, Project).Select(r => r.from).Min();
    }

    // The complexity arises from the fact that when a single 'range' is mapped 
    // it can enerate multiple 'ranges' in the output. Since we are dealing with 
    // this anyways, we can immediately solve the more general problem which 
    // gets an _array_ of ranges instead of just one and returns a ranges array.
    //
    // This function has a proper signature to be used with Aggregate, so that
    // we can push the input through multiple maps in one call.
    Range[] Project(Range[] inputRanges, Map map) {
        var todo = new Queue<Range>();
        foreach (var srcRange in inputRanges) {
            todo.Enqueue(srcRange);
        }

        var outputRanges = new List<Range>();
        while (todo.Any()) {
            var range = todo.Dequeue();
            // If a map entry completely contains a range, we can just map it into
            // a single range in the output. If there is no entry that intersects
            // with the range, we just keep it as it is. Otherwise some entry partly
            // intersects the range. In this case we 'chop' the range into two halfs
            // getting rid of the intersection. These pieces are added back to 
            // the queue for further processing and will be ultimately consumed
            // by the first two cases.
            var processed = false;
            foreach (var entry in map.entries) {
                if (entry.src.from <= range.from && range.to <= entry.src.to) {
                    // entry contains the range -> output
                    var shift = entry.dst.from - entry.src.from;
                    outputRanges.Add(new Range(range.from + shift, range.to + shift));
                    processed = true;
                    break;
                } else if (range.from < entry.src.from && entry.src.from <= range.to) {
                    // range contains the begining of the entry -> split
                    todo.Enqueue(new Range(range.from, entry.src.from - 1));
                    todo.Enqueue(new Range(entry.src.from, range.to));
                    processed = true;
                    break;
                } else if (range.from < entry.src.to && entry.src.to <= range.to) {
                    // range contains the end of the entry -> split
                    todo.Enqueue(new Range(range.from, entry.src.to));
                    todo.Enqueue(new Range(entry.src.to + 1, range.to));
                    processed = true;
                    break;
                }
            }
            if (!processed) {
                outputRanges.Add(new Range(range.from, range.to));
            }
        }
        return [..outputRanges];
    }

    // each number results in a 1 length range:
    IEnumerable<Range> PartOneRanges(long[] numbers) =>
        from n in numbers select new Range(n, n);

    // chunk is a great way to iterate over the pairs of numbers in the input:
    IEnumerable<Range> PartTwoRanges(long[] numbers) =>
        from c in numbers.Chunk(2) select new Range(c[0], c[0] + c[1] - 1);

    long[] ParseNumbers(string input) =>
        [.. from m in Regex.Matches(input, @"\d+") select long.Parse(m.Value)];

    Map ParseMap(string input) => new Map([..
        from line in input.Split("\n").Skip(1)
        let parts = line.Split(" ").Select(long.Parse).ToArray()
        let src = new Range(parts[1], parts[2] + parts[1] - 1)
        let dst = new Range(parts[0], parts[2] + parts[0] - 1)
        select new MapEntry(src, dst)
    ]);
}

record MapEntry(Range src, Range dst);
record Map(MapEntry[] entries);
record Range(long from, long to);
