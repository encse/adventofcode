using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day14;

[ProblemName("Parabolic Reflector Dish")]
class Solution : Solver {

    public object PartOne(string input) => Measure(Tilt(Parse(input)));
    public object PartTwo(string input) => Measure(Loop(Parse(input), 1_000_000_000));

    char[][] Parse(string input) {
        return input.Split('\n').Select(line => line.ToCharArray()).ToArray();
    }

    char[][] Loop(char[][] map, long count) {
        var seen = new List<string>();
        while (!seen.Contains(ToString(map)) && count > 0) {
            seen.Add(ToString(map));
            map = Cycle(map);
            count--;
        }

        var loop = seen.Count - seen.IndexOf(ToString(map));
        count %= loop;

        while (count > 0) {
            map = Cycle(map);
            count--;
        }
        return map;
    }

    char[][] Cycle(char[][] map) {
        for (var i = 0; i < 4; i++) {
            map = Rotate(Tilt(map));
        }
        return map;
    }

    char[][] Tilt(char[][] map) {
        var crow = map.Length;
        var ccol = map[0].Length;
        var f = true;
        while (f) {
            f = false;
            for (var irow = 0; irow < crow - 1; irow++) {
                for (var icol = 0; icol < ccol; icol++) {
                    if (map[irow][icol] == '.' && map[irow + 1][icol] == 'O') {
                        map[irow][icol] = 'O';
                        map[irow + 1][icol] = '.';
                        f = true;
                    }
                }
            }
        }
        return map;
    }

    char[][] Rotate(char[][] map) {
        var crowSrc = map.Length;
        var ccolSrc = map[0].Length;
        var crowDst = ccolSrc;
        var ccolDst = crowSrc;
        var res = new char[ccolSrc][];
        for (var irowDst = 0; irowDst < crowDst; irowDst++) {
            res[irowDst] = new char[crowDst];
            for (var icolDst = 0; icolDst < ccolDst; icolDst++) {
                res[irowDst][icolDst] = map[ccolDst - icolDst - 1][irowDst];
            }
        }
        return res;
    }

    int Measure(char[][] map) {
        var crow = map.Length;
        var ccol = map[0].Length;
        var w = 0;
        for (var irow = 0; irow < crow; irow++) {
            for (var icol = 0; icol < ccol; icol++) {
                if (map[irow][icol] == 'O') {
                    w += crow - irow;
                }
            }
        }
        return w;
    }

    string ToString(char[][] map) {
        return string.Join("\n", map.Select(line => string.Join("", line)));
    }
}
