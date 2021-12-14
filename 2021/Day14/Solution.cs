using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day14;

[ProblemName("Extended Polymerization")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, 10);
    public object PartTwo(string input) => Solve(input, 40);

    long Solve(string input, int steps) {
        var (polymer, elements, polymerFromMolecule) = Parse(input);

        var cache = new Dictionary<(string, int, char), long>();

        // getElementCountAfterNSteps: Determines how many atoms of the element 
        // are present after N steps, if we are starting from the given polymer. 

        // This function needs to be cached or it will never terminate.

        long getElementCountAfterNSteps(string polymer, int steps, char element) {
            var key = (polymer, steps, element);
            if (!cache.ContainsKey(key)) {
                long res = 0L;
                if (steps == 0) { 
                    // no more steps to do, just count the element in the polymer:
                    res = polymer.Count(ch => ch == element);
                } else {

                    // The idea is that we can go over the molecules in the polymer, 
                    // and deal with them one by one. Do one replacement, recurse and
                    // sum the element counts:

                    for (var j = 0; j < polymer.Length - 1; j++) {
                        var molecule = polymer.Substring(j, 2);

                        var count = getElementCountAfterNSteps(
                            polymerFromMolecule[molecule], 
                            steps - 1, 
                            element
                        );

                        // if the molecule ends with the searched element, the next one will include it as well,
                        // we shouldn't count it twice:
                        if (element == molecule[1] && j < polymer.Length - 2) {
                            count--; 
                        }

                        res += count;
                    }
                }
                cache[key] = res;
            }
            return cache[key];
        }

        // using the above helper, we can just ask for the count of each element, and quickly compute the answer
        var elementCountsAtTheEnd = (
            from element in elements
            select getElementCountAfterNSteps(polymer, steps, element)
        ).ToArray();

        return elementCountsAtTheEnd.Max() - elementCountsAtTheEnd.Min();
    }

    (string polymer, HashSet<char> elements, Dictionary<string, string> polymerFromMolecule) Parse(string input) {
        var blocks = input.Split("\n\n");

        // we will start from this polymer
        var polymer = blocks[0];

        // the map contains the resulted polymer after the replacement, not just the inserted element:
        var polymerFromMolecule = (
            from line in blocks[1].Split("\n")
            let parts = line.Split(" -> ")
            select (molecule: parts[0], element: parts[1])
        ).ToDictionary(p => p.molecule, p => p.molecule[0] + p.element + p.molecule[1]);

        // set of all elements for convenience:
        var elements = polymerFromMolecule.Keys.Select(molecule => molecule[0]).ToHashSet();

        return (polymer, elements, polymerFromMolecule);
    }
}
