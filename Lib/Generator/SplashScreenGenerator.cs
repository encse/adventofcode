using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using AdventOfCode.Model;

namespace AdventOfCode.Generator {
    
    public class SplashScreenGenerator {
        public string Generate(Calendar calendar) {
            string calendarPrinter = CalendarPrinter(calendar);
            return $@"
                |using System;
                |
                |namespace AdventOfCode.Y{calendar.Year} {{
                |
                |    class SplashScreenImpl : AdventOfCode.SplashScreen {{
                |
                |        public void Show() {{
                |
                |            var color = Console.ForegroundColor;
                |            {calendarPrinter.Indent(12)}
                |            Console.ForegroundColor = color;
                |            Console.WriteLine();
                |        }}
                |
                |       private static void Write(ConsoleColor color, string text){{
                |           Console.ForegroundColor = color;
                |           Console.Write(text);
                |       }}
                |    }}
                |}}".StripMargin();
        }

        private string CalendarPrinter(Calendar calendar) {
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


                ["calendar-tree-star"] = "ConsoleColor.Yellow",
                ["calendar-antenna-star"] = "ConsoleColor.Yellow",
                ["calendar-antenna-signal0"] = "ConsoleColor.Yellow",
                ["calendar-antenna-signal1"] = "ConsoleColor.Yellow",
                ["calendar-antenna-signal2"] = "ConsoleColor.Yellow",
                ["calendar-antenna-signal3"] = "ConsoleColor.Yellow",
                ["calendar-antenna-signal4"] = "ConsoleColor.Yellow",
                ["calendar-antenna-signal5"] = "ConsoleColor.Yellow",
                ["calendar-tree-ornament0"] = "ConsoleColor.Red",
                ["calendar-tree-ornament1"] = "ConsoleColor.Green",
                ["calendar-tree-ornament2"] = "ConsoleColor.Blue",
                ["calendar-tree-branches"] = "ConsoleColor.DarkGreen",
                ["calendar-tree-trunk"] = "ConsoleColor.Gray",
                ["calendar-streets"] = "ConsoleColor.Gray",
                ["calendar-window-dark"] = "ConsoleColor.DarkGray",
                ["calendar-window-red"] = "ConsoleColor.Red",
                ["calendar-window-green"] = "ConsoleColor.Green",
                ["calendar-window-blue"] = "ConsoleColor.Blue",
                ["calendar-window-yellow"] = "ConsoleColor.Yellow",
                ["calendar-window-brown"] = "ConsoleColor.DarkGreen",

                ["title"] = "ConsoleColor.Yellow",
            };

            var lines = calendar.Lines.Select(line =>
                new[] { new CalendarToken { Text = "           " } }.Concat(line)).ToList();
            lines.Insert(0, new []{new CalendarToken {
                Styles = new []{"title"},
                Text = $@"
                    |  __   ____  _  _  ____  __ _  ____     __  ____     ___  __  ____  ____         
                    | / _\ (    \/ )( \(  __)(  ( \(_  _)   /  \(  __)   / __)/  \(    \(  __)        
                    |/    \ ) D (\ \/ / ) _) /    /  )(    (  O )) _)   ( (__(  O )) D ( ) _)         
                    |\_/\_/(____/ \__/ (____)\_)__) (__)    \__/(__)     \___)\__/(____/(____)  {calendar.Year}
                    |"
                .StripMargin()
            }});

            var bw = new BufferWriter();
            foreach (var line in lines) {
                foreach (var token in line) {
                    var style = token.Styles.FirstOrDefault(styleT => theme.ContainsKey(styleT));
                    var consoleColor = style != null ? theme[style] : "ConsoleColor.DarkGray";
                    bw.Write(consoleColor, token.Text);
                }

                bw.Write(null, "\n");
            }
            return bw.GetContent();
        }

        class BufferWriter {
            StringBuilder sb = new StringBuilder();
            string bufferColor = null;
            string buffer = "";

            public void Write(string color, string text) {
                if (!string.IsNullOrWhiteSpace(text)) {
                    if (color != bufferColor && !string.IsNullOrWhiteSpace(buffer)) {
                        Flush();
                    }
                    bufferColor = color;
                }
                buffer += text;
            }

            private void Flush() {
                while(buffer.Length > 0){
                    var block = buffer.Substring(0, Math.Min(100, buffer.Length));
                    buffer = buffer.Substring(block.Length);
                    block = block.Replace("\\", "\\\\").Replace("\n", "\\n");
                    sb.AppendLine($@"Write({bufferColor}, ""{block}"");");
                }
                buffer = "";
            }

            public string GetContent() {
                Flush();
                return sb.ToString();
            }
        }
    }
}