using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019 {
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

    class Memory {
        long[] initial;
        Dictionary<long, long> mem = new Dictionary<long, long>();

        public Memory(long[] initial) {
            this.initial = initial;
        }

        public long this[long addr]{
            get {
                return mem.ContainsKey(addr) ? mem[addr] : addr < initial.Length ? initial[addr] : 0;
            }
            set {
                mem[addr] = value;
            }
        }

        public void Reset() {
            mem.Clear();
        }
    }

    class IntCodeMachine {

        private static int[] modeMask = new int[] { 0, 100, 1000, 10000 };
       
        public Memory mem;
        public long ip;
        public long bp;

        public IntCodeMachine(string stPrg) {
            this.mem = new Memory(stPrg.Split(",").Select(long.Parse).ToArray());
            Reset();
        }

        public void Reset() {
            mem.Reset();
            ip = 0;
            bp = 0;
        }
        public bool Halted() => (Opcode)(mem[ip] % 100) == Opcode.Hlt;

        public long[] Run(params long[] input) {

            var en = input.Cast<long>().GetEnumerator();
            var output = new List<long>();
            while (true) {
                Opcode opcode = (Opcode)(mem[ip] % 100);
                long addr(int i) {
                    var mode = mem[ip] / modeMask[i] % 10;
                    return mode switch
                    {
                        0 => mem[ip + i],
                        1 => ip + i,
                        2 => bp + mem[ip + i],
                        _ => throw new ArgumentException()
                    };
                }

                long arg(int i) => mem[addr(i)];

                switch (opcode) {
                    case Opcode.Add: mem[addr(3)] = arg(1) + arg(2); ip += 4; break;
                    case Opcode.Mul: mem[addr(3)] = arg(1) * arg(2); ip += 4; break;
                    case Opcode.In: {
                            if (!en.MoveNext()) {
                                return output.ToArray();
                            }
                            mem[addr(1)] = en.Current; ip += 2;
                            break;
                        }
                    case Opcode.Out: output.Add(arg(1)); ip += 2; break;
                    case Opcode.Jnz: ip = arg(1) != 0 ? arg(2) : ip + 3; break;
                    case Opcode.Jz: ip = arg(1) == 0 ? arg(2) : ip + 3; break;
                    case Opcode.Lt: mem[addr(3)] = arg(1) < arg(2) ? 1 : 0; ip += 4; break;
                    case Opcode.Eq: mem[addr(3)] = arg(1) == arg(2) ? 1 : 0; ip += 4; break;
                    case Opcode.StR: bp += arg(1); ip += 2; break;
                    case Opcode.Hlt: return output.ToArray();
                    default: throw new ArgumentException("invalid opcode " + opcode);
                }
            }
        }
    }
}