using System;
using System.Linq;

namespace AdventOfCode.Y2023.Day09;

[ProblemName("Mirage Maintenance")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, ExtrapolateRight);
    public object PartTwo(string input) => Solve(input, ExtrapolateLeft);

    long Solve(string input, Func<long[], long> extrapolate) =>
        input.Split("\n").Select(ParseNumbers).Select(extrapolate).Sum();

    long[] ParseNumbers(string line) =>
        line.Split(" ").Select(long.Parse).ToArray();

    // It's a common trick to zip a sequence with the skipped version of itself
    long[] Diff(long[] numbers) =>
        numbers.Zip(numbers.Skip(1)).Select(p => p.Second - p.First).ToArray();

    // I went a bit further and recurse until there are no numbers left. It's
    // more compact this way and doesn't affect the runtime much.
    long ExtrapolateRight(long[] numbers) =>
        !numbers.Any() ? 0 : ExtrapolateRight(Diff(numbers)) + numbers.Last();

    long ExtrapolateLeft(long[] numbers) =>
       !numbers.Any() ? 0 : numbers.First() - ExtrapolateLeft(Diff(numbers));
}
