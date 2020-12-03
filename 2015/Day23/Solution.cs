using System;
using System.Collections.Generic;

namespace AdventOfCode.Y2015.Day23 {

    [ProblemName("Opening the Turing Lock")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => Solve(input, 0);
        long PartTwo(string input) => Solve(input, 1);

        long Solve(string input, long a) {
            var regs = new Dictionary<string, long>();
            var ip = 0L;
            long getReg(string reg) {
                return long.TryParse(reg, out var n) ? n
                    : regs.ContainsKey(reg) ? regs[reg]
                    : 0;
            }
            void setReg(string reg, long value) {
                regs[reg] = value;
            }

            setReg("a", a);
            var prog = input.Split('\n');
            while (ip >= 0 && ip < prog.Length) {
                var line = prog[ip];
                var parts = line.Replace(",", "").Split(" ");
                switch (parts[0]) {
                    case "hlf":
                        setReg(parts[1], getReg(parts[1]) / 2);
                        ip++;
                        break;
                    case "tpl":
                        setReg(parts[1], getReg(parts[1]) * 3);
                        ip++;
                        break;
                    case "inc":
                        setReg(parts[1], getReg(parts[1]) + 1);
                        ip++;
                        break;
                    case "jmp":
                        ip += getReg(parts[1]);
                        break;
                    case "jie":
                        ip += getReg(parts[1]) % 2 == 0 ? getReg(parts[2]) : 1;
                        break;
                    case "jio":
                        ip += getReg(parts[1]) == 1 ? getReg(parts[2]) : 1;
                        break;
                    default: throw new Exception("Cannot parse " + line);
                }
            }
            return getReg("b");
        }
    }
}