namespace AdventOfCode.Y2023.Day23;

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
using Node = long; // will use powers of two to identify nodes.
record Edge(Node start, Node end, int distance);

[ProblemName("A Long Walk")]
class Solution : Solver {

    // Instead of dealing with the 'map' tiles directly, we convert it to a graph.
    // Nodes: the entry tile, the exit and the crossroad tiles.
    // Edges: two nodes are connected if there is a direct path between them that 
    //        doesn't contain crossroads.
    // This reduces a problem to ~30 nodes and 120 edges for the Part 2 case
    // which can be solved using a dynamic programming approach.

    static readonly Complex Up = -Complex.ImaginaryOne;
    static readonly Complex Down = Complex.ImaginaryOne;
    static readonly Complex Left = -1;
    static readonly Complex Right = 1;

    Complex[] Dirs = [Up, Down, Left, Right];

    Dictionary<char, Complex[]> exits = new() {
        ['<'] = [Left],
        ['>'] = [Right],
        ['^'] = [Up],
        ['v'] = [Down],
        ['.'] = [Up, Down, Left, Right],
        ['#'] = []
    };

    public object PartOne(string input) => Solve(input);
    public object PartTwo(string input) => Solve(RemoveSlopes(input));

    string RemoveSlopes(string st) =>
        string.Join("", st.Select(ch => ">v<^".Contains(ch) ? '.' : ch));

    int Solve(string input) {
        var (nodes, edges) = MakeGraph(input);
        var (start, goal) = (nodes.First(), nodes.Last()); 

        // Dynamic programming using a cache, 'visited' is a bitset of 'nodes'.
        var cache = new Dictionary<(Node, long), int>();
        int LongestPath(Node node, long visited) {
            if (node == goal) {
                return 0;
            } else if ((visited & node) != 0) {
                return int.MinValue; // small enough to represent '-infinity'
            }
            var key = (node, visited);
            if (!cache.ContainsKey(key)) {
                cache[key] = edges
                    .Where(e => e.start == node)
                    .Select(e => e.distance + LongestPath(e.end, visited | node))
                    .Max();
            }
            return cache[key];
        }
        return LongestPath(start, 0); 
    }

    (Node[], Edge[]) MakeGraph(string input) {
        var map = ParseMap(input);

        // order crossroads in row major order, so the
        // entry node becomes the first and exit is the last.
        var crossroads = (
            from pos in map.Keys
            orderby pos.Imaginary, pos.Real
            where IsFree(map, pos) && !IsRoad(map, pos)
            select pos
        ).ToArray();

        var nodes = (
            from i in Enumerable.Range(0, crossroads.Length)
            select 1L << i
        ).ToArray();

        var edges = (
            from i in Enumerable.Range(0, crossroads.Length)
            from j in Enumerable.Range(0, crossroads.Length)
            where i != j
            let distance = Distance(map, crossroads[i], crossroads[j])
            where distance > 0
            select new Edge(nodes[i], nodes[j], distance)
        ).ToArray();

        return (nodes, edges);
    }

    // Length of the road between two crossroads; -1 if not neighbours
    int Distance(Map map, Complex crossroadA, Complex crossroadB) {
        var q = new Queue<(Complex, int)>();
        q.Enqueue((crossroadA, 0));

        var visited = new HashSet<Complex> { crossroadA };
        while (q.Any()) {
            var (pos, dist) = q.Dequeue();
            foreach (var dir in exits[map[pos]]) {
                var posT = pos + dir;
                if (posT == crossroadB) {
                    return dist + 1;
                }  else if (IsRoad(map, posT) && !visited.Contains(posT)) {
                    visited.Add(posT);
                    q.Enqueue((posT, dist + 1));
                }
            }
        }
        return -1;
    }

    bool IsFree(Map map, Complex p) => map.ContainsKey(p) && map[p] != '#';
    bool IsRoad(Map map, Complex p) => Dirs.Count(d => IsFree(map, p + d)) == 2;

    Map ParseMap(string input) {
        var lines = input.Split('\n');
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            let pos = new Complex(icol, irow)
            select new KeyValuePair<Complex, char>(pos, lines[irow][icol])
        ).ToDictionary();
    }
}
