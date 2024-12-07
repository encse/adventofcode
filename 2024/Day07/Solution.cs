namespace AdventOfCode.Y2024.Day07;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[ProblemName("Bridge Repair")]
class Solution : Solver {

    public object PartOne(string input) => Filter(input, Check1).Sum();
    public object PartTwo(string input) => Filter(input, Check2).Sum();

    // returns those calibrations that are valid according to the checker
    private IEnumerable<long> Filter(string input, Func<long,long,List<long>, bool> check) => 
        from line in input.Split("\n")
            let parts = Regex.Matches(line, @"\d+").Select(m=>long.Parse(m.Value))
            let target = parts.First()
            let nums = parts.Skip(1).ToList()
        where check(target, 0, nums)
        select target;

    // separate checkers provided for the two parts, these recursive functions go
    // over the numbers and use all alloved operators to update the accumulated result.
    // at the end of the recursion we simply check if we reached the target
    private bool Check1(long target, long acc, List<long> nums) =>
        nums switch {
            [] => target == acc,
            _  => Check1(target, acc * nums[0], nums[1..]) ||
                  Check1(target, acc + nums[0], nums[1..])
        };

    private bool Check2(long target, long acc, List<long> nums) =>
        nums switch {
            _ when acc > target => false, // optimization: early exit from deadend
            [] => target == acc,
            _  => Check2(target, long.Parse($"{acc}{nums[0]}"), nums[1..]) ||
                  Check2(target, acc * nums[0], nums[1..]) ||
                  Check2(target, acc + nums[0], nums[1..])
        };
}
