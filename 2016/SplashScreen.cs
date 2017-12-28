
using System;

namespace AdventOfCode.Y2016 {

    class SplashScreenImpl : AdventOfCode.SplashScreen {

        public void Show() {

            var color = Console.ForegroundColor;
            Write(ConsoleColor.Yellow, "\n  __   ____  _  _  ____  __ _  ____     __  ____     ___  __  ____  ____         \n / _\\ (    \\/ )( ");
            Write(ConsoleColor.Yellow, "\\(  __)(  ( \\(_  _)   /  \\(  __)   / __)/  \\(    \\(  __)        \n/    \\ ) D (\\ \\/ / ) _) /    /  )( ");
            Write(ConsoleColor.Yellow, "   (  O )) _)   ( (__(  O )) D ( ) _)         \n\\_/\\_/(____/ \\__/ (____)\\_)__) (__)    \\__/(__)     \\");
            Write(ConsoleColor.Yellow, "___)\\__/(____/(____)  2016\n\n           ");
            Write(ConsoleColor.DarkGray, "                    *                                   \n                               |           ");
            Write(ConsoleColor.DarkGray, "                        \n                             +-|---+                               \n       ");
            Write(ConsoleColor.DarkGray, "                     /  |  /|                               \n                           +-----+ |   ");
            Write(ConsoleColor.DarkGray, "                            \n                           |:::::| |                          25 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "        +----+  |:::::| |---+      +-----------+   24 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "       /    / \\ |:::::| |  /|     / \\\\\\\\\\\\ [] /|   23 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "      /    / / \\|:::::| | / |    / \\\\\\\\\\\\ [] / |   22 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "     /    / / / \\:::::|/ /  |   +-----------+  |   21 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    +----+ / / / \\------+ ------|:::::::::::|  |   20 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |-----\\ / / / \\=====| ------|:::::::::::|  |   19 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |------\\ / / / \\====|   |   |:::::::::::|  |   18 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |-------\\ / / / +===|   |   |:::::::::::|  |   17 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |--------\\ / / /|===|   |   |:::::::::::|  |   16 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |---------\\ / / |===|   |  /|:::::::::::|  |   15 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |----------\\ /  |===|  /  //|:::::::::::| /    14 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    +-----------+   |===| /  //||:::::::::::|/     13 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |:::::::::::|   |===|/__//___________________  12 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |:::::::::::|   |______//|_____...._________   11 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |:::::::::::|   |     //| ____/ /_/___         10 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, " ---|:::::::::::|   |--------|[][]|_|[][]_\\------   9 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "----|:::::::::::|   |---------------------------    8 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, " || |:::::::::::|   |  //| ||  / / / ||      ||     7 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, " || |:::::::::::|   | //|  || /   /  ||      ||     6 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |:::::::::::|   |//|     / / /                  5 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |:::::::::::|   //|     /   /   ____________    4 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |:::::::::::|  //|     / / /___/ /#/ /#/#/ /    3 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |:::::::::::| //|     /    ___            /     2 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "    |:::::::::::|//|     / / /   /_/_/_/#/#/#/      1 ");
            Write(ConsoleColor.Yellow, "**\n           ");
            Write(ConsoleColor.DarkGray, "  ==============//======+...+====================       \n             - - - - - - -// - - -/   / - -");
            Write(ConsoleColor.DarkGray, " - - - - - - - -        \n           ==============//|==============================         \n       ");
            Write(ConsoleColor.DarkGray, "                 //|                                        \n           \n");
            
            Console.ForegroundColor = color;
            Console.WriteLine();
        }

       private static void Write(ConsoleColor color, string text){
           Console.ForegroundColor = color;
           Console.Write(text);
       }
    }
}