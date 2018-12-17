using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day17 {

    class Solution : Solver {

        public string GetName() => "Reservoir Research";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var mtx = new char[2000, 2000];
            var width = mtx.GetLength(0);
            var height = mtx.GetLength(1);

            for(var y = 0;y<height; y++){
                for(var x=0;x<width; x++){
                    mtx[x,y] = '.';
                }
            }


            foreach (var line in input.Split("\n")) {
                var nums = Regex.Matches(line, @"\d+").Select(g => int.Parse(g.Value)).ToArray();
                for (var i = nums[1]; i <= nums[2]; i++) {
                    if (line.StartsWith("x")) {
                        mtx[nums[0], i] = '#';
                    } else {
                        mtx[i, nums[0]] = '#';
                    }
                }
            }
         

            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;
            for(var y = 0;y<height; y++){
                for(var x=0;x<width; x++){
                    if(mtx[x,y] == '#'){
                        minY = Math.Min(minY, y);
                        maxY = Math.Max(maxY, y);

                        minX = Math.Min(minX, x);
                        maxX = Math.Max(maxX, x);
                    }
                }
            }
            Fill(mtx, 500, 0);
            var res = 0;
            for(var y = minY;y<=maxY; y++){
                for(var x=0;x<width; x++){
                    if(mtx[x,y] == '|' || mtx[x,y] == '~'){
                        res++;
                    }
                }
            }

            return res;
        }
        int Fill(char[,] mtx, int x, int y) {
            var width = mtx.GetLength(0);
            var height = mtx.GetLength(1);
            switch (mtx[x, y]) {
                case '#':
                    return 0;
                case '|':
                    return 0;
                case '~':
                    return 0;
                default:
                    mtx[x, y] = '|';
                    var res = 1;
                    if (y < height - 1) {
                        res += Fill(mtx, x, y + 1);
                        
                        if (mtx[x, y + 1] == '#' || mtx[x, y + 1] == '~') {
                            if (x > 0) {
                                res += Fill(mtx, x - 1, y);
                            }
                            if (x < width - 1) {
                                res += Fill(mtx, x + 1, y);
                            }
                        }

                        var still = true;
                        for (var xT = x - 1; still && xT >= 0; xT--) {
                            if (mtx[xT, y] == '.') {
                                still = false;
                            }
                            if(mtx[xT, y+1] == '|'){
                                still = false;
                            }
                            if (mtx[xT, y] == '#') {
                                break;
                            }
                        }

                        for (var xT = x + 1; still && xT < width; xT++) {
                            if (mtx[xT, y] == '.') {
                                still = false;
                            }
                            if(mtx[xT, y+1] == '|'){
                                still = false;
                            }
                            if (mtx[xT, y] == '#') {
                                break;
                            }
                        }

                        if (still) {
                            mtx[x, y] = '~';
                            for (var xT = x - 1; xT >= 0 && mtx[xT, y] == '|'; xT--) {
                                mtx[xT, y] = '~';
                            }

                            for (var xT = x + 1; xT < width && mtx[xT, y] == '|'; xT++) {
                                mtx[xT, y] = '~';
                            }
                        }
                    }
                    return res;
            }
        }
        int PartTwo(string input) {
            var mtx = new char[2000, 2000];
            var width = mtx.GetLength(0);
            var height = mtx.GetLength(1);

            for(var y = 0;y<height; y++){
                for(var x=0;x<width; x++){
                    mtx[x,y] = '.';
                }
            }


            foreach (var line in input.Split("\n")) {
                var nums = Regex.Matches(line, @"\d+").Select(g => int.Parse(g.Value)).ToArray();
                for (var i = nums[1]; i <= nums[2]; i++) {
                    if (line.StartsWith("x")) {
                        mtx[nums[0], i] = '#';
                    } else {
                        mtx[i, nums[0]] = '#';
                    }
                }
            }
         

            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;
            for(var y = 0;y<height; y++){
                for(var x=0;x<width; x++){
                    if(mtx[x,y] == '#'){
                        minY = Math.Min(minY, y);
                        maxY = Math.Max(maxY, y);

                        minX = Math.Min(minX, x);
                        maxX = Math.Max(maxX, x);
                    }
                }
            }
            Fill(mtx, 500, 0);
            var res = 0;
            for(var y = minY;y<=maxY; y++){
                for(var x=0;x<width; x++){
                    if(mtx[x,y] == '~'){
                        res++;
                    }
                }
            }

            return res;
        }
    }
}