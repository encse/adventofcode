using System.Linq;
using AdventOfCode.Model;

namespace AdventOfCode.Generator;

class ProjectReadmeGenerator {
    public string Generate(int firstYear, int lastYear) {
       
        return $@"
           > # Advent of Code ({firstYear}-{lastYear})
           > C# solutions to the Advent of Code problems.
           > Check out https://adventofcode.com.
           > 
           > <a href=""https://adventofcode.com""><img src=""{lastYear}/calendar.svg"" width=""80%"" /></a>
           > 
           > The goal is to keep my C# knowledge fresh and to follow the latest changes of the language.
           > 
           > Everything is self contained. I don't use any libraries to make things short or predefined algorithms 
           > to parameterize. Just stick to what .Net provides. Each problem is solved by plain C# classes without any 'base' to derive from.
           > The solvers have different entry points for part 1 and 2. There is no local state, part 2 starts from scratch, 
           > but code sharing between part 1 and 2 is important to me. (Unless it makes things hard to read.)
           > 
           > I prefer to use functional style, local or anonymous functions, immutability and linq over the state manipulation 
           > style of oop, but I'm not very strict about this. Whatever I see fit for the problem.
           > 
           > One thing that you will not see much in C# projects is K&R indentation. Sorry about that...
           > 
           > The way I solve the puzzles should be pretty consistent during an event but there are small changes over 
           > the years as I find something new or forget about stuff I learned last year.
           > 
           > I try to keep things tight and golf the solution to a certain level, but don't want to overgolf it. (Sometimes I fail.)
           > 
           > There aren't many comments, but if I find that the solution is not straightforward, the algorithm has a name, or it is 
           > using some special property of the input I might explain it in a line or two. 
           > 
           > You can browse my solutions as they are or fork the repo, remove everything and use just the lib part to 
           > start working on your own. The framework part is pretty stable and you get testing, scaffolding etc for free.
           > 
           > ## Dependencies

           > - This project is based on `.NET 7`  and `C# 11`. It should work on Windows, Linux and OS-X.
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

class ReadmeGeneratorForYear {
    public string Generate(Calendar calendar) {
        return $@"
           > # Advent of Code ({calendar.Year})
           > Check out https://adventofcode.com/{calendar.Year}.

           > <a href=""https://adventofcode.com/{calendar.Year}""><img src=""calendar.svg"" width=""80%"" /></a>
           
           > ".StripMargin("> ");
    }
}
