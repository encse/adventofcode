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
                    
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(".-----------------------------------------------.");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("       \n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                               ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("25");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                               ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("24");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                               ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("23");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                               ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("22");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                               ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("21");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                               ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("20");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("              ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                                ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("19");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("              ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└─┐");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                      ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┌─────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("18");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└──────────────");
                        
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("┤[]├");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────┘");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────┤");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("17");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                     ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──────────────────┘o───┘");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("16");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                     ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└──────────────────");
                        
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("oTo");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("15");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("       ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("─");
                        
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("∧∧∧");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("─────────");
                        
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("oTo");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────────────────");
                        
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("∧∧∧");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──┤");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("14");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("       ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└────────────────────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("o──────┐┌──");
                        
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("┤|├");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──┘");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("13");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                            ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┌─┘└┬───┐o─┐");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("12");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("            ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┌──────────");
                        
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("┤[]├");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──");
                        
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("oTo");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("─┘└───┘┌");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("o┴──┘");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("11");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("            ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└──");
                        
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("∧∧∧");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──────────┐┌─");
                        
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("┤[]├");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────┘└────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("10");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┌───────────┘│");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("───────");
                        
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("[─]");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────┤");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" 9");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("                ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└────");
                        
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("[─]");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("─────┘└──────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┌──────┘");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" 8");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──────────────────");
                        
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("[─]");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──");
                        
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("|(");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──────────┘│o─────┐");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" 7");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("├────────────────");
                        
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("┤[]├");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("o┬o┌───┘┌─────┤");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" 6");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└────┐┌──");
                        
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("┤|├");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("───o");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────────────┘V├─┘o───┴───┐┌┘");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" 5");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("┌───o└┤");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("───");
                        
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("[─]");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──┘o────────┬───┴┴──────────┘└┐");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" 4");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└─────┘└─────────────────┐└o┌─────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("o┬──o┌───┘");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" 3");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("─────────────");
                        
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("┤[]├");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("──────┐└──┴────o└┐├───┘┌──┐");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" 2");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("└──────");
                        
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("[─]");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("────────────");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("o┴──────────┘└────┘o─┘");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("|");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("  ");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" 1");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(" ");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("*");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\n");
                        
                            Console.Write("           ");
                            
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("'-----------------------------------------------'");
                        
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("       \n");
                        
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