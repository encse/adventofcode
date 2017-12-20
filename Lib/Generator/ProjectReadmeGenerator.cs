using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Model;

namespace AdventOfCode.Generator {
    
    public class ProjectReadmeGenerator {
        public string Generate(Calendar calendar) {
            var calendarLines =
                string.Join("\n",
                    from line in calendar.Lines
                    select string.Join("", from token in line select token.Text));
                    
            return $@"
               > # Advent of Code {calendar.Year}
               > ```
               > {calendarLines}
               > ```
               > C# solutions to http://adventofcode.com/{calendar.Year} using .NET Core 2.0.
               > 
               > ## Dependencies
               > 
               > - This library is based on `.NET Core 2.0`. It should work on Windows, Linux and OS X.
               > - `HtmlAgilityPack.NetCore` is used for problem download.
               > 
               > ## Running
               > 
               > To run the project:
               > 
               > 1. Install .NET Core.
               > 2. Download the code.
               > 3. Run `dotnet run <day>`.
               > 
               > To prepare for the next day:
               > 
               > 1. Run `dotnet run update <day>`.".StripMargin("> ");
        }
    }
}