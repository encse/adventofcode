
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
using AdventOfCode2017.Templates;

namespace AdventOfCode2017 {

    public class CalendarToken{
        public string Style { get; set; }
        public string Text { get; set; }
    }
    
    class CalendarParser {

        public IEnumerable<CalendarToken> Parse(string html) {
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

            var calendar = document.DocumentNode.SelectSingleNode("//*[contains(@class,'calendar')]");

            foreach (var line in calendar.ChildNodes) {
                if (line.SelectNodes(".//i") != null) {
                    string lastStyle = null;
                    var text = "";
                    foreach (var col in line.SelectNodes(".//i")) {
                        var style = col.ParentNode.Attributes["class"]?.Value;
                        if (style != lastStyle) {
                            yield return new CalendarToken { Style = lastStyle, Text = text };
                            text = "";
                        }
                        lastStyle = style;
                        text += col.InnerText;
                    }
                    yield return new CalendarToken { Style = lastStyle, Text = text + "\n" };
                }
            }
        }
    }
}