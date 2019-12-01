using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2016.Day08 {

    class Solution : Solver {

        public string GetName() => "Two-Factor Authentication";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var mtx = Execute(input);
            return (
                from irow in Enumerable.Range(0, mtx.GetLength(0))
                from icol in Enumerable.Range(0, mtx.GetLength(1))
                where mtx[irow, icol]
                select 1
            ).Count();
        }

        string PartTwo(string input) {
            var mtx = Execute(input);
            var map = new Dictionary<int, char>() {
                [0x19297A52] = 'A', [0x392E4A5C] = 'B', [0x1928424C] = 'C', [0x39294A5C] = 'D', [0x3D0E421E] = 'E',
                [0x3D0E4210] = 'F', [0x19285A4E] = 'G', [0x252F4A52] = 'H', [0x1C42108E] = 'I', [0x0C210A4C] = 'J',
                [0x254C5292] = 'K', [0x2108421E] = 'L', [0x19294A4C] = 'O', [0x39297210] = 'P', [0x39297292] = 'R',
                [0x1D08305C] = 'S', [0x1C421084] = 'T', [0x25294A4C] = 'U', [0x23151084] = 'Y', [0x3C22221E] = 'Z'
            };
            var res = "";
            for (int i = 0; i < 46; i += 5) {
                var hash = 0;
                for (var irow = 0; irow < 6; irow++) {
                    for (var icol = 0; icol < 5; icol++) {
                        hash <<= 1;
                        hash += mtx[irow, i + icol] ? 1 : 0;
                    }
                }
                res += map[hash];
            }
            return res;
        }


        bool[,] Execute(string input) {
            var (crow, ccol) = (6, 50);
            var mtx = new bool[crow, ccol];
            foreach (var line in input.Split('\n')) {
                if (Match(line, @"rect (\d+)x(\d+)", out var m)) {
                    var (ccolT, crowT) = (int.Parse(m[0]), int.Parse(m[1]));
                    for (var irow = 0; irow < crowT; irow++) {
                        for (var icol = 0; icol < ccolT; icol++) {
                            mtx[irow, icol] = true;
                        }
                    }
                } else if (Match(line, @"rotate row y=(\d+) by (\d+)", out m)) {
                    var (irow, d) = (int.Parse(m[0]), int.Parse(m[1]));
                    for (int i = 0; i < d; i++) {
                        var t = mtx[irow, ccol - 1];
                        for (var icol = ccol - 1; icol >= 1; icol--) {
                            mtx[irow, icol] = mtx[irow, icol - 1];
                        }
                        mtx[irow, 0] = t;
                    }
                } else if (Match(line, @"rotate column x=(\d+) by (\d+)", out m)) {
                    var (icol, d) = (int.Parse(m[0]), int.Parse(m[1]));
                    for (int i = 0; i < d; i++) {
                        var t = mtx[crow - 1, icol];
                        for (var irow = crow - 1; irow >= 1; irow--) {
                            mtx[irow, icol] = mtx[irow - 1, icol];
                        }
                        mtx[0, icol] = t;
                    }
                }
            }
            return mtx;
        }

        bool Match(string stm, string pattern, out string[] m) {
            var match = Regex.Match(stm, pattern);
            m = null;
            if (match.Success) {
                m = match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray();
                return true;
            } else {
                return false;
            }
        }
    }
}