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

        string PartOne(string input) => Parse(input)("abcdefghijklmnop");

        string PartTwo(string input) {
            var startState = "abcdefghijklmnop";
            var step = Parse(input);
            var mod = Mod(step, startState);

            var state = startState;
            for (int i = 0; i < 1000000000 % mod; i++) {
                state = step(state);
            }
            return state;
        }

        int Mod(Func<string, string> step, string startState) {
            var seen = new HashSet<string>();
            var state = startState;
            seen.Add(state);
            for (int i = 0; i < 1000000000; i++) {
                state = step(state);
                if (seen.Contains(state)) {
                    return i + 1;
                }
            }
            return 1000000000;
        }

        Func<string, string> Parse(string input) {
            var moves =
                from stm in input.Split(',')
                select
                    Move(stm, "s([0-9]+)", m => {
                        int n = int.Parse(m[0]);
                        return (order) => {
                            return order.Skip(order.Count - n).Concat(order.Take(order.Count - n)).ToList();
                        };
                    }) ??
                    Move(stm, "x([0-9]+)/([0-9]+)", m => {
                        int idx1 = int.Parse(m[0]);
                        int idx2 = int.Parse(m[1]);
                        return (order) => {
                            (order[idx1], order[idx2]) = (order[idx2], order[idx1]);
                            return order;
                        };
                    }) ??
                    Move(stm, "p([a-z])/([a-z])", m => {
                        var (c1, c2) = (m[0].Single(), m[1].Single());
                        return order => {
                            var (idx1, idx2) = (order.IndexOf(c1), order.IndexOf(c2));
                            order[idx1] = c2;
                            order[idx2] = c1;
                            return order;
                        };
                    }) ??
                    throw new Exception("Cannot parse " + stm);

            return startOrder => {
                var order = startOrder.ToList();
                foreach (var move in moves) {
                    order = move(order);
                }
                return string.Join("", order);
            };
        }

         Func<List<char>, List<char>> Move(string stm, string pattern, Func<string[], Func<List<char>, List<char>>> a) {
            var match = Regex.Match(stm , pattern);
            if (match.Success) {
                return a(match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray());
            } else {
                return null;
            }
        }
    }
}
