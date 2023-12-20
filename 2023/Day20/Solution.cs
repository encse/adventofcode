namespace AdventOfCode.Y2023.Day20;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gates = System.Collections.Generic.Dictionary<string, Gate>;
using Signal = (string sender, string receiver, bool value);

record Gate(string[] inputs, Func<Signal, IEnumerable<Signal>> handle);

[ProblemName("Pulse Propagation")]
class Solution : Solver {

    public object PartOne(string input) {
        var gates = ParseGates(input);
        var values = (
            from _ in Enumerable.Range(0, 1000)
            from signal in Trigger(gates)
            select signal.value
        ).ToArray();
        return values.Count(v => v) * values.Count(v => !v);
    }

    public object PartTwo(string input) {
        // The input has a special structure.
        //
        // Broadcaster feeds 4 disconnected substructures which are 
        // channeled into a single nand gate at the end. The nand gate is
        // connected into rx.
        //
        // I checked that the substructures work in a loop, and the length
        // of those are primes. We just need to multiply them all.
        var gates = ParseGates(input);
        var nand = gates["rx"].inputs.Single();
        var branches = gates[nand].inputs;
        return branches.Aggregate(1L, (m, branch) => m * LoopLength(input, branch));
    }

    int LoopLength(string input, string output) {
        var gates = ParseGates(input);
        for (var i = 1; ; i++) {
            var signals = Trigger(gates);
            if (signals.Any(s => s.sender == output && s.value)) {
                return i;
            }
        }
    }

    // emits a button press, executes until things settle down and returns 
    // all signals for investigation.
    IEnumerable<Signal> Trigger(Gates gates) {
        var q = new Queue<Signal>();
        q.Enqueue(new Signal("button", "broadcaster", false));
        while (q.TryDequeue(out var signal)) {
            yield return signal;
            var handler = gates[signal.receiver];
            foreach (var signalT in handler.handle(signal)) {
                q.Enqueue(signalT);
            }
        }
    }

    Gates ParseGates(string input) {
        var descriptions = (
            from line in input.Split('\n')
            let parts = from m in Regex.Matches(line, "[a-z]+") select m.Value
            select (kind: line[0], name: parts.First(), outputs: parts.Skip(1).ToArray())
        ).ToList();
        descriptions.Add((kind:'r', name: "rx", outputs: []));

        var gates = new Gates();
        foreach (var descr in descriptions) {
            var inputs = (
                from descrT in descriptions
                where descrT.outputs.Contains(descr.name)
                select descrT.name
            ).ToArray();

            gates[descr.name] = descr.kind switch {
                '&' => NandGate(descr.name, inputs, descr.outputs),
                '%' => FlipFlop(descr.name, inputs, descr.outputs),
                _ => Repeater(descr.name, inputs, descr.outputs)
            };
        }
        return gates;
    }

    Gate NandGate(string name, string[] inputs, string[] outputs) {
        // initially assign low value for each input:
        var state = inputs.ToDictionary(input => input, _ => false);

        return new Gate(inputs, (Signal signal) => {
            state[signal.sender] = signal.value;
            var value = !state.Values.All(b => b);
            return outputs.Select(o => new Signal(name, o, value));
        });
    }

    Gate FlipFlop(string name, string[] inputs, string[] outputs) {
        var state = false;

        return new Gate(inputs, (Signal signal) => {
            if (!signal.value) {
                state = !state;
                return outputs.Select(o => new Signal(name, o, state));
            } else {
                return [];
            }
        });
    }

    Gate Repeater(string name, string[] inputs, string[] outputs) {
        return new Gate(inputs, (Signal s) => 
            from o in outputs select new Signal(name, o, s.value)
        );
    }
}