using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Y2023.Day16;
using Map = Dictionary<Complex, char>;
using Beam = (Complex pos, Complex dir);

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
        // this is essentially just a flood fill algorithm..
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

    // go around the edges of the map and return the inward pointing directions
    IEnumerable<Beam> StartBeams(Map map) {
        var (tl, br) = (TopLeft(map), BottomRight(map));
        return [
            ..from pos in Positions(map, tl, Right) select (pos, Down),
            ..from pos in Positions(map, tl, Down) select (pos, Right),
            ..from pos in Positions(map, br, Left) select (pos, Up),
            ..from pos in Positions(map, br, Up) select (pos, Left),
        ];
    }

    // use a dictionary because of the easy bounds check (i.e. containskey)
    Map ParseMap(string input) {
        var lines = input.Split('\n');
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            let pos = new Complex(icol, irow)
            let cell = lines[irow][icol]
            select new KeyValuePair<Complex, char>(pos, cell)
        ).ToDictionary();
    }

    // map boundary
    Complex TopLeft(Map map) => Complex.Zero;
    Complex BottomRight(Map map) => map.Keys.MaxBy(pos => pos.Imaginary + pos.Real);

    // allowed positions of the map from 'start' going in 'dir'
    IEnumerable<Complex> Positions(Map map, Complex start, Complex dir) {
        for (var pos = start; map.ContainsKey(pos); pos += dir) {
            yield return pos;
        }
    }

    // The 'exit' direction(s) of the given cell when entered by a beam moving in 'dir'
    Complex[] Exits(char cell, Complex dir) {
        var p = (cell, dir);
        return
            p == ('-', Up)     ? [Left, Right] :
            p == ('-', Down)   ? [Left, Right] :
            p == ('|', Left)   ? [Up, Down] :
            p == ('|', Right)  ? [Up, Down] :
            p == ('/', Left)   ? [Down] :
            p == ('/', Right)  ? [Up] :
            p == ('/', Up)     ? [Right] :
            p == ('/', Down)   ? [Left] :
            p == ('\\', Left)  ? [Up] :
            p == ('\\', Right) ? [Down] :
            p == ('\\', Up)    ? [Left] :
            p == ('\\', Down)  ? [Right] :
                 /* otherwise */ [p.dir];
    }
}