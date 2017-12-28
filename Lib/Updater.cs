
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
using AdventOfCode.Generator;
using AdventOfCode.Model;

namespace AdventOfCode {

    class Updater {

        public async Task Update(int year, int day) {
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

                var calendar = await DownloadCalendar(client, year);
                var problem = await DownloadProblem(client, year, day);

                var dir = Dir(year, day);
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }

                UpdateProjectReadme(calendar);
                UpdateSplashScreen(calendar);
                UpdateReadmeForDay(problem);
                UpdateInput(problem);
                UpdateRefout(problem);
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

        string Dir(int year, int day) => SolverExtensions.WorkingDir(year, day);

        async Task<Calendar> DownloadCalendar(HttpClient client, int year) {
            var html = await Download(client, year.ToString());
            return Calendar.Parse(year, html);
        }

        async Task<Problem> DownloadProblem(HttpClient client, int year, int day) {
            var problemStatement = await Download(client, $"{year}/day/{day}");
            var input = await Download(client, $"{year}/day/{day}/input");
            return Problem.Parse(year, day, client.BaseAddress + $"/{year}/day/{day}", problemStatement, input);
        }

        void UpdateReadmeForDay(Problem problem) {
            var file = Path.Combine(Dir(problem.Year, problem.Day), "README.md");
            WriteFile(file, problem.ContentMd);
        }

        void UpdateSolutionTemplate(Problem problem) {
            var file = Path.Combine(Dir(problem.Year, problem.Day), "Solution.cs");
            if (!File.Exists(file)) {
                WriteFile(file, new SolutionTemplateGenerator().Generate(problem));
            }
        }

        void UpdateProjectReadme(Calendar calendar) {
            var file = Path.Combine("README.md");
            WriteFile(file, new ProjectReadmeGenerator().Generate(calendar));
        }

        void UpdateSplashScreen(Calendar calendar) {
            var file = Path.Combine(Path.Combine("Lib", "SplashScreen.cs"));
            WriteFile(file, new SplashScreenGenerator().Generate(calendar));
        }

        void UpdateInput(Problem problem) {
            var file = Path.Combine(Dir(problem.Year, problem.Day), "input.in");
            WriteFile(file, problem.Input);
        }

        void UpdateRefout(Problem problem) {
            var file = Path.Combine(Dir(problem.Year, problem.Day), "input.refout");
            if (problem.Answers.Any()) {
                WriteFile(file, problem.Answers);
            }
        }
    }
}