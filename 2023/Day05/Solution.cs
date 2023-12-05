using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day05;

[ProblemName("If You Give A Seed A Fertilizer")]
class Solution : Solver {

    public object PartOne(string input) {
        return Solve(input, ints => ints.Select(v => new Range(v, v)));
    }

    public object PartTwo(string input) {
        return Solve(input, ints => ints.Chunk(2).Select(v => new Range(v[0], v[0] + v[1] - 1)));
    }

    public long Solve(string input, Func<long[], IEnumerable<Range>> parseRanges) {
        var blocks = input.Split("\n\n");
        var ranges = parseRanges(ParseInts(blocks[0])).ToArray();
        var maps = blocks.Skip(1).Select(ParseMap).ToArray();

        ranges = Lookup(ranges, maps);

        return ranges.Select(r => r.from).Min();
    }

    long[] ParseInts(string input) => (
        from m in Regex.Matches(input, @"\d+")
        select long.Parse(m.Value)
    ).ToArray();

    Map ParseMap(string input) =>
        new Map(
            (
            from line in input.Split("\n").Skip(1)
            let parts = line.Split(" ").Select(long.Parse).ToArray()
            let src = new Range(parts[1], parts[2] + parts[1] - 1)
            let dst = new Range(parts[0], parts[2] + parts[0] - 1)
            select new MapEntry(src, dst)
            ).ToArray()
        );

    Range[] Lookup(Range[] ranges, Map[] maps) {
        for (var i = 0; i < maps.Length; i++) {
            ranges = ranges.SelectMany(range => Lookup(range, maps[i])).ToArray();
            ranges = Pack(ranges);
        }
        return ranges;
    } 

    Range[] Pack(Range[] ranges) {
        ranges = ranges.OrderBy(x => x.from).Distinct().ToArray();
        var res = new List<Range>();
        var range = ranges[0];
        for(var i = 1;i<ranges.Length;i++) {
            if (range.to == ranges[i].from - 1) {
                range = new Range(range.from, ranges[i].to);
            } else {
                res.Add(range);
                range = ranges[i];
            }
        }
        res.Add(range);
        return res.ToArray();
    }

    IEnumerable<Range> Lookup(Range range, Map map) {
        var q = new Queue<Range>();
        q.Enqueue(range);
        while (q.Any()) {
            range = q.Dequeue();
            var found = false;
            foreach (var entry in map.entries) {
                if (entry.src.from <= range.from && range.to <= entry.src.to) {
                    // entry src contains our range
                    var shift = entry.dst.from - entry.src.from;
                    yield return new Range(range.from + shift, range.to + shift);
                    found = true;
                } else if (range.from < entry.src.from && entry.src.from <= range.to) {
                    // range contains the begining of the entry
                    q.Enqueue(new Range(range.from, entry.src.from - 1));
                    q.Enqueue(new Range(entry.src.from, range.to));
                    found = true;
                } else if (range.from < entry.src.to && entry.src.to <= range.to) {
                    // range contains the end of the entry
                    q.Enqueue(new Range(range.from, entry.src.to));
                    q.Enqueue(new Range(entry.src.to + 1, range.to));
                    found = true;
                }
            }
            if (!found) {
                yield return new Range(range.from, range.to);
            }
        }
    }

}

record MapEntry(Range src, Range dst);
record Map(MapEntry[] entries);
record Range(long from, long to);