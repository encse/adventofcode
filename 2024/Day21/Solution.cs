namespace AdventOfCode.Y2024.Day21;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;
using System.Security.AccessControl;
using AngleSharp.Common;
using AdventOfCode.Y2021.Day11;
using System.Security.Cryptography;

[ProblemName("Keypad Conundrum")]
class Solution : Solver {

    public object PartOne(string input) {
        return input.Split("\n").Sum(Solve);
    }

    public object PartTwo(string input) {
        return 0;
    }

    static readonly Complex Left = -1;
    static readonly Complex Right = 1;
    static readonly Complex Up = Complex.ImaginaryOne;
    static readonly Complex Down = -Complex.ImaginaryOne;

    int Solve(string line) {
        var keypad1 = ParseKeypad("789\n456\n123\n 0A");
        var keypad2 = ParseKeypad(" ^A\n<v>");

        Console.WriteLine("========");
        // var q = "<v<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A";
        // Console.WriteLine(q);
        // q = Decode(q, keypad2);
        // Console.WriteLine(q);
        // q = Decode(q, keypad2);
        // Console.WriteLine(q);
        // q = Decode(q, keypad1);
        // Console.WriteLine(q);

        // Console.WriteLine();

        var xs = Encode(line, keypad1, keypad1['A']).ToList();
        var ys = xs.SelectMany(x => Encode(x, keypad2, keypad2['A'])).ToList();
        foreach (var y in ys) {
            Console.WriteLine(Decode(Decode(y, keypad2), keypad1));
        }

        var cache = new Dictionary<(Complex, string), string> ();

        var z = ys.Select(y => Mikkamakka(keypad2['A'], y, keypad2, cache)).MinBy(z => z.Length);

        Console.WriteLine(Decode(Decode(Decode(z, keypad2), keypad2), keypad1));
        return z.Length * int.Parse(line[0..^1]);
    }

    

    string Mikkamakka(Complex pos, string st, Dictionary<char, Complex> keypad, Dictionary<(Complex, string), string> cache) {
        if (st == "") {
            return "";
        }
        var key = (pos, st);
        if (!cache.ContainsKey(key)) {

            var target = keypad[st[0]];

            var dy = (int)(target.Imaginary - pos.Imaginary);
            var dx = (int)(target.Real - pos.Real);

            var res1 = "";
            if (pos + dy * Up != keypad[' ']) {
                if (dy < 0) {
                    res1 += new string('v', Math.Abs(dy));
                } else if (dy > 0) {
                    res1 += new string('^', Math.Abs(dy));
                }
                if (dx < 0) {
                    res1 += new string('<', Math.Abs(dx));
                } else if (dx > 0) {
                    res1 += new string('>', Math.Abs(dx));
                }
                res1 += "A";
                res1 += Mikkamakka(target, st[1..], keypad, cache);
            }
            var res2 = "";
            if (pos + dx * Right != keypad[' ']) {
                if (dx < 0) {
                    res2 += new string('<', Math.Abs(dx));
                } else if (dx > 0) {
                    res2 += new string('>', Math.Abs(dx));
                }

                if (dy < 0) {
                    res2 += new string('v', Math.Abs(dy));
                } else if (dy > 0) {
                    res2 += new string('^', Math.Abs(dy));
                }

                res2 += "A";
                res2 += Mikkamakka(target, st[1..], keypad, cache);
            }

            cache[key] = res1 == "" ? res2 : res2 == "" ? res1 : res1.Length < res2.Length ? res1 : res2;
        }
        return cache[key];
    }


    string Decode(string st, Dictionary<char, Complex> keymap) {
        var res = "";
        var pos = keymap['A'];
        foreach (var ch in st) {
            if (ch == '^') {
                pos += Up;
            } else if (ch == 'v') {
                pos += Down;
            } else if (ch == '<') {
                pos += Left;
            } else if (ch == '>') {
                pos += Right;
            } else if (ch == 'A') {
                res += keymap.Single(kvp => kvp.Value == pos).Key;
            }
        }
        return res;
    }

  
    IEnumerable<string> Encode(string st, Dictionary<char, Complex> keymap, Complex pos) {
        if (st == "") {
            yield return "";
            yield break;
        }


        var target = keymap[st[0]];

        var dy = (int)(target.Imaginary - pos.Imaginary);
        var dx = (int)(target.Real - pos.Real);

        if (pos + dy * Up != keymap[' ']) {
            var res = "";
            if (dy < 0) {
                res += new string('v', Math.Abs(dy));
            } else if (dy > 0) {
                res += new string('^', Math.Abs(dy));
            }
            if (dx < 0) {
                res += new string('<', Math.Abs(dx));
            } else if (dx > 0) {
                res += new string('>', Math.Abs(dx));
            }
            res += "A";
            foreach (var resT in Encode(st[1..], keymap, target)) {
                yield return res + resT;
            }
        }

        if (pos + dx * Right != keymap[' ']) {
            var res = "";
            if (dx < 0) {
                res += new string('<', Math.Abs(dx));
            } else if (dx > 0) {
                res += new string('>', Math.Abs(dx));
            }

            if (dy < 0) {
                res += new string('v', Math.Abs(dy));
            } else if (dy > 0) {
                res += new string('^', Math.Abs(dy));
            }

            res += "A";
            foreach (var resT in Encode(st[1..], keymap, target)) {
                yield return res + resT;
            }
        }

    }

    Dictionary<char, Complex> ParseKeypad(string keypad) {
        var lines = keypad.Split("\n");
        return (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<char, Complex>(lines[y][x], x + y * Down)
        ).ToDictionary();
    }
}