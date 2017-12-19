
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
using AdventOfCode2017.Generator;
using AdventOfCode2017.Model;

namespace AdventOfCode2017 {

    class Updater {

        public async Task Update(int day) {
            if (!System.Environment.GetEnvironmentVariables().Contains("SESSION")) {
                throw new Exception("Specify SESSION environment variable");
            }

            var cookieContainer = new CookieContainer();
            using (var client = new HttpClient(
                                    new HttpClientHandler {
                                        CookieContainer = cookieContainer,
                                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                                    })
            ) {

                var baseAddress = new Uri("https://adventofcode.com/");
                client.BaseAddress = baseAddress;
                cookieContainer.Add(baseAddress, new Cookie("session", System.Environment.GetEnvironmentVariable("SESSION")));

                var calendar = await DownloadCalendar(client);
                var problem = await DownloadProblem(client, day);

                var dir = Dir(day);
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }

                UpdateProjectReadme(calendar);
                UpdateSplashScreen(calendar);
                UpdateReadmeForDay(problem);
                UpdateInput(problem);
                UpdateSolutionTemplate(problem);
            }
        }

        async Task<string> Download(HttpClient client, string path) {
            Console.WriteLine($"Downloading {client.BaseAddress + path}");
            var response = await client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        void WriteFile(string file, string content) {
            Console.WriteLine($"Writing {file}");
            File.WriteAllText(file, content);
        }

        string Dir(int day) => $"Day{day.ToString("00")}";

        async Task<Calendar> DownloadCalendar(HttpClient client) {
            var html = await Download(client, "2017");
            return Calendar.Parse(html);
        }

        async Task<Problem> DownloadProblem(HttpClient client, int day) {
            var problemStatement = await Download(client, $"2017/day/{day}");
            var input = await Download(client, $"2017/day/{day}/input");
            return Problem.Parse(day, client.BaseAddress + $"/2017/day/{day}", problemStatement, input);
        }

        void UpdateReadmeForDay(Problem problem) {
            var fileTo = Path.Combine(Dir(problem.Day), "README.md");
            WriteFile(fileTo, problem.ContentMd);
        }

        void UpdateSolutionTemplate(Problem problem) {
            var solution = Path.Combine(Dir(problem.Day), "Solution.cs");
            if (!File.Exists(solution)) {
                WriteFile(solution, new SolutionTemplateGenerator().Generate(problem));
            }
        }

        void UpdateProjectReadme(Calendar calendar) {
            var file = Path.Combine("README.md");
            WriteFile(file, new ProjectReadmeGenerator().Generate(calendar));
        }

        void UpdateSplashScreen(Calendar calendar) {
            var file = Path.Combine(Path.Combine("lib", "SplashScreen.cs"));
            WriteFile(file, new SplashScreenGenerator().Generate(calendar));
        }

        void UpdateInput(Problem problem) {
            var inputFile = Path.Combine(Dir(problem.Day), "input.in");
            WriteFile(inputFile, problem.Input);
        }

    }
}