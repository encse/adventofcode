using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2017.Day12 {

    class Node {
        public string Id;
        public List<string> Neighbours;
    }

    class Solution : Solver {

        public string GetName() => "Digital Plumber";

        public void Solve(string input) {
            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
        }

        int PartOne(string input) => GetPartitions(input).Single(x => x.Contains("0")).Count();
        int PartTwo(string input) => GetPartitions(input).Count();

        IEnumerable<HashSet<string>> GetPartitions(string input) {
            var nodes = Parse(input);
            var parent = new Dictionary<string, string>();

            string getRoot(string id) {
                var root = id;
                while (parent.ContainsKey(root)) {
                    root = parent[root];
                }
                return root;
            }

            foreach (var nodeA in nodes) {
                var rootA = getRoot(nodeA.Id);
                foreach (var nodeB in nodeA.Neighbours) {
                    var rootB = getRoot(nodeB);
                    if (rootB != rootA) {
                        parent[rootB] = rootA;
                    }
                }
            }

            return
                from node in nodes
                let root = getRoot(node.Id)
                group node.Id by root into partitions
                select new HashSet<string>(partitions.ToArray());
            
        }

        List<Node> Parse(string input) {
            return (
                from line in input.Split('\n')
                let parts = Regex.Split(line, " <-> ")
                select new Node() {
                        Id = parts[0],
                        Neighbours = new List<string>(Regex.Split(parts[1], ", "))
                    }
            ).ToList();
        }

    }
}
