using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day01;

[ProblemName("Report Repair")]
class Solution : Solver {

    public object PartOne(string input) {
        var numbers = Numbers(input);
        return (
            from x in numbers 
            let y = 2020 - x
            where numbers.Contains(y)
            select x * y
        ).First();
    }

    public object PartTwo(string input) {
        var numbers = Numbers(input);
        return (
            from x in numbers 
            from y in numbers 
            let z = 2020 - x - y
            where numbers.Contains(z)
            select x * y * z
        ).First();
    }

    HashSet<int> Numbers(string input) {
        return input.Split('\n').Select(int.Parse).ToHashSet<int>();
    }
}
