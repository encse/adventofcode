using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2016.Day12;

[ProblemName("Leonardo's Monorail")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, 0);

    public object PartTwo(string input) => Solve(input, 1);

    int Solve(string input, int c) {
        var regs = new Dictionary<string, int>();
        int ip = 0;
        int getReg(string reg) {
            return int.TryParse(reg, out var n) ? n
                : regs.ContainsKey(reg) ? regs[reg]
                : 0;
        }
        void setReg(string reg, int value) {
            regs[reg] = value;
        }

        setReg("c", c);

        var prog = input.Split('\n').ToArray();
        while (ip >= 0 && ip < prog.Length) {
            var line = prog[ip];
            var stm = line.Split(' ');
            switch (stm[0]) {
                case "cpy":
                    setReg(stm[2], getReg(stm[1]));
                    ip++;
                    break;
                case "inc":
                    setReg(stm[1], getReg(stm[1]) + 1);
                    ip++;
                    break;
                case "dec":
                    setReg(stm[1], getReg(stm[1]) - 1);
                    ip++;
                    break;
                case "jnz":
                    ip += getReg(stm[1]) != 0 ? getReg(stm[2]) : 1;
                    break;
                default: throw new Exception("Cannot parse " + line);
            }
        }
        return getReg("a");
    }
}
