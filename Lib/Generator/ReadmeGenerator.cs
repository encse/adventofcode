using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Model;

namespace AdventOfCode.Generator {

    public class ProjectReadmeGenerator {
        public string Generate(int firstYear, int lastYear) {
           
            return $@"
               > # Advent of Code ({firstYear}-{lastYear})
               > My C# solutions to the advent of code problems.
               > Check out http://adventofcode.com.

               > ## Dependencies

               > - This library is based on `.NET Core 3.0`. It should work on Windows, Linux and OS X.
               > - `Newtonsoft.Json` for JSON parsing
               > - `HtmlAgilityPack.NetCore` is used for problem download.

               > ## Running

               > To run the project:

               > 1. Install .NET Core
               > 2. Clone the repo
               > 3. Get help with `dotnet run`
               > ```
               > {Usage.Get()}
               > ```
               > ".StripMargin("> ");
        }
    }

    public class ReadmeGeneratorForYear {
        public string Generate(Calendar calendar) {
            var calendarLines =
                string.Join("\n",
                    from line in calendar.Lines
                    select string.Join("", from token in line select token.Text));
                    
            return $@"
               > # Advent of Code ({calendar.Year})
               > Check out http://adventofcode.com/{calendar.Year}.
               > ```
               > {calendarLines}
               > ```
               > ".StripMargin("> ");
        }
    }
}