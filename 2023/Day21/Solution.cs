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

    void Shift(CircularBuffer<long> cb, long item) {
        var last = cb.Enqueue(item);
        cb[^2] += last;
    }

    public object PartTwo(string input) {

        var steps = 26501365;

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
        Complex[] entryPoints = [center, .. corners, .. middles];

        var bufferSize = 261;
        var buffers = new Dictionary<Complex, CircularBuffer<long>>();
        foreach (var entryPoint in entryPoints) {
            buffers[entryPoint] = new CircularBuffer<long>(bufferSize);
        }

        Shift(buffers[center], 1);
        Shift(buffers[center], 0);

        for (var i = 1; i < steps; i++) {
            Shift(buffers[center], 0);
            foreach (var corner in corners) {
                Shift(buffers[corner], i % 131 == 0 ? i / 131 : 0);
            }
            foreach (var corner in middles) {
                Shift(buffers[corner], i % 131 == 65 ? 1 : 0);
            }
        }

        var map = ParseMap(input);
        var res = 0L;
        foreach (var entryPoint in entryPoints) {
            var buffer = buffers[entryPoint];
            var pos = new HashSet<Complex> { entryPoint };
            for (var i = 0; i < bufferSize; i++) {
                res += pos.Count * buffer[i];
                pos = Step(map, pos);
            }
        }
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