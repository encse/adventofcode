
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2017.Model {

    public class CalendarToken {
        public string Style { get; set; }
        public string Text { get; set; }
    }

    public class Calendar {
        public IReadOnlyList<IReadOnlyList<CalendarToken>> Lines { get; private set; }

        public static Calendar Parse(string html) {
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

            var calendar = document.DocumentNode.SelectSingleNode("//*[contains(@class,'calendar')]");

            foreach (var script in calendar.SelectNodes(".//script").ToList()) {
                script.Remove();
            }

            var lines = new List<List<CalendarToken>>();
            var line = new List<CalendarToken>();
            lines.Add(line);

            foreach (var textNode in calendar.SelectNodes(".//text()")) {
                var style =
                    textNode.ParentNode.Attributes["class"]?.Value ??
                    textNode.ParentNode.ParentNode.Attributes["class"]?.Value;

                if (textNode.InnerText.EndsWith("\n")) {
                    line.Add(new CalendarToken {
                        Style = style,
                        Text = textNode.InnerText.Replace("\n", "")
                    });
                    line = new List<CalendarToken>();
                    lines.Add(line);
                } else if (textNode.InnerText.Contains("\n")) {
                    throw new NotImplementedException("Not supported 'new line inside'");
                } else {
                    line.Add(new CalendarToken { Style = style, Text = textNode.InnerText });
                }
            }

            return new Calendar { Lines = lines };
        }
    }
}