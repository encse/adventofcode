namespace AdventOfCode.Y2024.Day16;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
using State = (System.Numerics.Complex pos, System.Numerics.Complex dir);

[ProblemName("Reindeer Maze")]
class Solution : Solver {

    static readonly Complex North = -Complex.ImaginaryOne;
    static readonly Complex South = Complex.ImaginaryOne;
    static readonly Complex West = -1;
    static readonly Complex East = 1;
    static readonly Complex[] Dirs = { North, East, West, South };

    public object PartOne(string input) => FindDistance(GetMap(input));
    public object PartTwo(string input) => FindBestSpots(GetMap(input));

    int FindDistance(Map map) {
        var dist = DistancesTo(map, Goal(map));
        return dist[Start(map)];
    }

    // determines the number tiles that are on one of the shortest paths in the race.
    int FindBestSpots(Map map) {
        var dist = DistancesTo(map, Goal(map));
        var start = Start(map);

        // flood fill algorithm determines the best spots by following the shortest paths 
        // using the distance map as guideline.

        var q = new PriorityQueue<State, int>();
        q.Enqueue(start, dist[start]);
        var bestSpots = new HashSet<State> { start };

        while (q.TryDequeue(out var state, out var remainingScore)) {
            foreach (var (next, score) in Steps(map, state, forward: true)) {
                if (bestSpots.Contains(next)) {
                    continue;
                }
                var nextScore = remainingScore - score;
                if (dist[next] == nextScore) {
                    bestSpots.Add(next);
                    q.Enqueue(next, nextScore);
                }
            }
        }
        return bestSpots.DistinctBy(state => state.pos).Count();
    }

    Dictionary<State, int> DistancesTo(Map map, Complex goal) {
        var res = new Dictionary<State, int>();

        // a flood fill algorithm, works backwards from the goal, and 
        // computes the distances between any location in the map and the goal
        var q = new PriorityQueue<State, int>();
        foreach (var dir in Dirs) {
            q.Enqueue((goal, dir), 0);
            res[(goal, dir)] = 0;
        }

        while (q.TryDequeue(out var state, out var totalScore)) {
            if (totalScore != res[state]) {
                continue;
            }
            foreach (var (next, score) in Steps(map, state, forward: false)) {
                var nextCost = totalScore + score;
                if (res.ContainsKey(next) && res[next] < nextCost) {
                    continue;
                }

                res[next] = nextCost;
                q.Enqueue(next, nextCost);
            }
        }
        return res;
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