
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

namespace AdventOfCode2017 {

    class Updater {

        public static async Task Update(int day) {
            if (!System.Environment.GetEnvironmentVariables().Contains("SESSION")) {
                throw new Exception("Specify SESSION environment variable");
            }

            var dir = $"Day{day.ToString("00")}";
            CopyDirectory("Day00", dir);

            var title = "???";

            var cookieContainer = new CookieContainer();
            using (var client = new HttpClient(
                new HttpClientHandler {
                    CookieContainer = cookieContainer,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                })) {
                var baseAddress = new Uri("https://adventofcode.com/");
                client.BaseAddress = baseAddress;
                cookieContainer.Add(baseAddress, new Cookie("session", System.Environment.GetEnvironmentVariable("SESSION")));

                await UpdateSplashScreen(client);
                title = await UpdateReadme(client, day);
                await UpdateInput(client, day);
            }

            UpdateSolutionTemplate(day, title);
        }

        static void CopyDirectory(string from, string to) {
            if (!Directory.Exists(to)) {
                Console.WriteLine($"Creating directory {to}");
                Directory.CreateDirectory(to);
            }
            foreach (string newPath in Directory.GetFiles(from, "*.*", SearchOption.AllDirectories)) {
                var fileTo = newPath.Replace(from, to);
                if (!File.Exists(fileTo)) {
                    Console.WriteLine($"Copying {fileTo}");
                    File.Copy(newPath, fileTo, false);
                } 
            }
        }
        static async Task<string> Download(HttpClient client, string path) {
            Console.WriteLine($"Downloading {client.BaseAddress + path}");
            var response = await client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        static async Task UpdateSplashScreen(HttpClient client) {
             var response = await Download(client, $"2017");

            var document = new HtmlDocument();
            document.LoadHtml(response);
            var node = document.DocumentNode.SelectSingleNode("//*[contains(@class,'calendar')]");
            WriteFile("splashscreen.in", node.OuterHtml);

        }

        static async Task<string> UpdateReadme(HttpClient client, int day) {
            var response = await Download(client, $"2017/day/{day}");
        
            var md = ToMarkDown(response, client.BaseAddress + $"/2017/day/{day}");
            var fileTo = Path.Combine(Dir(day), "README.md");
            WriteFile(fileTo, md.content);
            return md.title;
        }

        static void UpdateSolutionTemplate(int day, string title) {
            var solution = Path.Combine(Dir(day),"Solution.cs");
            var stOld = File.ReadAllText(solution);

            var st = stOld
                .Replace("Day00", $"Day{day.ToString("00")}")
                .Replace("???", title);
            if (st != stOld) {
                WriteFile(solution, st);
            }
        }

        static async Task UpdateInput(HttpClient client, int day) {
            var response = await Download(client, $"2017/day/{day}/input");
            var inputFile = Path.Combine(Dir(day), "input.in");
            WriteFile(inputFile, response);
        }

        static void WriteFile(string file, string content) {
            Console.WriteLine($"Writing {file}");
            File.WriteAllText(file, content);
        }

        static string Dir(int day) => $"Day{day.ToString("00")}";

        static (string title, string content) ToMarkDown(string input, string url) {
            var document = new HtmlDocument();
            document.LoadHtml(input);
            var st = $"original source: [{url}]({url})\n";
            foreach (var article in document.DocumentNode.SelectNodes("//article")) {
                st += UnparseList("", article) + "\n";
            }
            var title = HtmlEntity.DeEntitize(document.DocumentNode.SelectNodes("//h2").First().InnerText);

            var match = Regex.Match(title, ".*: (.*) ---");
            if (match.Success) {
                title = match.Groups[1].Value;
            }
            return (title, st);
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