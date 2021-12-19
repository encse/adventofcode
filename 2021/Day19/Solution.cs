using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day19;

[ProblemName("Beacon Scanner")]
class Solution : Solver {

    public object PartOne(string input) {
        return GetCoords(input).beacons.Count;
    }

    public object PartTwo(string input) {
        var scanners = GetCoords(input).scanners;
        return (
            from sA in scanners
            from sB in scanners
            where sA != sB
            select Math.Abs(sA.x - sB.x) + Math.Abs(sA.y - sB.y) + Math.Abs(sA.z - sB.z)
        ).Max();
    }

    (HashSet<Coord> beacons, HashSet<Coord> scanners) GetCoords(string input) {
        var scanners = new List<Scanner>(Parse(input));
        var fixedScanners = new Dictionary<Coord, Scanner>();

        fixedScanners[new Coord(0, 0, 0)] = scanners[0];
        scanners.RemoveAt(0);

        while (scanners.Any()) {
            var (posB, scannerB) = GetCenter(fixedScanners, scanners);
            fixedScanners[posB] = scannerB;
            Console.WriteLine("found " + scanners.Count);
        }

        var beacons = new HashSet<Coord>();
        foreach (var (pos, scanner) in fixedScanners) {
            foreach (var c in scanner.GetBeaconsRelativeTo(pos)) {
                beacons.Add(c);
            }
        }
        return (beacons, fixedScanners.Keys.ToHashSet());
    }

    (Coord, Scanner) GetCenter(Dictionary<Coord, Scanner> fixedScanners, List<Scanner> scanners) {
        for (var i = 0; i < scanners.Count; i++) {
            var scannerB = scanners[i];
            foreach (var (posA, scannerA) in fixedScanners) {
                var ptAs = scannerA.GetBeaconsRelativeTo(posA).ToHashSet();
                foreach (var ptA in ptAs) {
                    for (var rotation = 0; rotation < 24; rotation++, scannerB = scannerB.Rotate()) {
                        foreach (var ptB in scannerB.GetBeaconsRelativeTo(new Coord(0, 0, 0))) {

                            var center = new Coord(ptA.x - ptB.x, ptA.y - ptB.y, ptA.z - ptB.z);
                            var ptBs = scannerB.GetBeaconsRelativeTo(center).ToHashSet();

                            var c = ptAs.Intersect(ptBs).Count();

                            if (c >= 12) {
                                scanners.RemoveAt(i);
                                return (center, scannerB);
                            }
                        }
                    }
                }
            }
        }
        Console.WriteLine("---");
        throw new Exception();
    }

    Scanner[] Parse(string input) => (
        from block in input.Split("\n\n")
        let beacons =
            from line in block.Split("\n").Skip(1)
            let parts = line.Split(",").Select(int.Parse).ToArray()
            select new Coord(parts[0], parts[1], parts[2])
        select new Scanner(0, beacons.ToList())
    ).ToArray();
}

record Coord(int x, int y, int z);
record Scanner(int rotation, List<Coord> beacons) {
    public Scanner Rotate() => new Scanner(rotation + 1, beacons);
    public Coord[] GetBeaconsRelativeTo(Coord coord) {
        Coord transform(Coord coord) {
            var (x, y, z) = coord;

            #pragma warning disable 1717
            // rotate coordinate system so that x-axis points in the possible 6 directions
            switch (rotation % 6) {
                case 0: (x, y, z) = (  x,  y,  z); break;
                case 1: (x, y, z) = ( -x,  y, -z); break;
                case 2: (x, y, z) = (  y, -x,  z); break;
                case 3: (x, y, z) = ( -y,  x,  z); break;
                case 4: (x, y, z) = (  z,  y, -x); break;
                case 5: (x, y, z) = ( -z,  y,  x); break;
            }

            // rotate around x-axis:
            switch ((rotation / 6) % 4) {
                case  0: (x, y, z) = ( x,  y,  z); break;
                case  1: (x, y, z) = ( x, -z,  y); break;
                case  2: (x, y, z) = ( x, -y, -z); break;
                case  3: (x, y, z) = ( x,  z, -y); break;
            }
            #pragma warning restore

            return new Coord(x, y, z);
        }
        return beacons.Select(beacon => {
            var t = transform(beacon);
            return new Coord(coord.x + t.x, coord.y + t.y, coord.z + t.z);
        }).ToArray();
    }
}
