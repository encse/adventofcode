using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2023.Day14;

[ProblemName("Parabolic Reflector Dish")]
class Solution : Solver {

    public object PartOne(string input) {
        var map = ParseMap(input);
        return Measure(Tilt(map));
    }

    public object PartTwo(string input) {
        var map = ParseMap(input);
        var seen = new List<string>();

        var rotations = 1_000_000_000;
        while (!seen.Contains(Tsto(map))) {
            seen.Add(Tsto(map));
            map = Cycle(map);
            rotations--;
        }

        var loop = seen.Count - seen.IndexOf(Tsto(map));
        while (rotations > loop) {
            rotations -= loop;
        }        

        for(var i =0;i<rotations;i++) {
            map = Cycle(map);
        }
        return Measure(map);
    }

    char[][] Cycle(char[][] map) {
        for(var i =0;i<4;i++) {
            map = Rotate(Tilt(map));
        }
        return map;
    }

    string Tsto(char[][] map) {
        return string.Join("\n", map.Select(line => string.Join("", line)));
    }

    char[][] ParseMap(string input) {
        return input.Split('\n').Select(line => line.ToCharArray()).ToArray();
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
                res[irowDst][icolDst] = map[crowDst - icolDst - 1][irowDst];
            }
        }
        return res;
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
}
