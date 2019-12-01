using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2015.Day18 {

    class Solution : Solver {

        public string GetName() => "Like a GIF For Your Yard";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) =>
            Enumerable.Range(0, 100).Aggregate(Parse(input), (acc, _) => Step(acc, false)).Select(row => row.Sum()).Sum();

        int PartTwo(string input) =>
            Enumerable.Range(0, 100).Aggregate(Parse(input), (acc, _) => Step(acc, true)).Select(row => row.Sum()).Sum();

        int[][] Step(int[][] input, bool stuck) {
            
            var res = new List<int[]>();
            var (crow, ccol) = (input.Length, input[0].Length);

            if (stuck) {
                input[0][0] = 1;
                input[crow - 1][0] = 1;
                input[0][ccol - 1] = 1;
                input[crow - 1][ccol - 1] = 1;
            }
            for (var irow = 0; irow < crow; irow++) {
                var row = new List<int>();
                for (var icol = 0; icol < ccol; icol++) {
                    if (stuck && 
                        ((icol == 0 && irow == 0) || (icol == ccol - 1 && irow == 0) || 
                            (icol == 0 && irow == crow - 1) || (icol == ccol - 1 && irow == crow - 1))
                    ) {
                        row.Add(1);
                    } else {
                        var neighbours =
                            (from d in new(int row, int col)[] { (-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1) }
                             let irowT = irow + d.row
                             let icolT = icol + d.col
                             where irowT >= 0 && irowT < crow && icolT >= 0 && icolT < ccol && input[irowT][icolT] == 1
                             select 1).Sum();
                        if (input[irow][icol] == 1) {
                            row.Add(new[] { 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 }[neighbours]);
                        } else {
                            row.Add(new[] { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }[neighbours]);
                        }
                    }
                }
                res.Add(row.ToArray());
            }
            return res.ToArray();
        }

        int[][] Parse(string input) =>(
                from line in input.Split('\n')
                select 
                    (from ch in line select ch == '#' ? 1 : 0).ToArray()
            ).ToArray();
    }
}