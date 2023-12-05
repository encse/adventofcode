using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day05;

[ProblemName("If You Give A Seed A Fertilizer")]
class Solution : Solver {

    public object PartOne(string input) {
        return 0;
        // var blocks = input.Split("\n\n");
        // var seeds = ParseSeeds(blocks[0]);
        // var maps = blocks.Skip(1).Select(ParseMap).ToArray();

        // return (from seed in seeds select Process(seed, maps)).Min();
    }

    public object PartTwo(string input) {

        var blocks = input.Split("\n\n");
        var ranges = ParseSeeds(blocks[0]).Chunk(2).Select(v => new Range(v[0], v[0] + v[1] - 1)).ToArray();
        var maps = blocks.Skip(1).Select(ParseMap).ToArray();

        for(var i=0;i<maps.Length;i++) {
            ranges = ranges.SelectMany(range => Lookup(range, maps[i])).ToArray();
        }

        return ranges.Select(r => r.from).Min();
    }

    long Process(long item, Map[] maps) => maps.Aggregate(item, Lookup);

    long[] ParseSeeds(string input) => (
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

    IEnumerable<Range> Lookup(Range range, Map map) {
        var q = new Queue<Range>();
        q.Enqueue(range);
        while(q.Any()) {
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
                    q.Enqueue(new Range(range.from, entry.src.from));
                    q.Enqueue(new Range(entry.src.from + 1, range.to));
                    found = true;
                } else if (range.from < entry.src.to && entry.src.to <= range.to ) {
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
        Console.WriteLine("x");
    }

    long Lookup(long item, Map map) {
        return Lookup(new Range(item,item), map).Single().from;
    }
}

record MapEntry(Range src, Range dst);
record Map(MapEntry[] entries);
record Range(long from, long to);