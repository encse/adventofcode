using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2016.Day20;

[ProblemName("Firewall Rules")]
class Solution : Solver {

    public object PartOne(string input) {
        var k = 0L;
        foreach (var range in Parse(input)) {
            if (k < range.min) {
                break;
            } else if (range.min <= k && k <= range.max) {
                k = range.max + 1;
            }
        }
        return k;
    }

    public object PartTwo(string input) {
        var k = 0L;
        var sum = 0L;
        foreach (var range in Parse(input)) {
            if (k < range.min) {
                sum += range.min - k;
                k = range.max + 1;
            } else if (range.min <= k && k <= range.max) {
                k = range.max + 1;
            }
        }

        var lim = 4294967296L;
        if (lim > k) {
            sum += lim - k;
        }
        return sum;
    }

    IEnumerable<(long min, long max)> Parse(string input) => (
            from line in input.Split('\n')
            let parts = line.Split('-')
            let min = long.Parse(parts[0])
            let max = long.Parse(parts[1])
            orderby min
            select (min, max)
        ).AsEnumerable();
}
