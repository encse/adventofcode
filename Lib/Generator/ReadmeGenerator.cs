using System.Linq;
using AdventOfCode.Model;

namespace AdventOfCode.Generator;

class ProjectReadmeGenerator {
    public string Generate(int firstYear, int lastYear) {
       
        return $@"
           > # Advent of Code ({firstYear}-{lastYear})
           > C# solutions to the Advent of Code problems.
           > Check out https://adventofcode.com.

           > <a href=""https://adventofcode.com""><img src=""{lastYear}/calendar.svg"" width=""80%"" /></a>
           > 
           > ## Dependencies

           > - This project is based on `.NET 8`  and `C# 12`. It should work on Windows, Linux and OS-X.
           > - `AngleSharp` is used for problem download.

           > ## Running

           > To run the project:

           > 1. Install .NET Core
           > 2. Clone the repo
           > 3. Get help with `dotnet run`
           > ```
           > {Usage.Get()}
           > ```

           > ## Input files
           > 
           > I encrypt my input files using git-crypt which is transparent to all git clients and services 
           > (eg. GitHub, BitBucket, etc) and has Linux, OSX and Windows support.
           > 
           > I created a secret key in my personal home directory called `aoc-crypt.key` such as:
           > 
           > ```
           > cd my-repo
           > git-crypt init
           > git-crypt export-key ~/aoc-crypt.key
           > ```
           
           > the `.gitattributes` file is set up so that it transparently encrypts and decrypts my input files.
           > 
           > After cloing the repo I need to 'unlock' the secrets with 

           > ```
           > git-crypt unlock ~/aoc-crypt.key
           > ```
           > 
           > If you have cloned my repo you won't be able to do this, since you don't have my key, but
           > you can setup your own repo following this guide, which is adapted from this 
           > link https://stackoverflow.com/a/45047100.

           > ## Working in Visual Studio Code
           > If you prefer, you can work directly in VSCode as well. 
 
           >  Open the command Palette (⇧ ⌘ P), select `Tasks: Run Task` then e.g. `update today`.
           > 
           >  Work on part 1. Check the solution with the `upload today` task. Continue with part 2.
           > 
           >  **Note:** this feature relies on the ""Memento Inputs"" extension to store your session cookie, you need 
           >  to set it up in advance from the Command Palette with `Install Extensions`.
           > 
  
           ".StripMargin("> ");
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
