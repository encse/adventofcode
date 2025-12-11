namespace AdventOfCode.Y2025.Day11;

using System;
using System.Collections.Generic;
using System.Linq;

record State(string node, string path);

[ProblemName("Reactor")]
class Solution : Solver {

    public object PartOne(string input) => 
        PathCount(Parse(input), "you", "out", new Dictionary<string, long>());
        
    public object PartTwo(string input) {
        var g = Parse(input);
        return 
            PathCount(g, "svr", "fft", new Dictionary<string, long>()) *
            PathCount(g, "fft", "dac", new Dictionary<string, long>()) *
            PathCount(g, "dac", "out", new Dictionary<string, long>()) +

            PathCount(g, "svr", "dac", new Dictionary<string, long>()) *
            PathCount(g, "dac", "fft", new Dictionary<string, long>()) *
            PathCount(g, "fft", "out", new Dictionary<string, long>());

    }

    long PathCount(
        Dictionary<string, string[]> g,
        string from, string to,
        Dictionary<string, long> cache
    ) {
        if (!cache.ContainsKey(from)) {
            if (from == to) {
                cache[from] = 1;
            } else {
                var res = 0L;
                foreach (var next in g.GetValueOrDefault(from) ?? []) {
                    res += PathCount(g, next, to, cache);
                }
                cache[from] = res;
            }
        }
        return cache[from];
    }
    Dictionary<string, string[]> Parse(string input) => (
        from line in input.Split("\n")
        let parts = line.Split(" ").ToArray()
        let frm = parts[0].TrimEnd(":").ToString()
        let to = parts[1..].ToArray()
        select new KeyValuePair<string, string[]>(frm, to)
    ).ToDictionary();
}