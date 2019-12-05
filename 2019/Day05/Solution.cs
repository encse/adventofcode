using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day05 {

    enum Opcode {
        Add = 1,
        Mul = 2,
        In = 3,
        Out = 4,
        Jnz = 5,
        Jz = 6,
        Lt = 7,
        Eq = 8,
        Hlt = 99,
    }
    class Solution : Solver {

        public string GetName() => "Sunny with a Chance of Asteroids";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            return ExecIntcode(input, 1).Last();
        }

        int PartTwo(string input) {
            return ExecIntcode(input, 5).Last();
        }

        IEnumerable<int> ExecIntcode(string st, int input) {
            var mem = st.Split(",").Select(int.Parse).ToArray();
            var ip = 0;

            while (true) {
                Opcode opcode = (Opcode)(mem[ip] % 100);
                Func<int, int> arg = (int i) =>
                    (mem[ip] / (int)Math.Pow(10, i + 1) % 10) == 0 ?
                        mem[mem[ip + i]] :
                        mem[ip + i];

                switch (opcode) {
                    case Opcode.Add: mem[mem[ip + 3]] = arg(1) + arg(2); ip += 4; break;
                    case Opcode.Mul: mem[mem[ip + 3]] = arg(1) * arg(2); ip += 4; break;
                    case Opcode.In: mem[mem[ip + 1]] = input; ip += 2; break;
                    case Opcode.Out: yield return arg(1); ip += 2; break;
                    case Opcode.Jnz: ip = arg(1) != 0 ? arg(2) : ip + 3; break;
                    case Opcode.Jz: ip = arg(1) == 0 ? arg(2) : ip + 3; break;
                    case Opcode.Lt: mem[mem[ip + 3]] = arg(1) < arg(2) ? 1 : 0; ip += 4; break;
                    case Opcode.Eq: mem[mem[ip + 3]] = arg(1) == arg(2) ? 1 : 0; ip += 4; break;
                    case Opcode.Hlt: yield break;
                    default: throw new ArgumentException("invalid opcode " + opcode);
                }
            }
        }
    }
}