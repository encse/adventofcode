using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2017.Day13 {

    class Layers : List<(int depth, int range)> {
        public Layers(IEnumerable<(int depth, int range)> layers) : base(layers) {
        }
    }

    class Solution : Solver {

        public string GetName() {
            return "Packet Scanners";
        }

        public void Solve(string input) {
            var layers = Parse(input);
            Console.WriteLine(PartOne(layers));
            Console.WriteLine(PartTwo(layers));
        }

        int PartOne( Layers layers) => Severities(layers, 0).Sum();

        int PartTwo(Layers layers) =>
            Enumerable
            .Range(0, int.MaxValue)
            .First(n => !Severities(layers, n).Any());

        Layers Parse(string input) =>
            new Layers(
                from line in input.Split('\n')
                let parts = Regex.Split(line, ": ").Select(int.Parse).ToArray()
                select (parts[0], parts[1])
            );

        IEnumerable<int> Severities(Layers layers, int t) {
            var packetPos = 0;
            foreach (var layer in layers) {
                t += layer.depth - packetPos;
                packetPos = layer.depth;
                var scannerPos = t % (2 * layer.range - 2);
                if (scannerPos == 0) {
                    yield return layer.depth * layer.range;
                }
            }
        }
    }
}
