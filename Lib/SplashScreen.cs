using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace AdventOfCode2017 {

    class SplashScreen {

        public static void Show() {

            var defaultColor = Console.ForegroundColor;
            try {


                    Console.Write("           ");
                    
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(".-----------------------------------------------.\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                               ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                               ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                               ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                               ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                               ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                               ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                         ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("     ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                         ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└───");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("─────┬───");
                        
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("|(");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("─┘");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┌─────────┬┴┴┴┬────────");
                        
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("oTo");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("─────┘o────┘┌────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┐");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└─┐┌──────┤  1├───────────────────────┘");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("───┘│");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┌─┘└────┐");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┤ A0├────────┬┴┴┴┬───");
                        
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("┤[]├");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────┘o─┬─┘");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└───┐┌──┘└┤ P7├┬┴┴┴┴┴┬─┤   ├────");
                        
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("∧∧∧");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└─o");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┌───┘└───┐┤ L1├┤     ├─┤  1├");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("───────────┐└──┐");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└────────┴┴┬┬┬┴┤     ├─┤  2├└─────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("o───┐└───┤");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────");
                        
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("∧∧∧");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("───────┤ 3141├o┤  v├──────┘┌───┴───o│");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└────────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┌────┤ 5926├─┴┬┬┬┴┬─────┐└────────┘");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────────┘└────┴┬┬┬┬┬┴─────┐└────o└──");
                        
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("┤[]├");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("───┐");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("├────────────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("o───┴─┘o─────┴─────");
                        
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("|(");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("─────────┘");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("│o─────┬────┐└─────┬────");
                        
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("┤[]├");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────────────────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└──────┘o───┘");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────┐└───────────┐o─┬──o┌─────┤");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──────");
                        
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("|(");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────┘o───┴──┐┌─o┌───┬o│┌─┘┌──┘o────┤");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└────────");
                        
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("∧∧∧");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("───────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("V│└──┴──┐└─┘└─┐└───────┐=");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──────────────────┘└┴─────┐└─────┴─────o┌┐└┐");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└──────────────");
                        
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("[─]");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("o───┴─────────────┘└─┘");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("'-----------------------------------------------'\n");
                        
                            Console.Write("           ");
                            
            } finally {
                Console.ForegroundColor = defaultColor;
            }
            Console.WriteLine();
            Console.WriteLine(
                string.Join("\n", @"
                      _      _             _          __    ___         _       ___ __  _ ____ 
                     /_\  __| |_ _____ _ _| |_   ___ / _|  / __|___  __| |___  |_  )  \/ |__  |
                    / _ \/ _` \ V / -_) ' \  _| / _ \  _| | (__/ _ \/ _` / -_)  / / () | | / / 
                   /_/ \_\__,_|\_/\___|_||_\__| \___/_|    \___\___/\__,_\___| /___\__/|_|/_/  
                   "
                .Split('\n').Skip(1).Select(x => x.Substring(19))));
        }
    }
}