namespace AdventOfCode.Y2024.Day24;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

record struct Rule(string in1, string in2, string kind, string output);

[ProblemName("Crossed Wires")]
class Solution : Solver {

    public object PartOne(string input) {
        var gate = Parse(input);
        var bits = gate.Keys.Where(k => k.StartsWith("z")).OrderByDescending(x => x).ToArray();
        var res = 0L;
        foreach (var b in bits) {
            res = res * 2 + gate[b]();
        }
        return res;
    }

    public object PartTwo(string input) {
        var swaps = Fix(ParseRules(input.Split("\n\n")[1]));
        return string.Join(",", swaps.OrderBy(x => x));
    }

    // the rules should define a full adder for two 44 bit numbers
    // this fixer is specific to my input.
    IEnumerable<string> Fix(List<Rule> rules) {
        var cin = Output(rules, "x00", "AND", "y00");
        for (var i = 1; i < 45; i++) {
            var x = $"x{i:D2}";
            var y = $"y{i:D2}";
            var z = $"z{i:D2}";

            var xor1 = Output(rules, x, "XOR", y);
            var and1 = Output(rules, x, "AND", y);

            var and2 = Output(rules, cin, "AND", xor1);
            var xor2 = Output(rules, cin, "XOR", xor1);

            if (xor2 == null && and2 == null) {
                return Swap(rules, xor1, and1);
            }

            var carry = Output(rules, and1, "OR", and2);
            if (xor2 != z) {
                return Swap(rules,z,xor2);
            } else {
                cin = carry;
            }
        }
        return [];
    }

    string Output(IEnumerable<Rule> rules, string x, string gate, string y) => 
        rules.SingleOrDefault(rule => 
            (rule.in1 == x && rule.kind == gate && rule.in2 == y) || 
            (rule.in1 == y && rule.kind == gate && rule.in2 == x) 
        ).output;

    IEnumerable<string> Swap(List<Rule> rules, string out1, string out2) {
        rules = rules.Select(rule => 
            rule.output == out1 ? rule with {output = out2} :
            rule.output == out2 ? rule with {output = out1} :
            rule  
        ).ToList();

        return Fix(rules).Concat([out1, out2]);
    }

     List<Rule> ParseRules(string input) => input
        .Split("\n")
        .Select(line => {
            var parts = line.Split(" ");
            return new Rule(in1: parts[0], in2: parts[2], kind: parts[1], output: parts[4]);
        })
        .ToList();
    Dictionary<string, Gate> Parse(string input) {

        var res = new Dictionary<string, Gate>();

        var blocks = input.Split("\n\n");

        foreach (var line in blocks[0].Split("\n")) {
            var parts = line.Split(": ");
            res.Add(parts[0], () => int.Parse(parts[1]));
        }

        foreach (var line in blocks[1].Split("\n")) {
            var parts = Regex.Matches(line, "[a-zA-z0-9]+").Select(m => m.Value).ToArray();
            Gate gate = (parts[0], parts[1], parts[2]) switch {
                (var in1, "AND", var in2) => () => res[in1]() & res[in2](),
                (var in1, "OR", var in2) => () => res[in1]() | res[in2](),
                (var in1, "XOR", var in2) => () => res[in1]() ^ res[in2](),
                _ => throw new Exception(),
            };
            res.Add(parts[3], gate);
        }
        return res;
    }

    delegate int Gate();
}