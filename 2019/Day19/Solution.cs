using System;
using System.Linq;

namespace AdventOfCode.Y2019.Day19;

[ProblemName("Tractor Beam")]
class Solution : Solver {

    Func<int, int, bool> Detector(string input) {
        var icm = new ImmutableIntCodeMachine(input);
        return (int x, int y) => {
            var (_, output) = icm.Run(x, y);
            return output[0] == 1;
        };
    }

    public object PartOne(string input) {
        var detector = Detector(input);
        return (from x in Enumerable.Range(0, 50)
                from y in Enumerable.Range(0, 50)
                where detector(x, y)
                select 1).Count();
    }

    public object PartTwo(string input) {

        var detector = Detector(input);

        var (xStart, y) = (0, 100);
        while (true) {
            while (!detector(xStart, y)) {
                xStart++;
            }
            var x = xStart;
            while (detector(x + 99, y)) {
                if (detector(x, y + 99) && detector(x + 99, y + 99)) {
                    return (x * 10000 + y);
                }
                x++;
            }
            y++;
        }

    }
}
