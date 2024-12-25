
using System;

class Terminal {
    public static void WriteLine(ConsoleColor color = ConsoleColor.Gray, string text = "") {
        Write(color, text + "\n");
    }
    public static void Write(ConsoleColor color = ConsoleColor.Gray, string text = "") {

        Console.Write($"\u001b[{ToAnsiColorCode(color)}");
        Console.Write(text);
        Console.Write("\u001b[0m");
    }

    private static string ToAnsiColorCode(ConsoleColor color) {
        switch(color) {
            case ConsoleColor.Black:        return "30m";
            case ConsoleColor.DarkRed:      return "31m";
            case ConsoleColor.DarkGreen:    return "32m";
            case ConsoleColor.DarkYellow:   return "33m";
            case ConsoleColor.DarkBlue:     return "34m";
            case ConsoleColor.DarkMagenta:  return "35m";
            case ConsoleColor.DarkCyan:     return "36m";
            case ConsoleColor.DarkGray:     return "37m";
            case ConsoleColor.Gray:         return "90m";
            case ConsoleColor.Red:          return "91m";
            case ConsoleColor.Green:        return "92m";
            case ConsoleColor.Yellow:       return "93m";
            case ConsoleColor.Blue:         return "94m";
            case ConsoleColor.Magenta:      return "95m";
            case ConsoleColor.Cyan:         return "96m";
            case ConsoleColor.White:        return "97m";
        }
        throw new Exception($"unhandled color code {color}");
    }
}