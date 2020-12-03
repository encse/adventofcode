using System.Collections.Generic;

namespace AdventOfCode.Y2020.Day03 {

    [ProblemName("Toboggan Trajectory")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => TreeCount(input, (1, 3));
        long PartTwo(string input) => TreeCount(input, (1, 1), (1, 3), (1, 5), (1, 7), (2, 1));

        long TreeCount(string input, params (int drow, int dcol)[] slopes) {
            var lines = input.Split("\n");
            var (crow, ccol) = (lines.Length, lines[0].Length);
            var mul = 1L;

            foreach (var (drow, dcol) in slopes) {
                var (irow, icol) = (drow, dcol);
                var trees = 0;
                while (irow < crow) {
                    if (lines[irow][icol % ccol] == '#') {
                        trees++;
                    }
                    (irow, icol) = (irow + drow, icol + dcol);
                }
                mul *= trees;
            }
            return mul;
        }
    }
}