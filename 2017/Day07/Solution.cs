using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2017.Day07 {

    class Node {
        public string Id;
        public string[] Children;
        public int Weight;
        public int TreeWeight = -1;
    }

    class Tree : Dictionary<string, Node> { }

    [ProblemName("Recursive Circus")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            var tree = new Tree();
            foreach (var line in input.Split('\n')) {
                var parts = Regex.Match(line, @"(?<id>[a-z]+) \((?<weight>[0-9]+)\)( -> (?<children>.*))?");

                tree.Add(
                    parts.Groups["id"].Value,
                    new Node {
                        Id = parts.Groups["id"].Value,
                        Weight = int.Parse(parts.Groups["weight"].Value),
                        Children = string.IsNullOrEmpty(parts.Groups["children"].Value)
                            ? new string[0]
                            : Regex.Split(parts.Groups["children"].Value, ", "),
                    });
            }

            yield return PartOne(tree);
            yield return PartTwo(tree);
        }

        string PartOne(Tree tree) => Root(tree).Id;

        int PartTwo(Tree tree) {
            var root = Root(tree);
            ComputeTreeWeights(root, tree);
            var bogusChild = BogusChild(root, tree);
            var desiredWeight = tree[root.Children.First(childId => childId != bogusChild.Id)].TreeWeight;
            return Fix(bogusChild, desiredWeight, tree);
        }

        Node Root(Tree tree) =>
            tree.Values.First(node => !tree.Values.Any(nodeParent => nodeParent.Children.Contains(node.Id)));

        int ComputeTreeWeights(Node node, Tree tree) {
            node.TreeWeight = node.Weight + node.Children.Select(childId => ComputeTreeWeights(tree[childId], tree)).Sum();
            return node.TreeWeight;
        }

        Node BogusChild(Node node, Tree tree) {
            var w =
                (from childId in node.Children
                 let child = tree[childId]
                 group child by child.TreeWeight into childrenByTreeWeight
                 orderby childrenByTreeWeight.Count()
                 select childrenByTreeWeight).ToArray();

            return w.Length == 1 ? null : w[0].Single();
        }

        int Fix(Node node, int desiredWeight, Tree tree) {
            if (node.Children.Length < 2) {
                throw new NotImplementedException();
            } 

            var bogusChild = BogusChild(node, tree);

            if (bogusChild == null) {
                return desiredWeight - node.TreeWeight + node.Weight;
            } else {
                desiredWeight = desiredWeight - node.TreeWeight + bogusChild.TreeWeight;
                return Fix(bogusChild, desiredWeight, tree);
            }
        }
    }
}
