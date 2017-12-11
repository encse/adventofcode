using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdventOfCode2017 {
    interface Solver {
        string GetName();
        void Solve(string input);
    }

    class Runner {
        static void Main(string[] args) {
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

            if (tSolver == null) {
                Console.WriteLine($"USAGE: dotnet run <day>");
                Console.WriteLine($"day: {Day(tSolvers.First())}-{Day(tSolvers.Last())}");
            } else {
                RunSolver(Activator.CreateInstance(tSolver) as Solver);
            }
        }

        static void RunSolver(Solver solver) {
            var name = solver.GetType().FullName.Split('.')[1];
            Console.WriteLine($"Day {Day(solver.GetType())}: {solver.GetName()}");
            Console.WriteLine();
            foreach (var file in Directory.EnumerateFiles(name)) {
                if (file.EndsWith(".in")) {
                    solver.Solve(File.ReadAllText(file));
                }
            }
        }

        static int Day(Type type) {
            return int.Parse(type.FullName.Split('.')[1].Substring(3));
        }
    }
}