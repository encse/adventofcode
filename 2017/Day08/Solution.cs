using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Day08 {

    [ProblemName("I Heard You Like Registers")]
    class Solution : Solver {
 
        public object PartOne(string input) => Run(input).lastMax;
        public object PartTwo(string input) => Run(input).runningMax;

        (int runningMax, int lastMax) Run(string input) {
            var regs = new Dictionary<string, int>();
            var runningMax = 0;
            foreach (var line in input.Split('\n')) {
                //hsv inc 472 if hsv >= 4637
                var words = line.Split(' ');
                var (regDst, op, num, regCond, cond, condNum) = (words[0], words[1], int.Parse(words[2]), words[4], words[5], int.Parse(words[6]));
                if (!regs.ContainsKey(regDst)) {
                    regs[regDst] = 0;
                }
                if (!regs.ContainsKey(regCond)) {
                    regs[regCond] = 0;
                }

                var conditionHolds = cond switch {
                    ">=" => regs[regCond] >= condNum,
                    "<=" => regs[regCond] <= condNum,
                    "==" => regs[regCond] == condNum,
                    "!=" => regs[regCond] != condNum,
                    ">"  => regs[regCond] > condNum,
                    "<"  => regs[regCond] < condNum,
                    _ => throw new NotImplementedException(cond)
                };
                if (conditionHolds) {
                    regs[regDst] += 
                        op == "inc" ? num :
                        op == "dec" ? -num :
                        throw new NotImplementedException(op);
                }
                runningMax = Math.Max(runningMax, regs[regDst]);
            }
            return (runningMax, regs.Values.Max());
        }
    }
}
