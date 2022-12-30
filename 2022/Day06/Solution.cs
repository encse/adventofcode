using System;
using System.Linq;

namespace AdventOfCode.Y2022.Day06;

[ProblemName("Tuning Trouble")]
class Solution : Solver {

    public object PartOne(string input) => StartOfBlock(input, 4);
    public object PartTwo(string input) => StartOfBlock(input, 14);

    // Slides a window of length l over the input and finds the first position
    // where each character is different. Returns the right of the window.
    int StartOfBlock(string input, int l) =>
         Enumerable.Range(l, input.Length)
            .First(i => input.Substring(i - l, l).ToHashSet().Count == l);
}
