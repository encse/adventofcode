using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace AdventOfCode {
    
    class ProblemName : Attribute {
        public readonly string Name;
        public ProblemName(string name){
            this.Name = name;
        }
    }

    interface Solver {
        IEnumerable<object> Solve(string input);
    }

    static class SolverExtensions {

        public static string GetName(this Solver solver) {
            return (
                solver
                    .GetType()
                    .GetCustomAttribute(typeof(ProblemName)) as ProblemName
            ).Name;
        }

        public static string DayName(this Solver solver) {
            return $"Day {solver.Day()}";
        }

        public static int Year(this Solver solver) {
            return Year(solver.GetType());
        }

        public static int Year(Type t) {
            return int.Parse(t.FullName.Split('.')[1].Substring(1));
        }
        public static int Day(this Solver solver) {
            return Day(solver.GetType());
        }

        public static int Day(Type t) {
            return int.Parse(t.FullName.Split('.')[2].Substring(3));
        }

        public static string WorkingDir(int year) {
            return Path.Combine(year.ToString());
        }

        public static string WorkingDir(int year, int day) {
            return Path.Combine(WorkingDir(year), "Day" + day.ToString("00"));
        }

        public static string WorkingDir(this Solver solver) {
            return WorkingDir(solver.Year(), solver.Day());
        }

        public static SplashScreen SplashScreen(this Solver solver) {
            var tsplashScreen = Assembly.GetEntryAssembly().GetTypes()
                 .Where(t => t.GetTypeInfo().IsClass && typeof(SplashScreen).IsAssignableFrom(t))
                 .Single(t => Year(t) == solver.Year());
            return (SplashScreen)Activator.CreateInstance(tsplashScreen);
        }
    }

    record UncheckedResult(int part, string answer);

    class Runner {

        private static string GetNormalizedInput(string file){
            var input = File.ReadAllText(file);
            if (input.EndsWith("\n")) {
                input = input.Substring(0, input.Length - 1);
            }
            return input;
        }

        public static UncheckedResult GetUncheckedResult(Solver solver) {
            var workingDir = solver.WorkingDir();
            var inputFile = Path.Combine(workingDir, "input.in");
            var input = GetNormalizedInput(inputFile);
            var refoutFile = (Path.Combine(workingDir, "input.refout"));
            var refout = (File.Exists(refoutFile) ? GetNormalizedInput(refoutFile): "")
                    .Split("\n")
                    .Where(x=>!string.IsNullOrWhiteSpace(x))
                    .ToArray();

            var output = solver.Solve(input).Select(res => res.ToString()).ToArray();

            return refout.Length < output.Length ? new UncheckedResult(refout.Length+1, output[refout.Length]) : null;
        }

        public static void RunAll(params Solver[] solvers) {
            var errors = new List<string>();

            var lastYear = -1;
            foreach (var solver in solvers) {
                if (lastYear != solver.Year()) {
                    solver.SplashScreen().Show();
                    lastYear = solver.Year();
                }

                var workingDir = solver.WorkingDir();
                WriteLine(ConsoleColor.White, $"{solver.DayName()}: {solver.GetName()}");
                WriteLine();
                foreach (var dir in new[] { workingDir, Path.Combine(workingDir, "test") }) {
                    if (!Directory.Exists(dir)) {
                        continue;
                    }
                    var files = Directory.EnumerateFiles(dir).Where(file => file.EndsWith(".in")).ToArray();
                    foreach (var file in files) {

                        if (files.Count() > 1) {
                            Console.WriteLine("  " + file + ":");
                        }
                        var refoutFile = file.Replace(".in", ".refout");
                        var refout = File.Exists(refoutFile) ? File.ReadAllLines(refoutFile) : null;
                        var input = GetNormalizedInput(file);
                        var dt = DateTime.Now;
                        var iline = 0;
                        foreach (var line in solver.Solve(input)) {
                            var now = DateTime.Now;
                            var (statusColor, status, err) =
                                refout == null || refout.Length <= iline ? (ConsoleColor.Cyan, "?", null) :
                                refout[iline] == line.ToString() ? (ConsoleColor.DarkGreen, "✓", null) :
                                (ConsoleColor.Red, "X", $"{solver.DayName()}: In line {iline + 1} expected '{refout[iline]}' but found '{line}'");

                            if (err != null) {
                                errors.Add(err);
                            }

                            Write(statusColor, $"  {status}");
                            Console.Write($" {line} ");
                            var diff = (now - dt).TotalMilliseconds;
                            WriteLine(
                                diff > 1000 ? ConsoleColor.Red :
                                diff > 500 ? ConsoleColor.Yellow :
                                ConsoleColor.DarkGreen,
                                $"({diff.ToString("F3")} ms)"
                            );
                            dt = now;
                            iline++;
                        }
                    }
                }

                WriteLine();
            }

            if (errors.Any()) {
                WriteLine(ConsoleColor.Red, "Errors:\n" + string.Join("\n", errors));
            }
        }

        private static void WriteLine(ConsoleColor color = ConsoleColor.Gray, string text = "") {
            Write(color, text + "\n");
        }
        private static void Write(ConsoleColor color = ConsoleColor.Gray, string text = "") {
            var c = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = c;
        }
    }
}