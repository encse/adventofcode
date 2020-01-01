using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        private Memory(long[] initial, Dictionary<long, long> mem) {
            this.initial = initial;
            this.mem = mem;
        }

        public long this[long addr] {
            get {
                return mem.ContainsKey(addr) ? mem[addr] : addr >= 0 && addr < initial.Length ? initial[addr] : 0;
            }
            set {
                mem[addr] = value;
            }
        }

        public long Length {
            get {
                return Math.Max(this.initial.Length, this.mem.Keys.Any() ? this.mem.Keys.Max() : 0);
            }
        }

        public Memory Clone() {
            return new Memory(initial, new Dictionary<long, long>(mem));
        }

        public void Reset() {
            mem.Clear();
        }
    }


    class ImmutableIntCodeMachine {
        IntCodeMachine icm;
        public ImmutableIntCodeMachine(string stPrg) : this(new IntCodeMachine(stPrg)) {
        }

        private ImmutableIntCodeMachine(IntCodeMachine icm) {
            this.icm = icm;
        }

        public (ImmutableIntCodeMachine iicm, long[] output) Run(params long[] input) {
            var immutableIntCodeMachine = new ImmutableIntCodeMachine(this.icm.Clone());
            return (immutableIntCodeMachine, immutableIntCodeMachine.icm.Run(input));
        }

        public (ImmutableIntCodeMachine iicm, long[] output) Run(params string[] input) {
            var immutableIntCodeMachine = new ImmutableIntCodeMachine(this.icm.Clone());
            return (immutableIntCodeMachine, immutableIntCodeMachine.icm.Run(input));
        }

        public bool Halted() => this.icm.Halted();
    }

    class IntCodeMachine {

        private static int[] modeMask = new int[] { 0, 100, 1000, 10000 };

        public Memory memory;
        public long ip;
        public long bp;
        public Queue<long> input;

        public IntCodeMachine(string stPrg) :
            this(new Memory(stPrg.Split(",").Select(long.Parse).ToArray()),
            0,
            0,
            new Queue<long>()
        ) {
        }

        private IntCodeMachine(Memory memory, long ip, long bp, Queue<long> input) {
            this.memory = memory;
            this.ip = ip;
            this.bp = bp;
            this.input = input;
        }

        public void Reset() {
            memory.Reset();
            ip = 0;
            bp = 0;
            input.Clear();
        }

        public IntCodeMachine Clone() {
            return new IntCodeMachine(memory.Clone(), ip, bp, new Queue<long>(input));
        }

        public bool Halted() => GetOpcode(ip) == Opcode.Hlt;

        private Mode GetMode(long addr, int i) => (Mode)(memory[addr] / modeMask[i] % 10);
        private Opcode GetOpcode(long addr) => (Opcode)(memory[addr] % 100);

        public long[] Run() {
            return Run(new long[0]);
        }

        public void RunInteractive() {
            var input = new long[0];
            while (true) {
                var output = Run(input);
                var c = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(AsciiDecode(output));
                Console.ForegroundColor = c;
                if (this.Halted()) {
                    break;
                }
                input = AsciiEncode(Console.ReadLine() + "\n");
            }
        }


        private long[] AsciiEncode(string st) {
            return (from ch in st select (long)ch).ToArray();
        }

        private string AsciiDecode(long[] items) {
            return string.Join("", from item in items select (char)item);
        }

        public long[] Run(params string[] input) {
            var st = string.Join("", from line in input select line + "\n");
            return Run(AsciiEncode(st));
        }

        public string RunAscii(params string[] input) {
            return AsciiDecode(Run(input));
        }

        bool Match(string stm, string pattern, out int[] m) {
            var match = Regex.Match(stm, pattern);
            m = null;
            if (match.Success) {
                m = match.Groups.Cast<Group>().Skip(1).Select(g => int.Parse(g.Value)).ToArray();
                return true;
            } else {
                return false;
            }
        }

        public long[] Run(params long[] input) {
            var cmd = AsciiDecode(input);

            int[] args;
            if (Match(cmd, @"!disass (\d+) (\d+)\n", out args)) {
                Console.WriteLine(Decompile(Disass(false, args[1], args[0])));
                return new long[0];
            }

            if (Match(cmd, @"!disass (\d+)\n", out args)) {
                Console.WriteLine(Decompile(Disass(false, args[0])));
                return new long[0];
            }

            if (Match(cmd, @"!mem\[(\d+)\]\n", out args)) {
                Console.WriteLine(this.memory[args[0]]);
                return new long[0];
            }

            foreach (var i in input) {
                this.input.Enqueue(i);
            }
            var output = new List<long>();
            while (true) {
                var opcode = GetOpcode(ip);
                var oldIp = ip;

                long addr(int i) {
                    return GetMode(oldIp, i) switch
                    {
                        Mode.Positional => memory[oldIp + i],
                        Mode.Immediate => oldIp + i,
                        Mode.Relative => bp + memory[oldIp + i],
                        _ => throw new ArgumentException()
                    };
                }

                long arg(int i) => memory[addr(i)];
                switch (opcode) {
                    case Opcode.Add: memory[addr(3)] = arg(1) + arg(2); ip += 4; break;
                    case Opcode.Mul: memory[addr(3)] = arg(1) * arg(2); ip += 4; break;
                    case Opcode.In: {
                            if (this.input.Any()) {
                                memory[addr(1)] = this.input.Dequeue(); ip += 2;
                            }
                            break;
                        }
                    case Opcode.Out: output.Add(arg(1)); ip += 2; break;
                    case Opcode.Jnz: ip = arg(1) != 0 ? arg(2) : ip + 3; break;
                    case Opcode.Jz: ip = arg(1) == 0 ? arg(2) : ip + 3; break;
                    case Opcode.Lt: memory[addr(3)] = arg(1) < arg(2) ? 1 : 0; ip += 4; break;
                    case Opcode.Eq: memory[addr(3)] = arg(1) == arg(2) ? 1 : 0; ip += 4; break;
                    case Opcode.StR: bp += arg(1); ip += 2; break;
                    case Opcode.Hlt: break;
                    default: throw new ArgumentException("invalid opcode " + opcode);
                }

                if (ip == oldIp) {
                    break;
                }
            }

            return output.ToArray();
        }

        public string Decompile(string st) {
            var inLines = st.Split("\n").ToList();
            var outLines = new List<string>();

            // function start
            for (var iline = 0; iline < inLines.Count; iline++) {
                string line(int i) {
                    return iline + i >= 0 ? inLines[iline + i] : "";
                }

                if (Regex.Match(line(0), @"bp \+= \d+;").Success) {
                    outLines.Add("fn_" + line(0).Split(" ")[0] + ":");
                }
                outLines.Add(line(0));
            }

            inLines = outLines.ToList();
            outLines.Clear();

            // return from function
            for (var iline = 0; iline < inLines.Count; iline++) {
                string line(int i) {
                    return iline + i >= 0 ? inLines[iline + i] : "";
                }
                outLines.Add(line(0));

                if (Regex.Match(line(-1), @"bp -= \d+;").Success && Regex.Match(line(0), @"goto mem\[bp\]").Success) {
                    outLines.Add("return;");
                }
            }

            return string.Join("\n", outLines);
        }

        public string Disass(bool trace = false, int count = int.MaxValue, long ip = -1) {
            if (ip == -1) {
                ip = this.ip;
            }
            var sb = new StringBuilder();

            string addr(int i) {
                return GetMode(ip, i) switch
                {
                    Mode.Positional => $"mem[{memory[ip + i]}]",
                    Mode.Immediate => $"{memory[ip + i]}",
                    Mode.Relative =>
                        memory[ip + i] > 0 ? $"mem[bp + {memory[ip + i]}]" :
                        memory[ip + i] == 0 ? $"mem[bp]" :
                        memory[ip + i] < 0 ? $"mem[bp - {-memory[ip + i]}]" :
                        throw new ArgumentException(),
                    _ => throw new ArgumentException()
                };
            }

            string arg(int i) {
                var st = addr(i);
                if (trace) {
                    var val = GetMode(ip, i) switch
                    {
                        Mode.Positional => memory[memory[ip + i]],
                        Mode.Immediate => memory[ip + i],
                        Mode.Relative => memory[bp + memory[ip + i]],
                        _ => throw new ArgumentException()
                    };

                    st += $" ({format(val)})";
                }
                return st;
            };

            string format(long v) {
                var st = v.ToString();
                if (v >= 32 && v < 128) {
                    st += $"  '{(char)(v)}'";
                }
                return st;
            }
            int a1, a2;

            for (var i = 0; i < count && ip < memory.Length; i++) {
                try {
                    sb.Append(ip.ToString("0000  "));
                    switch (GetOpcode(ip)) {
                        case Opcode.Add: {
                                if (int.TryParse(arg(1), out a1) && int.TryParse(arg(2), out a2)) {
                                    sb.AppendLine($"{addr(3)} = {a1 + a2};");
                                } else if (int.TryParse(arg(1), out a1) && a1 == 0) {
                                    sb.AppendLine($"{addr(3)} = {arg(2)};");
                                } else if (int.TryParse(arg(2), out a2) && a2 == 0) {
                                    sb.AppendLine($"{addr(3)} = {arg(2)};");
                                } else if (int.TryParse(arg(2), out a2) && a2 < 0) {
                                    sb.AppendLine($"{addr(3)} = {arg(1)} - {-a2};");
                                } else {
                                    sb.AppendLine($"{addr(3)} = {arg(1)} + {arg(2)};");
                                }
                                ip += 4;
                                break;
                            }
                        case Opcode.Mul: {
                                if (int.TryParse(arg(1), out a1) && int.TryParse(arg(2), out a2)) {
                                    sb.AppendLine($"{addr(3)} = {a1 * a2};");
                                } else if (int.TryParse(arg(1), out a1) && a1 == 0) {
                                    sb.AppendLine($"{addr(3)} = 0;");
                                } else if (int.TryParse(arg(1), out a1) && a1 == 1) {
                                    sb.AppendLine($"{addr(3)} = {arg(2)};");
                                } else if (int.TryParse(arg(2), out a2) && a2 == 0) {
                                    sb.AppendLine($"{addr(3)} = 0;");
                                } else if (int.TryParse(arg(2), out a2) && a2 == 1) {
                                    sb.AppendLine($"{addr(3)} = {arg(1)};");
                                } else {
                                    sb.AppendLine($"{addr(3)} = {arg(1)} * {arg(2)};");
                                }
                                ip += 4;
                                break;
                            }
                        case Opcode.In: sb.AppendLine($"{addr(1)} = input;"); ip += 2; break;
                        case Opcode.Out: {
                                sb.AppendLine($"output {arg(1)};"); ip += 2; break;
                            }
                        case Opcode.Jnz: {
                                if (int.TryParse(arg(1), out a1) && a1 != 0) {
                                    sb.AppendLine($"goto {arg(2)};");
                                } else if (int.TryParse(arg(1), out a1) && a1 == 0) {
                                    sb.AppendLine($";");
                                } else {
                                    sb.AppendLine($"if ({arg(1)}) goto {arg(2)};");

                                }
                                ip += 3;
                                break;
                            }
                        case Opcode.Jz: {
                                if (int.TryParse(arg(1), out a1) && a1 == 0) {
                                    sb.AppendLine($"goto {arg(2)};");
                                } else if (int.TryParse(arg(1), out a1) && a1 != 0) {
                                    sb.AppendLine($";");
                                } else {
                                    sb.AppendLine($"if (!{arg(1)}) goto {arg(2)};");
                                }
                                ip += 3;
                                break;
                            }
                        case Opcode.Lt: sb.AppendLine($"{addr(3)} = {arg(1)} < {arg(2)};"); ip += 4; break;
                        case Opcode.Eq: sb.AppendLine($"{addr(3)} = {arg(1)} == {arg(2)};"); ip += 4; break;
                        case Opcode.StR: {
                                if (int.TryParse(arg(1), out a1) && a1 < 0) {
                                    sb.AppendLine($"bp -= {-a1};");
                                } else {
                                    sb.AppendLine($"bp += {arg(1)};");
                                }
                                ip += 2; break;
                            }
                        case Opcode.Hlt: sb.AppendLine($"halt;"); ip += 1; break;
                        default: {
                                sb.AppendLine(format(memory[ip]));
                                ip += 1;
                                break;
                            }
                    }
                } catch {
                    sb.AppendLine($"{memory[ip]}"); ip += 2;
                }
            }

            return sb.ToString().TrimEnd();
        }
    }

}