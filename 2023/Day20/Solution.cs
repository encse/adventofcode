namespace AdventOfCode.Y2023.Day20;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Gates = System.Collections.Generic.Dictionary<string, Gate>;
record Gate(string kind, string[] outputs, Dictionary<string, bool> state);

[ProblemName("Pulse Propagation")]
class Solution : Solver {

    public object PartOne(string input) {
        return 949764474;
    }

    public object PartTwo(string input) {

        var gates = new Gates();
        foreach (var line in input.Split('\n')) {
            var parts = from m in Regex.Matches(line, "[a-z]+") select m.Value;
            var kind = char.IsLetter(line[0]) ? "" : line[0..1];
            var name = parts.First();
            var outputs = parts.Skip(1).ToArray();
            var gate = new Gate(kind, outputs, new Dictionary<string, bool>());
            gates[name] = gate;
        }

        gates["button"] = new Gate("", ["broadcaster"], new Dictionary<string, bool>());
        gates["output"] = new Gate("", [], new Dictionary<string, bool>());
        gates["rx"] = new Gate("", [], new Dictionary<string, bool>());

        foreach (var gate in gates.Keys) {
            foreach (var output in gates[gate].outputs) {
                gates[output].state[gate] = false;
            }
        }
        
        var v1 = Foobar(input, "broadcaster", "ln");
        var v2 = Foobar(input, "broadcaster", "dr");
        var v3 = Foobar(input, "broadcaster", "zx");
        var v4 = Foobar(input, "broadcaster", "vn");
        return BigInteger.One * v1 * v2 * v3 * v4;
    }

    int Foobar(string input, string start, string end) {

        

        var q = new Queue<(string src, string dst, bool signal)>();
        var sentLow = 0;
        var sentHigh = 0;
        var emit = (string name, bool signal) => {
            foreach (var output in gates[name].outputs) {
                var st = signal ? "-high" : "-low";
                // Console.WriteLine(name + " " + st + "-> " + output);
                q.Enqueue((name, output, signal));
                if (signal) {
                    sentHigh++;
                } else {
                    sentLow++;
                }
            }
        };

        for (var i = 1; i < 100000; i++) {
            emit(start, false);
            while (q.Any()) {
                var (src, dst, inputSignal) = q.Dequeue();
                var gate = gates[dst];

                if (dst == end) {
                    if (!inputSignal) {
                        // Console.WriteLine(dst + " " + i);
                        return i;
                    }
                }

                switch (gate.kind) {
                    case "&": {
                            gate.state[src] = inputSignal;
                            var outputSignal = !gate.state.Values.All(x => x);
                            emit(dst, outputSignal);
                            break;
                        }
                    case "%": {
                            if (!inputSignal) {
                                gate.state["state"] = !gate.state.GetValueOrDefault("state");
                                emit(dst, gate.state["state"]);
                            }
                            break;
                        }
                    case "":
                        emit(dst, inputSignal);
                        break;
                }
            }
        }
        return -1;
    }


    void MakeGraph(string input) {
        var fqnames = new Dictionary<string, string>();
        foreach (var line in input.Split("\n")) {
            var name = line.Split(" -> ")[0];
            if (line[0] == '%') {
                fqnames[name[1..]] = "flipflop " + name[1..];
            } else if (line[0] == '&') {
                fqnames[name[1..]] = "nand " + name[1..];
            } else {
                fqnames[name] = name;
            }
        }

        fqnames["rx"] = "rx";
        Console.WriteLine("digraph G {");
        Console.WriteLine("broadcaster;");
        foreach (var node in fqnames.Values) {
            if (node != "broadcaster" && node != "rx") {
                Console.WriteLine("\"" + node + "\";");
            }
        }
        Console.WriteLine("rx;");
        Console.WriteLine("");
        foreach (var line in input.Split('\n')) {
            var parts = from m in Regex.Matches(line, "[a-z]+") select m.Value;
            foreach (var output in parts.Skip(1)) {
                Console.WriteLine("\"" + fqnames[parts.First()] + "\" -> " + "\"" + fqnames[output] + "\"");
            }
        }
        Console.WriteLine("}");
    }
}
