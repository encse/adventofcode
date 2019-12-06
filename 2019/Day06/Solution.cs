using System;
using System.Collections.Generic;
using System.Linq;

using ChildToParent = System.Collections.Generic.Dictionary<string, string>;

namespace AdventOfCode.Y2019.Day06 {

    class Solution : Solver {

        public string GetName() => "Universal Orbit Map";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var childToParent = ParseTree(input);
            return (
                from node in childToParent.Keys
                select GetAncestors(childToParent, node).Count()
            ).Sum();
        }

        int PartTwo(string input) {
            var childToParent = ParseTree(input);
            var ancestors1 = GetAncestors(childToParent, "YOU").ToArray();
            var ancestors2 = GetAncestors(childToParent, "SAN").ToArray();
            for (var i = 0; i < ancestors1.Length; i++) {
                for (var k = 0; k < ancestors2.Length; k++) {
                    if (ancestors1[i] == ancestors2[k]) {
                        return i + k;
                    }
                }
            }
            throw new Exception();
        }

        ChildToParent ParseTree(string input) =>
            input
                .Split("\n")
                .Select(line => line.Split(")"))
                .ToDictionary(
                    parent_child => parent_child[1],
                    parent_child => parent_child[0]
                );

        IEnumerable<string> GetAncestors(ChildToParent childToParent, string node) {
            for (
                var parent = childToParent[node];
                parent != null;
                parent = childToParent.GetValueOrDefault(parent, null)
            ) {
                yield return parent;
            }

        }
    }
}