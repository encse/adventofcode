using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Y2021.Day14;

[ProblemName("Extended Polymerization")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, 10);
    public object PartTwo(string input) => Solve(input, 40);

    long Solve(string input, int steps) {

        var blocks = input.Split("\n\n");

        // We will start with this polymer:
        var polymer = blocks[0];

        // These are the 'molecule -> new element' rules
        var generatedElement = (
            from line in blocks[1].Split("\n")
            let parts = line.Split(" -> ")
            select (molecule: parts[0], element: parts[1])
        ).ToDictionary(p => p.molecule, p => p.element);

        
        //                        H H H H H           H
        //                      H-C-C-C-C-C- ....... -C-H
        //                        H H H H H           H

        // It's enough to maintain the molecule counts, no need to deal with them individually.

        // Cut the polymer into molecules first:
        var moleculeCount = new Dictionary<string, long>();
        foreach (var i in Enumerable.Range(0, polymer.Length - 1)) {
            var ab = polymer.Substring(i, 2);
            moleculeCount[ab] = moleculeCount.GetValueOrDefault(ab) + 1;
        }

        // Update the map in a loop:
        for (var i = 0; i < steps; i++) {
            var updated = new Dictionary<string, long>();
            foreach (var (molecule, count) in moleculeCount) {
                var (a, n, b) = (molecule[0], generatedElement[molecule], molecule[1]);
                updated[$"{a}{n}"] = updated.GetValueOrDefault($"{a}{n}") + count;
                updated[$"{n}{b}"] = updated.GetValueOrDefault($"{n}{b}") + count;
            }
            moleculeCount = updated;
        }

        //                        H H H H H           H
        //                      H-C-C-C-C-C- ....... -C-H
        //                        H H H H H           H

        // To count the elements consider just one end of each molecule:
        var elementCounts = new Dictionary<char, long>();
        foreach (var (molecule, count) in moleculeCount) {
            var a = molecule[0];
            elementCounts[a] = elementCounts.GetValueOrDefault(a) + count;
        }

        // The # of the closing element is off by one:
        elementCounts[polymer.Last()]++;

        return elementCounts.Values.Max() - elementCounts.Values.Min();
    }
}
