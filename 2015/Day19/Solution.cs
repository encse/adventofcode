using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day19 {

    class Solution : Solver {

        public string GetName() => "Medicine for Rudolph";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var (rules, m) = Parse(input);
            return ReplaceAll(rules, m).ToHashSet().Count;
        }

        int PartTwo(string input) {
            var (rules, mInit) = Parse(input);
            var lookahead = rules.Select(r => r.from.Length).Max();

            var q = new HashSet<string>();
            var qNext = new HashSet<string>();

            q.Add(mInit);
            
            var depth = 0;
            while (!q.Contains("e")) {
                var lim = -1;
                foreach (var m in q) {
                    foreach (var replacement in Replacements(rules, m, false)) {
                        if (lim == -1) {
                            lim = replacement.from + lookahead;
                        }
                        if (replacement.from > lim) {
                            break;
                        }
                        qNext.Add(Replace(m, replacement.from, replacement.to, replacement.length));
                    }
                }
                (q, qNext) = (qNext, q);
                qNext.Clear();
                depth++;
            }
            return depth;
        }

        IEnumerable<string> ReplaceAll((string from, string to)[] rules, string m) {
            foreach (var (from, length, to) in Replacements(rules, m, true)) {
                yield return Replace(m, from, to, length);
            }
        }

        string Replace(string m, int from, string to, int length) => m.Substring(0, from) + to + m.Substring(from + length);

        IEnumerable<(int from, int length, string to)> Replacements((string from, string to)[] rules, string m, bool forward) {
            var ich = 0;
            while (ich < m.Length) {
                foreach (var (a, b) in rules) {
                    var (from, to) = forward ? (a, b) : (b, a);
                    if (ich + from.Length <= m.Length) {
                        var i = 0;
                        while (i < from.Length) {
                            if (m[ich + i] != from[i]) {
                                break;
                            }
                            i++;
                        }
                        if (i == from.Length) {
                            yield return (ich, from.Length, to);
                        }
                    }
                }
                ich++;
            }
        }

        ((string from,string to)[] rules, string m) Parse(string input){
            var rules =
                (from line in input.Split('\n').TakeWhile(line => line.Contains("=>"))
                 let parts = line.Split(" => ")
                 select (parts[0], parts[1]))
                .ToArray();
            var m = input.Split('\n').Last();
            return (rules, m);
        }
    }
}