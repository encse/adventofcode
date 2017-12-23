using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Day23 {

    class Solution : Solver {

        public string GetName() => "Coprocessor Conflagration";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
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

            var prog = input.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            var mulCount = 0;
            while (ip >= 0 && ip < prog.Length) {
                var line = prog[ip];
                var parts = line.Split(' ');
                switch (parts[0]) {
                    case "set":
                        setReg(parts[1], getReg(parts[2]));
                        ip++;
                        break;
                    case "sub":
                        setReg(parts[1], getReg(parts[1]) - getReg(parts[2]));
                        ip++;
                        break;
                    case "mul":
                        mulCount++;
                        setReg(parts[1], getReg(parts[1]) * getReg(parts[2]));
                        ip++;
                        break;
                    case "jnz":
                        ip += getReg(parts[1]) != 0 ? getReg(parts[2]) : 1;
                        break;
                    default: throw new Exception("Cannot parse " + line);
                }
            }
            return mulCount;
        }

        int PartTwo(string input) {
            var c = 0;
            for (int b = 107900; b <= 124900; b += 17) {
                if (!IsPrime(b)) {
                    c++;
                }
            }
            return c;
        }

        bool IsPrime(int n) {
            for (int j = 2; j * j <= n; j++) {
                if (n % j == 0) return false;
            }
            return true;
        }

    }
}