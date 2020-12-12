using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day09 {

    [ProblemName("Sensor Boost")]
    class Solution : Solver {

        public object PartOne(string input) => new IntCodeMachine(input).Run(1).Single();
        public object PartTwo(string input) => new IntCodeMachine(input).Run(2).Single();
    }
}