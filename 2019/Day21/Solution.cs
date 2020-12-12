using System.Linq;

namespace AdventOfCode.Y2019.Day21 {

    [ProblemName("Springdroid Adventure")]
    class Solution : Solver {

        public object PartOne(string input) {
            var icm = new IntCodeMachine(input);
            
            // J = (¬A ∨ ¬B ∨ ¬C) ∧ D  
            // jump if no road ahead, but we can continue from D
            return new IntCodeMachine(input).Run(
                "OR A T",
                "AND B T",
                "AND C T",
                "NOT T J", 
                "AND D J", 
                "WALK"
            ).Last();
        }

        public object PartTwo(string input) {

             // J = (¬A ∨ ¬B ∨ ¬C) ∧ D ∧ (H ∨ E) 
             // same as part 1, but also check that D is not a dead end
            return new IntCodeMachine(input).Run(
                "OR A T",
                "AND B T",
                "AND C T",
                "NOT T J",  
                "AND D J", 
                "OR H T",
                "OR E T",
                "AND T J", 
                "RUN"
            ).Last();
        }
    }
}