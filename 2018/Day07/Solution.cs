using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Y2018.Day07;

[ProblemName("The Sum of Its Parts")]
class Solution : Solver {

    public object PartOne(string input) {

        var sb = new StringBuilder();
        var graph = Parse(input);
        while (graph.Any()) {
            char minKey = char.MaxValue;
            foreach (var key in graph.Keys) {
                if (graph[key].Count == 0) {
                    if (key < minKey) {
                        minKey = key;
                    }
                }
            }
            sb.Append(minKey);
            graph.Remove(minKey);
            foreach (var key in graph.Keys) {
                graph[key].Remove(minKey);
            }
        }
        return sb.ToString();
    }

    public object PartTwo(string input) {
        var time = 0;
        var graph = Parse(input);

        var works = new int[5];
        var items = new char[works.Length];

        while (graph.Any() || works.Any(work => work > 0)) {
            for (var i = 0; i < works.Length && graph.Any(); i++) {
                // start working
                if (works[i] == 0) {
                    char minKey = char.MaxValue;
                    foreach (var key in graph.Keys) {
                        if (graph[key].Count == 0) {
                            if (key < minKey) {
                                minKey = key;
                            }
                        }
                    }
                    if (minKey != char.MaxValue) {
                        works[i] = 60 + minKey - 'A' + 1;
                        items[i] = minKey;
                        graph.Remove(items[i]);
                    }
                }
            }

            time++;

            for (var i = 0; i < works.Length; i++) {
                if (works[i] == 0) {
                    // wait
                    continue;
                } else if (works[i] == 1) {
                    // finish
                    works[i]--;
                    foreach (var key in graph.Keys) {
                        graph[key].Remove(items[i]);
                    }

                } else if (works[i] > 0) {
                    // step
                    works[i]--;
                }
            }
        }

        return time;
    }

    Dictionary<char, List<char>> Parse(string input) {
        var dict = (
            from line in input.Split("\n")
            let parts = line.Split(" ")
            let part = parts[7][0]
            let partDependsOn = parts[1][0]
            group partDependsOn by part into g
            select g
        ).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var key in new List<char>(dict.Keys)) {
            foreach (var d in dict[key]) {
                if (!dict.ContainsKey(d)) {
                    dict.Add(d, new List<char>());
                }
            }
        }
        return dict;
    }
}
