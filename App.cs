using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    class App {
    
        static Solver[] GetSolvers(params Type[] tsolver) => 
            tsolver.Select(t => Activator.CreateInstance(t) as Solver).ToArray();
      
        static void Main(string[] args) {

            var tsolvers = Assembly.GetEntryAssembly()!.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
                .OrderBy(t => t.FullName)
                .ToArray();

            var action =
                Command(args, Args("update", "([0-9]+)/([0-9]+)"), m => {
                    var year = int.Parse(m[1]);
                    var day = int.Parse(m[2]);
                    return () => new Updater().Update(year, day).Wait();
                }) ??
                Command(args, Args("update", "last"), m => {
                    var dt = DateTime.UtcNow.AddHours(-5);
                    if (dt is { Month: 12, Day: >= 1 and <= 25 }) {
                        return () => new Updater().Update(dt.Year, dt.Day).Wait();
                    } else {
                        throw new Exception("Event is not active. This option works in Dec 1-25 only)");
                    }
                }) ??
                Command(args, Args("upload", "([0-9]+)/([0-9]+)"), m => {
                    var year = int.Parse(m[1]);
                    var day = int.Parse(m[2]);
                    return () => {
                        var tsolver = tsolvers.First(tsolver => 
                            SolverExtensions.Year(tsolver) == year && 
                            SolverExtensions.Day(tsolver) == day);

                        new Updater().Upload(GetSolvers(tsolver)[0]).Wait();
                    };
                }) ??
                Command(args, Args("upload", "last"), m => {
                    var dt = DateTime.UtcNow.AddHours(-5);
                    if (dt is { Month: 12, Day: >= 1 and <= 25 }) {
                        return () => {
                            new Updater().Upload(GetSolvers(tsolvers.Last())[0]).Wait();
                        };
                    } else {
                        throw new Exception("Event is not active. This option works in Dec 1-25 only)");
                    }
                }) ??
                Command(args, Args("([0-9]+)/([0-9]+)"), m => {
                    var year = int.Parse(m[0]);
                    var day = int.Parse(m[1]);
                    var tsolversSelected = tsolvers.First(tsolver => 
                        SolverExtensions.Year(tsolver) == year && 
                        SolverExtensions.Day(tsolver) == day);
                    return () => Runner.RunAll(GetSolvers(tsolversSelected));
                }) ??
                 Command(args, Args("[0-9]+"), m => {
                    var year = int.Parse(m[0]);
                    var tsolversSelected = tsolvers.Where(tsolver => 
                        SolverExtensions.Year(tsolver) == year);
                    return () => Runner.RunAll(GetSolvers(tsolversSelected.ToArray()));
                }) ??
                Command(args, Args("([0-9]+)/last"), m => {
                    var year = int.Parse(m[0]);
                    var tsolversSelected = tsolvers.Last(tsolver =>
                        SolverExtensions.Year(tsolver) == year);
                    return () => Runner.RunAll(GetSolvers(tsolversSelected));
                }) ??
                Command(args, Args("([0-9]+)/all"), m => {
                    var year = int.Parse(m[0]);
                    var tsolversSelected = tsolvers.Where(tsolver =>
                        SolverExtensions.Year(tsolver) == year);
                    return () => Runner.RunAll(GetSolvers(tsolversSelected.ToArray()));
                }) ??
                Command(args, Args("all"), m => {
                    return () => Runner.RunAll(GetSolvers(tsolvers));
                }) ??
                Command(args, Args("last"),  m => {
                    var tsolversSelected = tsolvers.Last();
                    return () => Runner.RunAll(GetSolvers(tsolversSelected));
                }) ??
                new Action(() => {
                    Console.WriteLine(Usage.Get());
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

                return parse(matches.SelectMany(m => m.Groups.Count > 1 ? m.Groups.Cast<Group>().Skip(1).Select(g => g.Value) : new []{m.Value}).ToArray());
            } catch {
                return null;
            }
        }

        static string[] Args(params string[] regex) {
            return regex;
        }

    }

    public class Usage {
        public static string Get(){
            return $@"
               > Usage: dotnet run [arguments]
               > Supported arguments:

               >  [year]/[day|last|all] Solve the specified problems
               >  [year]                Solve the whole year
               >  last                  Solve the last problem
               >  all                   Solve everything

               > To start working on new problems:
               > login to https://adventofcode.com, then copy your session cookie, and export it in your console like this

               >   export SESSION=73a37e9a72a...

               > then run the app with

               >  update [year]/[day]   Prepares a folder for the given day, updates the input,
               >                        the readme and creates a solution template.
               >  update last           Same as above, but for the current day. Works in December only.

               > You can directly upload your answer with:

               >  upload last [part(1/2)] [answer]           Upload the answer for the selected part on the current day
               >  upload [year]/[day] [part(1/2)] [answer]   Upload the answer for the selected part on the selected year and day

               > Or, you can do everything fron within VSCode:

               >  Open the command Palette ('Cmd\Ctrl + Shift + P')
               >  run the task ('Tasks: Run Task' command) : 'update'
               >  then Write / Debug your code for part 1.
               >  then run the task 'run part'
               >  then Write / Debug your code for part 2.
               >  then run the task 'run part'
               > ".StripMargin("> ");
        }
    }

     
}