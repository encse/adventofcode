using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2016.Day24 {

    class Solution : Solver {

        public string GetName() => "Air Duct Spelunking";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Routes(input, false).Min();

        int PartTwo(string input) => Routes(input, true).Min();

        IEnumerable<int> Routes(string input, bool loop) {
            var map = new Map(input);

            foreach (var permT in Permutations(Enumerable.Range(1, map.poi.Length - 1).ToArray())) {

                var perm = permT.Insert(0, 0);
                if (loop) {
                    perm = perm.Add(0);
                }
                var l = 0;
                for (int i = 0; i < perm.Count - 1; i++) {
                    l += map.ShortestPathLength(map.poi[perm[i]], map.poi[perm[i + 1]]);
                }
                yield return l;
            }
        }

        IEnumerable<ImmutableList<T>> Permutations<T>(IList<T> st) {

            IEnumerable<ImmutableList<T>> PermutationsRec(ImmutableList<T> prefix, bool[] fseen) {
                if (prefix.Count == st.Count()) {
                    yield return prefix;
                } else {
                    for (int i = 0; i < st.Count(); i++) {
                        if (!fseen[i]) {
                            fseen[i] = true;
                            var prefixT = prefix.Add(st[i]);
                            foreach (var res in PermutationsRec(prefixT, fseen)) {
                                yield return res;
                            }
                            fseen[i] = false;
                        }
                    }
                }
            }

            return PermutationsRec(ImmutableList<T>.Empty, new bool[st.Count()]);
        }

        class Map {

            string[] map;
            public int crow;
            public int ccol;
            public (int irow, int icol)[] poi;
            private Dictionary<(int, int, int, int), int> cache = new Dictionary<(int, int, int, int), int>();

            public Map(string input) {
                this.map = input.Split('\n');
                this.crow = map.Length;
                this.ccol = map[0].Length;

                poi = new(int irow, int icol)[10];
                var poiCount = 0;
                for (var irow = 0; irow < crow; irow++) {
                    for (var icol = 0; icol < ccol; icol++) {
                        if (int.TryParse($"{map[irow][icol]}", out var i)) {
                            poi[i] = (irow, icol);
                            poiCount++;
                        }
                    }
                }
                poi = poi.Take(poiCount).ToArray();
            }

            public int ShortestPathLength((int irow, int icol) from, (int irow, int icol) to) {
                var key = (from.irow, from.icol, to.irow, to.icol);
                if (!cache.ContainsKey(key)) {
                    var q = new Queue<(int steps, int irow, int icol)>();
                    q.Enqueue((0, from.irow, from.icol));
                    var seen = new HashSet<(int, int)>();
                    seen.Add(from);
                    while (q.Any()) {
                        var p = q.Dequeue();
                        if (p.irow == to.irow && p.icol == to.icol) {
                            cache[key] = p.steps;
                            break;
                        }
                        foreach (var (drow, dcol) in new[] { (-1, 0), (1, 0), (0, 1), (0, -1) }) {
                            var (irowT, icolT) = (p.irow + drow, p.icol + dcol);
                            if (irowT >= 0 && irowT < crow &&
                                icolT >= 0 && icolT < ccol &&
                                map[irowT][icolT] != '#' &&
                                !seen.Contains((irowT, icolT))
                            ) {
                                q.Enqueue((p.steps + 1, irowT, icolT));
                                seen.Add((irowT, icolT));
                            }
                        }
                    }
                }

                return cache[key];
            }
        }
    }
}