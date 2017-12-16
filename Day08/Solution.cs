using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2017.Day08 {

    class Solution : Solver {

        public string GetName() { 
            return "I Heard You Like Registers"; 
        }

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }
        
        int PartOne(string input) => Run(input).lastMax;
        int PartTwo(string input) => Run(input).runningMax;

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

                var conditionHolds = false;
                switch (cond) {
                    case ">=": conditionHolds = regs[regCond] >= condNum; break;
                    case "<=": conditionHolds = regs[regCond] <= condNum; break;
                    case "==": conditionHolds = regs[regCond] == condNum; break;
                    case "!=": conditionHolds = regs[regCond] != condNum; break;
                    case ">": conditionHolds = regs[regCond] > condNum; break;
                    case "<": conditionHolds = regs[regCond] < condNum; break;
                    default: throw new NotImplementedException(cond);
                }
                if (conditionHolds) {
                    switch(op){
                        case "inc": regs[regDst] += num; break;
                        case "dec": regs[regDst] -= num; break;
                        default: throw new NotImplementedException(op);
                    }
                }
                runningMax = Math.Max(runningMax, regs[regDst]);
            }
            return (runningMax, regs.Values.Max());
        }
    }
}
