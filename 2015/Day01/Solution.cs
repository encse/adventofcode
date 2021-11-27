using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2015.Day01;

[ProblemName("Not Quite Lisp")]
class Solution : Solver {

    public object PartOne(string input) => Levels(input).Last().level;

    public object PartTwo(string input) => Levels(input).First(p => p.level == -1).idx;

    IEnumerable<(int idx, int level)> Levels(string input){
        var level = 0;
        for (var i = 0; i < input.Length; i++) {
            level += input[i] == '(' ? 1 : -1;
            yield return (i+1, level);
        }
    }
}
