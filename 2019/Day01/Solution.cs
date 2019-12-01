using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day01 {

    class Solution : Solver {

        public string GetName() => "The Tyranny of the Rocket Equation";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Solve(input, false);
        int PartTwo(string input) => Solve(input, true);

        int Solve(string input, bool recursive) {
            var weights = new Queue<int>(input.Split("\n").Select(x => int.Parse(x)));
            var res = 0;
            while (weights.Any()) {
                var weight = weights.Dequeue();
                var fuel = (int)(Math.Floor(weight / 3.0) - 2);
                if (fuel > 0) {
                    if (recursive && fuel > 0) {
                        weights.Enqueue(fuel);
                    }
                    res += fuel;
                }
            }
            return res;
        }
    }
}