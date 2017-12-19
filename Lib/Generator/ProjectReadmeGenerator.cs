using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2017.Generator {
    
    public class ProjectReadmeGenerator {
        public string Generate(string Calendar) {
            return $@"
               > # Advent of Code 2017
               > ```
               > {Calendar}
               > ```
               > C# solutions to http://adventofcode.com/2017 using .NET Core 2.0.
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