using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Y2016.Day18 {

    class Solution : Solver {

        public string GetName() => "Like a Rogue";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => SafeCount(input, 40);

        int PartTwo(string input) => SafeCount(input, 400000);

        int SafeCount(string input, int lines) {
            var rowPrev = input;
            var safeCount = rowPrev.Count(ch => ch == '.');
            for (int i = 0; i < lines - 1; i++) {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < rowPrev.Length; j++) {
                    var leftTrap = j != 0 && rowPrev[j - 1] == '^';
                    var centerTrap = rowPrev[j] == '^';
                    var rightTrap = j != rowPrev.Length - 1 && rowPrev[j + 1] == '^';

                    var trap =
                        (leftTrap && centerTrap && !rightTrap) ||
                        (!leftTrap && centerTrap && rightTrap) ||
                        (leftTrap && !centerTrap && !rightTrap) ||
                        (!leftTrap && !centerTrap && rightTrap)
                        ;
                    sb.Append(trap ? '^' : '.');
                }
                rowPrev = sb.ToString();
                safeCount += rowPrev.Count(ch => ch == '.');
            }

            return safeCount;
        }
    }
}