
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace AdventOfCode2017 {

    class SplashScreen {

        public static void Show() {

            var defaultColor = Console.ForegroundColor;
            Write(ConsoleColor.Gray, "           .-----------------------------------------------.       \n           |                                               |  ");
              Write(ConsoleColor.DarkGray, "25\n           ");
              Write(ConsoleColor.Gray, "|                                               |  ");
              Write(ConsoleColor.DarkGray, "24\n           ");
              Write(ConsoleColor.Gray, "|                                               |  ");
              Write(ConsoleColor.DarkGray, "23\n           ");
              Write(ConsoleColor.Gray, "|                                               |  ");
              Write(ConsoleColor.DarkGray, "22\n           ");
              Write(ConsoleColor.Gray, "|                                 ");
              Write(ConsoleColor.Yellow, "*             ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "21\n           ");
              Write(ConsoleColor.Gray, "|                                 ");
              Write(ConsoleColor.DarkGray, "└───────┬───");
              Write(ConsoleColor.Yellow, "* ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "20 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "|                             ");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "─┐     ┌──┐└o┌─┤ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "19 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "──────────────────");
              Write(ConsoleColor.Magenta, "oTo");
              Write(ConsoleColor.DarkGray, "─────┘V└──┐┌─┘o─┴──┘o┘ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "18 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "└────");
              Write(ConsoleColor.Cyan, "┤[]├");
              Write(ConsoleColor.DarkGray, "┬─");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "o──┬────────────┘┌──┘├─────────┐ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "17 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "┌─");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "──");
              Write(ConsoleColor.Cyan, "┤[]├");
              Write(ConsoleColor.DarkGray, "┐└┐└──┐└─────────┐┌──┘o──┴─────o┌──┘ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "16 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "└o└───");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "┌o│o┘┌─┐└─────┐┌───┘└────");
              Write(ConsoleColor.Cyan, "┤[]├");
              Write(ConsoleColor.DarkGray, "────┐└──┐ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "15 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "─────┘└─┴──┘o┴──────┘└─┬┴┴┴┴┴┐┌───┬┴┴┴┴┴┬──┤ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "14 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "└─────────────");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "┌────────┤     ├│o─┐┤     ├─┐│ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "13 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "────┬┴┴┴┴┴┬──┘└────┬──┐┤ DECR├┘┌─┘┤    4├─┘= ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "12 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "├────┤     ├───────");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "└o┌┘┤ YPTR├─┴──┤    3├──┐ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "11 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "│o───┤   de├─o┌────┘┌");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "└─┴┬┬┬┬┬┴────┤   V0├──┘ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, "10 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "└────┤  bug├──┘┌────┘└──────────┬──┤   R0├──");
              Write(ConsoleColor.Yellow, "* ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, " 9 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "┌────┴┬┬┬┬┬┘┌──┴───");
              Write(ConsoleColor.Green, "|(");
              Write(ConsoleColor.DarkGray, "──o");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "──────┐└──┴┬┬┬┬┬┴──┘ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, " 8 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "└───┐┌──────┴o");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "─────────┘V┌────┘o────┘└─────┐ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, " 7 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "┌───┘└┐");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "──────┘o────┬────┴┘o──┬─────────────┤ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, " 6 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "└─────┘└───────────");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "│o─┬─────┐└───┐┌────o┌──┘ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, " 5 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "o──");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "──────");
              Write(ConsoleColor.Magenta, "oTo");
              Write(ConsoleColor.DarkGray, "──────┘└──┘┌───o└────┘├─");
              Write(ConsoleColor.White, "∧∧∧");
              Write(ConsoleColor.DarkGray, "─┘o─┐ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, " 4 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "┌──┘┌──");
              Write(ConsoleColor.Red, "[─]");
              Write(ConsoleColor.DarkGray, "───────────");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "┌─┴────┬───o┌┘┌─┬─────┘ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, " 3 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "├───┘o────────┬─┐");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "───┘=┌─────┘┌───┘o┴o└─────┐ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, " 2 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "| ");
              Write(ConsoleColor.DarkGray, "└───────");
              Write(ConsoleColor.Magenta, "oTo");
              Write(ConsoleColor.DarkGray, "───┘o┘└────");
              Write(ConsoleColor.Yellow, "*");
              Write(ConsoleColor.DarkGray, "└──");
              Write(ConsoleColor.DarkCyan, "┤|├");
              Write(ConsoleColor.DarkGray, "─┴─────────────┘ ");
              Write(ConsoleColor.Gray, "|  ");
              Write(ConsoleColor.DarkGray, " 1 ");
              Write(ConsoleColor.Yellow, "**\n           ");
              Write(ConsoleColor.Gray, "'-----------------------------------------------'       \n           \n");
              
            Console.ForegroundColor = defaultColor;
            Console.WriteLine();
            Console.WriteLine(
                string.Join("\n", @"
                      _      _             _          __    ___         _       ___ __  _ ____ 
                     /_\  __| |_ _____ _ _| |_   ___ / _|  / __|___  __| |___  |_  )  \/ |__  |
                    / _ \/ _` \ V / -_) ' \  _| / _ \  _| | (__/ _ \/ _` / -_)  / / () | | / / 
                   /_/ \_\__,_|\_/\___|_||_\__| \___/_|    \___\___/\__,_\___| /___\__/|_|/_/  
                  "
                .Split('\n').Skip(1).Select(x => x.Substring(18))));
        }

       private static void Write(ConsoleColor color, string text){
           Console.ForegroundColor = color;
           Console.Write(text);
       }
    }
}