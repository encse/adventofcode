using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day08;

[ProblemName("Seven Segment Search")]
class Solution : Solver {

    public object PartOne(string input) {
        var groups =
        from q in (
            from line in input.Split("\n")
            let parts = line.Split(" | ").ToArray()
            from q in parts[1].Split(" ")
            select q.Length
        )
        group q by q into g
        select g;
        var d = groups.ToDictionary(g => g.Key, g => g.Count());
        return d[2] + d[3] + d[4] + d[7];

    }

    public object PartTwo(string input) {
        var res = 0;
        
        foreach (var line in input.Split("\n")) {
            var parts = line.Split(" | ").ToArray();
            var left = parts[0].Split(" ").Select(x => x.ToHashSet()).ToArray();
            var right = parts[1].Split(" ").Select(x => x.ToHashSet()).ToArray();

            HashSet<char>[] digits = new HashSet<char>[10];

            digits[1] = left.Single(x => x.Count() == 2);
            digits[4] = left.Single(x => x.Count() == 4);
            digits[7] = left.Single(x => x.Count() == 3);
            digits[8] = left.Single(x => x.Count() == 7);

            var c235 = left.Where(x => x.Count() == 5).ToArray();
            var c25 = 
                c235[0].Union(c235[1]).Count() == 7 ? new [] { c235[0], c235[1] } :
                c235[1].Union(c235[2]).Count() == 7 ? new [] { c235[1], c235[2] } :
                c235[0].Union(c235[2]).Count() == 7 ? new [] { c235[0], c235[2] } :
                                                      throw new Exception();
            digits[3] = c235.Except(c25).Single();

            var dSegment = digits[3].Except(digits[7]).Intersect(digits[4]).Single();

            var c069 = left.Where(x => x.Count() == 6).ToArray();
            var c69 = c069.Where(x => x.Contains(dSegment));
            digits[0] = c069.Except(c69).Single();

            var eSegment = digits[0].Except(digits[4].Union(digits[3])).Single();

            digits[2] = c25.Single(x => x.Contains(eSegment));
            digits[5] = c25.Single(x => !x.Contains(eSegment));
            digits[6] = c69.Single(x => x.Contains(eSegment));
            digits[9] = c69.Single(x => !x.Contains(eSegment));

            var value = (HashSet<char> v) =>
                Enumerable.Range(0, 10).Single(i => digits[i].SetEquals(v));

            res += 
                (1000 * value(right[0])) +
                (100 * value(right[1])) +
                (10 * value(right[2])) +
                (1 * value(right[3]));

        }
        return res;
    }

}
