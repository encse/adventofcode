using System.Linq;

namespace AdventOfCode.Y2015.Day05;

[ProblemName("Doesn't He Have Intern-Elves For This?")]
class Solution : Solver {

    public object PartOne(string input) =>
        input.Split('\n').Count(line => {
            var threeVowels = line.Count(ch => "aeiou".Contains(ch)) >= 3;
            var duplicate = Enumerable.Range(0, line.Length - 1).Any(i => line[i] == line[i + 1]);
            var reserved = "ab,cd,pq,xy".Split(',').Any(line.Contains);
            return threeVowels && duplicate && !reserved;
        });

    public object PartTwo(string input) =>
        input.Split('\n').Count(line => {
            var appearsTwice = Enumerable.Range(0, line.Length - 1).Any(i => line.IndexOf(line.Substring(i, 2), i+2) >= 0); 
            var repeats = Enumerable.Range(0, line.Length - 2).Any(i => line[i] == line[i + 2]);
            return appearsTwice && repeats;
        });
}
