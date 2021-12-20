using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day19;

[ProblemName("Beacon Scanner")]
class Solution : Solver {

    public object PartOne(string input) =>
        LocateScanners(input)
            .SelectMany(scanner => scanner.GetBeaconsInWorld())
            .Distinct()
            .Count();

    public object PartTwo(string input) {
        var scanners = LocateScanners(input);
        return (
            from sA in scanners
            from sB in scanners
            where sA != sB
            select
                Math.Abs(sA.center.x - sB.center.x) +
                Math.Abs(sA.center.y - sB.center.y) +
                Math.Abs(sA.center.z - sB.center.z)
        ).Max();
    }

    HashSet<Scanner> LocateScanners(string input) {
        var scanners = new HashSet<Scanner>(Parse(input));
        var locatedScanners = new HashSet<Scanner>();
        var q = new Queue<Scanner>();
        
        // when a scanner is located, it get's into the queue so that we can
        // explore its neighbours.

        locatedScanners.Add(scanners.First());
        q.Enqueue(scanners.First()); 

        scanners.Remove(scanners.First());

        while (q.Any()) {
            var scannerA = q.Dequeue();
            foreach (var scannerB in scanners.ToArray()) {
                var maybeScannerB = TryToLocate(scannerA, scannerB);
                if (maybeScannerB != null) {

                    locatedScanners.Add(maybeScannerB);
                    q.Enqueue(maybeScannerB);

                    scanners.Remove(scannerB); // sic! 
                }
            }
        }

        return locatedScanners;
    }

    Scanner TryToLocate(Scanner scannerA, Scanner scannerB) {
        // We will go oveer all possible rotations for B and make pairs of beacons in A and B. 
        // if they represent the same beacons we should find 12 matching pairs in A and B.

        // 🐦 We can considerably speed up the search with pigeonhole principle which says 
        // that it's enough to take all but 11 beacons from A and B. 
        // If there is no match amongst those, there cannot be 12 matching pairs:
        IEnumerable<T> pick<T>(IEnumerable<T> ts) => ts.Take(ts.Count() - 11);

        var beaconsInA = scannerA.GetBeaconsInWorld().ToHashSet();

        foreach (var beaconInA in pick(beaconsInA)) {

            // considering all possible rotations:
            for (var rotation = 0; rotation < 24; rotation++, scannerB = scannerB.Rotate()) {

                // take the beacons visible by B, and try to match them with beaconInA
                foreach (var beaconInB in pick(scannerB.GetBeaconsInWorld())) {

                    // Moving scannerB so that A and B overlaps. Are there 12 matches? 

                    var center = new Coord(
                        beaconInA.x - beaconInB.x,
                        beaconInA.y - beaconInB.y,
                        beaconInA.z - beaconInB.z
                    );

                    if (12 == scannerB.Move(center)
                        .GetBeaconsInWorld()
                        .Where(beaconInB => beaconsInA.Contains(beaconInB))
                        .Take(12)
                        .Count()
                    ) {
                        return scannerB.Move(center);
                    }
                }
            }
        }

        // no luck
        return null;
    }

    Scanner[] Parse(string input) => (
        from block in input.Split("\n\n")
        let beacons =
            from line in block.Split("\n").Skip(1)
            let parts = line.Split(",").Select(int.Parse).ToArray()
            select new Coord(parts[0], parts[1], parts[2])
        select new Scanner(new Coord(0, 0, 0), 0, beacons.ToList())
    ).ToArray();
}

record Coord(int x, int y, int z);
record Scanner(Coord center, int rotation, List<Coord> beaconsInLocal) {
    public Scanner Rotate() => new Scanner(center, rotation + 1, beaconsInLocal);
    public Scanner Move(Coord center) => new Scanner(center, rotation, beaconsInLocal);

    public IEnumerable<Coord> GetBeaconsInWorld() {
        Coord rotate(Coord coord) {
            var (x, y, z) = coord;

#pragma warning disable 1717
            // rotate coordinate system so that x-axis points in the possible 6 directions
            switch (rotation % 6) {
                case 0: (x, y, z) = (x, y, z); break;
                case 1: (x, y, z) = (-x, y, -z); break;
                case 2: (x, y, z) = (y, -x, z); break;
                case 3: (x, y, z) = (-y, x, z); break;
                case 4: (x, y, z) = (z, y, -x); break;
                case 5: (x, y, z) = (-z, y, x); break;
            }

            // rotate around x-axis:
            switch ((rotation / 6) % 4) {
                case 0: (x, y, z) = (x, y, z); break;
                case 1: (x, y, z) = (x, -z, y); break;
                case 2: (x, y, z) = (x, -y, -z); break;
                case 3: (x, y, z) = (x, z, -y); break;
            }
#pragma warning restore

            return new Coord(x, y, z);
        }
        return beaconsInLocal.Select(beacon => {
            var rotated = rotate(beacon);
            return new Coord(center.x + rotated.x, center.y + rotated.y, center.z + rotated.z);
        });
    }
}
