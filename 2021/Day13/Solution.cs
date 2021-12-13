using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day13;

[ProblemName("Transparent Origami")]
class Solution : Solver {

    public object PartOne(string input) => GetFolds(input).First().Count();
    public object PartTwo(string input) => Ocr(GetFolds(input).Last());

    IEnumerable<HashSet<Point>> GetFolds(string input) {
        var blocks = input.Split("\n\n");
        // parse to points into a hashset
        var points = (
            from line in blocks[0].Split("\n") 
            let coords = line.Split(",")
            select new Point(int.Parse(coords[0]), int.Parse(coords[1]))
        ).ToHashSet();
        
        // fold line by line, yield result
        foreach (var line in blocks[1].Split("\n")) {
            var rule = line.Split("=");
            if (rule[0].EndsWith("x")) {
                points = FoldX(int.Parse(rule[1]), points);
            } else {
                points = FoldY(int.Parse(rule[1]), points);
            }
            yield return points;
        }
    }

    HashSet<Point> FoldX(int x, HashSet<Point> d) =>
        d.Select(p => p.x > x ? p with { x = 2 * x - p.x } : p).ToHashSet();

    HashSet<Point> FoldY(int y, HashSet<Point> d) =>
        d.Select(p => p.y > y ? p with { y = 2 * y - p.y } : p).ToHashSet();

    // Ocr for fun
    string Ocr(HashSet<Point> points) {
        var dict = new Dictionary<long, string>{
            {0x19297A52, "A"},
            {0x725C94B8, "B"},
            {0x32508498, "C"},
            {0x7A1C843C, "E"},
            {0x7A1C8420, "F"},
            {0x3D0E4210, "F"},
            {0x3250B49C, "G"},
            {0x252F4A52, "H"},
            {0x0C210A4C, "J"},
            {0x18421498, "J"},
            {0x2108421E, "L"},
            {0x7252E420, "P"},
            {0x7252E524, "R"},
            {0x462A2108, "Y"},
            {0x3C22221E, "Z"},
            {0, ""},
        };

        var charWidth = 5;
        var width = points.MaxBy(p => p.x).x;
        var height = points.MaxBy(p => p.y).y;

        var res = "";
        for (var ch = 0; ch < Math.Ceiling(width / (double)charWidth); ch++) {
            var hash = 0L;
            var st = "";
            for (var irow = 0; irow <= height; irow++) {
                for (var i = 0; i < charWidth; i++) {
                    var icol = (ch * charWidth) + i;

                    if (points.Contains(new Point(icol, irow))) {
                        hash += 1;
                        st += "#";
                    } else {
                        st += ".";
                    }
                    hash <<= 1;
                }
                st += "\n";
            }
            if (!dict.ContainsKey(hash)) {
                throw new Exception($"Unrecognized letter with hash: 0x{hash.ToString("X")}\n{st}");
            }
            res += dict[hash];
        }
        return res;
    }
}

record Point(int x, int y);
