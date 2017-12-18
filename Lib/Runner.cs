using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode2017 {
    interface Solver {
        string GetName();
        IEnumerable<object> Solve(string input);
    }

    class Runner {
       
        public static void RunSolver(Solver solver) {
            var name = solver.GetType().FullName.Split('.')[1];
            Console.WriteLine($"Day {Day(solver.GetType())}: {solver.GetName()}");
            Console.WriteLine();
            foreach (var file in Directory.EnumerateFiles(name)) {
                if (file.EndsWith(".in")) {
                    var dt = DateTime.Now;
                    foreach (var line in solver.Solve(File.ReadAllText(file))) {
                        var now = DateTime.Now;
                        Console.WriteLine($"{line} ({(now - dt).TotalMilliseconds}ms)");
                        dt = now;
                    }
                }
            }
        }

        static int Day(Type type) {
            return int.Parse(type.FullName.Split('.')[1].Substring(3));
        }
    }
}