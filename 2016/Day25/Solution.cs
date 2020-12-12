using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2016.Day25 {

    [ProblemName("Clock Signal")]
    class Solution : Solver {

        public object PartTwo(string input) => null;

        public object PartOne(string input) {
            for (int a = 0; ; a++) {
                var length = 0;
                var expectedBit = 0;
                foreach (var actualBit in Run(input, a).Take(100)) {
                    if (actualBit == expectedBit) {
                        expectedBit = 1 - expectedBit;
                        length++;
                    } else {
                        break;
                    }
                }
                if (length == 100) {
                    return a;
                }
            }
        }

        IEnumerable<int> Run(string input, int a) {
            var prg = Parse(input);
            var regs = new Dictionary<string, int>();
            var ip = 0;
            int getReg(string reg) {
                return int.TryParse(reg, out var n) ? n
                    : regs.ContainsKey(reg) ? regs[reg]
                    : 0;
            }
            void setReg(string reg, int value) {
                if (!int.TryParse(reg, out var _)) {
                    regs[reg] = value;
                }
            }

            setReg("a", a);

            while (ip < prg.Length) {
                var stm = prg[ip];
                switch (stm[0]) {
                    case "cpy":
                        setReg(stm[2], getReg(stm[1]));
                        ip++;
                        break;
                    case "inc":
                        setReg(stm[1], getReg(stm[1]) + 1);
                        ip++;
                        break;
                    case "out":
                        yield return getReg(stm[1]);
                        ip++;
                        break;
                    case "dec":
                        setReg(stm[1], getReg(stm[1]) - 1);
                        ip++;
                        break;
                    case "jnz":
                        ip += getReg(stm[1]) != 0 ? getReg(stm[2]) : 1;
                        break;
                    default:
                        throw new Exception("Cannot parse " + string.Join(" ", stm));
                }
            }
        }
        
        string[][] Parse(string input) =>
            input.Split('\n').Select(line => line.Split(' ')).ToArray();
    }
}