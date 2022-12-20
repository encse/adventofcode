using System;
using System.Linq;

namespace AdventOfCode.Y2022.Day20;

[ProblemName("Grove Positioning System")]
class Solution : Solver {

    public object PartOne(string input) => GetGrooveCoordinates(1, 1, input);
    public object PartTwo(string input) => GetGrooveCoordinates(10, 811589153L, input);

    long GetGrooveCoordinates(int iterations, long multiplier, string input) {
        var nums = input
            .Split("\n")
            .Select(line => multiplier * long.Parse(line))
            .ToArray();

        // permuted array position -> numbers array position
        // e.g. perm[5] means: 
        //    element 5 of the perm array can be found at perm[5] in the nums array
        var perm = Enumerable.Range(0, nums.Length).ToArray();

        for (var iter = 0; iter < iterations; iter++) {
            for (var inum = 0; inum < nums.Length; inum++) {

                // ith num should be moved to the right by 'num' steps with wrap around:
                var iperm = Array.IndexOf(perm, inum);
                var steps = nums[inum] % (nums.Length - 1);
                var dir = Math.Sign(steps);
                
                for (var i = 0; i != steps; i += dir) {
                    var ipermNext = (iperm + dir + nums.Length) % nums.Length;
                    (perm[ipermNext], perm[iperm]) = (perm[iperm], perm[ipermNext]);
                    iperm = ipermNext;
                }
            }
        }

        nums = perm.Select(inum => nums[inum]).ToArray();
        var idx0 = Array.IndexOf(nums, 0);
        return (
            nums[(idx0 + 1000) % nums.Length] +
            nums[(idx0 + 2000) % nums.Length] +
            nums[(idx0 + 3000) % nums.Length]
        );
    }
}
