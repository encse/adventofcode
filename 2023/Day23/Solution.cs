namespace AdventOfCode.Y2023.Day23;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using System.Linq;
using Map = System.Collections.Immutable.IImmutableDictionary<System.Numerics.Complex, char>;
record Node(int id, Complex pos);
record Edge(Node from, Node to, int length);

[ProblemName("A Long Walk")]
class Solution : Solver {

    public object PartOne(string input) {
        return Solve(ParseMap(input));
    }

    public object PartTwo(string input) {
        input = input.Replace(">", ".").Replace("v", ".");
        return Solve(ParseMap(input));
    }

    int Solve(Map map) {
        var startPos = 1;
        var goalPos = map.Keys.MaxBy(pos => pos.Imaginary + pos.Real) - 1;

        var nodes = Nodes(map).ToArray();
        var edges = Edges(map, nodes).ToArray();

        Console.WriteLine(("nodes", nodes.Length));
        Console.WriteLine(("edges", edges.Length));

        var start = nodes.Single(n => n.pos == startPos);
        var goal = nodes.Single(n => n.pos == goalPos);
        return Dp(start, goal, ImmutableHashSet<Node>.Empty.Add(start), edges, new Dictionary<string, int>());
    }

    int Dp(Node start, Node end, ImmutableHashSet<Node> visited, Edge[] edges, Dictionary<string, int> cache) {
        var key = start.id + "-" + string.Join(",", visited.OrderBy(x => x.id).Select(x => x.id));
        if (!cache.ContainsKey(key)) {
            var m = 0;
            foreach (var e in edges.Where(e => e.from == start)) {
                if (!visited.Contains(e.to)) {
                    m = Math.Max(m, e.length + Dp(e.to, end, visited.Add(e.to), edges, cache));
                }
            }
            cache[key] = m;
        }
        return cache[key];
    }

    Complex[] dirs = [1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne];

    bool IsFree(Map map, Complex pos) {
        return map.ContainsKey(pos) && map[pos] != '#';
    }
    bool IsRoad(Map map, Complex pos) {
        return IsFree(map, pos) && dirs.Count(dir => IsFree(map, pos + dir)) == 2;
    }

    Node[] Nodes(Map map) {
        var nodePositions = map.Keys.Where(pos => IsFree(map, pos) && !IsRoad(map, pos));
        return Enumerable.Zip(Enumerable.Range(0, int.MaxValue), nodePositions).Select(p =>
            new Node(p.First, p.Second)
        ).ToArray();
    }

    Edge[] Edges(Map map, Node[] nodes) {
        return (
            from nodeA in nodes
            from nodeB in nodes
            where nodeA != nodeB
            let path = GetPath(map, nodeA.pos, nodeB.pos)
            where path != int.MaxValue
            select new Edge(nodeA, nodeB, path)
        ).ToArray();
    }

    int GetPath(Map map, Complex from, Complex to) {
        var q = new Queue<(Complex, int)>();
        q.Enqueue((from, 0));
        var seen = new HashSet<Complex>();

        while (q.Any()) {
            var (pos, dist) = q.Dequeue();
            if (pos == to) {
                return dist;
            }
            foreach (var posT in Next(map, pos)) {
                if (!IsFree(map, posT) || seen.Contains(posT)) {
                    continue;
                }
                if (posT == to || IsRoad(map, posT)) {
                    seen.Add(posT);
                    q.Enqueue((posT, dist + 1));
                }
            }
        }
        return int.MaxValue;
    }

    IEnumerable<Complex> Next(Map map, Complex pos) {
        if (map[pos] == '.') {
            foreach (var dir in dirs) {
                yield return pos + dir;
            }
        } else if (map[pos] == '>') {
            yield return pos + 1;
        } else if (map[pos] == 'v') {
            yield return pos + Complex.ImaginaryOne;
        } else {
            Console.WriteLine(map[pos]);
            throw new Exception();
        }
    }

    Map ParseMap(string input) {
        var lines = input.Split('\n');
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            let cell = lines[irow][icol]
            let pos = new Complex(icol, irow)
            select new KeyValuePair<Complex, char>(pos, cell)
        ).ToImmutableDictionary();
    }
}