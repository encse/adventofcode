using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day08 {

    class Solution : Solver {

        public string GetName() => "Space Image Format";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var zeroMin = int.MaxValue;
            var checksum = 0;
            foreach (var layer in Layers(input)) {
                var zero = (from row in layer from col in row where col == 0 select 1).Count();
                var ones = (from row in layer from col in row where col == 1 select 1).Count();
                var twos = (from row in layer from col in row where col == 2 select 1).Count();

                if (zeroMin > zero) {
                    zeroMin = zero;
                    checksum = ones * twos;
                }
            }
            return checksum;
        }

        string PartTwo(string input) {
            var img = new int[6, 25];
            foreach (var layer in Layers(input).Reverse()) {
                for (var irow = 0; irow < 6; irow++) {
                    for (var icol = 0; icol < 25; icol++) {
                        var c = layer[irow][icol];
                        if (c != 2) {
                            img[irow, icol] = c;
                        }
                    }
                }
            }

            return OCR(img);
        }

        int[][][] Layers(string input) {
            var picture = input.Select(ch => int.Parse(ch.ToString())).ToArray();
            return Chunk(picture, 25 * 6).Select(layer => Chunk(layer, 25)).ToArray();
        }

        public T[][] Chunk<T>(IEnumerable<T> source, int chunksize) {
            var res = new List<T[]>();
            while (source.Any()) {
                res.Add(source.Take(chunksize).ToArray());
                source = source.Skip(chunksize);
            }
            return res.ToArray();
        }

        string OCR(int[,] mx) {
            var dict = new Dictionary<long, string>{
                {0x725C94B8, "B"},
                {0x32508498, "C"},
                {0x462A2108, "Y"},
                {0x7A1C843C, "E"},
                {0x7A1C8420, "F"},
            };

            var res = "";
            var width = 5;
            for (var ch = 0; ch < Math.Ceiling(mx.GetLength(1) / (double)width); ch++) {
                var hash = 0L;
                var st = "";
                for (var irow = 0; irow < mx.GetLength(0); irow++) {
                    for (var i = 0; i < width; i++) {
                        var icol = (ch * width) + i;

                        if (icol < mx.GetLength(1) && mx[irow, icol] == 1) {
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
}