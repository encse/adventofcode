
using System;

namespace AdventOfCode.Y2019 {

    class SplashScreenImpl : AdventOfCode.SplashScreen {

        public void Show() {

            var color = Console.ForegroundColor;
            Write(0xffff66, "\n  __   ____  _  _  ____  __ _  ____     __  ____     ___  __  ____  ____         \n / _\\ (    \\/ )( ");
            Write(0xffff66, "\\(  __)(  ( \\(_  _)   /  \\(  __)   / __)/  \\(    \\(  __)        \n/    \\ ) D (\\ \\/ / ) _) /    /  )( ");
            Write(0xffff66, "   (  O )) _)   ( (__(  O )) D ( ) _)         \n\\_/\\_/(____/ \\__/ (____)\\_)__) (__)    \\__/(__)     \\");
            Write(0xffff66, "___)\\__/(____/(____)  2019\n\n               ");
            Write(0x333333, ".         .                                    ");
            Write(0x888888, "25\n            ");
            Write(0x333333, ".             .   .                     .         ");
            Write(0x888888, "24\n                         ");
            Write(0x333333, ".      .     .         .         .   ");
            Write(0x888888, "23\n                ");
            Write(0x333333, ".                                             ");
            Write(0x888888, "22\n                                    ");
            Write(0x333333, ".              .          ");
            Write(0x888888, "21\n                                    ");
            Write(0x333333, ".      .     .            ");
            Write(0x888888, "20\n              ");
            Write(0x333333, ".                   .                       .   ");
            Write(0x888888, "19\n                       ");
            Write(0x333333, ".                    .                 ");
            Write(0x888888, "18\n                                                          ");
            Write(0x333333, ".   ");
            Write(0x888888, "17\n                          ");
            Write(0x333333, ".  .      ..                        ");
            Write(0x888888, "16\n                                          ");
            Write(0x333333, ".                   ");
            Write(0x888888, "15\n           '''''...                      ");
            Write(0x333333, ".                    ");
            Write(0x888888, "14\n                   ''..                               ");
            Write(0x333333, ".       ");
            Write(0x888888, "13\n           ''''...     '.   ");
            Write(0x333333, ".                          .      ");
            Write(0x888888, "12\n                  ''.    '.                                   11\n           '''''..   '.   '.  ");
            Write(0x333333, ".       .                       ");
            Write(0x888888, "10\n                  '.  '.   '.              ");
            Write(0x333333, ".    .         .   ");
            Write(0x888888, " 9\n                 ");
            Write(0x333333, ".");
            Write(0x888888, "  :  '.  [.]                                 8\n           '''.      :  :   :   ");
            Write(0x333333, "..     .                 .    ");
            Write(0x888888, " 7 ");
            Write(0xfff66, "*");
            Write(0xffff66, "*\n              ");
            Write(0x888888, ".      :  :   :  ");
            Write(0x333333, ".                 . .          ");
            Write(0x888888, " 6 ");
            Write(0xfff66, "*");
            Write(0xffff66, "*\n           ");
            Write(0x888888, "...'      :  :   :                        ");
            Write(0x333333, ".        ");
            Write(0x888888, " 5 ");
            Write(0xfff66, "*");
            Write(0xffff66, "*\n                    ");
            Write(0x888888, ".  .'   :                                  4 ");
            Write(0xfff66, "*");
            Write(0xffff66, "*\n           ");
            Write(0x888888, "       .'  .'   .'              ");
            Write(0x333333, ".     .    .   .   ");
            Write(0x888888, " 3 ");
            Write(0xfff66, "*");
            Write(0xffff66, "*\n           ");
            Write(0x888888, ".....''   .'   .'");
            Write(0x333333, ".                           .     ");
            Write(0x888888, " 2 ");
            Write(0xfff66, "*");
            Write(0xffff66, "*\n               ");
            Write(0x333333, ".");
            Write(0x888888, "  ..'    .'          ");
            Write(0x333333, ".                        ");
            Write(0x888888, " 1 ");
            Write(0xfff66, "*");
            Write(0xffff66, "*\n           \n");
            
            Console.ForegroundColor = color;
            Console.WriteLine();
        }

       private static void Write(int rgb, string text){
           Console.Write($"\u001b[38;2;{(rgb>>16)&255};{(rgb>>8)&255};{rgb&255}m{text}");
       }
    }
}