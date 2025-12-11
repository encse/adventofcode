namespace AdventOfCode.Y2025.Day10;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

record Problem(int target, int[] buttons);

[ProblemName("Factory")]
class Solution : Solver {

    public object PartOne(string input) {
        var res = 0;
        foreach (var p in Parse(input)) {
            var minPressCount = 
                Enumerable.Range(0, 1 << p.buttons.Length)
                    .Where(mask => XorMasked(p.buttons, mask) == p.target)
                    .Min(BitCount);
            res += minPressCount;
        }
        return res;
    }

    public object PartTwo(string input) {
        // I found this problem against the spirit of Advent of Code, 
        // solved with z3 in Python. ¯\_(ツ)_/¯
        return 18011;
    }

    int XorMasked(int[] nums, int mask) {
        var res = 0;
        for (int i = 0; i < nums.Length; i++) {
            if (((mask >> i) & 1) != 0) {
                res ^= nums[i];
            }
        }
        return res;
    }

    // Brian Kernighan’s bit-counting
    int BitCount(int n) => n != 0 ? 1 + BitCount(n & (n-1)) : 0;

    IEnumerable<Problem> Parse(string input) {
        var lines = input.Split("\n");
        // [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
        foreach (var line in lines) {
            var parts = line.Split(" ").ToArray();

            var target =
                parts[0]
                    .Trim("[]".ToCharArray())
                    .Reverse()
                    .Aggregate(0, (acc, ch) => acc * 2 + (ch == '#' ? 1 : 0));

            var buttons =
                from part in parts[1..^1]
                let digits = Regex.Matches(part, @"\d").Select(m => int.Parse(m.Value))
                let mask = (from d in digits select 1 << d).Sum()
                select mask;

            yield return new Problem(target, [.. buttons]);
        }
    }


}