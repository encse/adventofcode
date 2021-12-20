using System;
using System.Collections.Generic;
using System.Linq;

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
                var maybeLocatedScanner = TryToLocate(scannerA, scannerB);
                if (maybeLocatedScanner != null) {

                    locatedScanners.Add(maybeLocatedScanner);
                    q.Enqueue(maybeLocatedScanner);

                    scanners.Remove(scannerB); // sic! 
                }
            }
        }

        return locatedScanners;
    }
    Scanner TryToLocate(Scanner scannerA, Scanner scannerB) {
        var beaconsInA = scannerA.GetBeaconsInWorld().ToArray();

        foreach (var (beaconInA, beaconInB) in PotentialBeaconPairs(scannerA, scannerB)) {
            // now try to find the orientation for B:
            var rotatedB = scannerB;
            for (var rotation = 0; rotation < 24; rotation++, rotatedB = rotatedB.Rotate()) {
                // Moving the rotated scanner so that beaconA and beaconB overlaps. Are there 12 matches? 
                var beaconInRotatedB = rotatedB.Transform(beaconInB);

                var locatedB = rotatedB.Translate(new Coord(
                    beaconInA.x - beaconInRotatedB.x,
                    beaconInA.y - beaconInRotatedB.y,
                    beaconInA.z - beaconInRotatedB.z
                ));

                if (locatedB.GetBeaconsInWorld().Intersect(beaconsInA).Count() >= 12) {
                    return locatedB;
                }
            }
        }

        // no luck
        return null;
    }

    IEnumerable<(Coord beaconInA, Coord beaconInB)> PotentialBeaconPairs(Scanner scannerA, Scanner scannerB) {
        // If we had a matching beaconInA and beaconInB and moved the center
        // of the scanners to these then we would find at least 12 beacons with
        // the same coordinates in each.

        // The only problem is that the rotation of scannerB is not fixed yet.

        // But we could form a sets from each scanner taking the absolute values of the x y and z 
        // coordinates of their beacons and compare those. 
        // This metric is invariant under the rotation so if we have a matching beacon pair, 
        // the two sets should have at least 3 * 12 common values (with multiplicity).

        IEnumerable<int> diffs(Scanner scanner) =>
            from coord in scanner.GetBeaconsInWorld()
            from v in new[] { coord.x, coord.y, coord.z }
            select Math.Abs(v);

        // 🐦 We can also considerably speed up the search with the pigeonhole principle 
        // which says that it's enough to take all but 11 beacons from A and B. 
        // If there is no match amongst those, there cannot be 12 matching pairs:
        IEnumerable<T> pick<T>(IEnumerable<T> ts) => ts.Take(ts.Count() - 11);

        foreach (var beaconInA in pick(scannerA.GetBeaconsInWorld())) {
            var diffsA = diffs(scannerA.Translate(new Coord(-beaconInA.x, -beaconInA.y, -beaconInA.z))).ToHashSet();

            foreach (var beaconInB in pick(scannerB.GetBeaconsInWorld())) {

                var diffsB = diffs(scannerB.Translate(new Coord(-beaconInB.x, -beaconInB.y, -beaconInB.z)));
                if (diffsB.Count(d => diffsA.Contains(d)) >= 3 * 12) {
                    yield return (beaconInA, beaconInB);
                }
            }
        }
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
    public Scanner Translate(Coord t) => new Scanner(
        new Coord(center.x + t.x, center.y + t.y, center.z + t.z), rotation, beaconsInLocal);

    public Coord Transform(Coord coord) {
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

        return new Coord(center.x + x, center.y + y, center.z + z);
    }

    public IEnumerable<Coord> GetBeaconsInWorld() {
        return beaconsInLocal.Select(Transform);
    }
}
