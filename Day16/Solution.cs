using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;


namespace AdventOfCode2017.Day16 {

    class Solution : Solver {

        public string GetName() => "???";

        public void Solve(string input) {
            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
        }

        string PartOne(string input) {
            return Step(input, "abcdefghijklmnop");
        }

        string Step(string input, string orderT) {
            var order = orderT.ToList();
            var rxSpin = Parser(new Regex("s([0-9]+)"), m => int.Parse(m.Groups[1].Value));
            var rxExchange = Parser(new Regex("x([0-9]+)/([0-9]+)"), m => (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)));
            var rxPartner = Parser(new Regex("p([a-z])/([a-z])"), m => (m.Groups[1].Value[0], m.Groups[2].Value[0]));

            foreach (var stm in input.Split(',')) {
                if (rxSpin(stm) is var n && n.HasValue) {
                    order = order.Skip(order.Count - n.Value).Concat(order.Take(order.Count - n.Value)).ToList();
                } else if (rxExchange(stm) is var p && p.HasValue) {
                    (order[p.Value.Item1], order[p.Value.Item2]) = (order[p.Value.Item2], order[p.Value.Item1]);
                } else if (rxPartner(stm) is var e && e.HasValue) {
                    var (idx1, idx2) = (order.IndexOf(e.Value.Item1), order.IndexOf(e.Value.Item2));
                    order[idx1] = e.Value.Item2;
                    order[idx2] = e.Value.Item1;
                }
            }
            return string.Join("", order);
        }

        string PartTwo(string input) {
            var mod = Mod(input);

            var state = "abcdefghijklmnop";
            for (int i = 0; i < 1000000000 % mod; i++) {
                state = Step(input, state);
            }
            return state;
        }

        int Mod(string input) {
            var seen = new HashSet<string>();
            var state = "abcdefghijklmnop";
            seen.Add(state);
            for (int i = 0; i < 1000000000; i++) {
                state = Step(input, state);
                if (seen.Contains(state)) {
                    return i + 1;
                }
            }
            return 1000000000;
        }

        Func<string, T?> Parser<T>(Regex r, Func<Match, T> p) where T : struct {
            return (string stm) => {
                var match = r.Match(stm);
                if (match.Success) {
                    return new Nullable<T>(p(match));
                } else {
                    return null;
                }
            };
        }
    }
}
