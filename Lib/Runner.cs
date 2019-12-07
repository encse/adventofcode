using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace AdventOfCode {
    interface Solver {
        string GetName();
        IEnumerable<object> Solve(string input);
    }

    static class SolverExtensions {
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

    class Runner {

        public static void RunAll(params Type[] tsolvers) {
            var errors = new List<string>();

            var lastYear = -1;
            foreach (var solver in tsolvers.Select(tsolver => Activator.CreateInstance(tsolver) as Solver)) {
                if (lastYear != solver.Year()) {
                    solver.SplashScreen().Show();
                    lastYear = solver.Year();
                }

                var workingDir = solver.WorkingDir();
                try {
                    WriteLine(Terminal.White, $"{solver.DayName()}: {solver.GetName()}");
                    WriteLine();
                    foreach (var dir in new[] { workingDir, Path.Combine(workingDir, "test") }) {
                        if (!Directory.Exists(dir)) {
                            continue;
                        }
                        var files = Directory.EnumerateFiles(dir).Where(file => file.EndsWith(".in")).ToArray();
                        foreach (var file in files) {

                            if (files.Count() > 1) {
                                WriteLine(Terminal.Default, "  " + file + ":");
                            }
                            var refoutFile = file.Replace(".in", ".refout");
                            var refout = File.Exists(refoutFile) ? File.ReadAllLines(refoutFile) : null;
                            var input = File.ReadAllText(file);
                            if (input.EndsWith("\n")) {
                                input = input.Substring(0, input.Length - 1);
                            }
                            var dt = DateTime.Now;
                            var iline = 0;
                            foreach (var line in solver.Solve(input)) {
                                var now = DateTime.Now;
                                var (statusColor, status, err) =
                                    refout == null || refout.Length <= iline ? (Terminal.Cyan, "?", null) :
                                    refout[iline] == line.ToString() ? (Terminal.DarkGreen, "✓", null) :
                                    (Terminal.Red, "X", $"{solver.DayName()}: In line {iline + 1} expected '{refout[iline]}' but found '{line}'");

                                if (err != null) {
                                    errors.Add(err);
                                }

                                Write(statusColor, $"  {status}");
                                Write(Terminal.Default, $" {line} ");
                                var diff = (now - dt).TotalMilliseconds;
                                WriteLine(
                                    diff > 1000 ? Terminal.Red :
                                    diff > 500 ? Terminal.Yellow :
                                    Terminal.DarkGreen,
                                    $"({diff.ToString("F3")} ms)"
                                );
                                dt = now;
                                iline++;
                            }
                        }
                    }

                    WriteLine();
                } finally {
                    Terminal.ResetFont();
                }
            }

            if (errors.Any()) {
                WriteLine(Terminal.Red, "Errors:\n" + string.Join("\n", errors));
            }
        }

        private static void WriteLine(int color = Terminal.Gray, string text = "") {
            Write(color, text + "\n");
        }
        private static void Write(int color = Terminal.Gray, string text = "") {
            Terminal.SetFont(color, false);
            Console.Write(text);
        }
    }
}