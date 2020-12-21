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
               > 
               > I worked as a .Net developer for more than 10 years before switching to TypeScript. This repository is to
               > keep my C# knowledge fresh and to follow the latest changes of the language.
               > 
               > No rules are written in stone but I definitely don't use any external dependencies only what .Net provides.
               > No fancy libraries to make things short or predefined algorithms to parameterize.
               > 
               > Everything is self contained. Each problem is solved by plain C# classes without any 'base' to derive from.
               > The solvers have distinct entry points for part 1 and 2. There is no local state, part 2 starts from scratch, 
               > but code sharing between part 1 and 2 is important to me. (Unless it makes things hard to read.)
               > 
               > I prefer to use functional style, local or anonymous functions, immutability and linq over the state manipulation 
               > style of oop, but I'm not very strict about this. Whatever I see fit for the problem.
               > 
               > One thing that you dont see much in C# projects is K-R style parentheses alignment. Sorry about that...
               > 
               > My programming style should be pretty consistent during an event but I'm sure there are changes between 
               > the years as I find something new or forget about stuff I learned last year.
               > 
               > I try to keep things tight and golf the solution to a certain level, but don't want to overgolf it. (Sometimes I fail.)
               > 
               > I don't use many comments, but if I find that the solution is not straightforward, the algorithm has a name, or it is 
               > using some special property of the input I might explain it in a line or two. 
               > 
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
               >  to set it up in advance from the Command Palette with `Install Extensions`.
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