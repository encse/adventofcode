using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Y2021.Day14;

[ProblemName("Extended Polymerization")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, 10);
    public object PartTwo(string input) => Solve(input, 40);

    long Solve(string input, int steps) {

        var blocks = input.Split("\n\n");

        // we will start with this polymer:
        var polymer = blocks[0];

        var generatedElement = (
            from line in blocks[1].Split("\n")
            let parts = line.Split(" -> ")
            select (molecule: parts[0], element: parts[1])
        ).ToDictionary(p => p.molecule, p => p.element);

        // it's enough to maintain the molecule counts in each step, no
        // need to deal with them one by one.

        // cut the polymer into molecules first:
        var moleculeCount = new Dictionary<string, long>();
        foreach (var i in Enumerable.Range(0, polymer.Length - 1)) {
            var ab = polymer.Substring(i, 2);
            moleculeCount[ab] = moleculeCount.GetValueOrDefault(ab) + 1;
        }

        // update counts in each step:
        for (var i = 0; i < steps; i++) {
            var updated = new Dictionary<string, long>();
            foreach (var (molecule, count) in moleculeCount) {
                var (a, n, b) = (molecule[0], generatedElement[molecule], molecule[1]);
                updated[$"{a}{n}"] = updated.GetValueOrDefault($"{a}{n}") + count;
                updated[$"{n}{b}"] = updated.GetValueOrDefault($"{n}{b}") + count;
            }
            moleculeCount = updated;
        }

        // now switch to element counts. It's enough to take just one 
        // end of each molecule, so that we don't count elements twice.
        var elementCounts = new Dictionary<char, long>();
        foreach (var (molecule, count) in moleculeCount) {
            var a = molecule[0];
            elementCounts[a] = elementCounts.GetValueOrDefault(a) + count;
        }
        // but then, the count of the last element of the polymer is off by one:
        elementCounts[polymer.Last()]++;

        return elementCounts.Values.Max() - elementCounts.Values.Min();
    }
}
