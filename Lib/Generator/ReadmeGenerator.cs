using System.Linq;
using AdventOfCode.Model;

namespace AdventOfCode.Generator {

    public class ProjectReadmeGenerator {
        public string Generate(int firstYear, int lastYear) {
           
            return $@"
               > # Advent of Code ({firstYear}-{lastYear})
               > C# solutions to the Advent of Code problems.
               > Check out https://adventofcode.com.
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

               > ## Working in Visual Studio Code
               > If you prefer, you can work directly in VSCode as well. 
 
               >  Open the command Palette (⇧ ⌘ P), select `Tasks: Run Task` then e.g. `update today`.
               > 
               >  Work on part 1. Check the solution with the `upload today` task. Continue with part 2.
               > 
               >  **Note:** this feature relies on the ""Memento Inputs"" extension to store your session cookie, you need 
               >  to set it up it in advance from the Command Palette with `Install Extensions`.
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