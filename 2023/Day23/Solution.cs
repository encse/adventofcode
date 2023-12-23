namespace AdventOfCode.Y2023.Day23;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using Map = System.Collections.Immutable.IImmutableDictionary<System.Numerics.Complex, char>;
using System.Collections;

[ProblemName("A Long Walk")]
class Solution : Solver {

    public object PartOne(string input) {
        return Solve(ParseMap(input), 1, false).Max();
    }

    public object PartTwo(string input) {
       return Solve(ParseMap(input), 1, true).Max();
    }

    IEnumerable<int> Solve(Map map, Complex pos, bool part2) {
        var goal = map.Keys.MaxBy(pos => pos.Imaginary + pos.Real) - 1;
        Console.WriteLine(map[pos]);
        Console.WriteLine(map[goal]);
        var q = new Queue<(Complex pos, ImmutableHashSet<Complex> seen)>();
        q.Enqueue((pos, ImmutableHashSet<Complex>.Empty.Add(pos)));
        while (q.Any()) {
            (pos, var seen) = q.Dequeue();
            if (pos == goal) {
                Console.WriteLine((seen.Count, q.Count));
                yield return seen.Count-1;
            } else {
                foreach (var posT in Next(map, pos, part2)) {
                    if (!seen.Contains(posT)) {
                        q.Enqueue((posT, seen.Add(posT)));
                    }
                }
            }
        }
    }

    IEnumerable<Complex> Next(Map map, Complex pos, bool part2) {
        if (map[pos] == '.' || part2) {
            foreach (var dir in new[] { 1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne }) {
                var posT = pos + dir;
                if (map.ContainsKey(posT) && map[posT] != '#') {
                    yield return posT;
                }
            }
        } else if (map[pos] == '>') {
            yield return pos + 1;
        } else if (map[pos] == 'v') {
            yield return pos + Complex.ImaginaryOne;
        } else {
            Console.WriteLine(map[pos]);
            throw new Exception();
        }
    }

    Map ParseMap(string input) {
        var lines = input.Split('\n');
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            let cell = lines[irow][icol]
            let pos = new Complex(icol, irow)
            select new KeyValuePair<Complex, char>(pos, cell)
        ).ToImmutableDictionary();
    }
}