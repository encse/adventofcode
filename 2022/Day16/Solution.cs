using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day16;

[ProblemName("Proboscidea Volcanium")]
class Solution : Solver {

    record Map(int[,] distances, Valve[] valves);
    record Valve(int id, string name, int flowRate, string[] tunnels);

    public object PartOne(string input) {
        return Solve(input, true, 30);
    }
    public object PartTwo(string input) {
        return Solve(input, false, 26);
    }

    int Solve(string input, bool humanOnly, int time) {
        var map = Parse(input);
        var start = map.valves.Single(x => x.name == "AA");
        var valvesToOpen = map.valves.Where(valve => valve.flowRate > 0).ToArray();

        var cache = new Dictionary<string, int>();
        if (humanOnly) {
            return MaxFlow(cache, map, start, valvesToOpen.ToHashSet(), time);
        } else {
            return Pairings(valvesToOpen).Select(pairing =>
                 MaxFlow(cache, map, start, pairing.human, time) +
                 MaxFlow(cache, map, start, pairing.elephant, time)
            ).Max();
        }
    }

    // Divide the valves between human and elephant in all possible ways
    IEnumerable<(HashSet<Valve> human, HashSet<Valve> elephant)> Pairings(Valve[] valves) {
        var maxMask = 1 << (valves.Length - 1);

        for (var mask = 0; mask < maxMask; mask++) {
            var elephant = new HashSet<Valve>();
            var human = new HashSet<Valve>();

            elephant.Add(valves[0]);

            for (var ivalve = 1; ivalve < valves.Length; ivalve++) {
                if ((mask & (1 << ivalve)) == 0) {
                    human.Add(valves[ivalve]);
                } else {
                    elephant.Add(valves[ivalve]);
                }
            }
            yield return (human, elephant);
        }
    }

    int MaxFlow(
        Dictionary<string, int> cache,
        Map map,
        Valve currentValve,
        HashSet<Valve> valves,
        int remainingTime
    ) {
        string key =
            remainingTime + "-" +
            currentValve.id + "-" +
            string.Join("-", valves.OrderBy(x => x.id).Select(x => x.id));

        if (!cache.ContainsKey(key)) {
            // current valve gives us this much flow:
            var flowFromValve = currentValve.flowRate * remainingTime;

            // determine best use of the remaining time:
            var flowFromRest = 0;
            foreach (var valve in valves.ToArray()) {
                var distance = map.distances[currentValve.id, valve.id];

                if (remainingTime >= distance + 1) {
                    valves.Remove(valve);
                    remainingTime -= distance + 1;

                    flowFromRest = Math.Max(
                        flowFromRest, MaxFlow(cache, map, valve, valves, remainingTime));

                    remainingTime += distance + 1;
                    valves.Add(valve);
                }

            }
            cache[key] = flowFromValve + flowFromRest;
        }
        return cache[key];
    }

    Map Parse(string input) {
        // Valve BB has flow rate=0; tunnels lead to valve CC
        // Valve CC has flow rate=10; tunnels lead to valves DD, EE
        var valveList = new List<Valve>();
        foreach (var line in input.Split("\n")) {
            var name = Regex.Match(line, "Valve (.*) has").Groups[1].Value;
            var flow = int.Parse(Regex.Match(line, @"\d+").Groups[0].Value);
            var tunnels = Regex.Match(line, "to valves? (.*)").Groups[1].Value.Split(", ");
            valveList.Add(new Valve(0, name, flow, tunnels));
        }
        var valves = valveList
            .OrderByDescending(valve => valve.flowRate)
            .Select((v, i) => v with { id = i })
            .ToArray();

        return new Map(ComputeDistances(valves), valves);
    }

    int[,] ComputeDistances(Valve[] valves) {
        // Bellman-Ford style distance calculation for every pair of valves
        var distances = new int[valves.Length, valves.Length];
        for (var i = 0; i < valves.Length; i++) {
            for (var j = 0; j < valves.Length; j++) {
                distances[i, j] = int.MaxValue;
            }
        }
        foreach (var valve in valves) {
            foreach (var target in valve.tunnels) {
                var targetNode = valves.Single(x => x.name == target);
                distances[valve.id, targetNode.id] = 1;
                distances[targetNode.id, valve.id] = 1;
            }
        }

        var n = distances.GetLength(0);
        var done = false;
        while (!done) {
            done = true;
            for (var source = 0; source < n; source++) {
                for (var target = 0; target < n; target++) {
                    if (source != target) {
                        for (var through = 0; through < n; through++) {
                            if (distances[source, through] == int.MaxValue || 
                                distances[through, target] == int.MaxValue
                            ) {
                                continue;
                            }
                            var cost = distances[source, through] + distances[through, target];
                            if (cost < distances[source, target]) {
                                done = false;
                                distances[source, target] = cost;
                                distances[target, source] = cost;
                            }
                        }
                    }
                }
            }
        }
        return distances;
    }
}
