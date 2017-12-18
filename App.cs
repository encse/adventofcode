using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode2017 {

    class App {
        
        static void Main(string[] args) {
            SplashScreen();
            
            Type tSolver = null;
                var tSolvers = Assembly.GetEntryAssembly().GetTypes()
                    .Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
                    .OrderBy(t => t.FullName);

            var action =
                Command(args, Args("update", "[0-9]+"), m => {
                    var day = int.Parse(m[1]);
                    return () => Updater.Update(day).Wait();
                }) ??
                Command(args, Args("[0-9]+"), m => {
                    var day = int.Parse(m[0]);
                    tSolver = tSolvers.Where(x => x.FullName.Contains($"Day{day.ToString("00")}")).First();
                    return () => Runner.RunSolver(Activator.CreateInstance(tSolver) as Solver);
                }) ??
                Command(args, Args(), m => {
                    tSolver = tSolvers.Last();
                    return () => Runner.RunSolver(Activator.CreateInstance(tSolver) as Solver);
                }) ??
                new Action(() => {
                    Console.WriteLine("USAGE: dotnet [command]");
                    Console.WriteLine();
                    Console.WriteLine("Commands:");
                    Console.WriteLine($"  run update [day]  Prepares a folder for the given day, updates the input, the readme and creates a solution template.");
                    Console.WriteLine($"  run [day]         Solve the problem of the day");
                    Console.WriteLine($"  run               Solve the problem of the last day");
                });

            action();
        }

        static Action Command(string[] args, string[] regexes, Func<string[], Action> parse) {
            if (args.Length != regexes.Length) {
                return null;
            }
            var matches = Enumerable.Zip(args, regexes, (arg, regex) => new Regex("^" + regex + "$").Match(arg));
            if (!matches.All(match => match.Success)) {
                return null;
            }
            try {
                return parse(matches.Select(m => m.Value).ToArray());
            } catch {
                return null;
            }
        }

        static string[] Args(params string[] regex){
            return regex;
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
    }
}