using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day23;

[ProblemName("Crab Cups")]
class Solution : Solver {

    public object PartOne(string input) =>
        string.Join("", Solve(input, 9, 100).Take(8));

    public object PartTwo(string input) {
        var labels = Solve(input, 1000000, 10000000).Take(2).ToArray();
        return labels[0] * labels[1];
    }

    private IEnumerable<long> Solve(string input, int maxLabel, int rotate) {
        var digits = input.Select(d => int.Parse(d.ToString())).ToArray();

        // A compact linked list representation. The cup's label can be used as the index into the array. 
        int[] next = Enumerable.Range(1, maxLabel + 1).ToArray();
        next[0] = -1; // not used

        for (var i = 0; i < digits.Length; i++) {
            next[digits[i]] = digits[(i + 1) % digits.Length];
        }

        if (maxLabel > input.Length) {
            next[maxLabel] = next[digits.Last()];
            next[digits.Last()] = input.Length + 1;
        }

        var current = digits.First();

        for (var i = 0; i < rotate; i++) {
            var removed1 = next[current];
            var removed2 = next[removed1];
            var removed3 = next[removed2];
            next[current] = next[removed3];

            // omg
            var destination = current;
            do destination = destination == 1 ? maxLabel : destination - 1;
            while (destination == removed1 || destination == removed2 || destination == removed3);

            next[removed3] = next[destination];
            next[destination] = removed1;
            current = next[current];
        }

        // return the labels starting from the first cup.
        var cup = next[1];
        while (true) {
            yield return cup;
            cup = next[cup];
        }
    }
}
