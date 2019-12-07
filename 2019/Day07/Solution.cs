using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day07 {

    class Solution : Solver {

        public string GetName() => "Amplification Circuit";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string prg) => Solve(prg, false, new int[] { 0, 1, 2, 3, 4 });
        int PartTwo(string prg) => Solve(prg, true, new int[] { 5, 6, 7, 8, 9 });

        int Solve(string prg, bool loop, int[] prgids) {
            var amps = Enumerable.Range(0, 5).Select(x => new Amp()).ToArray();

            for (var i = 1; i < amps.Length; i++) {
                amps[i].input = amps[i - 1].output;
            }

            if (loop) {
                amps[0].input = amps[amps.Length - 1].output;
            }

            var max = 0;
            foreach (var perm in Permutations(prgids)) {
                max = Math.Max(max, ExecAmps(amps, prg, perm));
            }
            return max;
        }

        int ExecAmps(Amp[] amps, string prg, int[] prgid) {

            for (var i = 0; i < prgid.Length; i++) {
                amps[i].Reset(prg);
            }

            for (var i = 0; i < prgid.Length; i++) {
                amps[i].input.Enqueue(prgid[i]);
            }

            amps[0].input.Enqueue(0);

            var any = true;
            while (any) {
                any = false;
                foreach (var amp in amps) {
                    any |= amp.Step();
                }
            }
            return amps[amps.Length - 1].output.Single();
        }

        IEnumerable<T[]> Permutations<T>(T[] rgt) {
            void Swap(int i, int j) {
                (rgt[i], rgt[j]) = (rgt[j], rgt[i]);
            }

            IEnumerable<T[]> PermutationsRec(int i) {
                yield return rgt.ToArray();

                for (var j = i; j < rgt.Length; j++) {
                    Swap(i, j);
                    foreach (var perm in PermutationsRec(i + 1)) {
                        yield return perm;
                    }
                    Swap(i, j);
                }
            }

            return PermutationsRec(0);
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
        Hlt = 99,
    }
    
    class Amp {
        int[] mem;
        int ip;
        public Queue<int> input = new Queue<int>();
        public Queue<int> output = new Queue<int>();

        public void Reset(string prg) {
            mem = prg.Split(",").Select(int.Parse).ToArray();
            input.Clear();
            output.Clear();
            ip = 0;
        }

        public bool Step() {

            Opcode opcode = (Opcode)(mem[ip] % 100);
            Func<int, int> arg = (int i) =>
                (mem[ip] / (int)Math.Pow(10, i + 1) % 10) == 0 ?
                    mem[mem[ip + i]] :
                    mem[ip + i];

            switch (opcode) {
                case Opcode.Add: mem[mem[ip + 3]] = arg(1) + arg(2); ip += 4; break;
                case Opcode.Mul: mem[mem[ip + 3]] = arg(1) * arg(2); ip += 4; break;
                case Opcode.In: {
                        if (input.Count > 0) {
                            mem[mem[ip + 1]] = input.Dequeue(); ip += 2;
                        }
                        break;
                    }
                case Opcode.Out: output.Enqueue(arg(1)); ip += 2; break;
                case Opcode.Jnz: ip = arg(1) != 0 ? arg(2) : ip + 3; break;
                case Opcode.Jz: ip = arg(1) == 0 ? arg(2) : ip + 3; break;
                case Opcode.Lt: mem[mem[ip + 3]] = arg(1) < arg(2) ? 1 : 0; ip += 4; break;
                case Opcode.Eq: mem[mem[ip + 3]] = arg(1) == arg(2) ? 1 : 0; ip += 4; break;
                case Opcode.Hlt: return false;
                default: throw new ArgumentException("invalid opcode " + opcode);
            }
            return true;
        }
    }
}