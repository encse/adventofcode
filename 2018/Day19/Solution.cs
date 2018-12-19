using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day19 {

    class Solution : Solver {

        public string GetName() => "Go With The Flow";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var ip = 0;
            var ipReg = int.Parse(input.Split("\n").First().Substring("#ip ".Length));
            var prg = input.Split("\n").Skip(1).ToArray();
            var regs = new int[6];
            while (ip >= 0 && ip < prg.Length) {
                var args = prg[ip].Split(" ");
                regs[ipReg] = ip;
                regs = Step(regs, args[0], args.Skip(1).Select(int.Parse).ToArray());
                ip = regs[ipReg];
                ip++;
            }
            return regs[0];
        }

        int PartTwo(string input) {
            var t = 10551292;
            var r0 = 0;
            for (var x = 1; x <= t; x++) {
                if (t % x == 0)
                    r0 += x;
            }
            return r0;
        }

        int[] Step(int[] regs, string op, int[] stm) {
            regs = regs.ToArray();
            switch (op) {
                case "addr": regs[stm[2]] = regs[stm[0]] + regs[stm[1]]; break;
                case "addi": regs[stm[2]] = regs[stm[0]] + stm[1]; break;
                case "mulr": regs[stm[2]] = regs[stm[0]] * regs[stm[1]]; break;
                case "muli": regs[stm[2]] = regs[stm[0]] * stm[1]; break;
                case "banr": regs[stm[2]] = regs[stm[0]] & regs[stm[1]]; break;
                case "bani": regs[stm[2]] = regs[stm[0]] & stm[1]; break;
                case "borr": regs[stm[2]] = regs[stm[0]] | regs[stm[1]]; break;
                case "bori": regs[stm[2]] = regs[stm[0]] | stm[1]; break;
                case "setr": regs[stm[2]] = regs[stm[0]]; break;
                case "seti": regs[stm[2]] = stm[0]; break;
                case "gtir": regs[stm[2]] = stm[0] > regs[stm[1]] ? 1 : 0; break;
                case "gtri": regs[stm[2]] = regs[stm[0]] > stm[1] ? 1 : 0; break;
                case "gtrr": regs[stm[2]] = regs[stm[0]] > regs[stm[1]] ? 1 : 0; break;
                case "eqir": regs[stm[2]] = stm[0] == regs[stm[1]] ? 1 : 0; break;
                case "eqri": regs[stm[2]] = regs[stm[0]] == stm[1] ? 1 : 0; break;
                case "eqrr": regs[stm[2]] = regs[stm[0]] == regs[stm[1]] ? 1 : 0; break;
            }
            return regs;
        }
    }
}