namespace AdventOfCode.Y2025.Day03;

using System;
using System.Linq;

[ProblemName("Lobby")]
class Solution : Solver {

    public object PartOne(string input) => MaxJoltSum(input, 2);
    public object PartTwo(string input) => MaxJoltSum(input, 12);

    public long MaxJoltSum(string input, int batteryCount) =>
        input.Split("\n").Select(bank => MaxJolt(bank, batteryCount)).Sum();

    long MaxJolt(string bank, int batteryCount) {
        if (batteryCount == 0) {
            return 0;
        }
        if (bank.Length < batteryCount) {
            return -1;
        }
        for (int jolt = 9; jolt >= 0; jolt--) {
            var index = bank.IndexOf((char)(jolt + '0'));
            if (index >= 0) {
                var res = MaxJolt(bank[(index + 1)..], batteryCount - 1);
                if (res >= 0) {
                    return jolt * (long)Math.Pow(10, batteryCount-1) + res;
                }
            }
        }
        throw new Exception();
    }
}