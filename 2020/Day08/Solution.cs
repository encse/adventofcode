using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day08 {

    record Stm(string op, int arg);
    record State(int ip, int acc, bool terminated);

    [ProblemName("Handheld Halting")]
    class Solution : Solver {
     
        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var program = Parse(input);

            var state = new State(0, 0, false);
            for (var seen = new HashSet<int>(); !seen.Contains(state.ip);) {
                seen.Add(state.ip);
                state = Step(state, program);
            }

            return state.acc;
        }

        int PartTwo(string input) {
            foreach (var program in Patches(Parse(input))) {

                var state = new State(0, 0, false);
                for (var t = 0; t < 1000 /* big enough for this input */; t++) {
                    state = Step(state, program);
                }

                if (state.terminated) {
                    return state.acc;
                }
            }
            throw new Exception();
        }

        IEnumerable<Stm[]> Patches(Stm[] program) =>
            from lineToPatch in Enumerable.Range(0, program.Length) 
            where program[lineToPatch].op != "acc"
            select 
                program.Select((stm, line) => 
                    line != lineToPatch ? stm :
                    stm.op == "jmp"     ? stm with {op = "nop" }:
                    stm.op == "nop"     ? stm with {op = "jmp" } :
                    throw new Exception()
                ).ToArray();

        State Step(State state, Stm[] program) =>
            state.terminated           ? state :
            state.ip >= program.Length ? state with { terminated = true } :
            program[state.ip] switch {
                ("nop", _      ) => state with { ip = state.ip + 1 },
                ("acc", var arg) => state with { ip = state.ip + 1, acc = state.acc + arg },
                ("jmp", var arg) => state with { ip = state.ip + arg },
                _ => throw new Exception()
            };

        Stm[] Parse(string input) => (
                from line in input.Split("\n")
                let parts = line.Split(" ")
                select new Stm(parts[0], int.Parse(parts[1]))
            ).ToArray();
    }
}