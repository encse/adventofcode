
using System;

namespace AdventOfCode.Y2021;

class SplashScreenImpl : SplashScreen {

    public void Show() {

        var color = Console.ForegroundColor;
        Write(0xcc00, false, "           ▄█▄ ▄▄█ ▄ ▄ ▄▄▄ ▄▄ ▄█▄  ▄▄▄ ▄█  ▄▄ ▄▄▄ ▄▄█ ▄▄▄\n           █▄█ █ █ █ █ █▄█ █ █ █   █ █ █▄ ");
            Write(0xcc00, false, " █  █ █ █ █ █▄█\n           █ █ █▄█ ▀▄▀ █▄▄ █ █ █▄  █▄█ █   █▄ █▄█ █▄█ █▄▄  int y = 2021;\n           ");
            Write(0xcc00, false, " \n           ");
            Write(0x666666, false, "                   ~  ");
            Write(0xc8ff, false, "~");
            Write(0x666666, false, " ~ ");
            Write(0xc8ff, false, "~");
            Write(0x666666, false, "~ ");
            Write(0xc8ff, false, "~");
            Write(0x666666, false, "~");
            Write(0xc8ff, false, "~~");
            Write(0x666666, false, "~");
            Write(0xc8ff, false, "~~~~~~~~~~~~~~~  ");
            Write(0xcccccc, false, " 1 ");
            Write(0xffff66, false, "**\n                                                ");
            Write(0xb5ed, false, "  .   ");
            Write(0xa47a4d, false, "..''''  ");
            Write(0xcccccc, false, " 2 ");
            Write(0xffff66, false, "**\n           ");
            Write(0x333333, false, "                                          :        ");
            Write(0x666666, false, " 3\n                                                               4\n                                ");
            Write(0x666666, false, "                               5\n                                                               6\n  ");
            Write(0x666666, false, "                                                             7\n                                     ");
            Write(0x666666, false, "                          8\n                                                               9\n       ");
            Write(0x666666, false, "                                                       10\n                                          ");
            Write(0x666666, false, "                    11\n                                                              12\n            ");
            Write(0x666666, false, "                                                  13\n                                               ");
            Write(0x666666, false, "               14\n                                                              15\n                 ");
            Write(0x666666, false, "                                             16\n                                                    ");
            Write(0x666666, false, "          17\n                                                              18\n                      ");
            Write(0x666666, false, "                                        19\n                                                         ");
            Write(0x666666, false, "     20\n                                                              21\n                           ");
            Write(0x666666, false, "                                   22\n                                                              ");
            Write(0x666666, false, "23\n                                                              24\n                                ");
            Write(0x666666, false, "                              25\n           \n");
            
        Console.ForegroundColor = color;
        Console.WriteLine();
    }

   private static void Write(int rgb, bool bold, string text){
       Console.Write($"\u001b[38;2;{(rgb>>16)&255};{(rgb>>8)&255};{rgb&255}{(bold ? ";1" : "")}m{text}");
   }
}
