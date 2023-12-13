using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;

namespace AdventOfCode.Y2023.Day13;

[ProblemName("Point of Incidence")]
class Solution : Solver {

    Complex Right = 1;
    Complex Down = Complex.ImaginaryOne;
    Complex Ortho(Complex dir) => dir == Right ? Down : Right;

    public object PartOne(string input) => Solve(input, 0);
    public object PartTwo(string input) => Solve(input, 1);

    double Solve(string input, int allowedSmudges) => (
        from block in input.Split("\n\n")
        let map = ParseMap(block)
        select GetScore(map, allowedSmudges)
    ).Sum();

    double GetScore(Map map, int allowedSmudges) => (
        from dir in new Complex[] { Right, Down }
        from mirror in Positions(map, dir, dir)
        where FindSmudges(map, mirror, dir) == allowedSmudges
        select mirror.Real + 100 * mirror.Imaginary
    ).First();

    // cast a ray from each postion along the mirror and count the 'smuggles'
    int FindSmudges(Map map, Complex mirror, Complex rayDir) => (
        from ray0 in Positions(map, mirror, Ortho(rayDir))
        let rayA = Positions(map, ray0, rayDir)
        let rayB = Positions(map, ray0 - rayDir, -rayDir)
        select Enumerable.Zip(rayA, rayB).Count(p => map[p.First] != map[p.Second])
    ).Sum();

    Map ParseMap(string input) {
        var rows = input.Split("\n");
        return (
            from irow in Enumerable.Range(0, rows.Length)
            from icol in Enumerable.Range(0, rows[0].Length)
            let pos = new Complex(icol, irow)
            let cell = rows[irow][icol]
            select new KeyValuePair<Complex, char>(pos, cell)
        ).ToDictionary();
    }

    // allowed poistions of the map from 'start' going in 'dir'
    IEnumerable<Complex> Positions(Map map, Complex start, Complex dir) {
        for (var pos = start; map.ContainsKey(pos); pos += dir) {
            yield return pos;
        }
    }
}
