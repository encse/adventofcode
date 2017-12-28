using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2016.Day09 {

    class Solution : Solver {

        public string GetName() => "Explosives in Cyberspace";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) {
            input = input.TrimEnd();
            return Extend(input, 0, input.Length, false);
        }

        long PartTwo(string input) {
            input = input.TrimEnd();
            return Extend(input, 0, input.Length, true);
        }

        long Extend(string input, int i, int lim, bool recursive) {
            var res = 0L;
            while (i < lim) {
                if (input[i] == '(') {
                    var j = input.IndexOf(')', i + 1);
                    var m = Regex.Match(input.Substring(i + 1, j - i - 1), @"(\d+)x(\d+)");
                    var length = int.Parse(m.Groups[1].Value);
                    var mul = int.Parse(m.Groups[2].Value);
                    res += recursive ? Extend(input, j + 1, j + length + 1, recursive) * mul : length * mul;
                    i = j + length + 1;
                } else {
                    res++;
                    i++;
                }
            }
            return res;
        }
    }
}