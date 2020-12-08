using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day08 {

    [ProblemName("Handheld Halting")]
    class Solution : Solver {
        record Stm(string op, int arg);

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var seen = new HashSet<int>();
            return RunUntil(Parse(input), (ip) => {
                var res = !seen.Contains(ip);
                seen.Add(ip);
                return res;
            });
        }

        int RunUntil(Stm[] program, Func<int, bool> cond) {
            var ip = 0;
            var acc = 0;
            while (ip < program.Length && cond(ip)) {
                var stm = program[ip];
                switch (stm.op) {
                    case "nop": ip++; break;
                    case "acc": acc += stm.arg; ip++; break;
                    case "jmp": ip += stm.arg; break;
                }
            }
            return acc;
        }

        int PartTwo(string input) {
            Stm[] Patch(Stm[] program, int line) {
                program[line] = program[line] with {op = program[line].op =="jmp" ? "nop" : "jmp"};
                return program;
            }

            IEnumerable<Stm[]> Patches(Stm[] program) => 
                from line in Enumerable.Range(0, program.Length)
                where program[line].op != "acc"
                select Patch(program.ToArray(), line);

            foreach(var program in Patches(Parse(input))){
                var timeout = 10000000 / program.Length;
                var acc = RunUntil(program, _ => 0 < timeout--);
                if (timeout > 0){
                    return acc;
                }
            }
            throw new Exception();
        }

        Stm[] Parse(string input) => (
           from line in input.Split("\n")
           let parts = line.Split(" ")
           select new Stm(parts[0], int.Parse(parts[1]))
        ).ToArray();
    }
}