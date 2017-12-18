
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

    class CalendarParser {

        public IEnumerable<(string style, string text)> Parse(string html) {
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

            var calendar = document.DocumentNode.SelectSingleNode("//*[contains(@class,'calendar')]");

            foreach (var line in calendar.ChildNodes) {
                if (line.SelectNodes(".//i") != null) {
                    foreach (var col in line.SelectNodes(".//i")) {
                        yield return (col.ParentNode.Attributes["class"]?.Value, col.InnerText);
                    }
                    yield return (null, "\n");
                }
            }
        }
    }
}