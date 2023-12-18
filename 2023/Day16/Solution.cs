namespace AdventOfCode.Y2023.Day16;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
using Beam = (System.Numerics.Complex pos, System.Numerics.Complex dir);

[ProblemName("The Floor Will Be Lava")]
class Solution : Solver {

    static readonly Complex Up = -Complex.ImaginaryOne;
    static readonly Complex Down = Complex.ImaginaryOne;
    static readonly Complex Left = -Complex.One;
    static readonly Complex Right = Complex.One;

    public object PartOne(string input) =>
        EnergizedCells(ParseMap(input), (Complex.Zero, Right));

    public object PartTwo(string input) {
        var map = ParseMap(input);
        return (from beam in StartBeams(map) select EnergizedCells(map, beam)).Max();
    }

    // follow the beam in the map and return the energized cell count. 
    int EnergizedCells(Map map, Beam beam) {

        // this is essentially just a flood fill algorithm.
        var q = new Queue<Beam>([beam]);
        var seen = new HashSet<Beam>();

        while (q.TryDequeue(out beam)) {
            seen.Add(beam);
            foreach (var dir in Exits(map[beam.pos], beam.dir)) {
                var pos = beam.pos + dir;
                if (map.ContainsKey(pos) && !seen.Contains((pos, dir))) {
                    q.Enqueue((pos, dir));
                }
            }
        }

        return seen.Select(beam => beam.pos).Distinct().Count();
    }

    // go around the edges (top, right, bottom, left order) of the map
    // and return the inward pointing directions
    IEnumerable<Beam> StartBeams(Map map) {
        var br = map.Keys.MaxBy(pos => pos.Imaginary + pos.Real);
        return [
            ..from pos in map.Keys where pos.Real == 0 select (pos, Down),
            ..from pos in map.Keys where pos.Real == br.Real select (pos, Left),
            ..from pos in map.Keys where pos.Imaginary == br.Imaginary select (pos, Up),
            ..from pos in map.Keys where pos.Imaginary == 0 select (pos, Right),
        ];
    }

    // using a dictionary helps with bounds check (simply containskey):
    Map ParseMap(string input) {
        var lines = input.Split('\n');
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            let cell = lines[irow][icol]
            let pos = new Complex(icol, irow)
            select new KeyValuePair<Complex, char>(pos, cell)
        ).ToDictionary();
    }

    // the 'exit' direction(s) of the given cell when entered by a beam moving in 'dir'
    // we have some special cases for mirrors and spliters, the rest keeps the direction
    Complex[] Exits(char cell, Complex dir) => cell switch {
        '-' when dir == Up || dir == Down => [Left, Right],
        '|' when dir == Left || dir == Right => [Up, Down],
        '/' => [-new Complex(dir.Imaginary, dir.Real)],
        '\\' => [new Complex(dir.Imaginary, dir.Real)],
        _ => [dir]
    };
}