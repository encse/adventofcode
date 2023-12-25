namespace AdventOfCode.Y2023.Day25;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Snowverload")]
class Solution : Solver {

    public object PartOne(string input) {
        Random r = new Random();
        // call Karger's algorithm in a loop until it finds a cut with 3 edges:
        var (cutSize, c1, c2) = FindCut(input, r);
        while (cutSize != 3) {
            (cutSize, c1, c2) = FindCut(input, r);
        }
        return c1 * c2;
    }

    // https://en.wikipedia.org/wiki/Karger%27s_algorithm
    // The Karger algorithm returns the size of one 'cut' of the graph. 
    // It's a randomized algorithm that is 'likely' to find the minimal cut 
    // in a reasonable time. The algorithm is extended to also return the sizes 
    // of the two components separated by the cut.
    (int size, int c1, int c2) FindCut(string input, Random r) {

        // super inefficient version.

        var graph = Parse(input);
        var componentSize = graph.Keys.ToDictionary(k => k, _ => 1);

        while (graph.Count > 2) {
            // choose a random edge u-v to contract.
            var u = graph.Keys.ElementAt(r.Next(graph.Count));
            var v = graph[u][r.Next(graph[u].Count)];

            // Merge nodes u and v by removing 'v' from the graph and rebinding 
            // the edges of 'v' so that they start from 'u' now. 
            // There are no multiple edges between two nodes in the original
            // graph, but this algorithm will introduce some.

            // rebind
            foreach (var neighbour in graph[v].ToArray()) {
                graph[neighbour].Remove(v);
                graph[neighbour].Add(u);
            }

            // 'u' inherits 'v'-s edges
            graph[u] = [.. graph[u], ..graph[v]];

            // get rid of the self references introduced by the above steps
            graph[u] = [.. graph[u].Where(n => n != u)];

            // update component size 
            componentSize[u] = componentSize[u] + componentSize[v];
          
            // 'v' can be removed now
            graph.Remove(v);
            componentSize.Remove(v);
        }

        // at the end we have just two nodes with some edges between them,
        // the number of those edges equals to the size of the cut
        var nodeA = graph.Keys.First();
        var nodeB = graph.Keys.Last();
        return (graph[nodeA].Count, componentSize[nodeA], componentSize[nodeB]);
    }

    // Returns an adjacency list representation of the input. Edges are recorded 
    // both ways, unlike in the input which contains them in one direction only.
    Dictionary<string, List<string>> Parse(string input) {
        Dictionary<string, List<string>> res = new();

        var registerEdge = (string u, string v) => {
            if (!res.ContainsKey(u)) {
                res[u] = new();
            }
            res[u].Add(v);
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
        return res;
    }
}
