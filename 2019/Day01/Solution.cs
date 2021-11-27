using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day01;

[ProblemName("The Tyranny of the Rocket Equation")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, false);
    public object PartTwo(string input) => Solve(input, true);

    int Solve(string input, bool recursive) {
        var weights = new Queue<int>(input.Split("\n").Select(x => int.Parse(x)));
        var res = 0;
        while (weights.Any()) {
            var weight = weights.Dequeue();
            var fuel = (int)(Math.Floor(weight / 3.0) - 2);
            if (fuel > 0) {
                if (recursive) {
                    weights.Enqueue(fuel);
                }
                res += fuel;
            }
        }
        return res;
    }
}
