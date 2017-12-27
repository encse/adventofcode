using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Model {

    public class CalendarToken {
        public string[] Styles = new string[0];
        public string Text { get; set; }
    }

    public class Calendar {
        public int Year;

        public IReadOnlyList<IReadOnlyList<CalendarToken>> Lines { get; private set; }

        public static Calendar Parse(int year, string html) {
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

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
                var styles = textNode.Ancestors()
                    .SelectMany(node => new string[]{node.Attributes["class"]?.Value, node.Attributes["style"]?.Value})
                    .Where(style => style != null)
                    .ToArray();

                var text = textNode.InnerText;
                var widthSpec = styles.FirstOrDefault(style => style.StartsWith("width:"));
                if(widthSpec != null){
                    var m = Regex.Match(widthSpec, "[.0-9]+");
                    if(m.Success){
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
                        Styles = styles,
                        Text = HtmlEntity.DeEntitize(text.Substring(i, iNext - i))
                    });

                    if (iNext < text.Length) {
                        line = new List<CalendarToken>();
                        lines.Add(line);
                    }
                    i = iNext + 1;
                }
            }


            return new Calendar { Year = year, Lines = lines };
        }
    }
}