using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2018.Day25 {

    [ProblemName("Four-Dimensional Adventure")]
    class Solution : Solver {

        public object PartOne(string input) {
            var sets = new List<HashSet<int[]>>();

            foreach (var line in input.Split("\n")) {
                var set = new HashSet<int[]>();
                set.Add(line.Split(",").Select(int.Parse).ToArray());
                sets.Add(set);
            }

            foreach (var set in sets.ToList()) {
                var pt = set.Single();
                var closeSets = new List<HashSet<int[]>>();
                foreach (var setB in sets) {
                    foreach (var ptB in setB) {
                        if (Dist(pt, ptB) <= 3) {
                            closeSets.Add(setB);
                        }
                    }
                }
                var mergedSet = new HashSet<int[]>();
                foreach (var setB in closeSets) {
                    foreach (var ptB in setB) {
                        mergedSet.Add(ptB);
                    }
                    sets.Remove(setB);
                }
                sets.Add(mergedSet);
            }

            return sets.Count;
        }

        public object PartTwo(string input) => null;
       
        int Dist(int[] a, int[] b) => Enumerable.Range(0, a.Length).Select(i => Math.Abs(a[i] - b[i])).Sum();

    }
}