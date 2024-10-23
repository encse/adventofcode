using AdventOfCode.Model;

namespace AdventOfCode.Generator;

class ProjectReadmeGenerator {
    public string Generate(int firstYear, int lastYear) {

        return $"""
           # Advent of Code ({firstYear}-{lastYear})
           C# solutions to the [Advent of Code](https://adventofcode.com) problems.

           <a href="https://adventofcode.com"><img src="{lastYear}/calendar.svg" width="80%" /></a>

           If you want to use my framework, it's probably easiest to start out from the 
           https://github.com/encse/adventofcode-template repository.

           I put a lot of effort into my solutions. I aim for clarity, which means that 
           they are not super effective or super short, but hopefully more readable.

           If you find project useful, please [support](https://github.com/sponsors/encse) me.

           ## Dependencies
           - Based on `.NET 8`  and `C# 12`. 
           - `AngleSharp` is used for problem download.
           - git-crypt to store the input files in an encrypted form
        """;
    }
}

class ReadmeGeneratorForYear {
    public string Generate(Calendar calendar) {
        return $"""
           # Advent of Code ({calendar.Year})
           Check out https://adventofcode.com/{calendar.Year}.

           <a href="https://adventofcode.com/{calendar.Year}"><img src="calendar.svg" width="80%" /></a>
           
           """;
    }
}
