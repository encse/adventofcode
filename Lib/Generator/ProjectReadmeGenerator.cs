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
               > # Advent of Code
               > C# solutions to the advent of code problems (2015-2017).
               > Check out http://adventofcode.com.
               > 
               > ## Dependencies
               > 
               > - This library is based on `.NET Core 2.0`. It should work on Windows, Linux and OS X.
               > - `Newtonsoft.Json` for JSON parsing
               > - `HtmlAgilityPack.NetCore` is used for problem download.
               > 
               > ## Running
               > 
               > To run the project:
               > 
               > 1. Install .NET Core.
               > 2. Download the code.
               > 3. Run `dotnet run <year>/<day>`.
               > 
               > To prepare for the next day:
               > 
               > 1. Run `dotnet run update <year>/<day>`.".StripMargin("> ");
        }
    }
}