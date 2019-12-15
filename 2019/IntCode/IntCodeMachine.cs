using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Y2019 {

    enum Mode {
        Positional = 0,
        Immediate = 1,
        Relative = 2
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

    class Memory {
        public long[] initial;
        Dictionary<long, long> mem = new Dictionary<long, long>();

        public Memory(long[] initial) {
            this.initial = initial;
        }

        public long this[long addr] {
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
        public Queue<long> input = new Queue<long>();
        public IntCodeMachine(string stPrg) {
            this.mem = new Memory(stPrg.Split(",").Select(long.Parse).ToArray());
            // Console.WriteLine(Disass());
            Reset();
        }

        public void Reset() {
            mem.Reset();
            ip = 0;
            bp = 0;
            input.Clear();
        }

        public bool Halted() => (Opcode)(mem[ip] % 100) == Opcode.Hlt;

        private Mode GetMode(long addr, int i) => (Mode)(mem[addr] / modeMask[i] % 10);
        private Opcode GetOpcode(long addr) => (Opcode)(mem[addr] % 100);

        public long[] Run(params long[] input) {

            foreach (var i in input) {
                this.input.Enqueue(i);
            }
            var output = new List<long>();
            while (true) {
                // Console.WriteLine(this.Disass(1));
                var opcode = GetOpcode(ip);
                long addr(int i) {
                    return GetMode(ip, i) switch
                    {
                        Mode.Positional => mem[ip + i],
                        Mode.Immediate => ip + i,
                        Mode.Relative => bp + mem[ip + i],
                        _ => throw new ArgumentException()
                    };
                }

                long arg(int i) => mem[addr(i)];

                switch (opcode) {
                    case Opcode.Add: mem[addr(3)] = arg(1) + arg(2); ip += 4; break;
                    case Opcode.Mul: mem[addr(3)] = arg(1) * arg(2); ip += 4; break;
                    case Opcode.In: {
                            if (!this.input.Any()) {
                                return output.ToArray();
                            }
                            mem[addr(1)] = this.input.Dequeue(); ip += 2;
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

        public string Disass(int count = int.MaxValue) {
            var ip = this.ip;
            var sb = new StringBuilder();

            string guard<T>(Func<T> action) {
                try {
                    return action().ToString();
                } catch {
                    return "?";
                }
                
            }
            string addr(int i) {
                return GetMode(ip, i) switch
                {
                    Mode.Positional => $"mem[{mem[ip + i]}]",
                    Mode.Relative => $"mem[bp + {mem[ip + i]}]",
                    _ => throw new ArgumentException()
                };
            }

            string arg(int i) {
                return GetMode(ip, i) switch
                {
                    Mode.Positional => $"mem[{mem[ip + i]}] ({guard(() => mem[mem[ip + i]])})",
                    Mode.Immediate => $"{mem[ip + i]}",
                    Mode.Relative => $"mem[bp + {mem[ip + i]}] ({guard(() => mem[bp + mem[ip + i]] )})",
                    _ => throw new ArgumentException()
                };
            }

            for (var i = 0; i < count && ip < mem.initial.Length; i++) {
                try {
                    sb.Append(ip.ToString("0000  "));
                    switch (GetOpcode(ip)) {
                        case Opcode.Add: sb.AppendLine($"{addr(3)} = {arg(1)} + {arg(2)};"); ip += 4; break;
                        case Opcode.Mul: sb.AppendLine($"{addr(3)} = {arg(1)} * {arg(2)};"); ip += 4; break;
                        case Opcode.In: sb.AppendLine($"{addr(1)} = input()"); ip += 2; break;
                        case Opcode.Out: sb.AppendLine($"output({arg(1)})"); ip += 2; break;
                        case Opcode.Jnz: sb.AppendLine($"if ({arg(1)} != 0) goto {arg(2)}; "); ip += 3; break;
                        case Opcode.Jz: sb.AppendLine($"if ({arg(1)} == 0) goto {arg(2)}; "); ip += 3; break;
                        case Opcode.Lt: sb.AppendLine($"{addr(3)} = {arg(1)} < {arg(2)} ? 1 : 0;"); ip += 4; break;
                        case Opcode.Eq: sb.AppendLine($"{addr(3)} = {arg(1)} == {arg(2)} ? 1 : 0;"); ip += 4; break;
                        case Opcode.StR: sb.AppendLine($"bp += {arg(1)};"); ip += 2; break;
                        case Opcode.Hlt: sb.AppendLine($"halt();"); ip += 1; break;
                        default: sb.AppendLine($"{mem[ip]}"); ip += 1; break;
                    }
                } catch {
                    sb.AppendLine($"{mem[ip]}"); ip += 2;
                }
            }

            return sb.ToString();
        }
    }

}