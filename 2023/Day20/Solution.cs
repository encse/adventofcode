namespace AdventOfCode.Y2023.Day20;

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Gates = System.Collections.Generic.Dictionary<string, Gate>;
using Signal = (string sender, string receiver, bool value);
record Gate(string kind, string[] outputs, Dictionary<string, bool> inputs);

[ProblemName("Pulse Propagation")]
class Solution : Solver {

    public object PartOne(string input) {
        var (low, high) = (0, 0);
        var gates = ParseGates(input);
        for (var i = 0; i < 1000; i++) {
            foreach (var signal in Button(gates)) {
                if (signal.value) {
                    high++;
                } else {
                    low++;
                }
            }
        }
        return low * high;
    }

    public object PartTwo(string input) {
        var gates = ParseGates(input);
        var nand = gates["rx"].inputs.Keys.First();
        var m = BigInteger.One;
        foreach (var gate in gates[nand].inputs.Keys) {
            var s = 1;
            gates = ParseGates(input);
            while (!Button(gates).Any(signal => signal.receiver == gate && !signal.value)) {
                s++;
            }
            m *= s;
        }
        return m;
    }

    IEnumerable<Signal> Button(Gates gates) {
        var q = new Queue<(string src, string dst, bool signal)>();

        var emit = (string name, bool signal) => {
            foreach (var output in gates[name].outputs) {
                q.Enqueue((name, output, signal));
            }
        };

        emit("button", false);
        while (q.Any()) {
            var signal = q.Dequeue();
            yield return signal;

            var (sender, receiver, value) = signal;

            var gate = gates[receiver];
            switch (gate.kind) {
                case "&":
                    gate.inputs[sender] = value;
                    emit(receiver, !gate.inputs.Values.All(x => x));
                    break;
                case "%":
                    if (!value) {
                        gate.inputs["state"] = !gate.inputs.GetValueOrDefault("state");
                        emit(receiver, gate.inputs["state"]);
                    }
                    break;
                case "":
                    emit(receiver, value);
                    break;
            }
        }
    }

    Gates ParseGates(string input) {
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
        gates["rx"] = new Gate("", [], new Dictionary<string, bool>());
        foreach(var gate in gates.Keys){
            foreach(var output in gates[gate].outputs){
                gates[output].inputs[gate] = false;
            }
        }
        return gates;
    }
}
