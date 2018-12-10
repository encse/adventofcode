
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
            Write(ConsoleColor.DarkGray, "                                             12\n              |                                |    ");
            Write(ConsoleColor.DarkGray, "       |  11\n               -  -           - -         -                   10 ");
            Write(ConsoleColor.Yellow, "**\n                                              ");
            Write(ConsoleColor.Red, "_     __        ");
            Write(ConsoleColor.DarkGray, " 9 ");
            Write(ConsoleColor.Yellow, "**\n                   ");
            Write(ConsoleColor.Red, ".---_             _       ");
            Write(ConsoleColor.DarkRed, "| ");
            Write(ConsoleColor.Red, "|\\__");
            Write(ConsoleColor.DarkRed, "/");
            Write(ConsoleColor.Red, "_/)       ");
            Write(ConsoleColor.DarkGray, " 8 ");
            Write(ConsoleColor.Yellow, "**\n                  ");
            Write(ConsoleColor.Red, "/ ");
            Write(ConsoleColor.DarkRed, "/ ");
            Write(ConsoleColor.Red, "/\\|      ");
            Write(ConsoleColor.DarkGray, "__   ");
            Write(ConsoleColor.DarkRed, ") ");
            Write(ConsoleColor.Red, ")__   ");
            Write(ConsoleColor.DarkRed, "_|");
            Write(ConsoleColor.Red, "_|     /        ");
            Write(ConsoleColor.DarkGray, " 7 ");
            Write(ConsoleColor.Yellow, "**\n                ");
            Write(ConsoleColor.Red, "/ ");
            Write(ConsoleColor.DarkRed, "/ | ");
            Write(ConsoleColor.Red, "\\ ");
            Write(ConsoleColor.Yellow, "*    ");
            Write(ConsoleColor.DarkGray, "/ / \\ ");
            Write(ConsoleColor.DarkRed, "( ");
            Write(ConsoleColor.Red, "(   \\_");
            Write(ConsoleColor.DarkRed, "/");
            Write(ConsoleColor.Red, "_/      /         ");
            Write(ConsoleColor.DarkGray, " 6 ");
            Write(ConsoleColor.Yellow, "**\n               ");
            Write(ConsoleColor.Red, "/  ");
            Write(ConsoleColor.DarkRed, "/  \\ ");
            Write(ConsoleColor.Red, "\\    ");
            Write(ConsoleColor.DarkGray, "| | \\/  ");
            Write(ConsoleColor.DarkRed, "\\_");
            Write(ConsoleColor.Red, "\\____________/          ");
            Write(ConsoleColor.DarkGray, " 5 ");
            Write(ConsoleColor.Yellow, "**\n              ");
            Write(ConsoleColor.Red, "/ ");
            Write(ConsoleColor.DarkRed, "/  / \\  ");
            Write(ConsoleColor.Red, "\\    ");
            Write(ConsoleColor.DarkGray, "\\_\\______X_____X_____X_,          4 ");
            Write(ConsoleColor.Yellow, "**\n            ");
            Write(ConsoleColor.White, "./~~~~~~~~~~~\\.                                   ");
            Write(ConsoleColor.DarkGray, " 3 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.White, "( .\",^. -\". '.~ )                                  ");
            Write(ConsoleColor.DarkGray, " 2 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.White, "_'~~~~~~~~~~~~~'_________ ___ __ _  _   _    _     ");
            Write(ConsoleColor.DarkGray, " 1 ");
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