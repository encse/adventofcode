namespace AdventOfCode.Y2024.Day16;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using AngleSharp.Common;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
using State = (System.Numerics.Complex pos, System.Numerics.Complex dir);

[ProblemName("Reindeer Maze")]
class Solution : Solver {

    static readonly Complex North = -Complex.ImaginaryOne;
    static readonly Complex South = Complex.ImaginaryOne;
    static readonly Complex West = -1;
    static readonly Complex East = 1;
    static readonly Complex[] Dirs = { North, East, West, South };

    public object PartOne(string input) => FindBestScore(GetMap(input));
    public object PartTwo(string input) => FindBestSpots(GetMap(input));

    int FindBestScore(Map map) => Dijkstra(map, Goal(map))[Start(map)];

    int FindBestSpots(Map map) {
        var dist = Dijkstra(map, Goal(map));
        var start = Start(map);

        // track the shortest paths using the distance map as guideline.
        var q = new PriorityQueue<State, int>();
        q.Enqueue(start, dist[start]);

        var bestSpots = new HashSet<State> { start };
        while (q.TryDequeue(out var state, out var remainingScore)) {
            foreach (var (next, score) in Steps(map, state, forward: true)) {
                var nextRemainingScore = remainingScore - score;
                if (!bestSpots.Contains(next) && dist[next] == nextRemainingScore) {
                    bestSpots.Add(next);
                    q.Enqueue(next, nextRemainingScore);
                }
            }
        }
        return bestSpots.DistinctBy(state => state.pos).Count();
    }

    Dictionary<State, int> Dijkstra(Map map, Complex goal) {
        // Dijkstra algorithm; works backwards from the goal returns the
        // distances to all tiles and directions.
        var dist = new Dictionary<State, int>();

        var q = new PriorityQueue<State, int>();
        foreach (var dir in Dirs) {
            q.Enqueue((goal, dir), 0);
            dist[(goal, dir)] = 0;
        }

        while (q.TryDequeue(out var cur, out var totalDistance)) {
            foreach (var (next, score) in Steps(map, cur, forward: false)) {
                var nextCost = totalDistance + score;
                if (nextCost < dist.GetOrDefault(next, int.MaxValue)) {
                    q.Remove(next, out _, out _, null);
                    dist[next] = nextCost;
                    q.Enqueue(next, nextCost);
                }
            }
        }
        return dist;
    }

    // returns the possible next or previous states and the associated costs for a given state.
    // in forward mode we scan the possible states from the start state towards the goal.
    // in backward mode we are working backwards from the goal to the start.
    IEnumerable<(State, int cost)> Steps(Map map, State state, bool forward) {
        foreach (var dir in Dirs) {
            if (dir == state.dir) {
                var pos = forward ? state.pos + dir : state.pos - dir;
                if (map.GetValueOrDefault(pos) != '#') {
                    yield return ((pos, dir), 1);
                }
            } else if (dir != -state.dir) {
                yield return ((state.pos, dir), 1000);
            }
        }
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
    Complex Goal(Map map) => map.Keys.Single(k => map[k] == 'E');
    State Start(Map map) => (map.Keys.Single(k => map[k] == 'S'), East);
}