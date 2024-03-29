using System.Linq;

namespace AdventOfCode.Y2018.Day02;

[ProblemName("Inventory Management System")]
class Solution : Solver {

    public object PartOne(string input) {
        var doubles = (
            from line in input.Split("\n")
            where CheckLine(line, 2)
            select line
        ).Count();
        var tripples = (
            from line in input.Split("\n")
            where CheckLine(line, 3)
            select line
        ).Count();
        return doubles * tripples;
    }

    bool CheckLine(string line, int n) {
        return (from ch in line
                group ch by ch into g
                select g.Count()).Any(cch => cch == n);
    }

    public object PartTwo(string input) {
        var lines = input.Split("\n");
        return (from i in Enumerable.Range(0, lines.Length)
                from j in Enumerable.Range(i + 1, lines.Length - i - 1)
                let line1 = lines[i]
                let line2 = lines[j]
                where Diff(line1, line2) == 1
                select Common(line1, line2)
        ).Single();
    }

    int Diff(string line1, string line2) {
        return line1.Zip(line2, 
            (chA, chB) => chA == chB
        ).Count(x => x == false);
    }

    string Common(string line1, string line2) {
        return string.Join("", line1.Zip(line2, (chA, chB) => chA == chB ? chA.ToString() : ""));
    }
}
