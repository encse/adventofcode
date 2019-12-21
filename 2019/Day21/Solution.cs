using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2019.Day21 {

    class Solution : Solver {

        public string GetName() => "Springdroid Adventure";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) {
            var icm = new IntCodeMachine(input);
            var program = $@"
            | NOT C J
            | AND D J
            | NOT B T
            | AND D T
            | OR T J
            | NOT A T
            | AND D T
            | OR T J
            | WALK
            | ".StripMargin("| ").TrimStart();

            return icm.Run(program.Select(x => (long)x).ToArray()).Last();
        }

        long PartTwo(string input) {
            var icm = new IntCodeMachine(input);
            var program = $@"
            | NOT A J
            | NOT C T
            | AND D T
            | AND H T
            | OR T J
            | NOT C T
            | AND D T
            | AND E T
            | OR T J
            | NOT B T
            | AND D T
            | OR T J
            | RUN
            | ".StripMargin("| ").TrimStart();

            return icm.Run(program.Select(x => (long)x).ToArray()).Last();
        }
    }
}