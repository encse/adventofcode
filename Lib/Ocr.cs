using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode;

static class OcrExtension {
    public static OcrString Ocr(this string st) {
        return new OcrString(st);
    }
}

record OcrString(string st) {
    public override string ToString() {
        var lines = st.Split("\n")
            .SkipWhile(x => string.IsNullOrWhiteSpace(x))
            .TakeWhile(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        while (lines.All(line => line.StartsWith(" "))) {
            lines = GetRect(lines, 1, 0, lines[0].Length - 1, lines.Length).Split("\n");
        }

        while (lines.All(line => line.EndsWith(" "))) {
            lines = GetRect(lines, 0, 0, lines[0].Length - 1, lines.Length).Split("\n");
        }

        var width = lines[0].Length;
        var height = lines.Length;

        var smallAlphabet = StripMargin(@"
        | A    B    C    E    F    G    H    I    J    K    L    O    P    R    S    U    Y    Z    
        |  ##  ###   ##  #### ####  ##  #  #  ###   ## #  # #     ##  ###  ###   ### #  # #   ##### 
        | #  # #  # #  # #    #    #  # #  #   #     # # #  #    #  # #  # #  # #    #  # #   #   # 
        | #  # ###  #    ###  ###  #    ####   #     # ##   #    #  # #  # #  # #    #  #  # #   #  
        | #### #  # #    #    #    # ## #  #   #     # # #  #    #  # ###  ###   ##  #  #   #   #   
        | #  # #  # #  # #    #    #  # #  #   #  #  # # #  #    #  # #    # #     # #  #   #  #    
        | #  # ###   ##  #### #     ### #  #  ###  ##  #  # ####  ##  #    #  # ###   ##    #  #### 
        ");

        var largeAlphabet = StripMargin(@"
        | A       B       C       E       F       G       H       J       K       L       N       P       R       X       Z
        |   ##    #####    ####   ######  ######   ####   #    #     ###  #    #  #       #    #  #####   #####   #    #  ######  
        |  #  #   #    #  #    #  #       #       #    #  #    #      #   #   #   #       ##   #  #    #  #    #  #    #       #  
        | #    #  #    #  #       #       #       #       #    #      #   #  #    #       ##   #  #    #  #    #   #  #        #  
        | #    #  #    #  #       #       #       #       #    #      #   # #     #       # #  #  #    #  #    #   #  #       #   
        | #    #  #####   #       #####   #####   #       ######      #   ##      #       # #  #  #####   #####     ##       #    
        | ######  #    #  #       #       #       #  ###  #    #      #   ##      #       #  # #  #       #  #      ##      #     
        | #    #  #    #  #       #       #       #    #  #    #      #   # #     #       #  # #  #       #   #    #  #    #      
        | #    #  #    #  #       #       #       #    #  #    #  #   #   #  #    #       #   ##  #       #   #    #  #   #       
        | #    #  #    #  #    #  #       #       #   ##  #    #  #   #   #   #   #       #   ##  #       #    #  #    #  #       
        | #    #  #####    ####   ######  #        ### #  #    #   ###    #    #  ######  #    #  #       #    #  #    #  ######  
        ");

        var charMap =
            lines.Length == smallAlphabet.Length - 1 ? smallAlphabet :
            lines.Length == largeAlphabet.Length - 1 ? largeAlphabet :
            throw new Exception("Could not find alphabet");

        var charWidth = charMap == smallAlphabet ? 5 : 8;
        var charHeight = charMap == smallAlphabet ? 6 : 10;
        var res = "";
        for (var i = 0; i < width; i += charWidth) {
            res += Detect(lines, i, charWidth, charHeight, charMap);
        }
        return res;
    }

    string[] StripMargin(string st) => (
        from line in Regex.Split(st, "\r?\n")
        where Regex.IsMatch(line, @"^\s*\| ")
        select Regex.Replace(line, @"^\s* \| ", "")
    ).ToArray();

    public string Detect(string[] text, int icolLetter, int charWidth, int charHeight, string[] charMap) {
        var textRect = GetRect(text, icolLetter, 0, charWidth, charHeight);

        for (var icol = 0; icol < charMap[0].Length; icol += charWidth) {
            var ch = charMap[0][icol].ToString();
            var charPattern = GetRect(charMap, icol, 1, charWidth, charHeight);
            var found = Enumerable.Range(0, charPattern.Length).All(i => {
                var textWhiteSpace = " .".Contains(textRect[i]);
                var charWhiteSpace = " .".Contains(charPattern[i]);
                return textWhiteSpace == charWhiteSpace;
            });

            if (found) {
                return ch;
            }
        }

        throw new Exception($"Unrecognized letter: \n{textRect}\n");
    }

    string GetRect(string[] st, int icol0, int irow0, int ccol, int crow) {
        var res = "";
        for (var irow = irow0; irow < irow0 + crow; irow++) {
            for (var icol = icol0; icol < icol0 + ccol; icol++) {
                var ch = irow < st.Length && icol < st[irow].Length ? st[irow][icol] : ' ';
                res += ch;
            }
            if (irow + 1 != irow0 + crow) {
                res += "\n";
            }
        }
        return res;
    }
}