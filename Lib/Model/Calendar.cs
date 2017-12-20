using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Text;

namespace AdventOfCode.Model {

    public class CalendarToken {
        public string Style { get; set; }
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
                var style =
                    textNode.ParentNode.Attributes["class"]?.Value ??
                    textNode.ParentNode.ParentNode.Attributes["class"]?.Value;

                var i = 0;
                while (i < textNode.InnerText.Length) {
                    var iNext = textNode.InnerText.IndexOf("\n", i);
                    if (iNext == -1) {
                        iNext = textNode.InnerText.Length;
                    }

                    line.Add(new CalendarToken {
                        Style = style,
                        Text = HtmlEntity.DeEntitize(textNode.InnerText.Substring(i, iNext - i))
                    });

                    if (iNext < textNode.InnerText.Length) {
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