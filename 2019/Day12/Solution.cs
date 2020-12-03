using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2019.Day12 {

    [ProblemName("The N-Body Problem")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => (
                from planet in Simulate(input).ElementAt(999)
                let pot = planet.pos.Select(Math.Abs).Sum()
                let kin = planet.vel.Select(Math.Abs).Sum()
                select pot * kin
            ).Sum();

        long PartTwo(string input) {
            var statesByDim = new long[3];
            for (var dim = 0; dim < 3; dim++) {
                var states = new HashSet<(int,int,int,int,int,int,int,int)>();
                foreach (var planets in Simulate(input)) {
                    var state = (planets[0].pos[dim], planets[1].pos[dim], planets[2].pos[dim], planets[3].pos[dim],
                                 planets[0].vel[dim], planets[1].vel[dim], planets[2].vel[dim], planets[3].vel[dim]);
                    if (states.Contains(state)) {
                        break;
                    }
                    states.Add(state);
                }
                statesByDim[dim] = states.Count;
            }

            return Lcm(statesByDim[0], Lcm(statesByDim[1], statesByDim[2]));
        }

        long Lcm(long a, long b) => a * b / Gcd(a, b);
        long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);

        IEnumerable<(int[] pos, int[] vel)[]> Simulate(string input) {
            var planets = (
                from line in input.Split("\n")
                let m = Regex.Matches(line, @"-?\d+")
                let pos = (from v in m select int.Parse(v.Value)).ToArray()
                let vel = new int[3]
                select (pos, vel)
            ).ToArray();

            while (true) {
                foreach (var planetA in planets) {
                    foreach (var planetB in planets) {
                        for (var dim = 0; dim < 3; dim++) {
                            planetA.vel[dim] += Math.Sign(planetB.pos[dim] - planetA.pos[dim]);
                        }
                    }
                }

                foreach (var planet in planets) {
                    for (var dim = 0; dim < 3; dim++) {
                        planet.pos[dim] += planet.vel[dim];
                    }
                }

                yield return planets;
            }
        }
    }
}