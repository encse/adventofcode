namespace AdventOfCode.Y2025.Day06;

using System.Collections.Generic;
using System.Linq;

record Problem(char op, IEnumerable<long> nums);

[ProblemName("Trash Compactor")]
class Solution : Solver {

    // Split the input to separate blocks first, then parse them row by row
    // in part one. We can deal with part two by applying Transpose to the blocks
    // first. The operator moves from the bottom-left corner to the top-right:

    //  "15"              "1 +"
    //  " 5"   becomes    "55 "
    //  "+ "

    public object PartOne(string input) => Solve(
        from rows in ParseBlocks(input)
        select new Problem(
            rows.Last()[0],
            rows[..^1].Select(long.Parse)
        )
    );
    public object PartTwo(string input) => Solve(
        from cols in ParseBlocks(input).Select(Transpose)
        select new Problem(
            cols.First()[^1],
            cols.Select(col => long.Parse(col[..^1]))
        )
    );

    long Solve(IEnumerable<Problem> problems) {
        var res = 0L;
        foreach (var problem in problems) {
            if (problem.op == '+') {
                res += problem.nums.Sum();
            } else {
                res += problem.nums.Aggregate(1L, (a, b) => a * b);
            }
        }
        return res;
    }

    IEnumerable<string[]> ParseBlocks(string input) {
        var lines = input.Split("\n");
        int ccol = lines[0].Length;
        var blockStart = 0;
        for (int icol = 0; icol < ccol; icol++) {
            if (GetColumn(lines, icol).Trim() == "") {
                yield return GetBlock(lines, blockStart, icol);
                blockStart = icol+1;
            }
        }
        yield return GetBlock(lines, blockStart, ccol);
    }

    string[] GetBlock(string[] lines, int icolFrom, int icolTo) => (
        from line in lines
        select line[icolFrom..icolTo]
    ).ToArray();

    string[] Transpose(string[] lines) => (
        from icol in Enumerable.Range(0, lines[0].Length)
        select GetColumn(lines, icol)
    ).ToArray();

    string GetColumn(string[] lines, int icol) =>
        string.Join("", from line in lines select line[icol]);
}