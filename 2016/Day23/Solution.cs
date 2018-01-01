using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2016.Day23 {

    class Solution : Solver {

        public string GetName() => "Safe Cracking";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Solve(input, 7);
        int PartTwo(string input) => Solve(input, 12);
        
        int Solve(string input, int a) {
            var prg = Parse(Patch(input));
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

            while(ip < prg.Length){
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
                    case "mul":
                        setReg(stm[2], getReg(stm[1]) * getReg(stm[2]));
                        ip++;
                        break;
                    case "dec":
                        setReg(stm[1], getReg(stm[1]) - 1);
                        ip++;
                        break;
                    case "jnz":
                        ip += getReg(stm[1]) != 0 ? getReg(stm[2]) : 1;
                        break;
                    case "tgl":
                        var ipDst = ip + getReg(stm[1]);
                        if (ipDst >= 0 && ipDst < prg.Length) {
                            var stmDst = prg[ipDst];
                            switch (stmDst[0]) {
                                case "cpy": stmDst[0] = "jnz"; break;
                                case "inc": stmDst[0] = "dec"; break;
                                case "dec": stmDst[0] = "inc"; break;
                                case "jnz": stmDst[0] = "cpy"; break;
                                case "tgl": stmDst[0] = "inc"; break;
                            }
                        }
                        ip++;
                        break;
                    default: 
                        throw new Exception("Cannot parse " + string.Join(" ", stm));
                }
            }
            return getReg("a");
        }

        string Patch(string input) {
            var lines = input.Split('\n');
            lines[5] = "cpy c a";
            lines[6] = "mul d a";
            lines[7] = "cpy 0 d";
            lines[8] = "cpy 0 c";
            return string.Join("\n", lines);
        }
        
        string[][] Parse(string input) =>
            input.Split('\n').Select(line => line.Split(' ')).ToArray();
    }
}