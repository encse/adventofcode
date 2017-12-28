using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

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

        public static int Year(Type tsolver) {
            return int.Parse(tsolver.FullName.Split('.')[1].Substring(1));
        }
        public static int Day(this Solver solver) {
            return Day(solver.GetType());
        }

        public static int Day(Type tsolver) {
            return int.Parse(tsolver.FullName.Split('.')[2].Substring(3));
        }
        public static string WorkingDir(int year, int day) {
            return Path.Combine(year.ToString(), "Day" + day.ToString("00"));
        }

        public static string WorkingDir(this Solver solver) {
            return WorkingDir(solver.Year(), solver.Day());
        }
    }

    class Runner {

        public static void RunAll(params Type[] tsolvers) {
            var errors = new List<string>();

            foreach (var solver in tsolvers.Select(tsolver => Activator.CreateInstance(tsolver) as Solver)) {
                var workingDir = solver.WorkingDir();
                var color = Console.ForegroundColor;
                try {
                    WriteLine(ConsoleColor.White, $"{solver.DayName()}: {solver.GetName()}");
                    WriteLine();

                    foreach (var file in Directory.EnumerateFiles(workingDir)) {

                        if (file.EndsWith(".in")) {
                            var refoutFile = file.Replace(".in", ".refout");
                            var refout = File.Exists(refoutFile) ? File.ReadAllLines(refoutFile) : null;

                            var dt = DateTime.Now;
                            var iline = 0;
                            foreach (var line in solver.Solve(File.ReadAllText(file))) {
                                var now = DateTime.Now;
                                var (statusColor, status, err) =
                                    refout == null || refout.Length <= iline ? (ConsoleColor.Cyan, "?", null) :
                                    refout[iline] == line.ToString() ? (ConsoleColor.DarkGreen, "✓", null) :
                                    (ConsoleColor.Red, "X", $"{solver.DayName()}: In line {iline + 1} expected '{refout[iline]}' but found '{line}'");

                                if (err != null) {
                                    errors.Add(err);
                                }

                                Write(statusColor, $"  {status}");
                                Write(color, $" {line} ");
                                var diff = (now - dt).TotalMilliseconds;
                                WriteLine(
                                    diff > 1000 ? ConsoleColor.Red :
                                    diff > 500 ? ConsoleColor.Yellow :
                                    ConsoleColor.DarkGreen,
                                    $"({diff} ms)"
                                );
                                dt = now;
                                iline++;
                            }
                        }
                    }

                    WriteLine();
                } finally {
                    Console.ForegroundColor = color;
                }
            }

            if (errors.Any()) {
                WriteLine(ConsoleColor.Red, "Errors:\n" + string.Join("\n", errors));
            }
        }

        private static void WriteLine(ConsoleColor color = ConsoleColor.Gray, string text = "") {
            Write(color, text + "\n");
        }
        private static void Write(ConsoleColor color = ConsoleColor.Gray, string text = ""){
           Console.ForegroundColor = color;
           Console.Write(text);
        }
    }
}