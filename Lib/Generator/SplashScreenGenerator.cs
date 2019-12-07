using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using AdventOfCode.Model;
using ExCSS;

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
                |       private static void Write(int rgb, string text){{
                |           Console.Write($""\u001b[38;2;{{(rgb>>16)&255}};{{(rgb>>8)&255}};{{rgb&255}}m{{text}}"");
                |       }}
                |    }}
                |}}".StripMargin();
        }

        private string CalendarPrinter(Calendar calendar) {
            var theme = new Dictionary<string[], int>() {
                [new[] { ".title" }] = 0xffff66,
                [new[] { ".calendar-star" }] = 0xffff66,
                [new[] { ".calendar-complete", ".calendar-mark-complete" }] = 0xffff66,
                [new[] { ".calendar-verycomplete", ".calendar-mark-complete" }] = 0xfff66,
                [new[] { ".calendar-verycomplete", ".calendar-mark-verycomplete" }] = 0xffff66,

                [new[] { ".calendar-edge" }] = 0xcccccc,
                [new[] { ".calendar-print-edge" }] = 0x999999,
                [new[] { ".calendar-print-text" }] = 0xcccccc,

                [new[] { ".calendar-ornament0" }] = 0x0066ff,
                [new[] { ".calendar-ornament1" }] = 0xff9900,
                [new[] { ".calendar-ornament2" }] = 0xff0000,
                [new[] { ".calendar-ornament3" }] = 0xffff66,

                [new[] { ".calendar-tree-star" }] = 0xffff66,
                [new[] { ".calendar-antenna-star" }] = 0xffff66,
                [new[] { ".calendar-antenna-signal0" }] = 0xffff66,
                [new[] { ".calendar-antenna-signal1" }] = 0xffff66,
                [new[] { ".calendar-antenna-signal2" }] = 0xffff66,
                [new[] { ".calendar-antenna-signal3" }] = 0xffff66,
                [new[] { ".calendar-antenna-signal4" }] = 0xffff66,
                [new[] { ".calendar-antenna-signal5" }] = 0xffff66,
                [new[] { ".calendar-tree-ornament0" }] = 0x0066ff,
                [new[] { ".calendar-tree-ornament1" }] = 0xff9900,
                [new[] { ".calendar-tree-ornament2" }] = 0xff0000,
                [new[] { ".calendar-tree-branches" }] = 0x009900,
                [new[] { ".calendar-tree-trunk" }] = 0xaaaaaa,
                [new[] { ".calendar-trunk" }] = 0xcccccc,
                [new[] { ".calendar-streets" }] = 0x666666,
                [new[] { ".calendar-window-dark" }] = 0x333333,
                [new[] { ".calendar-window-red" }] = 0xff0000,
                [new[] { ".calendar-window-green" }] = 0x009900,
                [new[] { ".calendar-window-blue" }] = 0x0066ff,
                [new[] { ".calendar-window-yellow" }] = 0xffff66,
                [new[] { ".calendar-window-brown" }] = 0x553322,

                [new[] { ".calendar-lightbeam" }] = 0xffff66,

                [new[] { ".calendar-color-s" }] = 0x999999,
                [new[] { ".calendar-color-b" }] = 0x0066ff,
                [new[] { ".calendar-color-e" }] = 0xcccccc,
                [new[] { ".calendar-color-r" }] = 0xff0000,
                [new[] { ".calendar-color-d" }] = 0x880000,
                [new[] { ".calendar-color-n" }] = 0x886655,
                [new[] { ".calendar-color-k" }] = 0xcccccc,
                [new[] { ".calendar-color-g" }] = 0x009900,
                [new[] { ".calendar-color-w" }] = 0xcccccc,
                [new[] { ".calendar-color-t" }] = 0xcccccc,
                [new[] { ".calendar-color-i" }] = 0xff0000,
                [new[] { ".calendar-color-y" }] = 0xffff66,

            };

            foreach (var kvp in calendar.Theme) {
                theme[kvp.Key] = kvp.Value;
            }

            foreach (var kvp in theme) {
                Console.WriteLine(string.Join(" ", kvp.Key) + " " + kvp.Value);
            }

            Console.WriteLine("...");

            var lines = calendar.Lines.Select(line =>
                new[] { new CalendarToken { Text = "           " } }.Concat(line)).ToList();
            lines.Insert(0, new[]{new CalendarToken {
                Styles = new []{".title"},
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
                    var consoleColor = 0x888888;
                    var found = false;

                    if (token.ConsoleColor == null) {
                        foreach (var kvp in theme) {
                            if (kvp.Key.All(s => token.Styles.Contains(s))) {
                                consoleColor = kvp.Value;
                                break;
                            }
                        }
                    } else {
                        consoleColor = token.ConsoleColor.Value;
                    }

                    if (!found) {
                        Console.WriteLine(string.Join(" ", token.Styles));
                    }
                    bw.Write(consoleColor, token.Text);
                }

                bw.Write(-1, "\n");
            }
            return bw.GetContent();
        }

        class BufferWriter {
            StringBuilder sb = new StringBuilder();
            int bufferColor = -1;
            string buffer = "";

            public void Write(int color, string text) {
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
                    sb.AppendLine($@"Write(0x{bufferColor.ToString("x")}, ""{block}"");");
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