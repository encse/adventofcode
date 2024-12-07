namespace AdventOfCode.Y2024.Day07;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[ProblemName("Bridge Repair")]
class Solution : Solver {

    public object PartOne(string input) => Filter(input, Check1).Sum();
    public object PartTwo(string input) => Filter(input, Check2).Sum();

    private IEnumerable<long> Filter(string input, Func<long,long,List<long>, bool> check) => 
        from line in input.Split("\n")
            let parts = Regex.Matches(line, @"\d+").Select(m=>long.Parse(m.Value))
            let target = parts.First()
            let nums = parts.Skip(1).ToList()
        where check(target, 0, nums)
        select target;

    private bool Check1(long target, long acc, List<long> nums) =>
        nums switch {
            [] => target == acc,
            _  => Check1(target, acc * nums[0], nums[1..]) ||
                  Check1(target, acc + nums[0], nums[1..])
        };

    private bool Check2(long target, long acc, List<long> nums) =>
        nums switch {
            [] => target == acc,
            _  => Check2(target, acc * nums[0], nums[1..]) ||
                  Check2(target, acc + nums[0], nums[1..]) ||
                  Check2(target, long.Parse($"{acc}{nums[0]}"), nums[1..])
        };
}