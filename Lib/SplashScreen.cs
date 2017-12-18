
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

                var document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(File.ReadAllText("splashscreen.in"));

                var theme = new Dictionary<string, ConsoleColor>() {
                    ["calendar-edge"] = ConsoleColor.Gray,
                    ["calendar-star"] = ConsoleColor.Yellow,
                    ["calendar-ornament0"] = ConsoleColor.White,
                    ["calendar-ornament1"] = ConsoleColor.Green,
                    ["calendar-ornament2"] = ConsoleColor.Red,
                    ["calendar-ornament3"] = ConsoleColor.Blue,
                    ["calendar-ornament4"] = ConsoleColor.Magenta,
                    ["calendar-ornament5"] = ConsoleColor.Cyan,
                    ["calendar-ornament3"] = ConsoleColor.DarkCyan,
                };

                foreach (var line in document.DocumentNode.SelectSingleNode("pre").ChildNodes) {
                    if (line.SelectNodes(".//i") != null) {
                        Console.Write("             ");
                        foreach (var col in line.SelectNodes(".//i")) {
                            Console.ForegroundColor =
                                col.ParentNode.Attributes["class"] != null && theme.TryGetValue(col.ParentNode.Attributes["class"].Value, out var color) ?
                                color :
                                ConsoleColor.DarkGray;
                            Console.Write(col.InnerText);
                        }
                        Console.WriteLine();
                    }
                }

            } catch {
                Console.ForegroundColor = defaultColor;
            }
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