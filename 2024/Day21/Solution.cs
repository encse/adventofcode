namespace AdventOfCode.Y2024.Day21;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AngleSharp.Common;
using Cache = System.Collections.Concurrent.ConcurrentDictionary<(char, System.Numerics.Complex, int),  long>;
using Keypad = System.Collections.Generic.Dictionary<char, System.Numerics.Complex>;


[ProblemName("Keypad Conundrum")]
class Solution : Solver {

/*
                _________ _______ 
        |\     /|\__   __/(  ____ )
        | )   ( |   ) (   | (    )|
        | | _ | |   | |   | (____)|
        | |( )| |   | |   |  _____)
        | || || |   | |   | (      
        | () () |___) (___| )      
        (_______)\_______/|/       
                           
*/                           

    
    public object PartOne(string input) {
        return input.Split("\n").Sum(line => Solve2(line, 2).Item1);
    }
    public object PartTwo(string input) {
        return input.Split("\n").Sum(line => Solve2(line, 25).Item1);
    }

    static readonly Complex Left = -1;
    static readonly Complex Right = 1;
    static readonly Complex Up = Complex.ImaginaryOne;
    static readonly Complex Down = -Complex.ImaginaryOne;

    (long, string) Solve2(string line, int depth) {
        var keypad1 = ParseKeypad("789\n456\n123\n 0A");
        var keypad2 = ParseKeypad(" ^A\n<v>");

        var keypads = new List<Keypad>();
        for(var i =0;i<depth;i++) {
            keypads.Add(keypad2);
        }
        
        Cache cache = new Cache();
        var res = long.MaxValue;
        var st = "";
        foreach (var plan in Encode(line, keypad1, keypad1['A'])) {
            var length = EncodeString(plan, keypads.ToArray(), cache);
            if (length < res) {
                res = Math.Min(res, length);
            }
        }
        return (res * int.Parse(line.Substring(0, line.Length - 1)), st);
    }

    long EncodeString(string st, Span<Keypad> keypads, Cache cache) {
        if (keypads.Length == 0) {
            return st.Length;
        } else {
            var length = 0L;
            var pos = keypads[0]['A']; 
            foreach (var step in st) {
                long cost;
                cost = EncodeKey(step, pos, keypads, cache);
                length += cost;
                pos = keypads[0][step];
            }
            
            return length;
        }
    }
    long EncodeKey(char ch, Complex pos, Span<Keypad> keypads,  Cache cache) {
        var key = (ch, pos, keypads.Length);
        if (cache.ContainsKey(key)) {
            return cache[key];
        }

        var target = keypads[0][ch];

        var dy = (int)(target.Imaginary - pos.Imaginary);
        var dx = (int)(target.Real - pos.Real);

        var resCost = long.MaxValue;
        var resTop = Complex.Infinity;

        if (pos + dy * Up != keypads[0][' ']) {
            string toEncode = "";
            if (dy < 0) {
                toEncode += new string('v', Math.Abs(dy));
            } else if (dy > 0) {
                toEncode += new string('^', Math.Abs(dy));
            }
            if (dx < 0) {
                toEncode += new string('<', Math.Abs(dx));
            } else if (dx > 0) {
                toEncode += new string('>', Math.Abs(dx));
            }
            toEncode += "A";
            var cost = EncodeString(toEncode, keypads[1..], cache);

            if (cost < resCost) {
                resCost = cost;
            }
        }
 
        if (pos + dx * Right != keypads[0][' ']) {
            string toEncode = "";
            if (dx < 0) {
                toEncode += new string('<', Math.Abs(dx));
            } else if (dx > 0) {
                toEncode += new string('>', Math.Abs(dx));
            }

            if (dy < 0) {
                toEncode += new string('v', Math.Abs(dy));
            } else if (dy > 0) {
                toEncode += new string('^', Math.Abs(dy));
            }
            toEncode += "A";

            var cost = EncodeString(toEncode, keypads[1..], cache);

            if (cost < resCost) {
                resCost = cost;
            }
        }

        cache[key] = resCost;
        return cache[key];
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