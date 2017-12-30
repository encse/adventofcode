using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2016.Day10 {

    class Solution : Solver {

        public string GetName() => "Balance Bots";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        string PartOne(string input) =>
            Execute(Parse(input)).Single(v => v.min == 17 && v.max == 61).id.Split(' ')[1];

        int PartTwo(string input) {
            var m = Execute(Parse(input)).Last().machine;
            return m["output 0"].values.Single() * m["output 1"].values.Single() * m["output 2"].values.Single();
        }

        IEnumerable<(Dictionary<string, Node> machine, string id, int min, int max)> Execute(Dictionary<string, Node> machine) {
            var any = true;
            while (any) {
                any = false;
                foreach (var node in machine.Values) {
                    if (node.values.Count == 2 && node.outHigh != null) {
                        any = true;
                        var (min, max) = (node.values.Min(), node.values.Max());
                        machine[node.outLow].values.Add(min);
                        machine[node.outHigh].values.Add(max);
                        node.values.Clear();
                        yield return (machine, node.id, min, max);
                    }
                }
            }
        }

        Dictionary<string, Node> Parse(string input) {
            var res = new Dictionary<string, Node>();
            void ensureNodes(params string[] ids) {
                foreach (var id in ids) {
                    if (!res.ContainsKey(id)) {
                        res[id] = new Node { id = id };
                    }
                }
            }
            foreach (var line in input.Split('\n')) {
                if (Match(line, @"(.+) gives low to (.+) and high to (.+)", out var m)) {
                    ensureNodes(m);
                    res[m[0]].outLow = m[1];
                    res[m[0]].outHigh = m[2];
                } else if (Match(line, @"value (\d+) goes to (.+)", out m)) {
                    ensureNodes(m[1]);
                    res[m[1]].values.Add(int.Parse(m[0]));
                } else {
                    throw new NotImplementedException();
                }
            }
            return res;
        }

        bool Match(string stm, string pattern, out string[] m) {
            var match = Regex.Match(stm, pattern);
            m = null;
            if (match.Success) {
                m = match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray();
                return true;
            } else {
                return false;
            }
        }
    }

    class Node {
        public string id;
        public List<int> values = new List<int>();
        public string outLow;
        public string outHigh;
    }

}