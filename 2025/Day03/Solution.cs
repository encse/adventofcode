namespace AdventOfCode.Y2025.Day03;

using System.Linq;

[ProblemName("Lobby")]
class Solution : Solver {

    public object PartOne(string input) => MaxJoltSum(input, 2);
    public object PartTwo(string input) => MaxJoltSum(input, 12);

    public long MaxJoltSum(string input, int batteryCount) =>
        input.Split("\n").Select(bank => MaxJolt(bank, batteryCount)).Sum();

    long MaxJolt(string bank, int batteryCount) {
        long res = 0L;
        for (; batteryCount > 0; batteryCount--) {
            // jump forward to the highest available digit in the bank, but keep 
            // batteryCount digits in the suffix, so that we can select something 
            // for those remaining batteries as well.
            char jolt = bank[..^(batteryCount - 1)].Max();
            bank = bank[(bank.IndexOf(jolt) + 1)..];
            res = 10 * res + (jolt - '0');
        }
        return res;
    }
}