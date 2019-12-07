using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using SkiaSharp;
using System.IO;
using System.Text;

namespace AdventOfCode {

    class RenderToGif : TextWriter {

        TextWriter console;

        float x;
        float y;

        SKPaint paint;
        SKSurface surface;
        string escape = null;
        int leftMargin = 3;
        public RenderToGif(TextWriter console) {
            this.console = console;
            surface = SKSurface.Create(new SKImageInfo(1024, 768));

            paint = new SKPaint {
                TextSize = 12.0f,
                IsAntialias = true,
                Color = new SKColor(0xbb, 0xbb, 0xbb),
                Style = SKPaintStyle.Fill,
                Typeface = SKTypeface.FromFamilyName(
                    "monaco",
                    SKFontStyleWeight.Normal,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Upright)
            };

            surface.Canvas.Clear(SKColors.Black);
            x = leftMargin;
            y = paint.TextSize;
        }

        public override void Close() {
            using var output = File.OpenWrite("x.png");
            surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100)
                .SaveTo(output);

            base.Close();
        }

        public override void Write(char value) {
            if (value == '\u001b') {
                escape = "";
                return;
            }
            if (escape != null) {
                escape += value;
                if (value == 'm') {
                    Regex regex = new Regex(@"\[38;2;(?<r>\d{1,3});(?<g>\d{1,3});(?<b>\d{1,3})(?<bold>;1)?m");
                    Match match = regex.Match(escape);
                    if (match.Success) {
                        byte r = byte.Parse(match.Groups["r"].Value);
                        byte g = byte.Parse(match.Groups["g"].Value);
                        byte b = byte.Parse(match.Groups["b"].Value);

                        paint.Color = new SKColor(r, g, b);

                    } else {
                        Console.Error.WriteLine(escape);
                    }

                    escape = null;
                }
                return;
            }

            var st = value.ToString();



            var qqq = paint.Typeface;
            var fontManager = SKFontManager.Default;
            paint.Typeface = fontManager.MatchCharacter(paint.Typeface.FamilyName, value);
            var text = value.ToString();
            surface.Canvas.DrawText(value.ToString(), new SKPoint(x, y), paint);
            paint.Typeface = qqq;
            x += paint.GetGlyphWidths(text)[0];

            if (value == '\n') {
                y += paint.FontSpacing;

                x = leftMargin;
            }

            console.Write(value);
        }

        public override Encoding Encoding {
            get { return Encoding.Default; }
        }


    }
    class App {

        static void Main(string[] args) {

            using var renderToGif = new RenderToGif(Console.Out);
            Console.SetOut(renderToGif);

            var tsolvers = Assembly.GetEntryAssembly().GetTypes()
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
                    var dt = DateTime.Now;
                    if (dt.Month == 12 && dt.Day >= 1 && dt.Day <= 25) {
                        return () => new Updater().Update(dt.Year, dt.Day).Wait();
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
                     return () => Runner.RunAll(tsolversSelected);
                 }) ??
                 Command(args, Args("[0-9]+"), m => {
                     var year = int.Parse(m[0]);
                     var tsolversSelected = tsolvers.Where(tsolver =>
                         SolverExtensions.Year(tsolver) == year);
                     return () => Runner.RunAll(tsolversSelected.ToArray());
                 }) ??
                Command(args, Args("([0-9]+)/last"), m => {
                    var year = int.Parse(m[0]);
                    var tsolversSelected = tsolvers.Last(tsolver =>
                        SolverExtensions.Year(tsolver) == year);
                    return () => Runner.RunAll(tsolversSelected);
                }) ??
                Command(args, Args("([0-9]+)/all"), m => {
                    var year = int.Parse(m[0]);
                    var tsolversSelected = tsolvers.Where(tsolver =>
                        SolverExtensions.Year(tsolver) == year);
                    return () => Runner.RunAll(tsolversSelected.ToArray());
                }) ??
                Command(args, Args("all"), m => {
                    return () => Runner.RunAll(tsolvers);
                }) ??
                Command(args, Args("last"), m => {
                    var tsolversSelected = tsolvers.Last();
                    return () => Runner.RunAll(tsolversSelected);
                }) ??
                new Action(() => {
                    Console.WriteLine(Usage.Get());
                });

            action();

            renderToGif.Close();
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

                return parse(matches.SelectMany(m => m.Groups.Count > 1 ? m.Groups.Cast<Group>().Skip(1).Select(g => g.Value) : new[] { m.Value }).ToArray());
            } catch {
                return null;
            }
        }

        static string[] Args(params string[] regex) {
            return regex;
        }

    }

    public class Usage {
        public static string Get() {
            return $@"
               > Usage: dotnet run [arguments]
               > Supported arguments:

               >  [year]/[day|last|all] Solve the specified problems
               >  [year]                Solve the whole year
               >  last                  Solve the last problem
               >  all                   Solve everything

               > To start working on new problems:
               > login to https://adventofcode.com, then copy your session cookie, and export it in your console like this 

               >   export SESSION=73a37e9a72a87b550ef58c590ae48a752eab56946fb7328d35857279912acaa5b32be73bf1d92186e4b250a15d9120a0

               > then run the app with

               >  update [year]/[day]   Prepares a folder for the given day, updates the input, 
               >                        the readme and creates a solution template.
               >  update last           Same as above, but for the current day. Works in December only.  
               > ".StripMargin("> ");
        }
    }
}