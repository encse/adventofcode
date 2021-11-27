using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2016.Day15;

[ProblemName("Timing is Everything")]
class Solution : Solver {

    public object PartOne(string input) => Iterate(Parse(input)).First(v => v.ok).t;

    public object PartTwo(string input) => Iterate(Parse(input).Concat(new []{(pos: 0, mod: 11)}).ToArray()).First(v => v.ok).t;

    (int pos, int mod)[] Parse(string input) => (
            from line in input.Split('\n')
            let m = Regex.Match(line, @"Disc #\d has (\d+) positions; at time=0, it is at position (\d+).")
            select (pos: int.Parse(m.Groups[2].Value), mod: int.Parse(m.Groups[1].Value))
        ).ToArray();

    IEnumerable<(int t, bool ok)> Iterate((int pos, int mod)[] discs) {
        for (int t = 0; ; t++) {
            var ok = Enumerable.Range(0, discs.Length)
                .All(i => (discs[i].pos + t + i + 1) % discs[i].mod == 0);
            yield return (t, ok);
        }
    }
}
