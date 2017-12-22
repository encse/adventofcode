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

    class Runner {
       
        public static void RunSolver(Solver solver) {
            var name = solver.GetType().FullName.Split('.')[1];
            var color = Console.ForegroundColor;
            try {
                WriteLine(ConsoleColor.White, $"Day {Day(solver.GetType())}: {solver.GetName()}");
                WriteLine();
                var errors = new List<string>();
                foreach (var file in Directory.EnumerateFiles(name)) {
                    if (file.EndsWith(".in")) {
                        var refoutFile = file.Replace(".in", ".refout");
                        var refout = File.Exists(refoutFile) ? File.ReadAllLines(refoutFile) : null;

                        var dt = DateTime.Now;
                        var iline = 0;
                        foreach (var line in solver.Solve(File.ReadAllText(file))) {
                            var now = DateTime.Now;
                            var (statusColor, status, err) = 
                                refout == null || refout.Length <= iline ? (ConsoleColor.Gray, "?", null) :
                                refout[iline] == line.ToString() ? (ConsoleColor.Green, "✓", null) :
                                (ConsoleColor.Red, "X", $"Day {Day(solver.GetType())}: in line {iline +1} expected '{refout[iline]}' but found '{line}'");
                            
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
                if (errors.Any()) {
                    throw new Exception(string.Join("\n", errors));
                }
                WriteLine();
            } finally{
                Console.ForegroundColor = color;
            }
        }

        private static void WriteLine(ConsoleColor color = ConsoleColor.Gray, string text = "") {
            Write(color, text + "\n");
        }
        private static void Write(ConsoleColor color = ConsoleColor.Gray, string text = ""){
           Console.ForegroundColor = color;
           Console.Write(text);
        }

        static int Day(Type type) {
            return int.Parse(type.FullName.Split('.')[1].Substring(3));
        }
    }
}