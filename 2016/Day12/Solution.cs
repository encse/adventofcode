using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2016.Day12 {

    class Solution : Solver {

        public string GetName() => "Leonardo's Monorail";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Solve(input, 0);

        int PartTwo(string input) => Solve(input, 1);

        int Solve(string input, int c){
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

            var prog = input.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            var mulCount = 0;
            while (ip >= 0 && ip < prog.Length) {
                var line = prog[ip];
                var parts = line.Split(' ');
                switch (parts[0]) {
                    case "cpy":
                        setReg(parts[2], getReg(parts[1]));
                        ip++;
                        break;
                    case "inc":
                        setReg(parts[1], getReg(parts[1]) + 1);
                        ip++;
                        break;
                    case "dec":
                        mulCount++;
                        setReg(parts[1], getReg(parts[1]) - 1);
                        ip++;
                        break;
                    case "jnz":
                        ip += getReg(parts[1]) != 0 ? getReg(parts[2]) : 1;
                        break;
                    default: throw new Exception("Cannot parse " + line);
                }
            }
            return getReg("a");
        }
    }
}