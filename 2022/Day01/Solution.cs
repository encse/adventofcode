using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day01;

[ProblemName("Calorie Counting")]      
class Solution : Solver {

    public object PartOne(string input) => 
        GetCaloriesPerElf(input).First();

    public object PartTwo(string input) =>
        GetCaloriesPerElf(input).Take(3).Sum();

    // Returns the calories carried by the elves in descending
    // order.
    private IEnumerable<int> GetCaloriesPerElf(string input) =>
        from elf in input.Split("\n\n")
        let calories = elf.Split('\n').Select(int.Parse).Sum()
        orderby calories descending
        select calories;
}
