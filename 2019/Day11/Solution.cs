using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day11;

[ProblemName("Space Police")]
class Solution : Solver {

    public object PartOne(string input) => Run(input, 0).Count;

    public object PartTwo(string input) {
        var dict = Run(input, 1);
        var irowMin = dict.Keys.Select(pos => pos.irow).Min();
        var icolMin = dict.Keys.Select(pos => pos.icol).Min();
        var irowMax = dict.Keys.Select(pos => pos.irow).Max();
        var icolMax = dict.Keys.Select(pos => pos.icol).Max();
        var crow = irowMax - irowMin + 1;
        var ccol = icolMax - icolMin + 1;
        var mtx = new int[crow][];
        for (var irow = 0; irow < crow; irow++) {
            mtx[irow] = new int[ccol];
            for (var icol = 0; icol < ccol; icol++) {
                mtx[irow][icol] = dict.GetValueOrDefault((irowMin + irow, icolMin + icol), 0);
            }
        }
        return OCR(mtx);
    }

    Dictionary<(int irow, int icol), int> Run(string input, int startColor) {
        var mtx = new Dictionary<(int irow, int icol), int>();
        (int irow, int icol) pos = (0, 0);
        (int drow, int dcol) dir = (-1, 0);
        mtx[(0, 0)] = startColor;
        var icm = new IntCodeMachine(input);
        while (true) {
            var output = icm.Run(mtx.GetValueOrDefault(pos, 0));
            if (icm.Halted()) {
                return mtx;
            }
            mtx[pos] = (int)output[0];
            dir = output[1] switch {
                0 => (-dir.dcol, dir.drow),
                1 => (dir.dcol, -dir.drow),
                _ => throw new ArgumentException()
            };
            pos = (pos.irow + dir.drow, pos.icol + dir.dcol);
        }
    }

    string OCR(int[][] mx) {
        var dict = new Dictionary<long, string>{
            {0x725C94B8, "B"},
            {0x32508498, "C"},
            {0x462A2108, "Y"},
            {0x7A1C843C, "E"},
            {0x7A1C8420, "F"},
            {0x3D0E4210, "F"},
            {0x252F4A52, "H"},
            {0xC210A4C,  "J"},
            {0x19297A52, "A"},
            {0x2108421E, "L"},
            {0x3C22221E, "Z"},
            {0, ""},
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
