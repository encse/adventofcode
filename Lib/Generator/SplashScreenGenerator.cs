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
            var theme = new Dictionary<string[], string>() {
                [new[] { "calendar-edge" }] = "ConsoleColor.Gray",
                [new[] { "calendar-star" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-complete", "calendar-mark-complete" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-verycomplete", "calendar-mark-complete" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-verycomplete", "calendar-mark-verycomplete" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-ornament0" }] = "ConsoleColor.White",
                [new[] { "calendar-ornament1" }] = "ConsoleColor.Green",
                [new[] { "calendar-ornament2" }] = "ConsoleColor.Red",
                [new[] { "calendar-ornament3" }] = "ConsoleColor.Blue",
                [new[] { "calendar-ornament4" }] = "ConsoleColor.Magenta",
                [new[] { "calendar-ornament5" }] = "ConsoleColor.Cyan",
                [new[] { "calendar-ornament3" }] = "ConsoleColor.DarkCyan",


                [new[] { "calendar-tree-star" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-antenna-star" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-antenna-signal0" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-antenna-signal1" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-antenna-signal2" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-antenna-signal3" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-antenna-signal4" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-antenna-signal5" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-tree-ornament0" }] = "ConsoleColor.Red",
                [new[] { "calendar-tree-ornament1" }] = "ConsoleColor.Green",
                [new[] { "calendar-tree-ornament2" }] = "ConsoleColor.Blue",
                [new[] { "calendar-tree-branches" }] = "ConsoleColor.DarkGreen",
                [new[] { "calendar-tree-trunk" }] = "ConsoleColor.Gray",
                [new[] { "calendar-streets" }] = "ConsoleColor.Gray",
                [new[] { "calendar-window-dark" }] = "ConsoleColor.DarkGray",
                [new[] { "calendar-window-red" }] = "ConsoleColor.Red",
                [new[] { "calendar-window-green" }] = "ConsoleColor.Green",
                [new[] { "calendar-window-blue" }] = "ConsoleColor.Blue",
                [new[] { "calendar-window-yellow" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-window-brown" }] = "ConsoleColor.DarkGreen",

                [new[] { "calendar-lightbeam" }] = "ConsoleColor.Yellow",


                [new[] { "calendar-color-d" }] = "ConsoleColor.DarkRed",
                [new[] { "calendar-color-y" }] = "ConsoleColor.Yellow",
                [new[] { "calendar-color-w" }] = "ConsoleColor.White",
                [new[] { "calendar-color-r" }] = "ConsoleColor.Red",
                [new[] { "calendar-color-b" }] = "ConsoleColor.Blue",
                [new[] { "calendar-color-s" }] = "ConsoleColor.Gray",
                [new[] { "calendar-color-g" }] = "ConsoleColor.Green",

                [new[] { "title" }] = "ConsoleColor.Yellow",
            };

            var lines = calendar.Lines.Select(line =>
                new[] { new CalendarToken { Text = "           " } }.Concat(line)).ToList();
            lines.Insert(0, new[]{new CalendarToken {
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
                    var consoleColor = "ConsoleColor.DarkGray";
                    foreach (var kvp in theme) {
                        if (kvp.Key.All(s => token.Styles.Contains(s))) {
                            consoleColor = kvp.Value;
                            break;
                        }
                    }
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
                while (buffer.Length > 0) {
                    var block = buffer.Substring(0, Math.Min(100, buffer.Length));
                    buffer = buffer.Substring(block.Length);
                    block = block.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n");
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