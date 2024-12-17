namespace AdventOfCode.Y2024.Day17;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

[ProblemName("Chronospatial Computer")]
class Solution : Solver {

    enum Opcode {
        Adv, Bxl, Bst, Jnz, Bxc, Out, Bdv, Cdv
    }

    public object PartOne(string input) => Run(input);
    public object PartTwo(string input) => GenerateA(Parse(input).program.ToList()).Min();

    string Run(string input) {
        var (state, program) = Parse(input);
        var combo = (long op) => op < 4 ? (int)op : (int)state[op - 4];
        var res = new List<int>();
        for (var ip = 0; ip < program.Length; ip += 2) {
            switch ((Opcode)program[ip], program[ip + 1]) {
                case (Opcode.Adv, var op): state[0] = state[0] >> combo(op); break;
                case (Opcode.Bdv, var op): state[1] = state[0] >> combo(op); break;
                case (Opcode.Cdv, var op): state[2] = state[0] >> combo(op); break;
                case (Opcode.Bxl, var op): state[1] = state[1] ^ op; break;
                case (Opcode.Bst, var op): state[1] = combo(op) % 8; break;
                case (Opcode.Jnz, var op): ip = state[0] == 0 ? ip : (int)op - 2; break;
                case (Opcode.Bxc, var op): state[1] = state[1] ^ state[2]; break;
                case (Opcode.Out, var op): res.Add(combo(op) % 8); break;
            }
        }
        return string.Join(",", res);
    }

    /*
        Determines register A for the given output. The search works recursively and in reverse order, 
        starting from the last number to be printed and ending with the first.
    */
    IEnumerable<long> GenerateA(List<long> output) {
        if (!output.Any()) {
            yield return 0;
            yield break;
        }

        // this loop is pretty much the assembly code of the program reimplemented in c#
        foreach (var ah in GenerateA(output[1..])) {
            for (var al = 0; al < 8; al++) {
                var a = ah * 8 + al;
                long b = a % 8;
                b = b ^ 3;
                var c = a >> (int)b;
                b = b ^ c;
                b = b ^ 5;
                if (output[0] == b % 8) {
                    yield return a;
                }
            }
        }
    }
    
    (long[] state, long[] program) Parse(string input) {
        var blocks = input.Split("\n\n").Select(ParseNums).ToArray();
        return (blocks[0], blocks[1]);
    }
    
    long[] ParseNums(string st) =>
        Regex.Matches(st, @"\d+", RegexOptions.Multiline)
            .Select(m => long.Parse(m.Value))
            .ToArray();
}