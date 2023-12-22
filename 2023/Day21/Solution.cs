namespace AdventOfCode.Y2023.Day21;

using System.Collections.Generic;
using System.Linq;
using System.Numerics;

[ProblemName("Step Counter")]
class Solution : Solver {

    static readonly Complex center = new Complex(65, 65);
    static readonly Complex[] corners = [
        new Complex(0, 0),
        new Complex(0, 130),
        new Complex(130, 130),
        new Complex(130, 0),
    ];
    static readonly Complex[] middles = [
        new Complex(65, 0),
        new Complex(65, 130),
        new Complex(0, 65),
        new Complex(130, 65),
    ];
    static readonly Complex[] entrypoints = [center, .. corners, .. middles];
    static readonly Complex[] dirs = [ 1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne ];

    // wip
    public object PartOne(string input) {
        var map = ParseMap(input);
        var positions = new HashSet<Complex> { center };
        for (var i = 0; i < 64; i++) {
            positions = Step(map, positions);
        }
        return positions.Count;
    }

    public object PartTwo(string input) {

        var steps = 26501365; // 202300 * 131 + 65
        var bufferSize = 270; // anything that is > 260
        var buffers = new Dictionary<Complex, CircularBuffer<long>>();
        foreach (var entryPoint in entrypoints) {
            buffers[entryPoint] = new CircularBuffer<long>(bufferSize);
        }

        buffers[center][1] = 1;
        for (var i = 1; i < steps; i++) {
            foreach (var cb in buffers.Values) {
                cb[^3] += cb[^1];
            }
            buffers[center].Shift(0);
            foreach (var item in middles) {
                buffers[item].Shift(i % 131 == 65 ? 1 : 0);
            }
            foreach (var item in corners) {
                buffers[item].Shift(i % 131 == 0 ? i/131 : 0);
            }
        }

        var map = ParseMap(input);
        var res = 0L;
        foreach (var entryPoint in entrypoints) {
            var positions = new HashSet<Complex> { entryPoint };
            for (var i = 0; i < bufferSize; i++) {
                res += positions.Count * buffers[entryPoint][i];
                positions = Step(map, positions);
            }
        }
        return res;
    }

    HashSet<Complex> Step(HashSet<Complex> map, HashSet<Complex> pos) {
        var res = new HashSet<Complex>();
        foreach (var p in pos) {
            foreach (var dir in dirs) {
                var pT = p + dir;
                if (map.Contains(pT)) {
                    res.Add(pT);
                }
            }
        }
        return res;
    }

    HashSet<Complex> ParseMap(string input) {
        var lines = input.Split("\n");
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            where lines[irow][icol]  != '#'
            select new Complex(icol, irow)
        ).ToHashSet();
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

    public T Shift(T t) {
        i = (i + size - 1) % size;
        var res = items[i];
        items[i] = t;
        return res;
    }
}