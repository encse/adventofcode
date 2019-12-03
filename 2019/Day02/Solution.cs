using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day02 {

    class Solution : Solver {

        public string GetName() => "1202 Program Alarm";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var mem = input.Split(",").Select(int.Parse).ToArray();
            return ExecIntcode(mem, 12, 2);
        }

        int PartTwo(string input) {
            var mem = input.Split(",").Select(int.Parse).ToArray();
            for (var sum = 0; ; sum++) {
                for (var verb = 0; verb <= sum; verb++) {
                    var noun = sum - verb;
                    var res = ExecIntcode(mem, noun, verb);
                    if (res == 19690720) {
                        return 100 * noun + verb;
                    }
                }
            }
            throw new Exception();
        }

        int ExecIntcode(int[] mem, int noun, int verb) {
            mem = mem.ToArray();
            mem[1] = noun;
            mem[2] = verb;

            var ip = 0;
            while (mem[ip] != 99) {
                switch (mem[ip])
                {
                    case 1:
                        mem[mem[ip + 3]] = mem[mem[ip + 1]] + mem[mem[ip + 2]];
                        ip += 4;
                        break;
                    case 2:
                        mem[mem[ip + 3]] = mem[mem[ip + 1]] * mem[mem[ip + 2]];
                        ip += 4;
                        break;
                    default: throw new ArgumentException("invalid opcode " + mem[ip]);
                }
            }

            return mem[0];
        }
    }
}