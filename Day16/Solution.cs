using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;


namespace AdventOfCode2017.Day16 {

    class Solution : Solver {

        const string startState = "abcdefghijklmnop";

        public string GetName() => "???";

        public void Solve(string input) {
            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
        }

        string PartOne(string input) {
            return Step(input, startState);
        }

        string PartTwo(string input) {
            var mod = Mod(input);

            var state = startState;
            for (int i = 0; i < 1000000000 % mod; i++) {
                state = Step(input, state);
            }
            return state;
        }

        string Step(string input, string orderT) {
            var order = orderT.ToList();
            var spin = Move("s([0-9]+)", m => {
                    int n = int.Parse(m[0]);
                    order = order.Skip(order.Count - n).Concat(order.Take(order.Count - n)).ToList();
                });

            var exchange = Move("x([0-9]+)/([0-9]+)", m => {
                    int idx1 = int.Parse(m[0]);
                    int idx2 = int.Parse(m[1]);
                    (order[idx1], order[idx2]) = (order[idx2], order[idx1]);
                });

            var partner = Move("p([a-z])/([a-z])", m => {
                    var (c1, c2) = (m[0].Single(), m[1].Single());
                    var (idx1, idx2) = (order.IndexOf(c1), order.IndexOf(c2));
                    order[idx1] = c2;
                    order[idx2] = c1;
                });

            foreach (var stm in input.Split(',')) {
                spin(stm);
                exchange(stm);
                partner(stm);
            }
            
            return string.Join("", order);
        }

        int Mod(string input) {
            var seen = new HashSet<string>();
            var state = startState;
            seen.Add(state);
            for (int i = 0; i < 1000000000; i++) {
                state = Step(input, state);
                if (seen.Contains(state)) {
                    return i + 1;
                }
            }
            return 1000000000;
        }
        Func<string, bool> Move(string pattern, Action<string[]> a) {
            var rx = new Regex(pattern);
            return (string stm) => {
                var match = rx.Match(stm);
                if (match.Success) {
                    a(match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray());
                    return true;
                } else {
                    return false;
                }
            };
        }
    }
}
