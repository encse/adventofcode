namespace AdventOfCode.Y2023.Day25;

using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Common;

[ProblemName("Snowverload")]
class Solution : Solver {

    public object PartOne(string input) {
        Random r = new Random(25);

        // run Karger's algorithm until it finds a cut with 3 edges
        var (cutSize, c1, c2) = FindCut(input, r);
        while (cutSize != 3) {
            (cutSize, c1, c2) = FindCut(input, r);
        }
        return c1 * c2;
    }

    // https://en.wikipedia.org/wiki/Karger%27s_algorithm
    // Karger's algorithm finds a cut of a graph and returns its size. 
    // It's not necessarily the minimal cut, because it's a randomized algorithm 
    // but it's 'likely' to find the minimal cut in reasonable time. 
    // The algorithm is extended to return the sizes of the two components 
    // separated by the cut as well.
    (int size, int c1, int c2) FindCut(string input, Random r) {
        var graph = Parse(input);
        var componentSize = graph.Keys.ToDictionary(k => k, _ => 1);

        // updates backreferences of oldNode to point to newNode
        var rebind = (string oldNode, string newNode) => {
            foreach (var n in graph[oldNode]) {
                while (graph[n].Remove(oldNode)) {
                    graph[n].Add(newNode);
                }
            }
        };

        for (var id = 0; graph.Count > 2; id++) {
            // Decrease the the number of nodes one. First select two nodes u 
            // and v connected with an edge. Introduce a new node that inherits 
            // every edges goint out from tehse (excluding the edges between them). 
            // Set the new nodes' component size to the sum of the component 
            // sizes of u and v. Remove u and v from the graph.
            var u = graph.Keys.ElementAt(r.Next(graph.Count));
            var v = graph[u][r.Next(graph[u].Count)];
            string[] edge = [u, v];

            var merged = "merge-" + id;
            graph[merged] = [
                ..from n in graph[u] where n != v select n,
                ..from n in graph[v] where n != u select n
            ];
            rebind(u, merged);
            rebind(v, merged);

            componentSize[merged] = componentSize[u] + componentSize[v];

            graph.Remove(u);
            graph.Remove(v);
        }

        // two nodes remains with some edges between them, the number of those 
        // edges equals to the size of the cut. Component size tells the number 
        // of nodes in the two sides created by the cut.
        var nodeA = graph.Keys.First();
        var nodeB = graph.Keys.Last();
        return (graph[nodeA].Count(), componentSize[nodeA], componentSize[nodeB]);
    }

    // returns an adjacency list representation of the input. Edges are recorded 
    // both ways, unlike in the input which contains them in one direction only.
    Dictionary<string, List<string>> Parse(string input) {
        var graph = new Dictionary<string, List<string>>();

        var registerEdge = (string u, string v) => {
            if (!graph.ContainsKey(u)) {
                graph[u] = new();
            }
            graph[u].Add(v);
        };

        foreach (var line in input.Split('\n')) {
            var parts = line.Split(": ");
            var u = parts[0];
            var nodes = parts[1].Split(' ');
            foreach (var v in nodes) {
                registerEdge(u, v);
                registerEdge(v, u);
            }
        }
        return graph;
    }
}
