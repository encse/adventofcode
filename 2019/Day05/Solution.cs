using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day05 {

    [ProblemName("Sunny with a Chance of Asteroids")]
    class Solution : Solver {

        public object PartOne(string input) => new IntCodeMachine(input).Run(1).Last();

        public object PartTwo(string input) => new IntCodeMachine(input).Run(5).Last();

    }
}