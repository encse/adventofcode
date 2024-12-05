namespace AdventOfCode.Y2024.Day05;

using System.Collections.Generic;
using System.Linq;

[ProblemName("Print Queue")]
class Solution : Solver {

    public object PartOne(string input) {
        var (updates, comparer) = Parse(input);
        return updates
            .Where(pages => Sorted(pages, comparer))
            .Sum(GetMiddlePage);
    }

    public object PartTwo(string input) {
        var (updates, comparer) = Parse(input);
        return updates
            .Where(pages => !Sorted(pages, comparer))
            .Select(pages => pages.OrderBy(p => p, comparer).ToArray())
            .Sum(GetMiddlePage);
    }

    (string[][] updates, Comparer<string>) Parse(string input) {
        var parts = input.Split("\n\n");

        var ordering = new HashSet<string>(parts[0].Split("\n"));
        var comparer = 
            Comparer<string>.Create((p1, p2) => ordering.Contains(p1 + "|" + p2) ? -1 : 1);

        var updates = parts[1].Split("\n").Select(line => line.Split(",")).ToArray();
        return (updates, comparer);
    }
    int GetMiddlePage(string[] nums) => int.Parse(nums[nums.Length / 2]);
   
    bool Sorted(string[] pages, Comparer<string> comparer) =>
        Enumerable.SequenceEqual(pages, pages.OrderBy(x=>x, comparer));

}