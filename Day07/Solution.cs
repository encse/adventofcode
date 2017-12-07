using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2017.Day07 {

    class Node {
        public string Id;
        public string[] Children;
        public int Weight;
        public int RecursiveWeight = -1;
    }

    class Solution : Solver {

        public void Solve(string input) {
            var nodes = new Dictionary<string, Node>();
            foreach(var line in input.Split('\n')) {
                var parts = Regex.Match(line, @"(?<id>[a-z]+) \((?<weight>[0-9]+)\)( -> (?<children>.*))?");

                nodes.Add(
                    parts.Groups["id"].Value,
                    new Node {
                        Id = parts.Groups["id"].Value,
                        Weight = int.Parse(parts.Groups["weight"].Value),
                        Children = string.IsNullOrEmpty(parts.Groups["children"].Value) 
                            ? new string[0] 
                            : Regex.Split(parts.Groups["children"].Value, ", "),
                    });
            }

            Console.WriteLine(PartOne(nodes));
            Console.WriteLine(PartTwo(nodes));
        }

        string PartOne(Dictionary<string, Node> nodes) => Root(nodes).Id;
        int PartTwo(Dictionary<string, Node> nodes) {
            var root = Root(nodes);
            ComputeRecursiveWeights(root, nodes);
            return PartTwoRecursive(root, nodes);
        }

        int PartTwoRecursive(Node node, Dictionary<string, Node> nodes) {
            var w = 
                (from childId in node.Children
                let child = nodes[childId]
                group child by child.RecursiveWeight into g 
                orderby g.Count()
                select g).ToArray();

            var desiredWeight = w[1].Key;
            var bogusChild = w[0].Single();
            return Fix(bogusChild, desiredWeight, nodes);
        }

        int Fix(Node node, int desiredWeight, Dictionary<string, Node> nodes){
            if (node.Children.Length == 0) {
                return desiredWeight;
            } else if (node.Children.Length == 1) {
                var childrenWeightSum = node.RecursiveWeight - node.Weight;
                // desiredWeight = node.Weight + childrenWeightSum;
                if (desiredWeight - childrenWeightSum < 0) {
                    return Fix(nodes[node.Children.Single()], desiredWeight - node.Weight, nodes);
                } else {
                    return desiredWeight - childrenWeightSum;
                }
            } else {
                var childrenWeightSum = node.RecursiveWeight - node.Weight;
                var w = 
                    (from childId in node.Children
                    let child = nodes[childId]
                    group child by child.RecursiveWeight into g 
                    orderby g.Count()
                    select g).ToArray();
                if (w.Length == 1){
                    return desiredWeight - childrenWeightSum;
                } else {
                    var bogusChild = w[0].Single();
                    desiredWeight = desiredWeight - node.Weight - childrenWeightSum + bogusChild.RecursiveWeight;
                    return Fix(bogusChild, desiredWeight, nodes);
                }
            }
        }

        Node Root(Dictionary<string, Node> nodes) => 
            nodes.Values.First(node => !nodes.Values.Any(nodeParent => nodeParent.Children.Contains(node.Id)));

        
        int ComputeRecursiveWeights(Node node, Dictionary<string, Node> nodes) {
            node.RecursiveWeight = node.Weight + node.Children.Select(childId => ComputeRecursiveWeights(nodes[childId], nodes)).Sum();
            return node.RecursiveWeight;
        }
    }
}
