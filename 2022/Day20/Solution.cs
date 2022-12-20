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

        for (var i = 0; i < iterations; i++) {
            for (var inum = 0; inum < nums.Length; inum++) {
                var num = nums[inum];

                if (num < 0) {
                    // negative numbers can be thought of as positive numbers
                    // we don't want to implement moving in both directions
                    // so let's convert it to a positive number here 
                    num = -num % (nums.Length - 1);
                    num = (nums.Length - 1) - num;
                } else {
                    num = num % (nums.Length - 1);
                }

                // position of inum in the permuted array:
                var iperm = Array.IndexOf(perm, inum);

                // should be moved to the right by 'num' steps with wrap around:
                for (var j = 0; j < num; j++) {
                    var ipermNext = (iperm + 1) % nums.Length;
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
