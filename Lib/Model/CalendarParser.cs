
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

    class CalendarParser {

        public IEnumerable<CalendarToken> Parse(string html) {
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

            var calendar = document.DocumentNode.SelectSingleNode("//*[contains(@class,'calendar')]");


            foreach (var script in calendar.SelectNodes(".//script").ToList()) {
                script.Remove();
            }

            string lastStyle = null;
            var text = "";
            foreach (var textNode in calendar.SelectNodes(".//text()")) {
                var style =
                    textNode.ParentNode.Attributes["class"]?.Value ??
                    textNode.ParentNode.ParentNode.Attributes["class"]?.Value;

                if (style != lastStyle && text != "") {
                    yield return new CalendarToken { Style = lastStyle, Text = text };
                    text = "";
                }
                lastStyle = style;
                text += textNode.InnerText;
            }
            
            if (text != "") {
                yield return new CalendarToken { Style = lastStyle, Text = text };
            }

        }
    }
}