using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2016.Day21 {

    class Solution : Solver {

        public string GetName() => "Scrambled Letters and Hash";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        string PartOne(string input) => string.Join("", Parse(input)("abcdefgh"));

        string PartTwo(string input) {
            var scramble = Parse(input);
            return string.Join("", Permutations("abcdefgh").First(p => scramble(p).SequenceEqual("fbgdceah")));
        }

        IEnumerable<ImmutableList<char>> Permutations(string st) {

            IEnumerable<ImmutableList<char>> PermutationsRec(ImmutableList<char> prefix, bool[] fseen) {
                if (prefix.Count == st.Length) {
                    yield return prefix;
                } else {
                    for (int i = 0; i < st.Length; i++) {
                        if (!fseen[i]) {
                            fseen[i] = true;
                            var prefixT = prefix.Add(st[i]);
                            foreach (var res in PermutationsRec(prefixT, fseen)) {
                                yield return res;
                            }
                            fseen[i] = false;
                        }
                    }
                }
            }

            return PermutationsRec(ImmutableList<char>.Empty, new bool[st.Length]);
        }

        Func<IEnumerable<char>, IEnumerable<char>> Parse(string input) {
            var steps = (
                from line in input.Split('\n')
                select
                    Match(line, @"swap position (\d+) with position (\d+)", m => {
                        var x = int.Parse(m[0]);
                        var y = int.Parse(m[1]);
                        return chars => SwapPosition(chars, x, y);
                    }) ??
                    Match(line, @"swap letter (\w) with letter (\w)", m => {
                        var chX = m[0][0];
                        var chY = m[1][0];
                        return (chars) => SwapLetter(chars, chX, chY);
                    }) ??
                    Match(line, @"rotate left (\d+) step", m => {
                        var x = int.Parse(m[0]);
                        return chars => RotateLeft(chars, x);
                    }) ??
                    Match(line, @"rotate right (\d+) step", m => {
                        var x = int.Parse(m[0]);
                        return chars => RotateRight(chars, x);
                    }) ??
                    Match(line, @"rotate based on position of letter (\w)", m => {
                        var chX = m[0][0];
                        return chars => RotateBasedOnPosition(chars, chX);
                    }) ??
                    Match(line, @"reverse positions (\d+) through (\d+)", m => {
                        var x = int.Parse(m[0]);
                        var y = int.Parse(m[1]);
                        return chars => Reverse(chars, x, y);
                    }) ??
                    Match(line, @"move position (\d+) to position (\d+)", m => {
                        var x = int.Parse(m[0]);
                        var y = int.Parse(m[1]);
                        return chars => MovePosition(chars, x, y);
                    }) ??
                    throw new Exception("Cannot parse " + line)
                ).ToArray();

            return chars => {
                var charsArray = chars.ToArray();
                foreach (var step in steps) {
                    step(charsArray);
                }
                return charsArray;
            };
        }

        Action<char[]> Match(string stm, string pattern, Func<string[], Action<char[]>> a) {
            var match = Regex.Match(stm, pattern);
            if (match.Success) {
                return a(match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray());
            } else {
                return null;
            }
        }

        void SwapPosition(char[] chars, int x, int y) {
            (chars[x], chars[y]) = (chars[y], chars[x]);
        }

        void SwapLetter(char[] chars, char chX, char chY) {
            for (var i = 0; i < chars.Length; i++) {
                chars[i] = chars[i] == chX ? chY : chars[i] == chY ? chX : chars[i];
            }
        }

        void RotateBasedOnPosition(char[] chars, char chX) {
            var i = Array.IndexOf(chars, chX);
            RotateRight(chars, i >= 4 ? i + 2 : i + 1);
        }

        void RotateLeft(char[] chars, int t) {
            t %= chars.Length;
            Reverse(chars, 0, t - 1);
            Reverse(chars, t, chars.Length - 1);
            Reverse(chars, 0, chars.Length - 1);
        }

        void RotateRight(char[] chars, int t) {
            t %= chars.Length;
            Reverse(chars, 0, chars.Length - 1);
            Reverse(chars, 0, t - 1);
            Reverse(chars, t, chars.Length - 1);
        }

        void Reverse(char[] chars, int x, int y) {
            while (x < y) {
                (chars[x], chars[y]) = (chars[y], chars[x]);
                x++;
                y--;
            }
        }

        void MovePosition(char[] chars, int x, int y) {
            var d = x < y ? 1 : -1;

            var ch = chars[x];
            for (int i = x + d; i != y + d; i += d) {
                chars[i - d] = chars[i];
            }
            chars[y] = ch;
        }
    }
}