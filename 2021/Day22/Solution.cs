using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2021.Day22;

[ProblemName("Reactor Reboot")]
class Solution : Solver {

    public object PartOne(string input) => NumberOfActiveCubesInRange(input, 50);
    public object PartTwo(string input) => NumberOfActiveCubesInRange(input, int.MaxValue);

    public long NumberOfActiveCubesInRange(string input, int size) {
        var cmds = Parse(input);

        // Recursive approach

        // If we can determine the number of active cubes in subregions
        // we can compute the effect of the i-th cmd as well.

        // Specifically we are interested how things looked like before the i-th cmd.
        // We need the state of the whole region and the intersection with the region
        // affected by the i-th cmd.

        long activeCubesAfterCmd(int icmd, Region region) {

            // empty is empty...
            if (region.IsEmpty) {
                return 0;
            }

            var cmd = cmds[icmd];
            if (icmd == 0) {
                // this is also simple, either everything is on or off:
                if (cmd.on) {
                    return cmd.region.Intersect(region).Volume;
                } else {
                    return 0L;
                }
            } else {
                // now the interesting part:
                if (cmd.on) {
                    var v1 = activeCubesAfterCmd(icmd - 1, region); // before icmd
                    var v2 = cmd.region.Intersect(region).Volume; // icmd would turn on these
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
                new Section(-size, size),
                new Section(-size, size),
                new Section(-size, size)));
    }

    Cmd[] Parse(string input) {
        var res = new List<Cmd>();
        foreach (var line in input.Split("\n")) {
            var on = line.StartsWith("on");
            // get all the numbers with a regexp:
            var m = Regex.Matches(line, "-?[0-9]+").Select(m => int.Parse(m.Value)).ToArray();
            res.Add(new Cmd(on, new Region(new Section(m[0], m[1]), new Section(m[2], m[3]), new Section(m[4], m[5]))));
        }
        return res.ToArray();
    }
}

record Cmd(bool on, Region region);

record Section(int from, int to) {
    public bool IsEmpty => from > to;
    public long Length => IsEmpty ? 0 : to - from + 1;

    public Section Intersect(Section that) => 
        new Section(Math.Max(this.from, that.from), Math.Min(this.to, that.to));
}

record Region(Section x, Section y, Section z) {
    public bool IsEmpty => x.IsEmpty || y.IsEmpty || z.IsEmpty;
    public long Volume => x.Length * y.Length * z.Length;

    public Region Intersect(Region that) =>
        new Region(this.x.Intersect(that.x), this.y.Intersect(that.y), this.z.Intersect(that.z));
}
