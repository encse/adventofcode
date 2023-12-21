namespace AdventOfCode.Y2023.Day21;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

[ProblemName("Step Counter")]
class Solution : Solver {

    // wip
    public object PartOne(string input) {
        var map = ParseMap(input);
        var s = map.Keys.Where(k => map[k] == 'S').Single();
        var pos = new HashSet<Complex> { s };
        for (var i = 0; i < 64; i++) {
            pos = Step(map, pos);
        }
        return pos.Count;
    }

    int LoopLength(Dictionary<Complex, char> map, Complex pos) {
        var prev2 = new HashSet<Complex>();
        var prev1 = new HashSet<Complex>();
        var cur = new HashSet<Complex> { pos };
        for (var i = 0; ; i++) {
            prev2 = prev1;
            prev1 = cur;
            cur = Step(map, cur);
            if (prev2.Count == cur.Count && prev2.Intersect(cur).Count() == prev2.Count) {
                return i;
            }
        }
    }

    public object PartTwo(string input) {

        var steps = 26501365;
        var map = ParseMap(input);
        var s = map.Keys.Where(k => map[k] == 'S').Single();
        var br = map.Keys.MaxBy(pos => pos.Real + pos.Imaginary);

        Complex center = new Complex(65, 65);

        Complex[] corners = [
            new Complex(0, 0),
            new Complex(0, 130),
            new Complex(130, 130),
            new Complex(130, 0),
        ];

        Complex[] middles = [
            new Complex(65, 0),
            new Complex(65, 130),
            new Complex(0, 65),
            new Complex(130, 65),
        ];

        var cohorts = new Dictionary<Complex, CircularBuffer<long>>();

        var loop = 263;
        cohorts[center] = new CircularBuffer<long>(loop);
        foreach (var corner in corners) {
            cohorts[corner] = new CircularBuffer<long>(loop);
        }
        foreach (var middle in middles) {
            cohorts[middle] = new CircularBuffer<long>(loop);
        }


        foreach (var k in cohorts.Keys) {
            Console.WriteLine((k, LoopLength(map, k)));
        }

        cohorts[center][1] = 1;
        for (var i = 2; i <= steps; i++) {
            foreach (var item in cohorts.Keys) {
                var phase = cohorts[item];
                var last = phase.Enqueue(0);
                phase[^2] += last;
            }
            var rem = i % 131;
            if (rem == 1) {
                var newItems = i / 131;
                foreach (var corner in corners) {
                    cohorts[corner][0] += newItems;
                }
            } else if (rem == 66) {
                foreach (var middle in middles) {
                    cohorts[middle][0]++;
                }
            }
        }

        Console.WriteLine("y");
        var res = 0L;

        // var counts = 0;
        foreach (var item in cohorts.Keys) {
            var phase = cohorts[item];
            var pos = new HashSet<Complex> { item };
            for (var i = 0; i < loop; i++) {
                var count = phase[i];
                if (count > 0) {
                    Console.WriteLine((item, i, count, pos.Count, pos.Count * count));
                }
                res += pos.Count * count;
                pos = Step(map, pos);
            }
            Console.WriteLine("---");
        }
        Console.WriteLine("z");
        return res;
    }

    HashSet<Complex> Step(Dictionary<Complex, char> map, HashSet<Complex> pos) {
        var res = new HashSet<Complex>();
        foreach (var p in pos) {
            foreach (var dir in new Complex[] { 1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne }) {
                var pT = p + dir;
                if (map.ContainsKey(pT) && map[pT] != '#') {
                    res.Add(pT);
                }
            }
        }
        return res;
    }

    Dictionary<Complex, char> ParseMap(string input) {
        var lines = input.Split("\n");
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(
                new Complex(icol, irow), lines[irow][icol]
            )
        ).ToDictionary();
    }
}

class CircularBuffer<T>(int size) {
    public int Count => size;
    T[] items = new T[size];
    int i = 0;

    public T this[int index] {
        get { return items[(i + index) % size]; }
        set { items[(i + index) % size] = value; }
    }

    public T Enqueue(T t) {
        i = (i + size - 1) % size;
        var res = items[i];
        items[i] = t;
        return res;
    }
}