using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2015.Day17;

[ProblemName("No Such Thing as Too Much")]
class Solution : Solver {

    public object PartOne(string input) => Fill(Parse(input)).Count();
    public object PartTwo(string input) {
        var combinations = Fill(Parse(input)).ToArray();
        var shortest = combinations.Select(combination => combination.Count()).Min();
        return combinations.Count(combination => combination.Count() == shortest);
    }

    int[] Parse(string input) => input.Split('\n').Select(int.Parse).ToArray();

    IEnumerable<ImmutableList<int>> Fill(int[] containers) {
        IEnumerable<ImmutableList<int>> FillRecursive(int i, int amount) {
            if (i == containers.Length) {
                yield break;
            } else {
                if (amount == containers[i]) {
                    yield return ImmutableList.Create<int>(i);
                }
                if (amount >= containers[i]) {
                    foreach (var v in FillRecursive(i + 1, amount - containers[i])) {
                        yield return v.Add(i);
                    }
                }
                foreach (var v in FillRecursive(i + 1, amount)) {
                    yield return v;
                }
            }
        }

        return FillRecursive(0, 150);
    }
}
