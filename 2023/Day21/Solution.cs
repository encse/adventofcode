namespace AdventOfCode.Y2023.Day21;

using System.Collections.Generic;
using System.Linq;
using System.Numerics;

record Pos(Complex p, Complex tile);

[ProblemName("Step Counter")]
class Solution : Solver {

    public object PartOne(string input) => Steps(ParseMap(input)).ElementAt(64);

    // At first I solved this with carefully maintaining the number of different 
    // tiles (the 131x131 regions that repeat indefinitely) after each step. It 
    // turns out that there are only nine tile categories based on the direction 
    // closest to the starting point. The elf can go straight left, up, right 
    // and down and reach the next tile without obstacles. This is a special 
    // property of the input.
    //
    // Each tile in a category can be in a few hundred different states. The 
    // first one (what I call the 'seed') is the point where the elf enters the 
    // tile. This can be the center of an edge or one of its corners. After 
    // seeding, the tile 'ages' on its own pace. Thanks to an other property of 
    // the input, tiles are not affected by their neighbourhood. Aging continues 
    // until a tile 'grows' up, when it starts to oscillate between just two 
    // states back and forth.
    //
    // My first solution involved a 9 by 260 matrix containing the number of 
    // tiles in each state. I implemented the aging process and carefully 
    // computed when to seed new tiles for each category.
    //
    // It turns out that if we are looking at only steps where n = 131 * k + 65 
    // we can compute how many tiles are in each position of the matrix.
    // I haven't gone through this whole process, just checked a few examples 
    // until I convinced myself that each and every item in the matrix is either 
    // constant or a linear or quadratic function of n.
    // 
    // This is not that hard to see as it sounds. After some lead in at the 
    // beginning, things start to work like this: in each batch of 131 steps a 
    // set of center tiles and a set of corner styles is generated. 
    // Always 4 center tiles come in, but corner tiles are linear in n (1, 3, 5, ...)
    // That is: the grown up population for center tiles must be linear in n, 
    // and quadratic for the corners (can be computed using triangular numbers). 
    // 
    // If we know the active positions for each tile category and state, we
    // can multiply it with the number of tiles and sum it up to get the result.
    //
    // This all means that if we reorganize the equations we get to a form of:
    //
    //     a * n^2 + b * n + c      if n = k * 131 + 65
    //
    // We just need to compute this polynom for 3 values and interpolate.
    //
    // Finally evaluate for n = 26501365 which happens to be 202300 * 131 + 65
    // to get the final result.
    public object PartTwo(string input) {
       
        var n = 26501365;

        // Newton interpolation for 3 points: k * 131 + 65 for k = 0, 1, 2
        var steps = Steps(ParseMap(input)).Take(328).ToArray();

        (decimal x0, decimal y0) = (65, steps[65]);
        (decimal x1, decimal y1) = (196, steps[196]);
        (decimal x2, decimal y2) = (327, steps[327]);

        decimal y01 = (y1 - y0) / (x1 - x0);
        decimal y12 = (y2 - y1) / (x2 - x1);
        decimal y012 = (y12 - y01) / (x2 - x0);
        return (long)(y0 + y01 * (n - x0) + y012 * (n - x0) * (n - x1));
    }

    // starts walking and returns the number of available positions at each step
    IEnumerable<long> Steps(HashSet<Complex> map) {
        var positions = new HashSet<Pos> { new Pos(new Complex(65, 65), 0) };
        while(true) {
            yield return positions.Count;
            positions = Step(map, positions);
        }
    }
    
    HashSet<Pos> Step(HashSet<Complex> map, HashSet<Pos> positions) {
        Complex[] dirs = [1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne];

        var res = new HashSet<Pos>();
        foreach (var pos in positions) {
            // step in each direction taking care of the wrap around maintaining
            // the tile position as well.
            foreach (var dir in dirs) {
                var pT = pos.p + dir;
                var tileT = pos.tile;

                if (pT.Imaginary < 0) {
                    pT += 131 * Complex.ImaginaryOne;
                    tileT -= Complex.ImaginaryOne;
                } else if (pT.Imaginary > 130) {
                    pT -= 131 * Complex.ImaginaryOne;
                    tileT += Complex.ImaginaryOne;
                } else if (pT.Real < 0) {
                    pT += 131;
                    tileT -= 1;
                } else if (pT.Real > 130) {
                    pT -= 131;
                    tileT += 1;
                }

                if (map.Contains(pT)) {
                    res.Add(new Pos(pT, tileT));
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
            where lines[irow][icol] != '#'
            select new Complex(icol, irow)
        ).ToHashSet();
    }
}
