using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using AdventOfCode2017.Model;

namespace AdventOfCode2017.Generator {
    
    public class SplashScreenGenerator {
        public string Generate(IEnumerable<CalendarToken> calendar) {
            string calendarPrinter = CalendarPrinter(calendar);
            return $@"
                |using System;
                |using System.IO;
                |using System.Linq;
                |using System.Collections.Generic;
                |using HtmlAgilityPack;
                |
                |namespace AdventOfCode2017 {{
                |
                |    class SplashScreen {{
                |
                |        public static void Show() {{
                |
                |            var defaultColor = Console.ForegroundColor;
                |            try {{
                |               {calendarPrinter.Indent(15)}
                |            }} finally {{
                |                Console.ForegroundColor = defaultColor;
                |            }}
                |            Console.WriteLine();
                |            Console.WriteLine(
                |                string.Join(""\n"", @""
                |                      _      _             _          __    ___         _       ___ __  _ ____ 
                |                     /_\  __| |_ _____ _ _| |_   ___ / _|  / __|___  __| |___  |_  )  \/ |__  |
                |                    / _ \/ _` \ V / -_) ' \  _| / _ \  _| | (__/ _ \/ _` / -_)  / / () | | / / 
                |                   /_/ \_\__,_|\_/\___|_||_\__| \___/_|    \___\___/\__,_\___| /___\__/|_|/_/  
                |                  ""
                |                .Split('\n').Skip(1).Select(x => x.Substring(18))));
                |        }}
                |    }}
                |}}".StripMargin();
        }

        private string CalendarPrinter(IEnumerable<CalendarToken> calendar){
            StringBuilder sb = new StringBuilder();

            var theme = new Dictionary<string, string>() {
                ["calendar-edge"] = "ConsoleColor.Gray",
                ["calendar-star"] = "ConsoleColor.Yellow",
                ["calendar-mark-complete"] = "ConsoleColor.Yellow",
                ["calendar-mark-verycomplete"] = "ConsoleColor.Yellow",
                ["calendar-ornament0"] = "ConsoleColor.White",
                ["calendar-ornament1"] = "ConsoleColor.Green",
                ["calendar-ornament2"] = "ConsoleColor.Red",
                ["calendar-ornament3"] = "ConsoleColor.Blue",
                ["calendar-ornament4"] = "ConsoleColor.Magenta",
                ["calendar-ornament5"] = "ConsoleColor.Cyan",
                ["calendar-ornament3"] = "ConsoleColor.DarkCyan",
            };

            var tab = "           ";
            
            sb.AppendLine($@"Console.Write(""{tab}"");");
            foreach (var token in calendar) {
                var consoleColor = token.Style != null && theme.TryGetValue(token.Style, out var themeColor)
                    ? themeColor
                    : "ConsoleColor.DarkGray";

                var tokenLiteral = token.Text.Replace("\n", "\\n");
                
                sb.AppendLine($@"Console.ForegroundColor = {consoleColor};");
                sb.AppendLine($@"Console.Write(""{tokenLiteral}"");");
                if (token.Text.EndsWith("\n")) {
                    sb.AppendLine($@"Console.Write(""{tab}"");");
                }
            }
            return sb.ToString();
        }
    }
}