using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day09 {

    [ProblemName("Sensor Boost")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => new IntCodeMachine(input).Run(1).Single();
        long PartTwo(string input) => new IntCodeMachine(input).Run(2).Single();
    }
}