using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day21 {

    class Solution : Solver {

        public string GetName() => "Chronal Conversion";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Solver().First();
        int PartTwo(string input) => Solver().Last();

        List<int> Solver() {
            var (r1, r2, r3, r4, r5) = (0, 0, 0, 0, 0);
            r3 = 0;
            var seen = new List<int>();
            while (true) {

                r2 = r3 | 65536;
                r3 = 10736359;
                while (true) {
                    r1 = r2 & 255;
                    r3 = r3 + r1;
                    r3 = r3 & 16777215;
                    r3 = r3 * 65899;
                    r3 = r3 & 16777215;
                    if (r2 < 256) {
                        break;
                    }
                    r2 = r2 / 256;
                }

                if (seen.Contains(r3)) {
                    return seen;
                }
                seen.Add(r3);

            }
        }
       
    }
}