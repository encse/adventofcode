namespace AdventOfCode.Y2023.Day18;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

[ProblemName("Lavaduct Lagoon")]
class Solution : Solver {

    public object PartOne(string input)=> Area(Steps1(input));
    public object PartTwo(string input) => Area(Steps2(input));

    IEnumerable<Complex> Steps1(string input) =>
        from line in input.Split('\n')
        let parts = line.Split(' ')
        let dir = parts[0] switch {
            "R" => Complex.One,
            "U" => -Complex.ImaginaryOne,
            "L" => -Complex.One,
            "D" => Complex.ImaginaryOne,
            _ => throw new Exception()
        }
        let dist = int.Parse(parts[1])
        select dir * dist;

    IEnumerable<Complex> Steps2(string input) =>
        from line in input.Split('\n')
        let hex = line.Split(' ')[2]
        let dir = hex[7] switch {
            '0' => Complex.One,
            '1' => -Complex.ImaginaryOne,
            '2' => -Complex.One,
            '3' => Complex.ImaginaryOne,
            _ => throw new Exception()
        }
        let dist = Convert.ToInt32(hex[2..7], 16)
        select dir * dist;

    // We are using a combination of the shoelace formula with Pick's theorem
    double Area(IEnumerable<Complex> steps) {
        var vertices = Vertices(steps).ToList();

        // Shoelace formula https://en.wikipedia.org/wiki/Shoelace_formula
        var shiftedVertices = vertices.Skip(1).Append(vertices[0]);
        var shoelaces =
            from pair in vertices.Zip(shiftedVertices)
            select pair.First.Real * pair.Second.Imaginary - pair.First.Imaginary * pair.Second.Real;
        var area = Math.Abs(shoelaces.Sum()) / 2;

        // Pick's theorem  https://en.wikipedia.org/wiki/Pick%27s_theorem
        var boundary = steps.Select(x => x.Magnitude).Sum();
        var interior = area - boundary / 2 + 1; 

        // Integer area
        return boundary + interior;
    }

     IEnumerable<Complex> Vertices(IEnumerable<Complex> steps) {
        var pos = Complex.Zero;
        foreach (var step in steps) {
            pos += step;
            yield return pos;
        }
    }
}