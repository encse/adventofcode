using System.Linq;
using System.Text;

namespace AdventOfCode.Y2016.Day18;

[ProblemName("Like a Rogue")]
class Solution : Solver {

    public object PartOne(string input) => SafeCount(input, 40);

    public object PartTwo(string input) => SafeCount(input, 400000);

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
