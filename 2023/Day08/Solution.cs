using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day08;

[ProblemName("Haunted Wasteland")]
class Solution : Solver {

    // WIP

    public object PartOne(string input) => Solve(input, "AAA", "ZZZ");
    public object PartTwo(string input) => Solve(input, "A..", "..Z");

    long Solve(string input, string start, string end) {
        var parts = input.Split("\n\n");
        var dirs = parts[0];
        var map = new Dictionary<string, (string, string)>();
        foreach (var line in parts[1].Split("\n")) {
            var m = Regex.Matches(line, "[A-Z]+").ToArray();
            map[m[0].Value] = (m[1].Value, m[2].Value);
        }

        var res = 1L;
        foreach(var st in map.Keys) {
            if (Regex.IsMatch(st, start)) {
                res = Lcm(res, Steps(st, end, dirs, map));
            }
        }
        return res;
    }

    long Lcm(long a, long b) => a * b / Gcd(a, b);
    long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);

    long Steps(string st, string end, string dirs, Dictionary<string, (string, string)> map) {
        var i = 0;
        while (!Regex.IsMatch(st, end)) {
            var dir = dirs[i % dirs.Length];
            if (dir == 'L') {
                st = map[st].Item1;
            } else {
                st = map[st].Item2;
            }
            i++;
        }
        return i;
    }
}
