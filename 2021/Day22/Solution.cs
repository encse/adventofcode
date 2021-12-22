using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Numerics;

namespace AdventOfCode.Y2021.Day22;

[ProblemName("Reactor Reboot")]
class Solution : Solver {

    public object PartOne(string input) => NumberOfActiveCubesInRange(input, 50);
    public object PartTwo(string input) => NumberOfActiveCubesInRange(input, int.MaxValue);

    public BigInteger NumberOfActiveCubesInRange(string input, int size) {
        var cmds = Parse(input);

        // Recursive approach

        // If we can determine the number of active cubes in subregions
        // we can compute the effect of the i-th cmd as well.

        // Specifically we are interested how things looked like before the i-th cmd.
        // We need the state of the whole region and the intersection with the region
        // affected by the i-th cmd.

        long activeCubesAfterCmd(int icmd, Region region) {

            // empty is empty...
            if (region.IsEmpty()) {
                return 0;
            }

            var cmd = cmds[icmd];
            if (icmd == 0) {
                // this is also simple, either everything is on or off:
                if (cmd.on) {
                    return cmd.region.Intersect(region).Volume();
                } else {
                    return 0L;
                }
            } else {
                // now the interesting part:
                if (cmd.on) {
                    var v1 = activeCubesAfterCmd(icmd - 1, region); // before icmd
                    var v2 = cmd.region.Intersect(region).Volume(); // icmd would turn on these
                    var v3 = activeCubesAfterCmd(icmd - 1, cmd.region.Intersect(region)); // but these are already on
                    return v1 + v2 - v3;
                } else {
                    var v1 = activeCubesAfterCmd(icmd - 1, region); // before icmd
                    var v2 = activeCubesAfterCmd(icmd - 1, cmd.region.Intersect(region)); // but icmd turns off these
                    return v1 - v2;
                }
            }
        }

        return activeCubesAfterCmd(
            cmds.Length - 1,
            new Region(
                new Coord(-size, -size, -size),
                new Coord(size, size, size)));
    }

    Cmd[] Parse(string input) {
        var res = new List<Cmd>();
        foreach (var line in input.Split("\n")) {
            var on = line.StartsWith("on");
            // get all the numbers with a regexp:
            var m = Regex.Matches(line, "-?[0-9]+").Select(m => int.Parse(m.Value)).ToArray();
            res.Add(new Cmd(on, new Region(new Coord(m[0], m[2], m[4]), new Coord(m[1], m[3], m[5]))));
        }
        return res.ToArray();
    }
}

record Cmd(bool on, Region region);

record Coord(int x, int y, int z);

record Region(Coord from, Coord to) {
    public bool IsEmpty() {
        return this.from.x > this.to.x || this.from.y > this.to.y || this.from.z > this.to.z;
    }

    public long Volume() {
        return IsEmpty() ? 0 : 1L *
            (this.to.x - this.from.x + 1) *
            (this.to.y - this.from.y + 1) *
            (this.to.z - this.from.z + 1);
    }

    public Region Intersect(Region that) {

        // [a..b] and [c..d] are two sections [from..to] is the intersection, 
        // negative length sections are empty
        (int from, int to) sectionIntersection(int a, int b, int c, int d) {
            if (a > c) { 
                return sectionIntersection(c, d, a, b); // switch order
            } else if (b < c) {
                return (c, c - 1);
            } else {
                return (c, Math.Min(d, b));
            }
        }

        var x = sectionIntersection(this.from.x, this.to.x, that.from.x, that.to.x);
        var y = sectionIntersection(this.from.y, this.to.y, that.from.y, that.to.y);
        var z = sectionIntersection(this.from.z, this.to.z, that.from.z, that.to.z);

        return new Region(new Coord(x.from, y.from, z.from), new Coord(x.to, y.to, z.to));
    }
}
