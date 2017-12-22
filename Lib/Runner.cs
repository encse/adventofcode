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
        public static string DayName(this Solver solver){
            return $"Day {Day(solver.GetType())}";
        }

        static int Day(Type type) {
            return int.Parse(type.FullName.Split('.')[1].Substring(3));
        }
    }

    class Runner {

        public static void RunAll(params Type[] tsolvers) {
            var errors = new List<string>();

            foreach (var solver in tsolvers.Select(tsolver => Activator.CreateInstance(tsolver) as Solver)) {
                var name = solver.GetType().FullName.Split('.')[1];
                var color = Console.ForegroundColor;
                try {
                    WriteLine(ConsoleColor.White, $"{solver.DayName()}: {solver.GetName()}");
                    WriteLine();

                    foreach (var file in Directory.EnumerateFiles(name)) {

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