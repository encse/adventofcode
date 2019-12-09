using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day09 {

    class Solution : Solver {

        public string GetName() => "Sensor Boost";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => Solve(input, 1);
        long PartTwo(string input) => Solve(input, 2);

        long Solve(string prg, long input) {
            var m = new IntcodeMachine();
            m.Reset(prg);
            m.input.Enqueue(input);
            while (m.Step()) {
                ;
            }

            return m.output.Single();
        }
    }

    enum Opcode {
        Add = 1,
        Mul = 2,
        In = 3,
        Out = 4,
        Jnz = 5,
        Jz = 6,
        Lt = 7,
        Eq = 8,
        StR = 9,
        Hlt = 99,
    }

    class IntcodeMachine {
        long[] mem;
        long ip;
        long r;
        public Queue<long> input = new Queue<long>();
        public Queue<long> output = new Queue<long>();

        public void Reset(string stPrg) {
            mem = new long[1024 * 1024];
            var prg = stPrg.Split(",").Select(long.Parse).ToArray();
            Array.Copy(prg, mem, prg.Length);

            input.Clear();
            output.Clear();
            ip = 0;
        }

        public bool Step() {

            Opcode opcode = (Opcode)(mem[ip] % 100);
            long addr(int i) {
                var mode = (mem[ip] / (int)Math.Pow(10, i + 1) % 10);
                return mode switch
                {
                    0 => mem[ip + i],
                    1 => ip + i,
                    2 => r + mem[ip + i],
                    _ => throw new ArgumentException()
                };
            }

            long arg(int i) => mem[addr(i)];

            switch (opcode) {
                case Opcode.Add: mem[addr(3)] = arg(1) + arg(2); ip += 4; break;
                case Opcode.Mul: mem[addr(3)] = arg(1) * arg(2); ip += 4; break;
                case Opcode.In: {
                        if (input.Count > 0) {
                            mem[addr(1)] = input.Dequeue(); ip += 2;
                        }
                        break;
                    }
                case Opcode.Out: output.Enqueue(arg(1)); ip += 2; break;
                case Opcode.Jnz: ip = arg(1) != 0 ? arg(2) : ip + 3; break;
                case Opcode.Jz: ip = arg(1) == 0 ? arg(2) : ip + 3; break;
                case Opcode.Lt: mem[addr(3)] = arg(1) < arg(2) ? 1 : 0; ip += 4; break;
                case Opcode.Eq: mem[addr(3)] = arg(1) == arg(2) ? 1 : 0; ip += 4; break;
                case Opcode.StR: r += arg(1); ip += 2; break;
                case Opcode.Hlt: return false;
                default: throw new ArgumentException("invalid opcode " + opcode);
            }
            return true;
        }
    }
}