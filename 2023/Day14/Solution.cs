using System;
using System.Collections.Generic;
using System.Linq;
using Map = char[][];

namespace AdventOfCode.Y2023.Day14;

[ProblemName("Parabolic Reflector Dish")]
class Solution : Solver {

    public object PartOne(string input) => Measure(Tilt(Parse(input)));
    public object PartTwo(string input) => Measure(Iterate(Parse(input), Cycle, 1_000_000_000));

    Map Parse(string input) =>
        (from line in input.Split('\n') select line.ToCharArray()).ToArray();

    int Crow(char[][] map) => map.Length;
    int Ccol(char[][] map) => map[0].Length;

    Map Iterate(Map map, Func<Map, Map> cycle, long count) {
        // The usual trick: keep iterating until we find a loop, make a shortcut
        // and finish with the remaining elements. 
        var seen = new List<string>();
        while (count > 0) {
            map = cycle(map);
            count--;

            var hash = string.Join("", from line in map from ch in line select ch);
            if (!seen.Contains(hash)) {
                seen.Add(hash);
            } else {
                count %= seen.Count - seen.IndexOf(hash);
                break;
            }
        }

        for (; count > 0; count--) {
            map = cycle(map);
        }
        return map;
    }

    Map Cycle(Map map) {
        for (var i = 0; i < 4; i++) {
            map = Rotate(Tilt(map));
        }
        return map;
    }

    // Tilt the map to the North, so that the 'O' tiles roll to the top.
    Map Tilt(Map map) {
        for (var icol = 0; icol < Ccol(map); icol++) {
            bool colFinished = false;
            while (!colFinished) {
                colFinished = true;
                for (var irow = 0; irow < Crow(map) - 1; irow++) {
                    if (map[irow][icol] == '.' && map[irow + 1][icol] == 'O') {
                        map[irow][icol] = 'O';
                        map[irow + 1][icol] = '.';
                        colFinished = false;
                    }
                }
            }
        }
        return map;
    }

    // Ugly coordinate magic, turns the map 90º clockwise
    Map Rotate(Map src) {
        var dst = new char[Crow(src)][];
        for (var irow = 0; irow < Ccol(src); irow++) {
            dst[irow] = new char[Ccol(src)];
            for (var icol = 0; icol < Crow(src); icol++) {
                dst[irow][icol] = src[Crow(src) - icol - 1][irow];
            }
        }
        return dst;
    }

    // returns the cummulated distances of 'O' tiles from the bottom of the map
    int Measure(Map map) => (
        from irow in Enumerable.Range(0, Crow(map))
        from icol in Enumerable.Range(0, Ccol(map))
        where map[irow][icol] == 'O'
        select Crow(map) - irow
    ).Sum();
}