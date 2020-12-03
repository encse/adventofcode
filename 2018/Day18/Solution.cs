using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2018.Day18 {

    [ProblemName("Settlers of The North Pole")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Iterate(input, 10);
        int PartTwo(string input) => Iterate(input, 1000000000);

        int Iterate(string input, int lim) {
            var seen = new Dictionary<string, int>();
            var mtx = input.Split("\n");
            
            for (var t = 0; t < lim; t++) {
                var hash = string.Join("", mtx);
                if (seen.ContainsKey(hash)) {
                    var loopLength = t - seen[hash];
                    var remainingSteps = lim - t - 1;
                    var remainingLoops = remainingSteps / loopLength;
                    t += remainingLoops * loopLength;
                } else {
                    seen[hash] = t;
                }
                mtx = Step(mtx);
            }
            var res = string.Join("", mtx);
            return Regex.Matches(res, @"\#").Count * Regex.Matches(res, @"\|").Count;
        }

        string[] Step(string[] mtx) {
            var res = new List<string>();
            var crow = mtx.Length;
            var ccol = mtx[0].Length;
           
            for (var irow = 0; irow < crow; irow++) {
                var line = "";
                for (var icol = 0; icol < ccol; icol++) {
                    var (tree, lumberyard, empty) = (0, 0, 0);
                    foreach (var drow in new[] { -1, 0, 1 }) {
                        foreach (var dcol in new[] { -1, 0, 1 }) {
                            if (drow != 0 || dcol != 0) {
                                var (icolT, irowT) = (icol + dcol, irow + drow);
                                if (icolT >= 0 && icolT < ccol && irowT >= 0 && irowT < crow) {
                                    switch (mtx[irowT][icolT]) {
                                        case '#': lumberyard++; break;
                                        case '|': tree++; break;
                                        case '.': empty++; break;
                                    }
                                }
                            }
                        }
                    }

                    line += mtx[irow][icol] switch {
                        '#' when lumberyard >= 1 && tree >= 1 => '#',
                        '|' when lumberyard >= 3 => '#',
                        '.' when tree >= 3 => '|',
                        '#' => '.',
                        var c => c
                    };
                }
                res.Add(line);
            }
            return res.ToArray();
        }
    }
}