using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AngleSharp.Dom;

namespace AdventOfCode.Model {

    public class CalendarToken {
        public string Text { get; set; }
        public int ConsoleColor { get; set; }
        public bool Bold {get; set;}
    }

    public class Calendar {
        public int Year;

        public Dictionary<string[], int> Theme = new Dictionary<string[], int>();
        public IReadOnlyList<IReadOnlyList<CalendarToken>> Lines { get; private set; }

        public static Calendar Parse(int year, IDocument document) {

            var theme = new Dictionary<string[], int>();

            // anglesharp bug, it doesn't handle external stylehseets well.
            var q = document.CreateElement("style");
            q.SetInnerText($@"
                .calendar > span {{
                    color: #333333;
                }}
                .calendar > a {{
                    text-decoration: none;
                    color: #666666;
                    outline: none;
                    cursor: default;
                }}

                .calendar .calendar-day {{ color: #666666; }}
                .calendar a .calendar-day {{ color: #cccccc; }}
                .calendar a .calendar-mark-complete,
                .calendar a.calendar-complete     .calendar-mark-complete,
                .calendar a.calendar-verycomplete .calendar-mark-complete {{ color: #ffff66; }}
                .calendar a.calendar-verycomplete .calendar-mark-verycomplete {{ color: #ffff66; }}
            ");

            document.Head.Append(q);

            foreach (var item in document.QuerySelectorAll("link").ToList()) {
                item.Remove();
            }

            foreach (var item in document.QuerySelectorAll("script").ToList()) {
                item.Remove();
            }

            var calendar = document.QuerySelector(".calendar");

            var lines = new List<List<CalendarToken>>();
            var line = new List<CalendarToken>();
            lines.Add(line);


            foreach (var textNode in GetText(calendar)) {
                var text = textNode.Text();
                var style = textNode.ParentElement.ComputeCurrentStyle();
                var consoleColor = ParseRgbaColor(style["color"]);
                var bold = !string.IsNullOrEmpty(style["text-shadow"]);

                var widthSpec = string.IsNullOrEmpty(style["width"]) ? 
                    textNode.ParentElement.ParentElement.ComputeCurrentStyle()["width"] : style["width"];
                if (widthSpec != null) {

                    var m = Regex.Match(widthSpec, "[.0-9]+");
                    if (m.Success) {
                        var width = double.Parse(m.Value) * 1.7;
                        var c = (int)Math.Round(width - text.Length, MidpointRounding.AwayFromZero);
                        if (c > 0) {
                            text += new string(' ', c);
                        }
                    }
                }

                var i = 0;
                while (i < text.Length) {
                    var iNext = text.IndexOf("\n", i);
                    if (iNext == -1) {
                        iNext = text.Length;
                    }

                    line.Add(new CalendarToken {
                        Text = text.Substring(i, iNext - i),
                        ConsoleColor = consoleColor,
                        Bold = bold 
                    });

                    if (iNext < text.Length) {
                        line = new List<CalendarToken>();
                        lines.Add(line);
                    }
                    i = iNext + 1;
                }
            }


            return new Calendar { Year = year, Theme = theme, Lines = lines };
        }

        private static IEnumerable<INode> GetText(INode element) {
            if (element.NodeType == NodeType.Text) {
                yield return element;
            } else {
                element = element.FirstChild;
                while (element != null) {
                    foreach (var text in GetText(element)) {
                        yield return text;
                    }
                    element = element.NextSibling;
                }
            }
        }

        private static int ParseRgbaColor(string st) {
            Regex regex = new Regex(@"rgba\((?<r>\d{1,3}), (?<g>\d{1,3}), (?<b>\d{1,3}), (?<a>\d{1,3})\)");
            Match match = regex.Match(st);
            if (match.Success) {
                int r = int.Parse(match.Groups["r"].Value);
                int g = int.Parse(match.Groups["g"].Value);
                int b = int.Parse(match.Groups["b"].Value);

                return (r << 16) + (g << 8) + b;
            }
            return 0x888888;
        }
    }

}