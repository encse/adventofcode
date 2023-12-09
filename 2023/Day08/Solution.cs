using System.Linq;
using System.Text.RegularExpressions;

using Map = System.Collections.Generic.Dictionary<string, (string Left, string Right)>;

namespace AdventOfCode.Y2023.Day08;

[ProblemName("Haunted Wasteland")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, "AAA", "ZZZ");
    public object PartTwo(string input) => Solve(input, "A", "Z");

    long Solve(string input, string aMarker, string zMarker) {
        var blocks = input.Split("\n\n");
        var dirs = blocks[0];
        var map = ParseMap(blocks[1]);

        // From each start node calculate the steps to the first Z node, then 
        // suppose that if we continue wandering around in the desert the 
        /// distance between the Z nodes is always the same.
        // The input was set up this way, which justifies the use of LCM in 
        // computing the final result.
        return map.Keys
            .Where(w => w.EndsWith(aMarker))
            .Select(w => StepsToZ(w, zMarker, dirs, map))
            .Aggregate(1L, Lcm);
    }

    long Lcm(long a, long b) => a * b / Gcd(a, b);
    long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);

    long StepsToZ(string current, string zMarker, string dirs, Map map) {
        var i = 0;
        while (!current.EndsWith(zMarker)) {
            var dir = dirs[i % dirs.Length];
            current =  dir == 'L' ? map[current].Left : map[current].Right;
            i++;
        }
        return i;
    }

    Map ParseMap(string input) =>
        input.Split("\n")
            .Select(line => Regex.Matches(line, "[A-Z]+"))
            .ToDictionary(m => m[0].Value, m => (m[1].Value, m[2].Value));
}
