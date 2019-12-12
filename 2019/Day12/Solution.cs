using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2019.Day12 {

    class Solution : Solver {

        public string GetName() => "The N-Body Problem";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {

            var state = Simulate(input).ElementAt(999);
            return (
                from iplanet in Enumerable.Range(0, state.rgpos.Length)
                let pot = state.rgpos[iplanet].Select(Math.Abs).Sum()
                let kin = state.rgv[iplanet].Select(Math.Abs).Sum()
                select pot * kin
            ).Sum();
        }

        long PartTwo(string input) {
            var loop = new long[3];
            for (var i = 0; i < 3; i++) {
                var seen = new HashSet<(int p1, int p2, int p3, int p4, int vp1, int vp2, int vp3, int vp4)>();
                foreach (var state in Simulate(input)) {
                    var key = (state.rgpos[0][i], state.rgpos[1][i], state.rgpos[2][i], state.rgpos[3][i],
                               state.rgv[0][i], state.rgv[1][i], state.rgv[2][i], state.rgv[3][i]);
                    if (seen.Contains(key)) {
                        break;
                    }
                    seen.Add(key);
                }
                loop[i] = seen.Count;
            }

            return Lcm(loop[0], Lcm(loop[1], loop[2]));
        }


        long Lcm(long a, long b) => a * b / Gcd(a, b);
        long Gcd(long a, long b) {
            while (b != 0) {
                (a, b) = (b, a % b);
            }
            return a;
        }


        IEnumerable<(int[][] rgpos, int[][] rgv)> Simulate(string input) {
            var rgpos = (
                from line in input.Split("\n")
                let m = Regex.Matches(line, @"-?\d+")
                let pos = (from v in m select int.Parse(v.Value)).ToArray()
                select pos
            ).ToArray();

            var rgv = (from pos in rgpos select new int[3]).ToArray();

            while (true) {
                foreach (var iposA in Enumerable.Range(0, rgpos.Length)) {
                    foreach (var iposB in Enumerable.Range(0, rgpos.Length)) {
                        var (posA, vA) = (rgpos[iposA], rgv[iposA]);
                        var (posB, vB) = (rgpos[iposB], rgv[iposB]);
                        for (var i = 0; i < 3; i++) {
                            vA[i] += Math.Sign(posB[i] - posA[i]);
                        }
                    }
                }

                foreach (var iposA in Enumerable.Range(0, rgpos.Length)) {
                    var pos = rgpos[iposA];
                    var v = rgv[iposA];
                    for (var i = 0; i < 3; i++) {
                        pos[i] += v[i];
                    }
                }

                yield return (rgpos, rgv);
            }
        }
    }
}