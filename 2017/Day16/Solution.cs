using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2017.Day16 {

    [ProblemName("Permutation Promenade")]
    class Solution : Solver {

        public object PartOne(string input) {
            return ParseStep(input)("abcdefghijklmnop");
        }

        public object PartTwo(string input) {
            var step = ParseStep(input);
            var startState = "abcdefghijklmnop";

            var mod = Mod(step, startState);

            var state = startState;
            for (int i = 0; i < 1000000000 % mod; i++) {
                state = step(state);
            }
            return state;
        }

        int Mod(Func<string, string> step, string startState) {
            var state = startState;
            for (int i = 0; ; i++) {
                state = step(state);
                if (startState == state) {
                    return i + 1;
                }
            }
        }

        Func<string, string> ParseStep(string input) {
            var moves = (
                from stm in input.Split(',')
                select
                    ParseMove(stm, "s([0-9]+)", m => {
                        int n = int.Parse(m[0]);
                        return (order) => {
                            return order.Skip(order.Count - n).Concat(order.Take(order.Count - n)).ToList();
                        };
                    }) ??
                    ParseMove(stm, "x([0-9]+)/([0-9]+)", m => {
                        int idx1 = int.Parse(m[0]);
                        int idx2 = int.Parse(m[1]);
                        return (order) => {
                            (order[idx1], order[idx2]) = (order[idx2], order[idx1]);
                            return order;
                        };
                    }) ??
                    ParseMove(stm, "p([a-z])/([a-z])", m => {
                        var (c1, c2) = (m[0].Single(), m[1].Single());
                        return order => {
                            var (idx1, idx2) = (order.IndexOf(c1), order.IndexOf(c2));
                            order[idx1] = c2;
                            order[idx2] = c1;
                            return order;
                        };
                    }) ??
                    throw new Exception("Cannot parse " + stm)
            ).ToArray();

            return startOrder => {
                var order = startOrder.ToList();
                foreach (var move in moves) {
                    order = move(order);
                }
                return string.Join("", order);
            };
        }

        Func<List<char>, List<char>> ParseMove(string stm, string pattern, Func<string[], Func<List<char>, List<char>>> a) {
            var match = Regex.Match(stm , pattern);
            if (match.Success) {
                return a(match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray());
            } else {
                return null;
            }
        }
    }
}
