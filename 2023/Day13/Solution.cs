using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
using AngleSharp.Common;

namespace AdventOfCode.Y2023.Day13;

[ProblemName("Point of Incidence")]
class Solution : Solver {

    public object PartOne(string input) {
        var blocks = input.Split("\n\n");
        var s = 0;
        foreach (var block in blocks) {
            var rows = block.Split("\n");
            var crow = rows.Length;
            var ccol = rows[0].Length;
            var map = ParseMap(rows);
            s += Solve(map, crow, ccol).Single();
        }
        return s;
    }

    public object PartTwo(string input) {

        var blocks = input.Split("\n\n");
        var s = 0;
        var iblock = 0;
        foreach (var block in blocks) {
            iblock++;
            var rows = block.Split("\n");
            var crow = rows.Length;
            var ccol = rows[0].Length;
            var map = ParseMap(rows);

            var orig = Solve(map, crow, ccol).Single();
            int[] patched = null;
            foreach (var key in map.Keys) {
                if (map[key] == '.') {
                    map[key] = '#';
                    patched = Solve(map, crow, ccol);
                    map[key] = '.';
                } else {
                    map[key] = '.';
                    patched = Solve(map, crow, ccol);
                    map[key] = '#';
                }

                patched = patched.Where(x => x != orig).ToArray();
                if (patched.Length>0) {
                    break;
                }
            }
            if (patched.Length!=1) {
                Console.WriteLine(iblock);
                Console.WriteLine(block);
                Console.WriteLine(patched.Length);
                Console.WriteLine();
            } else {
                s += patched.Single();
            }
        }
        return s;
    }

    int[] Solve(Map map, int crow, int ccol) {
        return SolveHoriz(map, crow).Concat(SolveVert(map, ccol)).ToArray();
    }

    int[] SolveHoriz(Map map, int crow) {
        HashSet<int> res = null;
         for (var irow = 0; irow < crow; irow++) {
            var mirrors = Mirrors(map, new Complex(0, irow), Right).ToArray();
            if (res == null) {
                res = new HashSet<int>(mirrors);
            } else {
                res.IntersectWith(mirrors);
            }
        }
        return res.Select(r => r + 1).ToArray();
    }
     int[] SolveVert(Map map, int ccol) {
        HashSet<int> res = null;
        res = null;
        for (var icol = 0; icol < ccol; icol++) {
            var mirrors = Mirrors(map, new Complex(icol, 0), Down).ToArray();
            if (res == null) {
                res = new HashSet<int>(mirrors);
            } else {
                res.IntersectWith(mirrors);
            }
        }
        return res.Select(r => (r + 1) * 100).ToArray();
    }

    Complex Right = 1;
    Complex Down = Complex.ImaginaryOne;
    Complex Flip(Complex dir) => dir == Down ? Right : Down;

    IEnumerable<int> Mirrors(Map map, Complex pos, Complex dir) {
        var i = 0;
        while (map.ContainsKey(pos + dir)) {
            var s = 0.5 * dir;
            var ok = true;
            while (map.ContainsKey(pos + 0.5 * dir - s) && map.ContainsKey(pos + 0.5 * dir + s) && ok) {
                var pleft = pos + 0.5 * dir - s;
                var pright = pos + 0.5 * dir + s;
                var left = map[pleft];
                var right = map[pright];
                ok = left == right;
                s += dir;
            }
            if (ok) {
                yield return i;
            }
            pos += dir;
            i++;
        }
    }

    Map ParseMap(string[] rows) {
        return (
            from irow in Enumerable.Range(0, rows.Length)
            from icol in Enumerable.Range(0, rows[0].Length)
            let pos = new Complex(icol, irow)
            let cell = rows[irow][icol]
            select new KeyValuePair<Complex, char>(pos, cell)
        ).ToDictionary();
    }

}
