using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day05 {

    class Solution : Solver {

        public string GetName() => "Sunny with a Chance of Asteroids";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => new IntCodeMachine(input).Run(1).Last();

        long PartTwo(string input) => new IntCodeMachine(input).Run(5).Last();

    }
}