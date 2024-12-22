namespace AdventOfCode.Y2024.Day21;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using AngleSharp.Common;
using Cache = System.Collections.Concurrent.ConcurrentDictionary<(char, char, int),  long>;
using Keypad = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
record struct Vec2(long x, long y);


[ProblemName("Keypad Conundrum")]
class Solution : Solver {

    public object PartOne(string input) {
        return input.Split("\n").Sum(line => Solve2(line, 2));
    }
    public object PartTwo(string input) {
        return input.Split("\n").Sum(line => Solve2(line, 25));
    }

    static readonly Complex Right = 1;
    static readonly Complex Up = Complex.ImaginaryOne;
    static readonly Complex Down = -Complex.ImaginaryOne;

    long Solve2(string line, int depth) {
        var keypad1 = ParseKeypad("789\n456\n123\n 0A");
        var keypad2 = ParseKeypad(" ^A\n<v>");
        var keypads = Enumerable.Repeat(keypad2, depth).Prepend(keypad1).ToArray();
        var res = EncodeString(line, keypads,  new Cache());
        return res * int.Parse(line.Substring(0, line.Length - 1));
    }

    long EncodeString(string st, Keypad[] keypads, Cache cache) {
        if (keypads.Length == 0) {
            return st.Length;
        } else {

            // the robot starts and finishes by pointing to 'A' key
            var currentKey = 'A';
            var length = 0L;
            foreach (var nextKey in st) {
                length += EncodeKey(currentKey, nextKey, keypads, cache);
                currentKey = nextKey;
            }
            Debug.Assert(st.Last() == 'A', "The robot should point to the 'A' key");
            return length;
        }
    }
    long EncodeKey(char currentKey, char nextKey, Keypad[] keypads,  Cache cache) {
        return cache.GetOrAdd((currentKey, nextKey, keypads.Length), _ => {
            var currentPos = keypads[0].Single(kvp => kvp.Value == currentKey).Key;
            var nextPos = keypads[0].Single(kvp => kvp.Value == nextKey).Key;

            var dy = (int)(nextPos.Imaginary - currentPos.Imaginary);
            var dx = (int)(nextPos.Real - currentPos.Real);

            var vert = new string(dy < 0 ? 'v' : '^', Math.Abs(dy));
            var horiz = new string(dx < 0 ? '<' : '>', Math.Abs(dx));

            var cost = long.MaxValue;

            if (keypads[0][currentPos + dy * Up] != ' ') {
                cost = Math.Min(cost, EncodeString($"{vert}{horiz}A", keypads[1..], cache));
            }
    
            if (keypads[0][currentPos + dx * Right] != ' ') {
                cost = Math.Min(cost, EncodeString($"{horiz}{vert}A", keypads[1..], cache));
            }
            return cost;
        });
    }

    Keypad ParseKeypad(string keypad) {
        var lines = keypad.Split("\n");
        return (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Down, lines[y][x])
        ).ToDictionary();
    }
}