namespace AdventOfCode.Y2023.Day21;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

[ProblemName("Step Counter")]
class Solution : Solver {

    public object PartOne(string input) {
        var map = ParseMap(input);
        var s = map.Keys.Where(k => map[k] == 'S').Single();
        return StepN(map, 64, s).Count();
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

    HashSet<Complex> StepN(Dictionary<Complex, char> map, int n, Complex s) {
        var pos = new HashSet<Complex> { s };
        for (var i = 0; i < n; i++) {
            pos = Step(map, pos);
        }
        return pos;
    }

    HashSet<(Complex, Complex)> Step2(Dictionary<Complex, char> map, HashSet<(Complex, Complex)> pos) {
        var br = map.Keys.MaxBy(pos => pos.Real + pos.Imaginary);
        var crow = br.Real + 1;
        var ccol = br.Imaginary + 1;

        var res = new HashSet<(Complex, Complex)>();
        foreach (var (p, tile) in pos) {
            foreach (var dir in new Complex[] { 1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne }) {
                var posT = p + dir;
                var tileT = tile;
                if (posT.Real == -1) {
                    posT += ccol;
                    tileT -= 1;
                } else if (posT.Real == ccol) {
                    posT -= ccol;
                    tileT += 1;
                } else if (posT.Imaginary == -1) {
                    posT += Complex.ImaginaryOne * crow;
                    tileT -= Complex.ImaginaryOne;
                } else if (posT.Imaginary == crow) {
                    posT -= Complex.ImaginaryOne * crow;
                    tileT += Complex.ImaginaryOne;
                }

                if (map.ContainsKey(posT) && map[posT] != '#') {
                    res.Add((posT, tileT));
                }
            }
        }
        return res;
    }

    bool Eq(HashSet<Complex> h1, HashSet<Complex> h2) {
        return h1.Count == h2.Count && h1.Intersect(h2).Count() == h1.Count;
    }

    public object PartTwo(string input) {
        var steps = 26501365;
        steps = 330;

        Console.WriteLine("expected "+PartTwoCheck(input, steps));
        Console.WriteLine("actual "+PartTwoB(input, steps));
        return 0;

    }

    public object PartTwoCheck(string input, int steps) {
        var map = ParseMap(input);
        var s = map.Keys.Where(k => map[k] == 'S').Single();
        var pos = new HashSet<(Complex, Complex)> { (s, 0) };
        for (var i = 0; i < steps; i++) {
            Console.WriteLine(i);
            pos = Step2(map, pos);
        }
        return pos.Count;
    }
    public object PartTwoB(string input, int steps) {
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
        var cohorts = new Dictionary<Complex, int[]>();

        cohorts[center] = new int[loop + 1];
        foreach (var corner in corners) {
            cohorts[corner] = new int[loop + 1];
        }
        foreach (var middle in middles) {
            cohorts[middle] = new int[loop + 1];
        }

        var m = 0;
        cohorts[center][m] = 1;
        var generated = 0;

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

            if (i % 1000 == 0) {
                Console.WriteLine((double)i / steps);
            }
            if (i == 132) {
                foreach (var corner in corners) {
                    cohorts[corner][m]++;
                    generated++;
                }
            } else if (i == 263) {
                foreach (var corner in corners) {
                    cohorts[corner][m]+=2;
                    generated++;
                }
            } else if (i == 66 || i == 197 || i == 328) {
                foreach (var middle in middles) {
                    cohorts[middle][m]++;
                    generated++;
                }
            }

        }

        var res = 0L;

        var counts = 0;
        foreach (var item in cohorts.Keys) {
            var phase = cohorts[item];
            var pos = new HashSet<Complex> { item };
            for (var i = 0; i < phase.Length; i++) {
                var count = phase[(m + i) % phase.Length];
                counts += count;
                res += (long)pos.Count * count;
                pos = Step(map, pos);
            }
        }

        // Console.WriteLine((steps / 66 * 4, generated, counts, res));
        return res;
        // var s1 = interior * paintedCount[^1] + paintedCount[remains] * boundary;
        // var s2 = interior * paintedCount[^1] + paintedCount[remains-1] * boundary;
        // 6727872752
        // 12154430745
        // 98630436444205
        // 1238804309550935
        // return s1 - s2;
    }

