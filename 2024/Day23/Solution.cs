namespace AdventOfCode.Y2024.Day23;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Graph = System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>;
using Component = string;

[ProblemName("LAN Party")]
class Solution : Solver {
    public object PartOne(string input) {
        var g = GetGraph(input);
        var components = g.Keys.ToHashSet();
        components = Grow(g, components);
        components = Grow(g, components);
        return components.Count(c => Members(c).Any(m => m.StartsWith("t")));
    }

    public object PartTwo(string input) {
        var g = GetGraph(input);
        var components = g.Keys.ToHashSet();
        while (components.Count > 1) {
            components = Grow(g, components);
        }
        return components.Single();
    }
    
    HashSet<Component> Grow(Graph g, HashSet<Component> components) => (
        from c in components.AsParallel()
        let members = Members(c)
        from neighbour in members.SelectMany(m => g[m]).Distinct()
        where !members.Contains(neighbour)
        where members.All(m => g[neighbour].Contains(m))
        select Extend(c, neighbour)
    ).ToHashSet();

    IEnumerable<string> Members(Component c) => 
        c.Split(",");
    Component Extend(Component c, string item) => 
        string.Join(",", Members(c).Append(item).OrderBy(x=>x));

    Graph GetGraph(string input) {
        var edges = 
            from line in input.Split("\n")
            let nodes = line.Split("-")
            from edge in new []{(nodes[0], nodes[1]), (nodes[1], nodes[0])}
            select (From: edge.Item1, To: edge.Item2);

       return (
            from e in edges
            group e by e.From into g
            select (g.Key, g.Select(e => e.To).ToHashSet())
        ).ToDictionary();
    }
}