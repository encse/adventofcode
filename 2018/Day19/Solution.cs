using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2018.Day19 {

    [ProblemName("Go With The Flow")]
    class Solution : Solver {

        public object PartOne(string input) {
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

        public object PartTwo(string input) {
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
            regs[stm[2]] = op switch {
                "addr" => regs[stm[0]] + regs[stm[1]],
                "addi" => regs[stm[0]] + stm[1],
                "mulr" => regs[stm[0]] * regs[stm[1]],
                "muli" => regs[stm[0]] * stm[1],
                "banr" => regs[stm[0]] & regs[stm[1]],
                "bani" => regs[stm[0]] & stm[1],
                "borr" => regs[stm[0]] | regs[stm[1]],
                "bori" => regs[stm[0]] | stm[1],
                "setr" => regs[stm[0]],
                "seti" => stm[0],
                "gtir" => stm[0] > regs[stm[1]] ? 1 : 0,
                "gtri" => regs[stm[0]] > stm[1] ? 1 : 0,
                "gtrr" => regs[stm[0]] > regs[stm[1]] ? 1 : 0,
                "eqir" => stm[0] == regs[stm[1]] ? 1 : 0,
                "eqri" => regs[stm[0]] == stm[1] ? 1 : 0,
                "eqrr" => regs[stm[0]] == regs[stm[1]] ? 1 : 0,
                _ => throw new ArgumentException()
            };
            return regs;
        }
    }
}