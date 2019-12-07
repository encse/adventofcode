
using System;

namespace AdventOfCode.Y2019 {

    class SplashScreenImpl : AdventOfCode.SplashScreen {

        public void Show() {

            var color = Console.ForegroundColor;
            Write(0xffff66, "\n  __   ____  _  _  ____  __ _  ____     __  ____     ___  __  ____  ____         \n / _\\ (    \\/ )( ");
            Write(0xffff66, "\\(  __)(  ( \\(_  _)   /  \\(  __)   / __)/  \\(    \\(  __)        \n/    \\ ) D (\\ \\/ / ) _) /    /  )( ");
            Write(0xffff66, "   (  O )) _)   ( (__(  O )) D ( ) _)         \n\\_/\\_/(____/ \\__/ (____)\\_)__) (__)    \\__/(__)     \\");
            Write(0xffff66, "___)\\__/(____/(____)  2019\n\n            ");
            Write(0x333333, ".    .  .               .         .               ");
            Write(0x666666, "25\n                                           ");
            Write(0x333333, ".    .             ");
            Write(0x666666, "24\n                                        ");
            Write(0x333333, ".          .          ");
            Write(0x666666, "23\n                                            ");
            Write(0x333333, ".                 ");
            Write(0x666666, "22\n                              ");
            Write(0x333333, ". .                             ");
            Write(0x666666, "21\n                                                              20\n                                ");
            Write(0x666666, "          ");
            Write(0x333333, ".       .           ");
            Write(0x666666, "19\n                                                              18\n                        ");
            Write(0x333333, ".                            .     .  ");
            Write(0x666666, "17\n                                                              16\n                                ");
            Write(0x666666, "             ");
            Write(0x333333, ".                ");
            Write(0x666666, "15\n           ");
            Write(0x333333, "'''''...                                           ");
            Write(0x666666, "14\n           ");
            Write(0x333333, "        ''..     .            .    .               ");
            Write(0x666666, "13\n           ");
            Write(0x333333, "''''... .   '.                                     ");
            Write(0x666666, "12\n           ");
            Write(0x333333, "       ''.    '.                                   ");
            Write(0x666666, "11\n           ");
            Write(0x333333, "'''''..  .'.  .'.                                  ");
            Write(0x666666, "10\n           ");
            Write(0x333333, "       '.. '.   '. .            .  ..         .    ");
            Write(0x666666, " 9\n            ");
            Write(0x333333, ".       :  '.  [.]                        .       ");
            Write(0x666666, " 8\n           '''.      :  :   :                          ");
            Write(0x333333, "..     ");
            Write(0xcccccc, " 7 ");
            Write(0xffff66, "**\n              ");
            Write(0xbebcbe, ".");
            Write(0x666666, "      :  :   :      ");
            Write(0x333333, ".              ..          ");
            Write(0xcccccc, " 6 ");
            Write(0xffff66, "**\n           ");
            Write(0x666666, "...'      : ");
            Write(0x333333, ".");
            Write(0x666666, ":   :                              ");
            Write(0x333333, ".  ");
            Write(0xcccccc, " 5 ");
            Write(0xffff66, "**\n                  ");
            Write(0x333333, ". ");
            Write(0xe3e2e0, ".");
            Write(0x666666, "  .'   :        ");
            Write(0x333333, ".  .            .   .    ");
            Write(0xcccccc, " 4 ");
            Write(0xffff66, "**\n               ");
            Write(0x333333, ".");
            Write(0x666666, "  .'  .'   .'          ");
            Write(0x333333, ".       .              ");
            Write(0xcccccc, " 3 ");
            Write(0xffff66, "**\n           ");
            Write(0x666666, ".....''   ");
            Write(0x91a5bd, ".");
            Write(0x666666, "'   .'                                  ");
            Write(0xcccccc, " 2 ");
            Write(0xffff66, "**\n           ");
            Write(0x666666, "       ..'    .'    ");
            Write(0x333333, ".          .        ..  ..     ");
            Write(0xcccccc, " 1 ");
            Write(0xffff66, "**\n           \n");
            
            Console.ForegroundColor = color;
            Console.WriteLine();
        }

       private static void Write(int rgb, string text){
           Console.Write($"\u001b[38;2;{(rgb>>16)&255};{(rgb>>8)&255};{rgb&255}m{text}");
       }
    }
}