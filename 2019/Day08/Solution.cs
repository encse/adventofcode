using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day08;

[ProblemName("Space Image Format")]
class Solution : Solver {

    public object PartOne(string input) {
        var zeroMin = int.MaxValue;
        var checksum = 0;
        foreach (var layer in Layers(input)) {
            var zero = layer.Count(item => item == 0);
            var ones = layer.Count(item => item == 1);
            var twos = layer.Count(item => item == 2);

            if (zeroMin > zero) {
                zeroMin = zero;
                checksum = ones * twos;
            }
        }
        return checksum;
    }

    public object PartTwo(string input) {
        var img = new int[6 * 25];
        foreach (var layer in Layers(input).Reverse()) {
            for (var i = 0; i < img.Length; i++) {
                var c = layer[i];
                if (c != 2) {
                    img[i] = c;
                }
            }
        }
        return OCR(Chunk(img, 25));
    }

    int[][] Layers(string input) {
        var picture = input.Select(ch => int.Parse(ch.ToString())).ToArray();
        return Chunk(picture, 25 * 6);
    }

    public T[][] Chunk<T>(IEnumerable<T> source, int chunksize) {
        var res = new List<T[]>();
        while (source.Any()) {
            res.Add(source.Take(chunksize).ToArray());
            source = source.Skip(chunksize);
        }
        return res.ToArray();
    }

    string OCR(int[][] mx) {
        var dict = new Dictionary<long, string>{
            {0x725C94B8, "B"},
            {0x32508498, "C"},
            {0x462A2108, "Y"},
            {0x7A1C843C, "E"},
            {0x7A1C8420, "F"},
        };

        var res = "";
        var width = 5;
        for (var ch = 0; ch < Math.Ceiling(mx[0].Length / (double)width); ch++) {
            var hash = 0L;
            var st = "";
            for (var irow = 0; irow < mx.Length; irow++) {
                for (var i = 0; i < width; i++) {
                    var icol = (ch * width) + i;

                    if (icol < mx[0].Length && mx[irow][icol] == 1) {
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
