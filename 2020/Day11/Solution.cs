using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day11 {

    [ProblemName("Seating System")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        string Step1(string st) {
            var lines = st.Split("\n");
            var crow = lines.Length;
            var ccol = lines[0].Length;

            var res = "";
            for (var irow = 0; irow < crow; irow++) {
                for (var icol = 0; icol < ccol; icol++) {
                    var n = 0;

                    foreach (var drow in new[] { -1, 0, 1 }) {
                        foreach (var dcol in new[] { -1, 0, 1 }) {
                            if ((drow != 0 || dcol != 0) &&
                                irow + drow >= 0 && irow + drow < crow &&
                                icol + dcol >= 0 && icol + dcol < ccol &&
                                lines[irow + drow][icol + dcol] == '#'
                            ) {
                                n++;
                            }



                        }
                    }
                    res += lines[irow][icol] switch {
                        'L' => n == 0 ? '#' : 'L',
                        '#' => n >= 4 ? 'L' : '#',
                        var ch => ch
                    };
                }
                if (irow < crow - 1)
                    res += "\n";
            }
            return res;
        }

        string Step2(string st) {
            var lines = st.Split("\n");
            var crow = lines.Length;
            var ccol = lines[0].Length;

            var res = "";
            for (var irow = 0; irow < crow; irow++) {
                for (var icol = 0; icol < ccol; icol++) {
                    var n = 0;

                    foreach (var drow in new[] { -1, 0, 1 }) {
                        foreach (var dcol in new[] { -1, 0, 1 }) {
                            if ((drow != 0 || dcol != 0)){
                                var (irowT, icolT) = (irow+drow, icol+dcol);
                                while( 
                                    irowT >= 0 && irowT < crow &&
                                    icolT >= 0 && icolT < ccol
                                ){
                                    if(lines[irowT][icolT] == 'L'){
                                        break;
                                    } else if(lines[irowT][icolT] == '#'){
                                        n++;
                                        break;
                                    }
                                    (irowT, icolT) = (irowT+drow, icolT+dcol);
                                }
                            }
                        }
                    }
                    res += lines[irow][icol] switch {
                        'L' => n == 0 ? '#' : 'L',
                        '#' => n >= 5 ? 'L' : '#',
                        var ch => ch
                    };
                }
                if (irow < crow - 1)
                    res += "\n";
            }
            return res;
        }

        int PartOne(string input) {
            var (prev, cur) = (input, input.Replace('L', '#'));
            while (prev != cur) {
                (prev, cur) = (cur, Step1(cur));
            }
            return prev.Count(ch => ch == '#');
        }

        int PartTwo(string input) {
            var (prev, cur) = (input, input.Replace('L', '#'));
            while (prev != cur) {
                (prev, cur) = (cur, Step2(cur));
            }
            return prev.Count(ch => ch == '#');
        }
    }
}