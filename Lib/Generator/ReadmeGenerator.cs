using System.Linq;
using AdventOfCode.Model;

namespace AdventOfCode.Generator {

    public class ProjectReadmeGenerator {
        public string Generate(int firstYear, int lastYear) {
           
            return $@"
               > # Advent of Code ({firstYear}-{lastYear})
               > C# solutions to the Advent of Code problems.
               > Check out http://adventofcode.com.
               > ![](demo.gif)
               > ## Dependencies

               > - This project is based on `.NET 5`. It should work on Windows, Linux and OS X.
               > - `AngleSharp` is used for problem download.

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
               > Check out https://adventofcode.com/{calendar.Year}.
               > ```
               > {calendarLines}
               > ```
               > ".StripMargin("> ");
        }
    }
}