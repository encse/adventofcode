using System.Linq;

namespace AdventOfCode.Y2021.Day14;

[ProblemName("Extended Polymerization")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, 10);
    public object PartTwo(string input) => Solve(input, 40);

    long Solve(string input, int steps) {

        var blocks = input.Split("\n\n");

        // we will start with this polymer:
        var polymer = blocks[0];

        // replacement generates two molecules from one:
        var generatedMolecules = (
            from line in blocks[1].Split("\n")
            let parts = line.Split(" -> ")
            select (molecule: parts[0], element: parts[1])
        ).ToDictionary(
            p => p.molecule,
            p => new string[] { p.molecule[0] + p.element, p.element + p.molecule[1] }
        );

        // cut the polymer into molecules, to get initial count:
        var moleculeCount = (
            from i in Enumerable.Range(0, polymer.Length - 1)

            let molecule = polymer.Substring(i, 2)
            
            group molecule by molecule into g
            
            select (molecule: g.Key, count: (long)g.Count())
        ).ToDictionary(p => p.molecule, p => p.count);

        // update molecule count in each step:
        for (var i = 0; i < steps; i++) {
            moleculeCount = (
                from kvp in moleculeCount
                let molecule = kvp.Key 
                let count = kvp.Value
                from generatedMolecule in generatedMolecules[molecule]
                group count by generatedMolecule into g
                select (newMolecule: g.Key, count: g.Sum())
            ).ToDictionary(g => g.newMolecule, g => g.count);
        }

        // get occurrences by element, it's enough to take just one end of each molecule:
        var elementCounts = (
            from kvp in moleculeCount
            group kvp.Value by kvp.Key[0] into g
            select (element: g.Key, count: g.Sum())
        ).ToDictionary(kvp => kvp.element, kvp => kvp.count);
        // but then, the count of the last element of the polymer is off by one:
        elementCounts[polymer.Last()]++;

        return elementCounts.Values.Max() - elementCounts.Values.Min();
    }
}
