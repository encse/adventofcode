namespace AdventOfCode.Y2024.Day16;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Numerics;
using AdventOfCode.Y2018.Day13;
using AdventOfCode.Y2023.Day15;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
using Reindeer = (System.Numerics.Complex pos, System.Numerics.Complex dir);

[ProblemName("Reindeer Maze")]
class Solution : Solver {

    static readonly Complex Up = -Complex.ImaginaryOne;
    static readonly Complex Down = Complex.ImaginaryOne;
    static readonly Complex Left = -1;
    static readonly Complex Right = 1;
    static readonly Complex[] Dirs = { Up, Right, Left, Down };

    public object PartOne(string input) {
        var map = GetMap(input);
        var start = map.Keys.Single(k => map[k] == 'S');
        var goal = map.Keys.Single(k => map[k] == 'E');

        var dist = Distances(map, goal);
        return dist[(start, Right)];
    }

    public object PartTwo(string input) {
        var map = GetMap(input);
        var start = map.Keys.Single(k => map[k] == 'S');
        var goal = map.Keys.Single(k => map[k] == 'E');

        var dist = Distances(map, goal);
        return FindBestSpots(map, dist, start).Count;
    }

    IEnumerable<Reindeer> Prev(Reindeer r) {
        foreach (var dir in Dirs) {
            if (dir == r.dir) {
                yield return (r.pos - dir, dir);
            } else if (dir != -r.dir) {
                yield return (r.pos, dir);
            }
        }
    }
     IEnumerable<Reindeer> Next(Reindeer r) {
        foreach (var dir in Dirs) {
            if (dir == r.dir) {
                yield return (r.pos + dir, dir);
            } else if (dir != -r.dir) {
                yield return (r.pos, dir);
            }
        }
    }
    HashSet<Complex> FindBestSpots(Map map, Dictionary<Reindeer, int> dist, Complex start) {

        var q = new PriorityQueue<Reindeer, int>();
        q.Enqueue((start, Right), dist[(start, Right)]);
        var seen = new HashSet<Reindeer>{(start, Right)};

        while (q.TryDequeue(out var reindeer, out var totalCost)) {
            foreach (var next in Next(reindeer)) {
                if (map.GetValueOrDefault(next.pos) == '#') {
                    continue;
                }
                
                if (seen.Contains(next)) {
                    continue;
                }

                var cost = next.dir == reindeer.dir ? 1 : 1000;
                var qqqq = dist[next];
                if (dist[next] + cost == totalCost) {
                    seen.Add(next);
                    q.Enqueue(next, dist[next]);
                }
            }
        }
        return seen.Select(x=>x.pos).ToHashSet();
    }

    Dictionary<Reindeer, int> Distances(Map map, Complex goal) {
        var res = new Dictionary<Reindeer, int>();

        var q = new PriorityQueue<Reindeer, int>();
        foreach (var dir in Dirs) {
            q.Enqueue((goal, dir), 0);
            res[(goal, dir)] = 0;
        }

        while (q.TryDequeue(out var reindeer, out var totalCost)) {
            if (totalCost != res[reindeer]) {
                continue;
            }
            foreach (var next in Prev(reindeer)) {
                if (map.GetValueOrDefault(next.pos) == '#') {
                    continue;
                }

                var cost = next.dir == reindeer.dir ? 1 : 1000;
                var nextCost = totalCost + cost;

                if (res.ContainsKey(next) && res[next] < nextCost) {
                    continue;
                }

                res[next] = nextCost;
                q.Enqueue(next, nextCost);
            }
        }
        return res;
    }

    (Complex dir, int totalCost) Solve(Map map, Reindeer start, Complex goal) {

        var q = new PriorityQueue<Reindeer, int>();

        q.Enqueue(start, 0);

        var seen = new HashSet<Reindeer>();

        while (q.TryDequeue(out var reindeer, out var totalCost)) {
            if (reindeer.pos == goal) {
                return (reindeer.dir, totalCost);
            }

            foreach (var nextDir in new[] { Right, Left, Up, Down }) {
                var nextPos = reindeer.pos + nextDir;

                if (nextDir == -reindeer.dir) {
                    continue;
                }

                if (map.GetValueOrDefault(nextPos) == '#') {
                    continue;
                }


                var next = (nextPos, nextDir);
                if (seen.Contains(next)) {
                    continue;
                }

                var cost = nextDir == reindeer.dir ? 1 : 1001;
                seen.Add(next);
                q.Enqueue(next, totalCost + cost);

            }
        }

        return (0, int.MaxValue / 2);
    }

    // store the points in a dictionary so that we can iterate over them and 
    // to easily deal with points outside the area using GetValueOrDefault
    Map GetMap(string input) {
        var map = input.Split("\n");
        return (
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Complex, char>(Complex.ImaginaryOne * y + x, map[y][x])
        ).ToDictionary();
    }
}