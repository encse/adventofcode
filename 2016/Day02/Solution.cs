using System;
using System.Collections.Generic;

namespace AdventOfCode.Y2016.Day02 {

    [ProblemName("Bathroom Security")]
    class Solution : Solver {

        public object PartOne(string input) => Solve(input, "123\n456\n789");
        public object PartTwo(string input) => Solve(input, "  1  \n 234 \n56789\n ABC \n  D  ");

        string Solve(string input, string keypad) {
            var res = "";
            var lines = keypad.Split('\n');
            var (crow, ccol) = (lines.Length, lines[0].Length);
            var (irow, icol) = (crow / 2, ccol / 2);
            foreach (var line in input.Split('\n')) {
                foreach (var ch in line) {
                    var (drow, dcol) = ch switch {
                        'U' => (-1, 0),
                        'D' =>  (1, 0),
                        'L' => (0, -1),
                        'R' => (0, 1),
                        _ => throw new ArgumentException()
                    };

                    var (irowT, icolT) = (irow + drow, icol + dcol);
                    if (irowT >= 0 && irowT < crow && icolT >= 0 && icolT < ccol && lines[irowT][icolT] != ' ') {
                        (irow, icol) = (irowT, icolT);
                    }
                }
                res += lines[irow][icol];
            }
            return res;
        }
    }
}