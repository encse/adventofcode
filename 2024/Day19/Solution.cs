namespace AdventOfCode.Y2024.Day19;

using System;
using System.Collections.Generic;
using System.Linq;

using Cache = System.Collections.Concurrent.ConcurrentDictionary<string, long>;

[ProblemName("Linen Layout")]
class Solution : Solver {

    public object PartOne(string input) => MatchCounts(input).Count(c => c != 0);

    public object PartTwo(string input) => MatchCounts(input).Sum();

    IEnumerable<long> MatchCounts(string input) {
        var blocks = input.Split("\n\n");
        var towels = blocks[0].Split(", ");
        return 
            from pattern in  blocks[1].Split("\n") 
            select MatchCount(towels, pattern, new Cache());
    }

    // computes the number of ways the pattern can be build up from the towels. 
    // works recursively by matching the prefix of the pattern with each towel.
    // a full match is found when the pattern becomes empty. the cache is applied 
    // to _drammatically_ speed up execution
    long MatchCount(string[] towels, string pattern, Cache cache) =>
        cache.GetOrAdd(pattern, (pattern) => 
            pattern switch {
                "" => 1,
                _  =>  towels
                    .Where(pattern.StartsWith)
                    .Sum(towel => MatchCount(towels, pattern[towel.Length ..], cache))
            }
        );
}