using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2015.Day01 {

    class Solution : Solver {

        public string GetName() => "Not Quite Lisp";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Levels(input).Last().level;

        int PartTwo(string input) => Levels(input).First(p => p.level == -1).idx;

        IEnumerable<(int idx, int level)> Levels(string input){
            var level = 0;
            for (var i = 0; i < input.Length; i++) {
                level += input[i] == '(' ? 1 : -1;
                yield return (i+1, level);
            }
        }
    }
}