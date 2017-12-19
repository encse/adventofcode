using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode2017.Generator {

    public class SolutionTemplateGenerator {
        public string Generate(string title, int day) {
            return $@"using System;
                 |using System.Collections.Generic;
                 |using System.Collections.Immutable;
                 |using System.Linq;
                 |
                 |namespace AdventOfCode2017.Day{day.ToString("00")} {{
                 |
                 |    class Solution : Solver {{
                 |
                 |        public string GetName() => ""{title}"";
                 |
                 |        public IEnumerable<object> Solve(string input) {{
                 |            yield return PartOne(input);
                 |            yield return PartTwo(input);
                 |        }}
                 |
                 |        int PartOne(string input) {{
                 |            return 0;
                 |        }}
                 |
                 |        string PartTwo(string input) {{
                 |            return """";
                 |        }}
                 |    }}
                 |}}".StripMargin();
        }
    }
}