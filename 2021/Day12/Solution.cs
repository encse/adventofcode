using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2021.Day12;

[ProblemName("Passage Pathing")]
class Solution : Solver {

    public object PartOne(string input) => Explore(input, false);
    public object PartTwo(string input) => Explore(input, true);

    int Explore(string input, bool part2) {
        var map = GetMap(input);

        // Recursive approach this time.
        int pathCount(string currentCave, ImmutableHashSet<string> visitedCaves, bool anySmallCaveWasVisitedTwice) {

            if (currentCave == "end") {
                return 1;
            }

            var res = 0;
            foreach (var cave in map[currentCave]) {
                var isBigCave = cave.ToUpper() == cave;
                var seen = visitedCaves.Contains(cave);

                if (!seen || isBigCave) {
                    // we can visit big caves any number of times, small caves only once
                    res += pathCount(cave, visitedCaves.Add(cave), anySmallCaveWasVisitedTwice);
                } else if (part2 && !isBigCave && cave != "start" && !anySmallCaveWasVisitedTwice) {
                    // part 2 also lets us to visit a single small cave twice (except for start and end)
                    res += pathCount(cave, visitedCaves, true);
                }
            }
            return res;
        }

        return pathCount("start", ImmutableHashSet.Create<string>("start"), false);
    }

    Dictionary<string, string[]> GetMap(string input) {
        // taking all connections 'there and back':
        var connections =
            from line in input.Split("\n")
            let parts = line.Split("-")
            let caveA = parts[0]
            let caveB = parts[1]
            from connection in new[] { (From: caveA, To: caveB), (From: caveB, To: caveA) }
            select connection;

        // grouped by "from":
        return (
            from p in connections
            group p by p.From into g
            select g
        ).ToDictionary(g => g.Key, g => g.Select(connnection => connnection.To).ToArray());
    }
}
