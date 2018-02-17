using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2015.Day25 {

    class Solution : Solver {

        public string GetName() => "Let It Snow";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
        }

        long PartOne(string input) {
            var m = 20151125L;
            var (irow, icol) = (1, 1);
            var (irowDst, icolDst) = Parse(input);
            while (irow != irowDst || icol != icolDst) {
                irow--;
                icol++;
                if (irow == 0) {
                    irow = icol;
                    icol = 1;
                }
                m = (m * 252533L) % 33554393L;
            }
            return m;
        }

        (int irowDst, int icolDst) Parse(string  input){
            var m = Regex.Match(input, @"To continue, please consult the code grid in the manual.  Enter the code at row (\d+), column (\d+).");
            return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
        }
    }
}