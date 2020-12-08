using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day08 {

    record Stm(string op, int arg);

    [ProblemName("Handheld Halting")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Run(Parse(input)).acc;

        int PartTwo(string input) =>
            Patches(Parse(input))
                .Select(Run)
                .First(res => res.terminated).acc;

        Stm[] Parse(string input) =>
            input.Split("\n")
                .Select(line => line.Split(" "))
                .Select(parts => new Stm(parts[0], int.Parse(parts[1])))
                .ToArray();

        IEnumerable<Stm[]> Patches(Stm[] program) =>
            Enumerable.Range(0, program.Length)
                .Where(line => program[line].op != "acc")
                .Select(lineToPatch =>
                    program.Select((stm, line) =>
                        line != lineToPatch ? stm :
                        stm.op == "jmp" ? stm with { op = "nop" } :
                        stm.op == "nop" ? stm with { op = "jmp" } :
                        throw new Exception()
                    ).ToArray()
                );

        (int acc, bool terminated) Run(Stm[] program) {
            var (ip, acc, seen) = (0, 0, new HashSet<int>());

            while (true) {
                if (ip >= program.Length) {
                    return (acc, true);
                } else if (seen.Contains(ip)) {
                    return (acc, false);
                } else {
                    seen.Add(ip);
                    var stm = program[ip];
                    switch (stm.op) {
                        case "nop": ip++; break;
                        case "acc": ip++; acc += stm.arg; break;
                        case "jmp": ip += stm.arg; break;
                    };
                }
            }
        }
    }
}