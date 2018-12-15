using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day14 {

    class Solution : Solver {

        public string GetName() => "Chocolate Charts";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
            
        }

        string PartOne(string input) => Window(10).ElementAt(int.Parse(input)).st;

        int PartTwo(string input) => Window(input.Length).First(item => item.st == input).i;

        IEnumerable<(int i, string st)> Window(int w) {
            var st = "";
            var i = 0;
            foreach (var score in Scores()) {
                i++;
                st += score;
                if (st.Length > w) {
                    st = st.Substring(st.Length - w);
                }
                if (st.Length == w) {
                    yield return (i - w, st);
                }
            }
        }

        IEnumerable<int> Scores() {
            var scores = new List<int>();
            Func<int, int> add = (i) => { scores.Add(i); return i; };

            var elf1 = 0;
            var elf2 = 1;

            yield return add(3);
            yield return add(7);

            while (true) {
                var sum = scores[elf1] + scores[elf2];
                if (sum >= 10) {
                    yield return add(sum / 10);
                }
                yield return add(sum % 10);

                elf1 = (elf1 + scores[elf1] + 1) % scores.Count;
                elf2 = (elf2 + scores[elf2] + 1) % scores.Count;
            }
        }
    }
}