using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day25;

[ProblemName("Sea Cucumber")]
class Solution : Solver {

    public object PartOne(string input) {
        return Run(input.Split('\n')).Count();
        // var map = input.Split('\n');
        // for (var i = 0; i < 58; i++) {
        //     map = Step(map);
        //     Console.WriteLine("\nstep" + (i + 1));
        //     foreach (var line in map) {
        //         Console.WriteLine(line);
        //     }

        // }
        // return 0;
    }

    public object PartTwo(string input) {
        return 0;
    }

    IEnumerable<string[]> Run(string[] map) {
        yield return map;
        var (ccol, crow) = (map[0].Length, map.Length);

        int right(int icol) => (icol + 1) % ccol;
        int left(int icol) => (icol - 1 + ccol) % ccol;
        int up(int irow) => (irow - 1 + crow) % crow;
        int down(int irow) => (irow + 1) % crow;

        bool movesRight(int irow, int icol) =>
            map[irow][icol] == '>' && map[irow][right(icol)] == '.';
        bool movesDown(int irow, int icol) =>
            map[irow][icol] == 'v' && map[down(irow)][icol] == '.';

        while (true) {
            var moved = false;
            foreach (var ch in ">v") {
                var res = new List<string>();
                for (var irow = 0; irow < crow; irow++) {
                    var st = "";
                    for (var icol = 0; icol < ccol; icol++) {
                        if (ch == '>') {
                            moved |= movesRight(irow, icol);
                            st +=
                                movesRight(irow, icol) ? '.' :
                                movesRight(irow, left(icol)) ? '>' :
                                map[irow][icol];
                        } else {
                            moved |= movesDown(irow, icol);
                            st +=
                                movesDown(irow, icol) ? '.' :
                                movesDown(up(irow), icol) ? 'v' :
                                map[irow][icol];
                        }
                    }
                    res.Add(st);
                }
                map = res.ToArray();
            }

            if (!moved) {
                yield break;
            } else {
                yield return map;
            }
        }
    }

    string[] GetMap(string input) {
        return input.Split("\n");
    }
}
