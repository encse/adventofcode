namespace AdventOfCode.Y2023.Day21;

using System.Collections.Generic;
using System.Linq;
using System.Numerics;

[ProblemName("Step Counter")]
class Solution : Solver {

    public object PartOne(string input) => Steps(ParseMap(input)).ElementAt(64);
    public object PartTwo(string input) {
        // Exploiting some nice properties of the input it reduces to quadratic 
        // interpolation over 3 points: k * 131 + 65 for k = 0, 1, 2
        // I used the Newton method.
        var steps = Steps(ParseMap(input)).Take(328).ToArray();

        (decimal x0, decimal y0) = (65, steps[65]);
        (decimal x1, decimal y1) = (196, steps[196]);
        (decimal x2, decimal y2) = (327, steps[327]);

        decimal y01 = (y1 - y0) / (x1 - x0);
        decimal y12 = (y2 - y1) / (x2 - x1);
        decimal y012 = (y12 - y01) / (x2 - x0);

        var n = 26501365;
        return (long)(y0 + y01 * (n - x0) + y012 * (n - x0) * (n - x1));
    }

    // walks around and returns the number of available positions at each step
    IEnumerable<long> Steps(HashSet<Complex> map) {
        var positions = new HashSet<Complex> { new Complex(65, 65) };
        while(true) {
            yield return positions.Count;
            positions = Step(map, positions);
        }
    }
    
    HashSet<Complex> Step(HashSet<Complex> map, HashSet<Complex> positions) {
        Complex[] dirs = [1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne];

        var res = new HashSet<Complex>();
        foreach (var pos in positions) {
            foreach (var dir in dirs) {
                var posT = pos + dir;
                var tileCol = Mod(posT.Real, 131);
                var tileRow = Mod(posT.Imaginary, 131);
                if (map.Contains(new Complex(tileCol, tileRow))) {
                    res.Add(posT);
                }
            }
        }
        return res;
    }

    // the double % takes care of negative numbers
    double Mod(double n, int m) => ((n % m) + m) % m;

    HashSet<Complex> ParseMap(string input) {
        var lines = input.Split("\n");
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            where lines[irow][icol] != '#'
            select new Complex(icol, irow)
        ).ToHashSet();
    }
}
