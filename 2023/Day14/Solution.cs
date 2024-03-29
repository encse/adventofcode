namespace AdventOfCode.Y2023.Day14;

using System;
using System.Collections.Generic;
using System.Linq;
using Map = char[][];

[ProblemName("Parabolic Reflector Dish")]
class Solution : Solver {

    public object PartOne(string input) => 
        Measure(Tilt(Parse(input)));

    public object PartTwo(string input) => 
        Measure(Iterate(Parse(input), Cycle, 1_000_000_000));

    Map Parse(string input) => (
        from l in input.Split('\n') select l.ToCharArray()
    ).ToArray();

    int Crow(char[][] map) => map.Length;
    int Ccol(char[][] map) => map[0].Length;

    Map Iterate(Map map, Func<Map, Map> cycle, int count) {
        // The usual trick: keep iterating until we find a loop, make a shortcut
        // and read the result from the accumulated history.
        var history = new List<string>();
        while (count > 0) {
            map = cycle(map);
            count--;

            var mapString = string.Join("\n", map.Select(l=> new string(l)));
            var idx = history.IndexOf(mapString);
            if (idx < 0) {
                history.Add(mapString);
            } else {
                var loopLength = history.Count - idx;
                var remainder = count % loopLength; 
                return Parse(history[idx + remainder]);
            }
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
            var irowT = 0; // tells where to roll up the next 'O' tile
            for (var irowS = 0; irowS < Crow(map); irowS++) {
                if (map[irowS][icol] == '#') {
                    irowT = irowS + 1;
                } else if (map[irowS][icol] == 'O') {
                    map[irowS][icol] = '.'; 
                    map[irowT][icol] = 'O'; 
                    irowT++;
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
    int Measure(Map map) =>  
        map.Select((row, irow) => 
            (Crow(map) - irow) * row.Count(ch => ch == 'O')
        ).Sum();
}