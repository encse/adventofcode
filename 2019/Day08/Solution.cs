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
        var img = new char[6 * 25];
        foreach (var layer in Layers(input).Reverse()) {
            for (var i = 0; i < img.Length; i++) {
                img[i] = layer[i] switch {
                    0 => ' ',
                    1 => '#',
                    _ => img[i]
                };
            }
        }
        return string.Join("", 
            img.Chunk(25).Select(line => string.Join("", line)+"\n")
        ).Ocr();
    }

    int[][] Layers(string input) =>
        input.Select(ch => ch - '0').Chunk(6 * 25).ToArray();
}
