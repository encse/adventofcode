using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day05 {

    [ProblemName("Binary Boarding")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Seats(input).Max();

        int PartTwo(string input) {
            var seats = Seats(input);
            var (min, max) = (seats.Min(), seats.Max());
            return Enumerable.Range(min, max - min + 1).Single(id => !seats.Contains(id));
        }

        HashSet<int> Seats(string input) =>
            input
                .Replace("B", "1")
                .Replace("F", "0")
                .Replace("R", "1")
                .Replace("L", "0")
                .Split("\n")
                .Select(row => Convert.ToInt32(row, 2))
                .ToHashSet();
    }
}