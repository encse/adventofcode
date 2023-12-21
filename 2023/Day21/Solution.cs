namespace AdventOfCode.Y2023.Day21;

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

    public object PartTwo(string input) {
        var steps = 26501365;
        var map = ParseMap(input);
        var s = map.Keys.Where(k => map[k] == 'S').Single();
        var br = map.Keys.MaxBy(pos => pos.Real + pos.Imaginary);
        var loop = 260;

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
        var cohorts = new Dictionary<Complex, long[]>();

        cohorts[center] = new long[loop + 1];
        foreach (var corner in corners) {
            cohorts[corner] = new long[loop + 1];
        }
        foreach (var middle in middles) {
            cohorts[middle] = new long[loop + 1];
        }

        var m = 0;
        cohorts[center][m] = 1;
        var phaseLength = loop + 1;
        for (var i = 1; i <= steps; i++) {

            var nextM = (m + phaseLength - 1) % phaseLength;
            foreach (var item in cohorts.Keys) {
                var phase = cohorts[item];
                var a = phase[(m + phase.Length - 1) % phase.Length];
                var b = phase[(m + phase.Length - 2) % phase.Length];
                var c = phase[(m + phase.Length - 3) % phase.Length];

                phase[nextM] = 0;
                phase[(nextM + phase.Length - 1) % phase.Length] = b;
                phase[(nextM + phase.Length - 2) % phase.Length] = a + c;
            }
            m = nextM;

            if (i >= 132 && (i - 132) % 131 == 0) {
                var newItems = i / 131;
                foreach (var corner in corners) {
                    cohorts[corner][m] += newItems;
                }
            } else if (i >= 66 && (i - 66) % 131 == 0) {
                foreach (var middle in middles) {
                    cohorts[middle][m]++;
                }
            }
        }

        var res = 0L;

        // var counts = 0;
        foreach (var item in cohorts.Keys) {
            var phase = cohorts[item];
            var pos = new HashSet<Complex> { item };
            for (var i = 0; i < phase.Length; i++) {
                var count = phase[(m + i) % phase.Length];
                res += pos.Count * count;
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