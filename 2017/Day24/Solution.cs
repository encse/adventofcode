using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Day24 {

    [ProblemName("Electromagnetic Moat")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => StrongestBridge(input, (a, b) => a.strength - b.strength);
        int PartTwo(string input) => StrongestBridge(input, (a, b) => a.CompareTo(b));

        int StrongestBridge(string input, Func<(int length, int strength), (int length, int strength), int> compare) {

            (int length, int strength) fold(int pinIn, HashSet<Component> components) {
                var strongest = (0, 0);
                foreach (var component in components.ToList()) {
                    var pinOut =
                        pinIn == component.pinA ? component.pinB :
                        pinIn == component.pinB ? component.pinA :
                         -1;

                    if (pinOut != -1) {
                        components.Remove(component);
                        var curr = fold(pinOut, components);
                        (curr.length, curr.strength) = (curr.length + 1, curr.strength + component.pinA + component.pinB);
                        strongest = compare(curr, strongest) > 0 ? curr : strongest;
                        components.Add(component);
                    }
                }
                return strongest;
            }
            return fold(0, Parse(input)).strength;
        }

        HashSet<Component> Parse(string input) {
            var components = new HashSet<Component>();
            foreach (var line in input.Split('\n')) {
                var parts = line.Split('/');
                components.Add(new Component { pinA = int.Parse(parts[0]), pinB = int.Parse(parts[1]) });
            }
            return components;
        }
    }

    class Component {
        public int pinA;
        public int pinB;
    }
}