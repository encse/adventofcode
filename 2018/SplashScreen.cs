
using System;

namespace AdventOfCode.Y2018 {

    class SplashScreenImpl : AdventOfCode.SplashScreen {

        public void Show() {

            var color = Console.ForegroundColor;
            Write(ConsoleColor.Yellow, "\n  __   ____  _  _  ____  __ _  ____     __  ____     ___  __  ____  ____         \n / _\\ (    \\/ )( ");
            Write(ConsoleColor.Yellow, "\\(  __)(  ( \\(_  _)   /  \\(  __)   / __)/  \\(    \\(  __)        \n/    \\ ) D (\\ \\/ / ) _) /    /  )( ");
            Write(ConsoleColor.Yellow, "   (  O )) _)   ( (__(  O )) D ( ) _)         \n\\_/\\_/(____/ \\__/ (____)\\_)__) (__)    \\__/(__)     \\");
            Write(ConsoleColor.Yellow, "___)\\__/(____/(____)  2018\n\n                                                              ");
            Write(ConsoleColor.DarkGray, "25\n                                                              24\n                                ");
            Write(ConsoleColor.DarkGray, "                              23\n                                                              22\n  ");
            Write(ConsoleColor.DarkGray, "                                                            21\n                                     ");
            Write(ConsoleColor.DarkGray, "                         20\n                                                              19\n       ");
            Write(ConsoleColor.DarkGray, "                                                       18\n                                          ");
            Write(ConsoleColor.DarkGray, "                    17\n                                                              16\n            ");
            Write(ConsoleColor.DarkGray, "                                                  15\n                                               ");
            Write(ConsoleColor.DarkGray, "               14\n                                                              13\n                 ");
            Write(ConsoleColor.DarkGray, "                                             12\n                                                    ");
            Write(ConsoleColor.DarkGray, "          11\n                                                              10\n                      ");
            Write(ConsoleColor.DarkGray, "                                         9\n                                                         ");
            Write(ConsoleColor.DarkGray, "      8\n                  /    \\                                       7\n                           ");
            Write(ConsoleColor.DarkGray, "                                    6\n               /       \\                                      ");
            Write(ConsoleColor.DarkGray, " 5\n                                                               4\n             /           \\      ");
            Write(ConsoleColor.DarkGray, "                               3\n           (               )                                   2\n  ");
            Write(ConsoleColor.DarkGray, "         _               _________ ___ __ _  _   _    _      1 ");
            Write(ConsoleColor.Yellow, "**\n           \n");
            
            Console.ForegroundColor = color;
            Console.WriteLine();
        }

       private static void Write(ConsoleColor color, string text){
           Console.ForegroundColor = color;
           Console.Write(text);
       }
    }
}