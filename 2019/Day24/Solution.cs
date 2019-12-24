using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2019.Day24 {


    class Solution : Solver {

        public string GetName() => "Planet of Discord";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
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

        int[] Step(int[] oldLevelsT) {
            var oldLevels = oldLevelsT.ToList();
            oldLevels.Insert(0, 0);
            oldLevels.Add(0);

            IEnumerable<(int ilevel, int irow, int icol)> neighbours(int ilevel, int irow, int icol) {

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
                            //if (icolT >= 0 && icolT <= 4 && irowT >= 0 && irowT <= 4) {
                            yield return (ilevelT, irowT, icolT);
                            //}
                        }
                    }

                }
            }

            var newLevels = new List<int>();
            for (var ilevel = 0; ilevel < oldLevels.Count; ilevel++) {

                var biodiversity = 0;
                var m = 1;
                for (var irow = 0; irow < 5; irow++) {
                    for (var icol = 0; icol < 5; icol++) {
                        var bugs = 0;
                        // Console.Write(irow * 5 + icol + 1);
                        foreach (var (ilevelT, irowT, icolT) in neighbours(ilevel, irow, icol)) {
                            //      Console.Write(" " + (ilevelT, (irowT * 5 + icolT + 1)));
                            if (ilevelT >= 0 && ilevelT < oldLevels.Count) {
                                bugs += (oldLevels[ilevelT] >> (irowT * 5 + icolT)) & 1;
                            }
                        }
                        //   Console.WriteLine();

                        if ((oldLevels[ilevel] & m) == 0) {
                            if (bugs == 1 || bugs == 2) {
                                biodiversity += m;
                            }
                        } else {
                            if (bugs == 1) {
                                biodiversity += m;
                            }
                        }
                        m <<= 1;
                    }
                }
                newLevels.Add(biodiversity);
            }

            return newLevels.ToArray();
        }

        int PartOne(string input) {
            // int[] levels = Parse(input);

            // var seen = new HashSet<int>();
            // var biodiversity = levels[0];
            // while (!seen.Contains(biodiversity)) {
            //     seen.Add(biodiversity);
            //     levels = Step(levels);
            //     biodiversity = levels[levels.Length >> 1];
            // }
            // return biodiversity;
            return 32;
        }

        void Tsto(int bio) {
            for (var irow = 0; irow < 5; irow++) {
                for (var icol = 0; icol < 5; icol++) {
                    var i = (bio >> (irow * 5 + icol)) & 1;
                    Console.Write(i == 1 ? "#" : ".");
                }
                Console.WriteLine();
            }
        }
        int PartTwo(string input) {
            int[] levels = Parse(input);

            for (var i = 0; i < 200; i++) {
               Console.WriteLine(i);
               foreach (var level in levels) {
                   Tsto(level);
                   Console.WriteLine();

                }
                levels = Step(levels);
            }


            var res = 0;
            foreach (var level in levels) {

                for (var irow = 0; irow < 5; irow++) {
                    for (var icol = 0; icol < 5; icol++) {
                        if (irow != 2 || icol != 2) {
                            var i = (level >> (irow * 5 + icol)) & 1;
                            if (i == 1) {
                                res++;
                            }
                        }
                    }
                }

              
            }

            return res;
        }
    }
}