using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode;

public static class StringExtensions {
    public static string Indent(this string st, int l, bool firstLine = false) {
        var indent = new string(' ', l);
        var res = string.Join("\n" + new string(' ', l),
            from line in Regex.Split(st, "\r?\n")
            select Regex.Replace(line, @"^\s*\|", "")
        );
        return firstLine ? indent + res : res;
    }
    public static ColoredString WithColor(this string st, ConsoleColor c) {
        return new ColoredString(c, st);
    }
}

public record ColoredString(ConsoleColor c, string st);
