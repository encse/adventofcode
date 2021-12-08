using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day08;

[ProblemName("Seven Segment Search")]
class Solution : Solver {

   /*
              0:      1:      2:      3:      4:      5:      6:      7:      8:      9:
             aaaa    ....    aaaa    aaaa    ....    aaaa    aaaa    aaaa    aaaa    aaaa
            b    c  .    c  .    c  .    c  b    c  b    .  b    .  .    c  b    c  b    c
            b    c  .    c  .    c  .    c  b    c  b    .  b    .  .    c  b    c  b    c
             ....    ....    dddd    dddd    dddd    dddd    dddd    ....    dddd    dddd
            e    f  .    f  e    .  .    f  .    f  .    f  e    f  .    f  e    f  .    f
            e    f  .    f  e    .  .    f  .    f  .    f  e    f  .    f  e    f  .    f
             gggg    ....    gggg    gggg    ....    gggg    gggg    ....    gggg    gggg
    */

    public object PartOne(string input) {

        // we can identify digits 1, 7, 4 and 8 by their active segments count:
        var segmentCounts = new[] { "cd", "acf", "bcdf", "abcdefg" }.Select(x => x.Length).ToHashSet();

        return (
            from line in input.Split("\n")
            let parts = line.Split(" | ")
            from segment in parts[1].Split(" ")
            where segmentCounts.Contains(segment.Length)
            select 1
        ).Count();
    }

    public object PartTwo(string input) {
        var res = 0;
        foreach(var line in input.Split("\n")) {
            var parts = line.Split(" | ");
            var patterns = parts[0].Split(" ").Select(x => x.ToHashSet()).ToArray();

            // let's figure out what segments belong to each digit
            var digits = new HashSet<char>[10];

            // we can do these by length:
            digits[1] = patterns.Single(pattern => pattern.Count() == "cf".Length);
            digits[4] = patterns.Single(pattern => pattern.Count() == "bcdf".Length);

            // it turns out that the following tripplet uniquely identifies the rest:
            var lookup = (int segmentCount, int commonWithOne, int commonWithFour) =>
                patterns.Single(pattern => 
                    pattern.Count() == segmentCount && 
                    pattern.Intersect(digits[1]).Count() == commonWithOne && 
                    pattern.Intersect(digits[4]).Count() == commonWithFour
                );
            
            digits[0] = lookup(6, 2, 3);  
            digits[2] = lookup(5, 1, 2);
            digits[3] = lookup(5, 2, 3);
            digits[5] = lookup(5, 1, 3);
            digits[6] = lookup(6, 1, 3);
            digits[7] = lookup(3, 2, 2);
            digits[8] = lookup(7, 2, 4);
            digits[9] = lookup(6, 2, 4);
                    
            var reader = (string v) => 
                Enumerable.Range(0, 10).Single(i => digits[i].SetEquals(v));

            // then decode the digits one by one:
            res += parts[1].Split(" ").Aggregate(0, (n, digit) => n * 10 + reader(digit));
        }
        return res;
    }
}
