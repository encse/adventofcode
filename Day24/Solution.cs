using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day24 {

    class Solution : Solver {

        public string GetName() => "Electromagnetic Moat";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) =>
           LongestBridge(input, 
                0, 
                (component, rest) => rest + component.pinA + component.pinB);

        int PartTwo(string input) =>
            LongestBridge(
                input,
                (length: 0, strength: 0),
                (component, rest) => (rest.length + 1, rest.strength + component.pinA + component.pinB)
            ).strength;

        T LongestBridge<T>(string input, T seed, Func<Component, T, T> plug) where T : IComparable<T> {

            T fold(int pinIn, HashSet<Component> components) {
                var tMax = seed;
                foreach (var component in components.ToList()) {
                    var pinOut =
                        pinIn == component.pinA ? component.pinB :
                        pinIn == component.pinB ? component.pinA :
                         -1;

                    if (pinOut != -1) {
                        components.Remove(component);
                        var curr = plug(component, fold(pinOut, components));
                        tMax = curr.CompareTo(tMax) > 0 ? curr : tMax;
                        components.Add(component);
                    }
                }
                return tMax;
            }
            return fold(0, Parse(input));
        }

        HashSet<Component> Parse(string input) {
            var components = new HashSet<Component>();
            foreach (var line in input.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line))) {
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