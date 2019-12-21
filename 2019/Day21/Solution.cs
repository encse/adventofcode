using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day21 {

    class Solution : Solver {

        public string GetName() => "Springdroid Adventure";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) {
            var icm = new IntCodeMachine(input);
            return new IntCodeMachine(input).Run(
                "NOT C J",
                "AND D J",
                "NOT B T",
                "AND D T",
                "OR T J",
                "NOT A T",
                "AND D T",
                "OR T J",
                "WALK"
            ).Last();
        }

        long PartTwo(string input) {
            return new IntCodeMachine(input).Run(
                "NOT A J", 
                "NOT C T", 
                "AND D T", 
                "AND H T", 
                "OR T J", 
                "NOT C T", 
                "AND D T", 
                "AND E T", 
                "OR T J", 
                "NOT B T", 
                "AND D T", 
                "OR T J", 
                "RUN"
            ).Last();
        }
    }
}