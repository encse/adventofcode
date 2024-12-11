using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace AdventOfCode;


class SlocChart {
    public static void Show(int year, List<(int day, int sloc)> slocs) {
        if (slocs.Count < 2) {
            return;
        }

        Console.WriteLine($"  {year } in code lines");
        Console.WriteLine("");

        var chars = "█▁▂▃▄▅▆▇";
        var max = slocs.Select(sloc => sloc.sloc).Max();

        var columns = new List<List<ColoredString>>();

        var icol = 0;
        var prevSloc = -1;
        foreach (var sloc in slocs) {
            icol++;
            var col = new List<ColoredString>();
            var h = sloc.sloc;

            var color =
                h > 200 ? ConsoleColor.Red :
                h > 100 ? ConsoleColor.Yellow :
                ConsoleColor.DarkGray;
            h /= 2;

            if (Math.Abs(prevSloc - sloc.sloc) > 20 || prevSloc < 100 && sloc.sloc < 100) {
                var slocSt = sloc.sloc.ToString();
                col.Add(slocSt.WithColor(ConsoleColor.White));
            }
            prevSloc = sloc.sloc;
            if (h % chars.Length != 0) {
                var ch = chars[h % chars.Length];
                col.Add($"{ch}{ch}".WithColor(color));
                h -= h % chars.Length;
            }
            while (h >= 0) {
                var ch = chars[0];
                col.Add($"{ch}{ch}".WithColor(color));
                h -= chars.Length;
            }
            col.Add(sloc.day.ToString().PadLeft(2, ' ').WithColor(ConsoleColor.White));
            var w = 3;
            col = col.Select(r => r.st.PadLeft(w).WithColor(r.c)).ToList();
            columns.Add(col);
        }

        var rows = new List<List<ColoredString>>();
        var height = columns.Select(col => col.Count).Max();
        for (var irow = 0; irow < height; irow++) {
            var row = new List<ColoredString>();
            foreach (var col in columns) {
                var color = col.Count > irow ? col[col.Count - irow - 1].c : ConsoleColor.Gray;
                var st = col.Count > irow ? col[col.Count - irow - 1].st : "";
                var w = col.Select(r => r.st.Length).Max();
                st = st.PadLeft(w);
                row.Add(st.WithColor(color));
            }
            rows.Insert(0, row);
        }

        var c = Console.ForegroundColor;
        foreach (var row in rows) {
            foreach (var item in row) {
                Console.ForegroundColor = item.c;
                Console.Write(item.st);
            }
            Console.WriteLine();
        }
        Console.ForegroundColor = c;
        Console.WriteLine("");
    }
}