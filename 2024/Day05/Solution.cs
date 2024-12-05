namespace AdventOfCode.Y2024.Day05;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Print Queue")]
class Solution : Solver {

    public object PartOne(string input) {
        var (expected, updates) = Parse(input);
        return updates
            .Where(pages => Sorted(expected, pages))
            .Sum(GetMiddlePage);
    }

    public object PartTwo(string input) {
        var (expected, updates) = Parse(input);
        return updates
            .Where(pages => !Sorted(expected, pages))
            .Select(pages => Sort(expected, pages))
            .Sum(GetMiddlePage);
    }

    (HashSet<string> expected, string[][] updates) Parse(string input) {
        var parts = input.Split("\n\n");
        var expected = new HashSet<string>(parts[0].Split("\n"));
        var updates = parts[1].Split("\n").Select(line => line.Split(",")).ToArray();
        return (expected, updates);
    }
    int GetMiddlePage(string[] nums) => int.Parse(nums[nums.Length / 2]);
   
    // checks that all possible pairs in pages are in the right order
    bool Sorted(HashSet<string> expected, string[] pages) {
        var actuals = (
            from i in Enumerable.Range(0, pages.Length - 1)
            from j in Enumerable.Range(i + 1, pages.Length - i - 1)
            select pages[i] + "|" + pages[j]
        );
        return actuals.All(expected.Contains);
    }

    string[] Sort(HashSet<string> expected, string[] pages) {
        // Ideally we would do some topological sorting, but it's an overkill for today. 
        // A single call to Array.Sort solves my input, so that' how life is... I might
        // get back to this later but probably not :D
        Array.Sort(pages, (page1, page2) => expected.Contains(page1 + "|" + page2) ? -1 : 1);
        return pages;
    }
}