using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day11 {

    class Solution : Solver {

        public string GetName() => "Space Police";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Run(input, 0).Count;

        string PartTwo(string input) {
            var dict = Run(input, 1);
            var irowMin = dict.Keys.Select(pos => pos.irow).Min();
            var icolMin = dict.Keys.Select(pos => pos.icol).Min();
            var irowMax = dict.Keys.Select(pos => pos.irow).Max();
            var icolMax = dict.Keys.Select(pos => pos.icol).Max();
            var crow = irowMax - irowMin + 1;
            var ccol = icolMax - icolMin + 1;
            var mtx = new int[crow][];
            for (var irow = 0; irow < crow; irow++) {
                mtx[irow] = new int[ccol];
                for (var icol = 0; icol < ccol; icol++) {
                    mtx[irow][icol] = dict.GetValueOrDefault((irowMin + irow, icolMin + icol), 0);
                    Console.Write(mtx[irow][icol] == 0 ? " " : "#");
                }
                Console.WriteLine();
            }
            return OCR(mtx);
        }

        Dictionary<(int irow, int icol), int> Run(string input, int startColor) {
            var mtx = new Dictionary<(int irow, int icol), int>();
            (int irow, int icol) pos = (0, 0);
            (int drow, int dcol) dir = (-1, 0);
            mtx[(0, 0)] = startColor;
            var icm = new IntcodeMachine(input, 1024 * 1024);
            while (true) {
                icm.input.Enqueue(mtx.GetValueOrDefault(pos, 0));
                while (icm.output.Count != 2) {
                    if (!icm.Step()) {
                        return mtx;
                    }
                }
                mtx[pos] = (int)icm.output.Dequeue();
                dir = icm.output.Dequeue() switch {
                    0 => (-dir.dcol, dir.drow),
                    1 => (dir.dcol, -dir.drow),
                    _ => throw new ArgumentException()
                };
                pos = (pos.irow + dir.drow, pos.icol + dir.dcol);
            }
        }

        string OCR(int[][] mx) {
            var dict = new Dictionary<long, string>{
                {0x725C94B8, "B"},
                {0x32508498, "C"},
                {0x462A2108, "Y"},
                {0x7A1C843C, "E"},
                {0x7A1C8420, "F"},
                {0x3D0E4210, "F"},
                {0x252F4A52, "H"},
                {0xC210A4C,  "J"},
                {0x19297A52, "A"},
                {0x2108421E, "L"},
                {0x3C22221E, "Z"},
                {0, ""},
            };

            var res = "";
            var width = 5;
            for (var ch = 0; ch < Math.Ceiling(mx[0].Length / (double)width); ch++) {
                var hash = 0L;
                var st = "";
                for (var irow = 0; irow < mx.Length; irow++) {
                    for (var i = 0; i < width; i++) {
                        var icol = (ch * width) + i;

                        if (icol < mx[0].Length && mx[irow][icol] == 1) {
                            hash += 1;
                            st += "#";
                        } else {
                            st += ".";
                        }
                        hash <<= 1;
                    }
                    st += "\n";
                }
                if (!dict.ContainsKey(hash)) {
                    throw new Exception($"Unrecognized letter with hash: 0x{hash.ToString("X")}\n{st}");
                }
                res += dict[hash];
            }
            return res;
        }
    }


    class IntcodeMachine {
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

        private int[] modeMask = new int[] { 0, 100, 1000, 10000 };
        long[] mem;
        long ip;
        long r;
        public Queue<long> input = new Queue<long>();
        public Queue<long> output = new Queue<long>();

        public IntcodeMachine(string stPrg, int memsize) {
            mem = new long[1024 * 1024];
            var prg = stPrg.Split(",").Select(long.Parse).ToArray();
            Array.Copy(prg, mem, prg.Length);
        }

        public bool Step() {

            Opcode opcode = (Opcode)(mem[ip] % 100);
            long addr(int i) {
                var mode = mem[ip] / modeMask[i] % 10;
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