    Dictionary<Complex, HashSet<Complex>> Step2b(
        Dictionary<Complex, char> map,
        Dictionary<Complex, HashSet<Complex>> positions
    ) {
        var br = map.Keys.MaxBy(pos => pos.Real + pos.Imaginary);
        var crow = br.Real + 1;
        var ccol = br.Imaginary + 1;

        var res = new Dictionary<Complex, HashSet<Complex>>();
        foreach (var pos in positions.Keys) {
            foreach (var dir in new Complex[] { 1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne }) {
                var posT = pos + dir;
                var tiles = positions[pos]; //.ToHashSet();
                if (posT.Real == -1) {
                    tiles = tiles.Select(tile => tile - 1).ToHashSet();
                    posT += ccol;
                } else if (posT.Real == ccol) {
                    tiles = tiles.Select(tile => tile + 1).ToHashSet();
                    posT -= ccol;
                } else if (posT.Imaginary == -1) {
                    tiles = tiles.Select(tile => tile - Complex.ImaginaryOne).ToHashSet();
                    posT += Complex.ImaginaryOne * crow;
                } else if (posT.Imaginary == crow) {
                    tiles = tiles.Select(tile => tile + Complex.ImaginaryOne).ToHashSet();
                    posT -= Complex.ImaginaryOne * crow;
                }
                if (map[posT] != '#') {
                    if (res.ContainsKey(posT)) {
                        var h1 = res[posT];
                        var intersection = h1.Intersect(tiles).ToHashSet();
                        var union = h1.Union(tiles).ToHashSet();
                        if (intersection.Count != union.Count) {
                            Console.WriteLine("x");
                        }
                        res[posT] = res[posT].ToHashSet();
                        res[posT].UnionWith(tiles);
                    } else {
                        res[posT] = tiles;
                    }
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


    void Tsto(Dictionary<Complex, char> map, HashSet<Complex> painted) {
        var br = map.Keys.MaxBy(pos => pos.Real + pos.Imaginary);
        var crow = br.Imaginary + 1;
        var ccol = br.Real + 1;
        for (var irow = 0; irow < crow; irow++) {
            for (var icol = 0; icol < ccol; icol++) {
                if (painted.Contains(new Complex(icol, irow))) {
                    Console.Write("!");
                } else {
                    Console.Write(map[new Complex(icol, irow)]);
                }
            }
            Console.WriteLine("");
        }

        // Console.WriteLine("");
        //  for(var irow=0;irow<crow;irow++){
        //     for(var icol=0;icol<ccol;icol++){
        //         Console.Write(map[new Complex(icol, irow)]);
        //     }
        //     Console.WriteLine("");
        // }


        /*
         var steps = 26501365;
        var map = ParseMap(input);
        var s = map.Keys.Where(k => map[k] == 'S').Single();

        var br = map.Keys.MaxBy(pos => pos.Real + pos.Imaginary);
        var size = (int)(br.Real + 1);

        var painted = new HashSet<Complex> { s };
        var pos = new HashSet<Complex> { s };
        var paintedCount = new List<long>{1};

        while (true) {
            pos = Step(map, pos);
            painted.UnionWith(pos);
            if (paintedCount[^1] == painted.Count){
                break;
            }
            paintedCount.Add(painted.Count);

        }
        // Tsto(map, painted);
        var tiles = steps / size;
        var remains = tiles % size;

        var boundary = 0L;
        var interior = 1L;
        for(var i = 0;i<tiles;i++) {
            var step = i + 1;
            interior += boundary;
            boundary += 4;
            // Console.WriteLine("s:" + step + " b:" + boundary + " i:" + interior);
        }
        var s1 = interior * paintedCount[^1] + paintedCount[remains] * boundary;
        var s2 = interior * paintedCount[^1] + paintedCount[remains-1] * boundary;
        // 98630436444205
        // 1238804309550935
        return s1 - s2;
        */
    }


    // var size = (int)(br.Real + 1);

    // var painted = new HashSet<Complex> { s };
    // var pos = new HashSet<Complex> { s };

    // var oddPos = new List<HashSet<Complex>>();
    // //oddPos.Add(pos);

    // while (true) {
    //     pos = Step(map, pos);
    //     oddPos.Add(pos);
    //     pos = Step(map, pos);
    //     oddPos.Add(pos);
    //     if (oddPos.Count > 2 && oddPos[^1].Count == oddPos[^3].Count && oddPos[^1].Intersect(oddPos[^3]).Count() == oddPos[^1].Count) {
    //         Console.WriteLine(true);
    //         break;
    //     }
    // }

    // // 134 lepes utan tele lesz

    // // Tsto(map, painted);
    // var tiles = steps / (size + 2);
    // var remains = tiles % (size + 2);

    // var boundary = 0L;
    // var interior = 1L;
    // for (var i = 0; i < tiles; i++) {
    //     var step = i + 1;
    //     interior += boundary;
    //     boundary += 4;
    //     // Console.WriteLine("s:" + step + " b:" + boundary + " i:" + interior);
    // }
    // return 0;


}