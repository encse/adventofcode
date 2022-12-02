using System;
using System.Linq;

namespace AdventOfCode.Y2022.Day02;

[ProblemName("Rock Paper Scissors")]
class Solution : Solver {

     public object PartOne(string input) {
        return (
            from match in input.Split('\n')
            select Score(match, 'A', 'B', 'C')
        ).Sum();
    }

    public object PartTwo(string input) {
        return (
            from match in input.Split('\n')
            select Score(match, Prev(match[0]), match[0], Next(match[0]))
        ).Sum();
    }
    
    int Score(string match, char xMeaning, char yMeaning, char zMeaning) {
        var you = 
            match[2] == 'X' ? xMeaning :
            match[2] == 'Y' ? yMeaning :
            match[2] == 'Z' ? zMeaning :
            throw new Exception();

        return 
            you == Prev(match[0]) ? 0 + you - 'A' + 1:
            you == match[0]       ? 3 + you - 'A' + 1:
            you == Next(match[0]) ? 6 + you - 'A' + 1:
            throw new Exception();
    }

    char Next(char ch) => "ABCAB"[ch - 'A' + 1];
    char Prev(char ch) => "ABCAB"[ch - 'A' + 2];
}
