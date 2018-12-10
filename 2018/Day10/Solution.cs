using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day10 {

    class Solution : Solver {

        public string GetName() => "The Stars Align";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        string PartOne(string input) => Solver(input).st;

        int PartTwo(string input) => Solver(input).seconds;

        (string st, int seconds) Solver(string input){
            // position=< 21992, -10766> velocity=<-2,  1>
            var rx = new Regex(@"position=\<\s*(?<x>-?\d+),\s*(?<y>-?\d+)\> velocity=\<\s*(?<vx>-?\d+),\s*(?<vy>-?\d+)\>");
            var points = (
                from line in input.Split("\n")
                let m = rx.Match(line)
                select new Point {
                    x = int.Parse(m.Groups["x"].Value),
                    y = int.Parse(m.Groups["y"].Value),
                    vx = int.Parse(m.Groups["vx"].Value),
                    vy = int.Parse(m.Groups["vy"].Value)
                }
            ).ToArray();

            var seconds = 0;
            Func<bool, (int left, int top, long width, long height)> step = (bool forward) => {
                foreach (var point in points) {
                    if(forward){
                        point.x += point.vx;
                        point.y += point.vy;
                    } else {
                        point.x -= point.vx;
                        point.y -= point.vy;
                    }
                }
                seconds += forward ? 1 : -1;

                var minX = points.Min(pt => pt.x);
                var maxX = points.Max(pt => pt.x);
                var minY = points.Min(pt => pt.y);
                var maxY = points.Max(pt => pt.y);
                return (minX, minY, maxX - minX + 1, maxY - minY + 1);
            };

            var area = long.MaxValue;
            while (true) {

                var rect = step(true);
                var areaNew = (rect.width) * (rect.height);
                
                if (areaNew > area) {
                    rect = step(false);
                    var mx = new char[rect.width, rect.height];
                    foreach(var point in points){
                        mx[point.x - rect.left, point.y - rect.top] = '#';
                    }
                    
                    var sb = new StringBuilder();
                    for(var irow = 0;irow<mx.GetLength(1);irow++){
                        
                        for(var icol = 0;icol<mx.GetLength(0);icol++){
                            sb.Append(mx[icol, irow] == '#' ? "#" : ".");
                        }
                        sb.AppendLine("");
                    }
                   
                    return (sb.ToString(), seconds);
                }
                area = areaNew;
            }
        }
    }
    class Point {
        public int x;
        public int y;
        public int vx;
        public int vy;
    }
}