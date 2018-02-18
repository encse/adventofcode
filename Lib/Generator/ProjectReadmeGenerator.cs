using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Model;

namespace AdventOfCode.Generator {
    
    public class ReadmeGeneratorForYear {
        public string Generate(Calendar calendar) {
            var calendarLines =
                string.Join("\n",
                    from line in calendar.Lines
                    select string.Join("", from token in line select token.Text));
                    
            return $@"
               > # Advent of Code
               > C# solutions to the advent of code problems ({calendar.Year}).
               > Check out http://adventofcode.com/{calendar.Year}.
               > ```
               > {calendarLines}
               > ```
               > ".StripMargin("> ");
        }
    }
}