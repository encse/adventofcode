using System;

namespace AdventOfCode.Y2025;

class SplashScreenImpl : SplashScreen {

    public void Show() {

        var color = Console.ForegroundColor;
        Write(0xcc00, false, "           ▄█▄ ▄▄█ ▄ ▄ ▄▄▄ ▄▄ ▄█▄  ▄▄▄ ▄█  ▄▄ ▄▄▄ ▄▄█ ▄▄▄\n           █▄█ █ █ █ █ █▄█ █ █ █   █ █ █▄ ");
            Write(0xcc00, false, " █  █ █ █ █ █▄█\n           █ █ █▄█ ▀▄▀ █▄▄ █ █ █▄  █▄█ █   █▄ █▄█ █▄█ █▄▄  {'year': 2025}\n          ");
            Write(0xcc00, false, "  \n            ");
            Write(0xffffff, false, "' '     ____     '     ");
            Write(0xffff66, true, "* ");
            Write(0xffffff, false, ". ' '.   . . ' ");
            Write(0xff9900, false, "<");
            Write(0xffffff, false, "o   '         \n           ________/");
            Write(0x999999, false, "O___");
            Write(0xffffff, false, "\\__________");
            Write(0xff0000, false, "|");
            Write(0xffffff, false, "_________________O______  ");
            Write(0xcccccc, false, " 1 ");
            Write(0xffff66, false, "**\n           ");
            Write(0x32211a, false, "'' ");
            Write(0x999999, false, "_______");
            Write(0xaabbcc, false, "||");
            Write(0x999999, false, "_________     ");
            Write(0x32211a, false, "' '      ' .  . .   ' .       \n            ' ");
            Write(0x999999, false, "| ");
            Write(0x9b715b, false, "_");
            Write(0xbb66ff, false, "@");
            Write(0x9b715b, false, "__ ");
            Write(0xaabbcc, false, "|| ");
            Write(0x9b715b, false, "_");
            Write(0x66ff, false, "o");
            Write(0x9b715b, false, "_  ");
            Write(0xff0000, false, "'.");
            Write(0x999999, false, "|_ _________________________   ");
            Write(0xcccccc, false, " 2 ");
            Write(0xffff66, false, "**\n            ");
            Write(0x32211a, false, ". ");
            Write(0x999999, false, "|_");
            Write(0xff0000, false, "&");
            Write(0x999999, false, "_");
            Write(0xffff66, false, "%");
            Write(0x999999, false, "__");
            Write(0xaabbcc, false, "||");
            Write(0x999999, false, "_");
            Write(0x66ff, false, "o");
            Write(0xff9900, false, "o");
            Write(0x999999, false, "__");
            Write(0xaabbcc, false, "^");
            Write(0x9b715b, false, "=");
            Write(0x999999, false, "_");
            Write(0x9b715b, false, "[ ");
            Write(0x999999, false, "\\|");
            Write(0xff00, false, "..'  ");
            Write(0xaabbcc, false, "_    ");
            Write(0xff0000, false, ".");
            Write(0x666666, false, ". ");
            Write(0xff0000, false, ".");
            Write(0x666666, false, ". ");
            Write(0xff0000, false, ".");
            Write(0x666666, false, ".     ");
            Write(0x999999, false, "|        \n                                \\_");
            Write(0xaabbcc, false, "]");
            Write(0x999999, false, "__");
            Write(0x9b715b, false, "--");
            Write(0xaabbcc, false, "|_|");
            Write(0x999999, false, "___");
            Write(0xaabbcc, false, "[]");
            Write(0x999999, false, "_");
            Write(0xaabbcc, false, "[]");
            Write(0x999999, false, "_");
            Write(0xaabbcc, false, "[]");
            Write(0x999999, false, "__");
            Write(0xaabbcc, false, "//");
            Write(0x999999, false, "_|   ");
            Write(0xcccccc, false, " 3 ");
            Write(0xffff66, false, "**\n           ");
            Write(0x333333, false, "                               ____________//___        \n                                          |");
            Write(0x333333, false, " \\        // @@|   ");
            Write(0x666666, false, " 4\n           ");
            Write(0x333333, false, "                               |_[#]_@@__//_@@@|        \n                                           ");
            Write(0x333333, false, "                   ");
            Write(0x666666, false, " 5\n                                                                   \n                             ");
            Write(0x666666, false, "                                  6\n                                                                ");
            Write(0x666666, false, "   \n                                                               7\n                               ");
            Write(0x666666, false, "                                    \n                                                               ");
            Write(0x666666, false, "8\n                                                                   \n                              ");
            Write(0x666666, false, "                                 9\n                                                                 ");
            Write(0x666666, false, "  \n                                                              10\n                                ");
            Write(0x666666, false, "                                   \n                                                              11");
            Write(0x666666, false, "\n                                                                   \n                               ");
            Write(0x666666, false, "                               12\n           \n");
            
        Console.ForegroundColor = color;
        Console.WriteLine();
    }

    private static void Write(int rgb, bool bold, string text){
       Console.Write($"\u001b[38;2;{(rgb>>16)&255};{(rgb>>8)&255};{rgb&255}{(bold ? ";1" : "")}m{text}");
    }
}