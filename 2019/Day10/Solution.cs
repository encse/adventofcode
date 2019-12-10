using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2019.Day10 {

    class Solution : Solver {

        public string GetName() => "Monitoring Station";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var map = input.Split("\n");
            var (crow, ccol) = (map.Length, map[0].Length);
            var max = 0;
            for (var irow = 0; irow < crow; irow++) {
                for (var icol = 0; icol < ccol; icol++) {
                    if (map[irow][icol] != '#') {
                        continue;
                    }

                    var visible = new HashSet<(int, int)>();

                    for (var irowT = 0; irowT < crow; irowT++) {
                        for (var icolT = 0; icolT < ccol; icolT++) {

                            if (map[irowT][icolT] != '#') {
                                continue;
                            }

                            if (irowT != irow || icolT != icol) {
                                var (dcol, drow) = (icolT - icol, irowT - irow);
                                var gcd = Math.Abs(Gcd(drow, dcol));
                                visible.Add((drow / gcd, dcol / gcd));
                            }
                        }
                    }

                    max = Math.Max(max, visible.Count);

                }
            }
            return max;
        }

        int PartTwo(string input) {
            var map = input.Split("\n");
            var (crow, ccol) = (map.Length, map[0].Length);
            var d = new Dictionary<(int drow, int dcol), List<(int irow, int icol)>>();
            int irowS = 0;
            int icolS = 0;
            for (var irow = 0; irow < crow; irow++) {
                for (var icol = 0; icol < ccol; icol++) {
                    if (map[irow][icol] != '#') {
                        continue;
                    }

                    var visible = new Dictionary<(int x, int y), List<(int irow, int icol)>>();

                    for (var irowT = 0; irowT < crow; irowT++) {
                        for (var icolT = 0; icolT < ccol; icolT++) {

                            if (map[irowT][icolT] != '#') {
                                continue;
                            }

                            if (irowT != irow || icolT != icol) {
                                var (dcol, drow) = (icolT - icol, irowT - irow);
                                var gcd = Math.Abs(Gcd(drow, dcol));

                                var key = (drow / gcd, dcol / gcd);

                                if (!visible.ContainsKey(key)) {
                                    visible[key] = new List<(int irow, int icol)>();
                                }
                                visible[key].Add((irowT, icolT));
                            }
                        }
                    }

                    if (visible.Count > d.Count) {
                        d = visible;
                        (irowS, icolS) = (irow, icol);
                    }

                }
            }

            var keys = d.Keys.OrderBy(key => Math.Atan2(key.drow, key.dcol)).ToArray();
            var s = 0;
            while (keys[s].dcol != 0 || keys[s].drow > 0) {
                s++;
            }
            var b = 0;
            while (true) {
                var key = keys[s % keys.Length];
                if (d[key].Any()) {
                    var (irow, icol) = d[key].OrderBy(a => Math.Abs(a.irow - irowS) + Math.Abs(a.icol - icolS)).First();

                    d[key].Remove((irow, icol));

                    b++;

                    if (b == 200) {
                        return (icol * 100 + irow);
                    }
                }
                s++;

            }
            throw new Exception();
        }

        int Gcd(int a, int b) {
            while (b != 0) {
                (a, b) = (b, a % b);
            }
            return a;
        }
    }
}