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
                foreach (var file in Directory.EnumerateFiles(name)) {
                    if (file.EndsWith(".in")) {
                        var dt = DateTime.Now;
                        foreach (var line in solver.Solve(File.ReadAllText(file))) {
                            var now = DateTime.Now;
                            Write(color, $"  {line} ");
                            var diff = (now - dt).TotalMilliseconds;
                            WriteLine(
                                diff > 1000 ? ConsoleColor.Red :
                                diff > 500 ? ConsoleColor.Yellow : 
                                ConsoleColor.DarkGreen, 
                                $"({diff} ms)"
                            );
                            dt = now;
                        }
                    }
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