using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using AdventOfCode.Model;

namespace AdventOfCode.Generator {

    public class SolutionTemplateGenerator {
        public string Generate(Problem problem) {
            return $@"using System;
                 |using System.Collections.Generic;
                 |using System.Collections.Immutable;
                 |using System.Linq;
                 |using System.Text.RegularExpressions;
                 |using System.Text;
                 |
                 |namespace AdventOfCode.Day{problem.Day.ToString("00")} {{
                 |
                 |    class Solution : Solver {{
                 |
                 |        public string GetName() => ""{problem.Title}"";
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