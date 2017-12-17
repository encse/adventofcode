using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace AdventOfCode2017 {
    interface Solver {
        string GetName();
        IEnumerable<object> Solve(string input);
    }

    class Runner {
        static void Main(string[] args) {
            SplashScreen();

            if (args.Length == 2 && args[0] == "update" && int.TryParse(args[1], out int _)) {
                Updater.Update(int.Parse(args[1])).Wait();
                return;
            } else if (args.Length == 1 || args.Length == 0) {

                Type tSolver = null;
                var tSolvers = Assembly.GetEntryAssembly().GetTypes()
                    .Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
                    .OrderBy(t => t.FullName);

                if (args.Length == 1) {
                    int day;
                    if (int.TryParse(args[0], out day)) {
                        tSolver = tSolvers.Where(x => x.FullName.Contains($"Day{day.ToString("00")}")).FirstOrDefault();
                    }
                } else {
                    tSolver = tSolvers.Last();
                }

                if (tSolver != null) {
                    RunSolver(Activator.CreateInstance(tSolver) as Solver);
                    return;
                }
            } 

            Console.WriteLine("USAGE: dotnet [command]");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine($"  run update [day]  Prepares a folder for the given day, updates the input, the readme and creates a solution template.");
            Console.WriteLine($"  run [day]         Solve the problem of the day");
            Console.WriteLine($"  run               Solve the problem of the last day");

        }

        static void SplashScreen(){
             Console.WriteLine(
                string.Join("\n", @"
                                        *             ,
                                                    _/^\_
                                                   <     >
                                  *                 /.-.\         *
                                           *        `/&\`                   *
                                                   ,@.*;@,
                                                  /_o.I %_\    *
                                     *           (`'--:o(_@;
                                                /`;--.,__ `')             *
                                               ;@`o % O,*`'`&\ 
                                         *    (`'--)_@ ;o %'()\      *
                                              /`;--._`''--._O'@;
                                             /&*,()~o`;-.,_ `""`)
                                 *          /`,@ ;+& () o*`;-';\
                                           (`""""--.,_0 +% @' &()\
                                           /-.,_    ``''--....-'`)  *
                                     *     /@%;o`:;'--,.__   __.'\
                                          ;*,&(); @ % &^;~`""`o;@();         *
                                          /(); o^~; & ().o@*&`;&%O\
                                          `""=""==""""==,,,.,=""==""===""`
                                       __.----.(\-''#####---...___...-----._
                                     '`         \)_`""""""""""`
                                             .--' ')
                                           o(  )_-\
                                             `""""""` `
                   
                      _      _             _          __    ___         _       ___ __  _ ____ 
                     /_\  __| |_ _____ _ _| |_   ___ / _|  / __|___  __| |___  |_  )  \/ |__  |
                    / _ \/ _` \ V / -_) ' \  _| / _ \  _| | (__/ _ \/ _` / -_)  / / () | | / / 
                   /_/ \_\__,_|\_/\___|_||_\__| \___/_|    \___\___/\__,_\___| /___\__/|_|/_/  
                   "
                .Split('\n').Skip(1).Select(x => x.Substring(19))));
        }
        static void RunSolver(Solver solver) {
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