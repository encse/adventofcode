using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2016.Day02 {

    class Solution : Solver {

        public string GetName() => "Bathroom Security";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        string PartOne(string input) => Foo(input, "123\n456\n789");
        string PartTwo(string input) => Foo(input, "  1  \n 234 \n56789\n ABC \n  D  ");

        string Foo(string input, string keypad) {
            var res = "";
            var lines = keypad.Split('\n');
            var (crow, ccol) = (lines.Length, lines[0].Length);
            var (irow, icol) = (crow / 2, ccol / 2);
            foreach (var line in input.Split('\n')) {
                foreach (var ch in line) {
                    var (drow, dcol) = (0, 0);
                    switch (ch) {
                        case 'U': (drow, dcol) = (-1, 0); break;
                        case 'D': (drow, dcol) = (1, 0); break;
                        case 'L': (drow, dcol) = (0, -1); break;
                        case 'R': (drow, dcol) = (0, 1); break;
                    }
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