using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

namespace AdventOfCode.Y2018.Day22 {

    class Solution : Solver {

        public string GetName() => "Mode Maze";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var lines = input.Split("\n");
            var depth = Regex.Matches(lines[0], @"\d+").Select(x => int.Parse(x.Value)).Single();
            var target = Regex.Matches(lines[1], @"\d+").Select(x => int.Parse(x.Value)).ToArray();
            var (targetX, targetY) = (target[0], target[1]);
            var m = 20183;

            var geologicIndex = new BigInteger[targetX+1, targetY+1];
            for (var x = 0; x <= targetX; x++) {
                
                geologicIndex[x, 0] = (x * new BigInteger(16807)) ;
            }

            for (var y = 0; y <= targetY; y++) {
                
                geologicIndex[0, y] = (y * new BigInteger(48271)) ;
            }

            for (var y = 1; y <= targetY; y++) {
                for (var x = 1; x <= targetX; x++) {

                   
                    geologicIndex[x, y] = (geologicIndex[x, y - 1] * geologicIndex[x - 1, y]);
                }
            }

            geologicIndex[targetX, targetY] = 0;

            var erosionLevel = new int[targetX+1, targetY+1];
            for (var y = 0; y <= targetY; y++) {
                for (var x = 0; x <= targetX; x++) {
                    erosionLevel[x, y] = (int)(geologicIndex[x, y] + depth) % m;
                }
            }

            var regionType = new int[targetX+1, targetY+1];
            for (var y = 0; y <= targetY; y++) {
                for (var x = 0; x <= targetX; x++) {
                    regionType[x, y] = erosionLevel[x, y] % 3;
                }
            }

            var riskLevel = 0;

            for (var y = 0; y <= targetY; y++) {
                for (var x = 0; x <= targetX; x++) {
                    riskLevel += regionType[x, y];
                }
            }
            return riskLevel;
        }

        int PartTwo(string input) {
            return 0;
        }
    }
}