using System.Collections.Generic;
using System;

namespace AdventOfCode;

static class OcrExtension {
    public static OcrString Ocr(this string st,  int charWidth = 5, int charHeight = 5){
        return new OcrString(st, charWidth, charHeight);
    }
}

record OcrString (string st, int charWidth = 5, int charHeight = 5) {
    public override string ToString() {
        var lines = st.Split("\n");
        var width = lines[0].Length;
        var height = lines.Length;

        var dict = new Dictionary<long, string>{
            // 5 x 5
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

            // 8x10
            {0x09F109090909F000, "B"},
            {0x010101010108F000, "C"},
            {0x010139090918E800, "G"},
            {0x101010111110E000, "J"},
            {0x102040810101F800, "Z"},

            {0, ""},
        };

        var res = "";

        for (var ch = 0; ch < Math.Ceiling(width / (double)charWidth); ch++) {
            var hash = 0L;
            var stChar = "";
            for (var irow = 0; irow <= charHeight; irow++) {
                for (var i = 0; i < charWidth; i++) {
                    var icol = (ch * charWidth) + i;
                    var point = irow < height && icol < lines[irow].Length ? lines[irow][icol] : ' ';
                    stChar += point;
                    if (point != ' ' && point != '.') {
                        hash += 1;
                    } 
                    hash <<= 1;
                }
                stChar += "\n";
            }
            if (!dict.ContainsKey(hash)) {
                throw new Exception($"Unrecognized letter with hash: 0x{hash.ToString("X")}\n{stChar}");
            }
            res += dict[hash];
        }
        return res;
    }
}