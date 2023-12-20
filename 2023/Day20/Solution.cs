namespace AdventOfCode.Y2023.Day20;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gates = System.Collections.Generic.Dictionary<string, Gate>;
using Signal = (string sender, string receiver, bool value);

// Different gate types come with different handlers. No OOP this year.
record Gate(string[] inputs, Func<Signal, IEnumerable<Signal>> handle);

[ProblemName("Pulse Propagation")]
class Solution : Solver {

    public object PartOne(string input) {
        // Press the button 1000 times and count the high and low signals:
        var emitted = new Dictionary<bool, int>{[false] = 0, [true]=0};
        var gates = ParseGates(input);
        for (var i = 0; i < 1000; i++) {
            foreach (var signal in PressButton(gates)) {
                emitted[signal.value]++;
            }
        }
        return emitted[false] * emitted[true];
    }

    public object PartTwo(string input) {
        // The input has a special structure.
        //
        // The broadcaster feeds 4 disconnected substructures which are 
        // channeld into a single nand gate at the end. The nand gate is
        // connected into rx.
        // I checked that the substructures work in a loop, and the length
        // of those are relative primes. So we can just multiply them all
        // to get the final result.

        var gates = ParseGates(input);
        var nand = gates["rx"].inputs.Single();

        var m = 1L;
        foreach (var substructure in gates[nand].inputs) {
            gates = ParseGates(input); // always start from a fresh input

            var press = 0;
            while (true) {
                var signals = PressButton(gates);
                press++;
                if (signals.Any(s => s.receiver == substructure && !s.value)) {
                    break;
                }
            }
            m *= press;
        }
        return m;
    }

    // emits a button press, executes until things settle down and returns 
    // all signals for investigation.
    IEnumerable<Signal> PressButton(Gates gates) {
        var q = new Queue<Signal>();
        q.Enqueue(new Signal("button", "broadcaster", false));
        while (q.TryDequeue(out var signal)) {
            yield return signal;
            var receiver = gates[signal.receiver];
            foreach (var signalT in receiver.handle(signal)) {
                q.Enqueue(signalT);
            }
        }
    }

    Gates ParseGates(string input) {
        var entries = (
            from line in input.Split('\n')
            let kind = char.IsLetter(line[0]) ? "" : line[0..1]
            let parts = from m in Regex.Matches(line, "[a-z]+") select m.Value
            select (kind, name: parts.First(), outputs: parts.Skip(1).ToArray())
        ).ToList();

        // these extra guys are needed
        entries.Add(("", "button", ["broadcaster"]));
        entries.Add(("", "rx", []));

        var gates = new Gates();
        foreach (var entry in entries) {
            var inputs = (
                from e in entries
                where e.outputs.Contains(entry.name)
                select e.name
            ).ToArray();

            gates[entry.name] = entry.kind switch {
                "&" => NandGate(entry.name, inputs, entry.outputs),
                "%" => FlipFlop(entry.name, inputs, entry.outputs),
                _ => Repeater(entry.name, inputs, entry.outputs)
            };
        }
        return gates;
    }

    Gate NandGate(string gate, string[] inputs, string[] outputs) {
        var state = inputs.ToDictionary(input => input, _ => false);
        return new Gate(inputs, (Signal signal) => {
            state[signal.sender] = signal.value;
            var value = !state.Values.All(b => b);
            return outputs.Select(receiver => new Signal(gate, receiver, value));
        });
    }

    Gate FlipFlop(string gate, string[] inputs, string[] outputs) {
        var state = false;
        return new Gate(inputs, (Signal signal) => {
            if (!signal.value) {
                state = !state;
                return outputs.Select(receiver => new Signal(gate, receiver, state));
            } else {
                return [];
            }
        });
    }

    Gate Repeater(string gate, string[] inputs, string[] outputs) {
        return new Gate(inputs, (Signal s) => 
            from receiver in outputs select new Signal(gate, receiver, s.value)
        );
    }
}