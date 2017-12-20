
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

namespace AdventOfCode.Model {
    public class Problem {
        public string Title { get; private set; }
        public string ContentMd { get; private set; }
        public int Day { get; private set; }
        public string Input { get; private set; }

        public static Problem Parse(int day, string url, string html, string input) {

            var document = new HtmlDocument();
            document.LoadHtml(html);
            var md = $"original source: [{url}]({url})\n";
            foreach (var article in document.DocumentNode.SelectNodes("//article")) {
                md += UnparseList("", article) + "\n";
            }
            var title = HtmlEntity.DeEntitize(document.DocumentNode.SelectNodes("//h2").First().InnerText);

            var match = Regex.Match(title, ".*: (.*) ---");
            if (match.Success) {
                title = match.Groups[1].Value;
            }
            return new Problem { Day = day, Title = title, ContentMd = md, Input = input };
        }

        static string UnparseList(string sep, HtmlNode node) {
            return string.Join(sep, node.ChildNodes.SelectMany(Unparse));
        }

        static IEnumerable<string> Unparse(HtmlNode node) {
            switch (node.Name) {
                case "h2":
                    yield return "## " + UnparseList("", node) + "\n";
                    break;
                case "p":
                    yield return UnparseList("", node) + "\n";
                    break;
                case "em":
                    yield return "*" + UnparseList("", node) + "*";
                    break;
                case "code":
                    if (node.ParentNode.Name == "pre") {
                        yield return UnparseList("", node);
                    } else {
                        yield return "`" + UnparseList("", node) + "`";
                    }
                    break;
                case "span":
                    yield return UnparseList("", node);
                    break;
                case "s":
                    yield return "~~" + UnparseList("", node) + "~~";
                    break;
                case "ul":
                    foreach (var unparsed in node.ChildNodes.SelectMany(Unparse)) {
                        yield return unparsed;
                    }
                    break;
                case "li":
                    yield return " - " + UnparseList("", node);
                    break;
                case "pre":
                    yield return "```\n";
                    var freshLine = true;
                    foreach (var item in node.ChildNodes) {
                        foreach (var unparsed in Unparse(item)) {
                            freshLine = unparsed[unparsed.Length - 1] == '\n';
                            yield return unparsed;
                        }
                    }
                    if (freshLine) {
                        yield return "```\n";
                    } else {
                        yield return "\n```\n";
                    }
                    break;
                case "a":
                    yield return "[" + UnparseList("", node) + "](" + node.Attributes["href"].Value + ")";
                    break;
                case "#text":
                    yield return HtmlEntity.DeEntitize(node.InnerText);
                    break;
                default:
                    throw new NotImplementedException(node.InnerHtml);
            }
        }
    }
}