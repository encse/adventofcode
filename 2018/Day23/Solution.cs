using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day23 {

    class Solution : Solver {

        public string GetName() => "Experimental Emergency Teleportation";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int Dist((int x, int y, int z) a, (int x, int y, int z) b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);

        int PartOne(string input) {
            var drones = Parse(input);
            var maxRange = drones.Select(drone => drone.r).Max();
            var maxDrone = drones.Single(drone => drone.r == maxRange);
            return drones.Count(drone => Dist(drone.pos, maxDrone.pos) <= maxRange);
        }

        IEnumerable<(int x, int y, int z)> Corners(Drone[] drones) => (
            from drone in drones
            from dx in new[] { -1, 0, 1 }
            from dy in new[] { -1, 0, 1 }
            from dz in new[] { -1, 0, 1 }
            where dx * dx + dy * dy + dz * dz == 1
            select (drone.pos.x + dx * drone.r, drone.pos.y + dy * drone.r, drone.pos.z + dz * drone.r)
        ).ToArray();

        Drone[] Parse(string input) => (
            from line in input.Split("\n")
            let parts = Regex.Matches(line, @"-?\d+").Select(x => int.Parse(x.Value)).ToArray()
            select new Drone { pos = (parts[0], parts[1], parts[2]), r = parts[3] }
        ).ToArray();

        int PartTwo(string input) {
            var drones = Parse(input);
            var corners = Corners(drones);
            var maxDrones = 0;
            var distance = int.MaxValue;
            var pt = (x: 0, y: 0, z: 0);
            foreach (var corner in corners) {
                var dronesInRange = drones.Count(drone => Dist(drone.pos, corner) <= drone.r);
                var distanceFromOrigin = Dist(corner, (0, 0, 0));
                if (dronesInRange > maxDrones) {
                    maxDrones = dronesInRange;
                    distance = distanceFromOrigin;
                    pt = corner;
                } else if (dronesInRange == maxDrones && distanceFromOrigin < distance) {
                    throw new Exception();
                }
            }

            var closestDrones = drones.Where(drone => Dist(drone.pos, pt) <= drone.r).ToArray();
            var f = true;
            while (f) {
                f = false;
                foreach (var s in new[] { 1, -1 }) {
                    foreach (var i in Enumerable.Range(0, 30).Reverse()) {
                        foreach (var dx in new[] { -1, 0, 1 }) {
                            foreach (var dy in new[] { -1, 0, 1 }) {
                                foreach (var dz in new[] { -1, 0, 1 }) {
                                    var step = (int)s * (1<<i);
                                    var ptNext = (pt.x + dx * step, pt.y + dy * step, pt.z + dz * step);
                                    if (Dist(ptNext, (0, 0, 0)) < Dist(pt, (0, 0, 0)) &&
                                        closestDrones.All(drone => Dist(drone.pos, ptNext) <= drone.r)
                                    ) {
                                        pt = ptNext;
                                        f = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Dist(pt, (0, 0, 0));
        }
    }

    class Drone {
        public (int x, int y, int z) pos;
        public int r;
    }
}