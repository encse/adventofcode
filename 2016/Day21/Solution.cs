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
            
            IEnumerable<ImmutableList<char>> PermRecursive(ImmutableList<char> prefix, bool[] fseen) {
                if (prefix.Count == st.Length) {
                    yield return prefix;
                } else {
                    for (int i = 0; i < st.Length; i++) {
                        if (!fseen[i]) {
                            fseen[i] = true;
                            var prefixT = prefix.Add(st[i]);
                            foreach (var res in PermRecursive(prefixT, fseen)) {
                                yield return res;
                            }
                            fseen[i] = false;
                        }
                    }
                }
            }

            return PermRecursive(ImmutableList<char>.Empty, new bool[st.Length]);
        }

        Func<IEnumerable<char>, IEnumerable<char>> Parse(string input) {
            var steps = (
                from line in input.Split('\n')
                select
                    Match(line, @"swap position (\d+) with position (\d+)", m => {
                        var x = int.Parse(m[0]);
                        var y = int.Parse(m[1]);
                        return (chars) => {
                            (chars[x], chars[y]) = (chars[y], chars[x]);
                        };
                    }) ??
                    Match(line, @"swap letter (\w) with letter (\w)", m => {
                        var chX = m[0][0];
                        var chY = m[1][0];
                        return (chars) => {
                            for (var i = 0; i < chars.Length; i++) {
                                chars[i] = chars[i] == chX ? chY : chars[i] == chY ? chX : chars[i];
                            }
                        };
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
                        return chars => {
                            var i = Array.IndexOf(chars, chX);
                            RotateRight(chars, i >= 4 ? i + 2 : i + 1);
                        };
                    }) ??
                    Match(line, @"reverse positions (\d+) through (\d+)", m => {
                        var x = int.Parse(m[0]);
                        var y = int.Parse(m[1]);
                        return chars => Reverse(chars, x, y);
                    }) ??
                    Match(line, @"move position (\d+) to position (\d+)", m => {
                        var x = int.Parse(m[0]);
                        var y = int.Parse(m[1]);
                        var d = x < y ? 1 : -1;
                        return chars => {
                            var ch = chars[x];
                            for (int i = x + d; i != y + d; i += d) {
                                chars[i - d] = chars[i];
                            }
                            chars[y] = ch;
                        };
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

        char[] RotateLeft(char[] chars, int t) {
            t = t % chars.Length;
            Reverse(chars, 0, t - 1);
            Reverse(chars, t, chars.Length - 1);
            Reverse(chars, 0, chars.Length - 1);
            return chars;
        }

        char[] RotateRight(char[] chars, int t) {
            t = t % chars.Length;
            Reverse(chars, 0, chars.Length - 1);
            Reverse(chars, 0, t - 1);
            Reverse(chars, t, chars.Length - 1);
            return chars;
        }

        char[] Reverse(char[] chars, int x, int y) {
            var (i, j) = (x, y);
            while (i < j) {
                (chars[i], chars[j]) = (chars[j], chars[i]);
                i++;
                j--;
            }
            return chars;
        }
    }
}