namespace AdventOfCode.Y2025.Day01;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Secret Entrance")]
class Solution : Solver {

    public object PartOne(string input) => Dial(Parse1(input)).Count(x => x == 0);
    public object PartTwo(string input) => Dial(Parse2(input)).Count(x => x == 0);

    IEnumerable<int> Dial(IEnumerable<int> rotations) {
        int pos = 50;
        foreach (var rotation in rotations) {
            pos = (pos + rotation) % 100;
            yield return pos;
        }
    }

    IEnumerable<int> Parse1(string input) =>
        from line in input.Split("\n")
        let d = line[0] == 'R' ? 1 : -1
        let a = int.Parse(line.Substring(1))
        select a * d;

    IEnumerable<int> Parse2(string input) =>
        from line in input.Split("\n")
        let d = line[0] == 'R' ? 1 : -1
        let a = int.Parse(line.Substring(1))
        from i in Enumerable.Range(0, a)
        select d;
}
