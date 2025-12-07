namespace AdventOfCode.Y2025.Day07;

using System.Linq;

[ProblemName("Laboratories")]
class Solution : Solver {

    public object PartOne(string input) => RunManifold(input).splits;

    public object PartTwo(string input) => RunManifold(input).timelines;
    
    public (int splits, long timelines) RunManifold(string input) {
        // Dynamic programming over the grid:
        //
        // Each cell in row i depends only on the values from row i-1 directly above it.
        // We propagate a vector of "timeline counts" downward row by row instead of
        // keeping a full 2D DP table, which reduces memory to O(columns).
        //
        // At forks ('^'), a timeline splits into left/right branches, and we count a
        // "split" whenever an active timeline actually forks (>0 incoming paths).

        var lines = input.Split("\n").Select(line => line.ToCharArray()).ToArray();
        var crow = lines.Length;
        var ccol = lines[0].Length;
        var splits = 0;
        var timelines = new long[ccol];

        for (int irow = 0; irow < crow; irow++) {
            var nextTimelines = new long[ccol];
            for (var icol = 0; icol < ccol; icol++) {
                if (lines[irow][icol] == 'S') {
                    nextTimelines[icol] = 1;
                } else if (lines[irow][icol] == '^') {
                    splits += timelines[icol] > 0 ? 1 : 0;
                    nextTimelines[icol - 1] += timelines[icol];
                    nextTimelines[icol + 1] += timelines[icol];
                } else {
                    nextTimelines[icol] += timelines[icol];
                }
            }
            timelines = nextTimelines;
        }
        return (splits, timelines.Sum());
    }
}