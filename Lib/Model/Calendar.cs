using System;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using ExCSS;

namespace AdventOfCode.Model {

    public class CalendarToken {
        public string[] Styles = new string[0];
        public string Text { get; set; }
        public int? ConsoleColor { get; set; }
    }

    public class Calendar {
        public int Year;

        public Dictionary<string[], int> Theme = new Dictionary<string[], int>();
        public IReadOnlyList<IReadOnlyList<CalendarToken>> Lines { get; private set; }

        public static Calendar Parse(int year, string html) {
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

            var theme = new Dictionary<string[], int>();

            var styleNodes = document.DocumentNode.SelectNodes("//style");
            var styleSheetParser = new StylesheetParser();

            foreach (var style in styleNodes) {

                var stylesheet = styleSheetParser.Parse(style.InnerText);
                foreach (ExCSS.StyleRule rule in stylesheet.StyleRules) {

                    var color = ParseRgbColor(rule.Style.Color);
                    if(color != null){
                        theme.Add(rule.Selector.Text.Split(" "), color.Value);
                    }
                }
            }

            var calendar = document.DocumentNode.SelectSingleNode("//*[contains(@class,'calendar')]");

            if (calendar.SelectNodes(".//script") != null) {
                foreach (var script in calendar.SelectNodes(".//script").ToList()) {
                    script.Remove();
                }
            }

            var lines = new List<List<CalendarToken>>();
            var line = new List<CalendarToken>();
            lines.Add(line);

            foreach (var textNode in calendar.SelectNodes(".//text()")) {
                var styles = new List<string>();
                int? consoleColor = null;
                foreach (var node in textNode.Ancestors()) {
                    if (node.Attributes["class"] != null) {
                        styles.AddRange(node.Attributes["class"].Value.Split(' ').Select(x => "." + x));
                    }

                    if (node.Attributes["style"] != null) {
                        var miniStyleSheet = "x { " + node.Attributes["style"].Value + " } ";
                        var stylesheet = styleSheetParser.Parse(miniStyleSheet);
                        if (consoleColor == null) {
                            consoleColor = ParseRgbColor(stylesheet.StyleRules.Cast<StyleRule>().Single().Style.Color);
                        }
                    }
                }

                var text = textNode.InnerText;
                var widthSpec = styles.FirstOrDefault(style => style.StartsWith("width:"));
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
                        Styles = styles.ToArray(),
                        Text = HtmlEntity.DeEntitize(text.Substring(i, iNext - i)),
                        ConsoleColor = consoleColor
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

        private static int? ParseRgbColor(string st) {
            Regex regex = new Regex(@"rgb\((?<r>\d{1,3}), (?<g>\d{1,3}), (?<b>\d{1,3})\)");
            Match match = regex.Match(st);
            if (match.Success) {
                int r = int.Parse(match.Groups["r"].Value);
                int g = int.Parse(match.Groups["g"].Value);
                int b = int.Parse(match.Groups["b"].Value);

                return (r << 16) + (g << 8) + b;
            }
            return null;
        }
    }

}