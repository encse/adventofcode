using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2016.Day16 {

    class Solution : Solver {

        public string GetName() => "Dragon Checksum";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        string PartOne(string input) => Checksum(input, 272);

        string PartTwo(string input) => Checksum(input, 35651584);

        string Checksum(string st, int length) {

            while (st.Length < length) {
                var a = st;
                var b = string.Join("", from ch in a.Reverse() select ch == '0' ? '1' : '0');
                st = a + "0" + b;
            }
            st = st.Substring(0, length);
            var sb = new StringBuilder();

            while (sb.Length % 2 == 0) {
                sb.Clear();
                for (int i = 0; i < st.Length; i += 2) {
                    sb.Append(st[i] == st[i + 1] ? "1" : "0");
                }
                st = sb.ToString();
            }
            return st;
        }
    }
}