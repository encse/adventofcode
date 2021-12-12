using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day12;

[ProblemName("Passage Pathing")]
class Solution : Solver {

    public object PartOne(string input) => Explore(input, false);
    public object PartTwo(string input) => Explore(input, true);

    int Explore(string input, bool part2) {
        var map = GetMap(input);

        // Take the recursive approach this time.
        int pathCount(string currentCave, HashSet<string> visitedCaves, bool anySmallCaveWasVisitedTwice) {

            if (currentCave == "end") {
                return 1;
            }

            var res = 0;
            foreach (var cave in map[currentCave]) {
                // we can visit big caves any number of times, small caves only once
                // in part 2 we are allowed to visit a single small cave twice (except for start and end)

                var bigCave = cave.ToUpper() == cave;
                var smallCave = !bigCave && cave != "start" && cave != "end";
                var seen = visitedCaves.Contains(cave);

                if (bigCave || !seen) {
                    visitedCaves.Add(cave);
                    res += pathCount(cave, visitedCaves, anySmallCaveWasVisitedTwice);
                    visitedCaves.Remove(cave);
                } else if (part2 && smallCave && !anySmallCaveWasVisitedTwice) {
                    res += pathCount(cave, visitedCaves, true);
                }
            }
            return res;
        }

        return pathCount("start", new HashSet<string> { "start" }, false);
    }

    Dictionary<string, List<string>> GetMap(string input) {
        // taking all connections there and back:
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
        ).ToDictionary(g => g.Key, g => g.Select(connnection => connnection.To).ToList());
    }
}
