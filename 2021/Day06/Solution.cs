using System.Linq;

namespace AdventOfCode.Y2021.Day06;

[ProblemName("Lanternfish")]
class Solution : Solver {

    public object PartOne(string input) => FishCountAfterNDays(input, 80);
    public object PartTwo(string input) => FishCountAfterNDays(input, 256);

    long FishCountAfterNDays(string input, int days) {

        // group the fish by their timer, no need to deal with them one by one:
        var fishCountByInternalTimer = new long[9];
        foreach (var ch in input.Split(',')) {
            fishCountByInternalTimer[int.Parse(ch)]++;
        }
        
        // we will model a circular shift register, with an additional feedback:
        //       0123456           78 
        //   ┌──[.......]─<─(+)───[..]──┐
        //   └──────>────────┴─────>────┘
        //     reproduction     newborn

        for (var t = 0; t < days; t++) {
            fishCountByInternalTimer[(t + 7) % 9] += fishCountByInternalTimer[t % 9];
        }

        return fishCountByInternalTimer.Sum();
    }
}
