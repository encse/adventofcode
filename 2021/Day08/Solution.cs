using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day08;

[ProblemName("Seven Segment Search")]
class Solution : Solver {

   /*
              0:      1:      2:      3:      4:
             aaaa    ....    aaaa    aaaa    ....
            b    c  .    c  .    c  .    c  b    c
            b    c  .    c  .    c  .    c  b    c
             ....    ....    dddd    dddd    dddd
            e    f  .    f  e    .  .    f  .    f
            e    f  .    f  e    .  .    f  .    f
             gggg    ....    gggg    gggg    ....

              5:      6:      7:      8:      9:
             aaaa    aaaa    aaaa    aaaa    aaaa
            b    .  b    .  .    c  b    c  b    c
            b    .  b    .  .    c  b    c  b    c
             dddd    dddd    ....    dddd    dddd
            .    f  e    f  .    f  e    f  .    f
            .    f  e    f  .    f  e    f  .    f
             gggg    gggg    ....    gggg    gggg
    */

    public object PartOne(string input) {
        // we can identify digits 1, 7, 4 and 8 by their active segments count
        // digit 1 -> two active segments, digit 7 -> three, etc
        var activeSegmentCounts = new[] { 2, 3, 4, 7 };

        return (
            from line in input.Split("\n")
            let parts = line.Split(" | ")
            from segment in parts[1].Split(" ")
            where activeSegmentCounts.Contains(segment.Length)
            select 1
        ).Count();
    }

    public object PartTwo(string input) {
        var ns =
            from line in input.Split("\n")
            let parts = line.Split(" | ")

            // first we need to find a function that decodes the digit from a shuffled segment pattern:
            let reader = GetSegmentReader(parts[0])
            
            // then decode the digits one by one
            let segments = parts[1].Split(" ")
            select segments.Aggregate(0, (n, segment) => n * 10 + reader(segment));

        return ns.Sum();
    }

    Func<string, int> GetSegmentReader(string input) {
     
        var segments = new[]{
            "abcefg", "cf", "acdeg", "acdfg", "bcdf", "abdfg", "abdefg", "acf", "abcdefg", "abcdfg"
        }.Select(x => x.ToHashSet()).ToArray();

        var segmentCount = (HashSet<char> a) => a.Count();
        var commonSegmentCount = (HashSet<char> a, HashSet<char> b) => a.Intersect(b).Count();

        var left = input.Split(" ").Select(x => x.ToHashSet()).ToArray();
        
        // It so happens that we can find the digits with the below A) and B) properties in this order:
        var shuffledSegments = new Dictionary<int, HashSet<char>>();
        foreach (var i in new[] { 1, 4, 7, 8, 3, 5, 6, 9, 0, 2 }) {
            shuffledSegments[i] = left.Single(candidate =>
                // A) digit i should have the proper active segment count
                segmentCount(candidate) == segmentCount(segments[i]) &&

                // B) for all digit j: |segments[i] ∩ segments[j]| == |shuffledSegments[i] ∩ shuffledSegments[j]|
                shuffledSegments.Keys.All(j =>
                    commonSegmentCount(candidate, shuffledSegments[j]) ==
                    commonSegmentCount(segments[i], segments[j]))
            );
        }

        return (string v) => shuffledSegments.Single(kvp => kvp.Value.SetEquals(v)).Key;
    }
}
