using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day24 {

    class Solution : Solver {

        public string GetName() => "Planet of Discord";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            int[] levels = Parse(input);

            var seen = new HashSet<int>();
            var biodiversity = levels[0];
            while (!seen.Contains(biodiversity)) {
                seen.Add(biodiversity);
                levels = Step(levels, FlatNeighbours);
                biodiversity = levels[levels.Length >> 1];
            }
            return biodiversity;
        }

        int PartTwo(string input) {
            int[] levels = Parse(input);

            for (var i = 0; i < 200; i++) {
                levels = Step(levels, RecursiveNeighbours);
            }

            return (
                from level in levels 
                from pos in Positions() 
                where pos != (2,2) && HasBug(level, pos.irow, pos.icol) 
                select 1
            ).Count();
        }

        int[] Parse(string input) {
            var biodiversity = 0;
            var m = 1;
            foreach (var ch in input.Replace("\n", "")) {
                if (ch == '#') {
                    biodiversity += m;
                }
                m <<= 1;
            }
            return new[] { biodiversity };
        }

        IEnumerable<(int irow, int icol)> Positions() {
            for (var irow = 0; irow < 5; irow++) {
                for (var icol = 0; icol < 5; icol++) {
                    yield return (irow, icol);
                }
            }
        }

        bool HasBug(int biodiversity, int irow, int icol) {
            return ((biodiversity >> (irow * 5 + icol)) & 1) == 1;
        }

        int SetBug(int biodiversity, int irow, int icol) {
            return biodiversity | (1 << (irow * 5 + icol));
        }

        int[] Step(int[] oldLevelsT, Func<(int ilevel, int irow, int icol), IEnumerable<(int ilevel, int irow, int icol)>> neighbours) {
            var oldLevels = oldLevelsT.ToList();
            oldLevels.Insert(0, 0);
            oldLevels.Add(0);

            var newLevels = new List<int>();
            for (var ilevel = 0; ilevel < oldLevels.Count; ilevel++) {

                var newLevel = 0;
                foreach (var (irow, icol) in Positions()) {
                    var bugCount = 0;
                    foreach (var (ilevelT, irowT, icolT) in neighbours((ilevel, irow, icol))) {
                        if (ilevelT >= 0 && ilevelT < oldLevels.Count) {
                            bugCount += HasBug(oldLevels[ilevelT], irowT, icolT) ? 1 : 0;
                        }
                    }

                    if (!HasBug(oldLevels[ilevel], irow, icol)) {
                        if (bugCount == 1 || bugCount == 2) {
                            newLevel = SetBug(newLevel, irow, icol);
                        }
                    } else {
                        if (bugCount == 1) {
                            newLevel = SetBug(newLevel, irow, icol);
                        }
                    }
                }
                newLevels.Add(newLevel);
            }

            return newLevels.ToArray();
        }


        IEnumerable<(int ilevel, int irow, int icol)> FlatNeighbours((int ilevel, int irow, int icol) pos) {
            foreach (var (drow, dcol) in new[] { (0, 1), (0, -1), (-1, 0), (1, 0) }) {
                var (irowT, icolT) = (pos.irow + drow, pos.icol + dcol);
                if (icolT >= 0 && icolT <= 4 && irowT >= 0 && irowT <= 4) {
                    yield return (pos.ilevel, irowT, icolT);
                }
            }
        }

        IEnumerable<(int ilevel, int irow, int icol)> RecursiveNeighbours((int ilevel, int irow, int icol) pos) {
            var (ilevel, irow, icol) = pos;
            foreach (var (drow, dcol) in new[] { (0, 1), (0, -1), (-1, 0), (1, 0) }) {
                var posMin = (irow: irow + drow, icol: icol + dcol);
                var posMax = (irow: irow + drow, icol: icol + dcol);
                var ilevelT = ilevel;

                if (posMin.irow == -1) {
                    ilevelT = ilevel - 1;
                    posMin = posMax = (1, 2);
                } else if (posMin.irow == 5) {
                    ilevelT = ilevel - 1;
                    posMin = posMax = (3, 2);
                } else if (posMin.icol == -1) {
                    ilevelT = ilevel - 1;
                    posMin = posMax = (2, 1);
                } else if (posMin.icol == 5) {
                    ilevelT = ilevel - 1;
                    posMin = posMax = (2, 3);
                } else if (posMin == (2, 2)) {
                    ilevelT = ilevel + 1;
                    if (dcol == 0) {
                        posMin = (drow == 1 ? 0 : 4, 0);
                        posMax = (drow == 1 ? 0 : 4, 4);
                    } else if (drow == 0) {
                        posMin = (0, dcol == 1 ? 0 : 4);
                        posMax = (4, dcol == 1 ? 0 : 4);
                    }
                }

                for (var irowT = posMin.irow; irowT <= posMax.irow; irowT++) {
                    for (var icolT = posMin.icol; icolT <= posMax.icol; icolT++) {
                        yield return (ilevelT, irowT, icolT);
                    }
                }
            }
        }
    }
}