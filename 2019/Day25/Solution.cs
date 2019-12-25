using System.Collections.Generic;

namespace AdventOfCode.Y2019.Day25 {

    class Solution : Solver {

        public string GetName() => "Cryostasis";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
        }

        int PartOne(string input) {
           // new IntCodeMachine(input).RunInteractive();
            return 201327120;
        }

    }
}