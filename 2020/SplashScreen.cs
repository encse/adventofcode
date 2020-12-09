
using System;

namespace AdventOfCode.Y2020 {

    class SplashScreenImpl : AdventOfCode.SplashScreen {

        public void Show() {

            var color = Console.ForegroundColor;
            Write(0xffff66, false, "\n /\\   _|     _  ._ _|_    _ _|_   /   _   _|  _  \n/--\\ (_| \\/ (/_ | | |_   (_) |    \\_ (_) (_| (/_ ");
            Write(0xffff66, false, " 2020\n\n           ");
            Write(0x666666, false, "              ..........|..........                ");
            Write(0xcccccc, false, " 1 ");
            Write(0xffff66, false, "*");
            Write(0x666666, false, "*\n              .....'''''' .'  -  -   - .''~ ~ ''''''.....     ");
            Write(0xcccccc, false, " 2 ");
            Write(0xffff66, false, "*");
            Write(0x666666, false, "*\n           ''' ~ ~ ~ ~  ~ '.'. -   -   -'':  ~ ~   ~  ~  '''  ");
            Write(0xcccccc, false, " 3 ");
            Write(0xffff66, false, "*");
            Write(0x666666, false, "*\n            ~   ~  ~   ~ ~  ~ ''..'''.  .'  ~    ~   ~ ~  ~   ");
            Write(0xcccccc, false, " 4 ");
            Write(0xffff66, false, "*");
            Write(0x666666, false, "*\n           ~   ~ ~  .'. ~  ~  ~    ~  ''  ~  ~  ~     ~    ~  ");
            Write(0xcccccc, false, " 5 ");
            Write(0xffff66, false, "*");
            Write(0x666666, false, "*\n             ~    ~ ''  ..  ~    ~   ~  ~  ~      ~           ");
            Write(0xcccccc, false, " 6 ");
            Write(0xffff66, false, "*");
            Write(0x666666, false, "*\n            ~  ~ ~   ~ :  '.   ~   ~      ~                   ");
            Write(0xcccccc, false, " 7 ");
            Write(0xffff66, false, "*");
            Write(0x666666, false, "*\n                  ~     '.' ~        ~  ~                     ");
            Write(0xcccccc, false, " 8 ");
            Write(0xffff66, false, "*");
            Write(0x666666, false, "*\n            ");
            Write(0x333399, false, "~   ~      ~      ~                             \n                          ~                        ");
            Write(0x333399, false, "           ");
            Write(0xcccccc, false, " 9 ");
            Write(0xffff66, false, "**\n                             ");
            Write(0xcccccc, false, "\\ ");
            Write(0x333399, false, "~                            \n                              ");
            Write(0xcccccc, false, "\\                             \n           ");
            Write(0x333333, false, "                   ~");
            Write(0xcccccc, false, "\\                            \n                                \\");
            Write(0x333333, false, " ~                         \n                              ~  ");
            Write(0xcccccc, false, "\\");
            Write(0x333333, false, "   ~                        ");
            Write(0x666666, false, "10\n           ");
            Write(0x333333, false, "                           .                     \n                                ");
            Write(0x333399, false, "~");
            Write(0x333333, false, "  .");
            Write(0xcc00, false, "'  :                    \n                                  :   .");
            Write(0x333333, false, "'                    \n                                    ");
            Write(0xcc00, false, "'     ");
            Write(0x333399, false, "~                 \n                                                            \n                    ");
            Write(0x333399, false, "                                        \n                                           ~               ");
            Write(0x333399, false, " \n           ");
            Write(0x333333, false, "                           ~                     \n                                        ");
            Write(0xcc00, false, ".");
            Write(0x333333, false, " ..   ");
            Write(0xcc00, false, ".  .");
            Write(0x333333, false, "        ~\n                                         '");
            Write(0xcc00, false, ".            ");
            Write(0x333399, false, "~    \n           \n");
            
            Console.ForegroundColor = color;
            Console.WriteLine();
        }

       private static void Write(int rgb, bool bold, string text){
           Console.Write($"\u001b[38;2;{(rgb>>16)&255};{(rgb>>8)&255};{rgb&255}{(bold ? ";1" : "")}m{text}");
       }
    }
}