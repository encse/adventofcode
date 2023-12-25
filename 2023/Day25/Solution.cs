namespace AdventOfCode.Y2023.Day25;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;
using AngleSharp.Common;

[ProblemName("Snowverload")]
class Solution : Solver
{

    (string, string) rev((string, string) p) => (p.Item2, p.Item1);
    public object PartOne(string input)
    {

        var g = Parse(input);
        var m = FindMinCut(g);
        Console.WriteLine(m);
        while (m != 3)
        {
            g = Parse(input);
            m = FindMinCut(g);
            Console.WriteLine(m);
        }
        return m;
    }

    Random r = new Random();


    int FindMinCut(Dictionary<string, List<string>> graph)
    {
        Check(graph);
        Dictionary<string, int> componentSize = graph.Keys.ToDictionary(k => k, _ => 1);
        while (graph.Count > 2)
        {
            // Choose a random edge
            int randomEdgeIndex = r.Next(graph.Count);
            var u = graph.Keys.ElementAt(randomEdgeIndex);
            var v = graph[u][r.Next(graph[u].Count)];

            if (u == v)
            {
                Console.WriteLine("????");
            }

            if (!graph.ContainsKey(v))
            {
                Console.WriteLine("????");
            }
            // Contract the edge (merge vertices u and v)
            var neighbours = graph[v].ToArray();
            foreach (var neighbor in neighbours)
            {
                if(neighbor == u){
                    continue;
                }
                if(neighbor == v){
                    Console.WriteLine("????");
                }
                if (!graph.ContainsKey(neighbor))
                {
                    Console.WriteLine("????");
                }

                graph[neighbor].Remove(v);
                graph[neighbor].Add(u);
            }

            componentSize[u] = componentSize[u] + componentSize[v];
            graph[u] = graph[u].Concat(graph[v]).Where(x => x != u && x != v).ToList();
            graph.Remove(v);
        }

        // The remaining graph has only two vertices
        var firstVertex = graph.Keys.First();
        if (graph[firstVertex].Count == 3){
            var c1 = componentSize[graph.Keys.First()];
            var c2 = componentSize[graph.Keys.Last()];
            Console.WriteLine(c1 * c2);
        }
        return graph[firstVertex].Count;
    }

    private bool Check(Dictionary<string, List<string>> graph)
    {
        foreach (var v in graph.Keys)
        {
            foreach (var u in graph[v])
            {
                if (!graph.ContainsKey(u))
                {
                    return false;
                }
                if (!graph[u].Contains(v))
                {
                    return false;
                }
            }
        }
        return true;
    }


    Dictionary<string, List<string>> Parse(string input)
    {
        var res = new Dictionary<string, List<string>>();
        var add = (string a, string b) =>
        {
            if (!res.ContainsKey(a))
            {
                res[a] = new List<string>();
            }
            if (res[a].Contains(b))
            {
                Console.WriteLine("???");
            }
            res[a].Add(b);
        };

        foreach (var line in input.Split('\n'))
        {
            var parts = line.Split(": ");
            var left = parts[0];
            var rights = parts[1].Split(' ');
            foreach (var right in rights)
            {
                add(left, right);
                add(right, left);
            }
        }

        return res;
    }


}


// int FindMinCut(Dictionary<string, List<string>> graph)
// {
//     Check(graph);
//     var inode = 0;
//     while (graph.Count > 2)
//     {
//         // Choose a random edge
//         int randomEdgeIndex = r.Next(graph.Count);
//         var u = graph.Keys.ElementAt(randomEdgeIndex);
//         var v = graph[u][r.Next(graph[u].Count)];

//         if (u == v)
//         {
//             Console.WriteLine("????");
//         }


//         // Contract the edge (merge vertices u and v)
//         var newNode = "" + inode;
//         inode++;
//         if (graph.ContainsKey(newNode))
//         {
//             Console.WriteLine("????");
//         }

//         foreach (var q in new[] { u, v })
//         {
//             if (!graph.ContainsKey(q))
//             {
//                 continue;
//             }
//             var neighbours = graph[q];
//             foreach (var neighbor in neighbours.ToArray())
//             {
//                 if (neighbor == u || neighbor == v)
//                 {
//                     continue;
//                 }
//                 if (!graph.ContainsKey(neighbor))
//                 {
//                     continue;

//                 }

//                 graph[neighbor].Remove(q);
//                 if (!graph[neighbor].Contains(newNode))
//                 {
//                     graph[neighbor].Add(newNode);
//                     if (neighbor == newNode)
//                     {
//                         Console.WriteLine("xxx");
//                     }
//                 }
//             }
//         }

//         graph[newNode] = new ();
//         if (graph.ContainsKey(u))
//         {
//             graph[newNode].AddRange(graph[u]);
//         }
//         if (graph.ContainsKey(v))
//         {
//             graph[newNode].AddRange(graph[v]);
//         }
//         graph[newNode] = graph[newNode].Distinct().ToList();
//         graph[newNode].Remove(u);
//         graph[newNode].Remove(v);

//         if (graph[newNode].Count == 0){
//             graph.Remove(newNode);
//         }

//         graph.Remove(u);
//         graph.Remove(v);
//     }

//     // The remaining graph has only two vertices
//     var firstVertex = graph.Keys.First();
//     var lastVertex = graph.Keys.Last();
//     var res = graph[firstVertex].Count + graph[lastVertex].Count;
//     if(res == 3){
//         Console.WriteLine(string.Join(", ", graph[firstVertex]));
//         Console.WriteLine(string.Join(", ", graph[lastVertex]));
//     }
//     return res;
// }
