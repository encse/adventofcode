namespace AdventOfCode.Y2025.Day02;

using System;
using System.Linq;
using System.Collections.Generic;

[ProblemName("Gift Shop")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, st => 2);

    public object PartTwo(string input) => Solve(input, st => st.Length);

    long Solve(string input, Func<string, int> maxRepetition) => (
        from id in GetIds(input)
        let st = id.ToString()
        where Enumerable.Range(2, maxRepetition(st)-1).Any(k => Periodic(st, k))
        select id
    ).Sum();

    public IEnumerable<long> GetIds(string input) {
        foreach (var range in input.Split(",")) {
            var parts = range.Split("-").Select(long.Parse).ToArray();
            for (var i = parts[0]; i <= parts[1]; i++) {
                yield return i;
            }
        }
    }

    // Returns true if the string consists of the same substring repeated repetitionCount times.
    // Example: repetitionCount == 3 returns true for "abcabcabc".
    bool Periodic(string st, int repetitionCount) {
        if (st.Length % repetitionCount != 0) {
            return false;
        }

        int period = st.Length / repetitionCount;
        for (int i = period; i < st.Length; i += period) {
            if (st[..period] != st[i..(i + period)]) {
                return false;
            }
        }
        return true;
    }
}