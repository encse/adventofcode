using System;
using System.Linq;

namespace AdventOfCode.Y2022.Day04;

[ProblemName("Camp Cleanup")]
class Solution : Solver {
    public object PartOne(string input) =>  DuplicateWorkCount(input, Contains);
    public object PartTwo(string input) =>  DuplicateWorkCount(input, Overlaps);

    record struct Range(int from, int to);
    bool Contains(Range r1, Range r2) => r1.from <= r2.from && r2.to <= r1.to; 
    bool Overlaps(Range r1, Range r2) => r1.to >= r2.from && r1.from <= r2.to; 

    private int DuplicateWorkCount(string input, Func<Range, Range, bool> rangeCheck) {
        var parseRange = (string input) => 
            new Range(int.Parse(input.Split('-')[0]), int.Parse(input.Split('-')[1]));
        var parseWorkOrder = (string input) => input.Split(',').Select(parseRange);
        var parseWorkOrders = (string input) => input.Split("\n").Select(parseWorkOrder);

        return parseWorkOrders(input).Count(workOrder => 
            rangeCheck(workOrder.ElementAt(0), workOrder.ElementAt(1)) || 
            rangeCheck(workOrder.ElementAt(1), workOrder.ElementAt(0))
        );
    }
}
