using System;
using System.Linq;

namespace AdventOfCode.Y2022.Day02;

[ProblemName("Rock Paper Scissors")]
class Solution : Solver {

    // There are many obscure ways of solving this challenge, this is a 
    // rather explicit one. We parse the input lines into a pair of
    // Rock/Paper/Scissors signs represented as 1,2,3 (the values from the
    // problem description). We calculate the score for each pair and sum up
    // the result.

    enum Sign {
        Rock = 1,
        Paper = 2,
        Scissors = 3,
    }

    public object PartOne(string input) => (
            from match in input.Split('\n')
                let elf = 
                    match[0] == 'A' ? Sign.Rock :
                    match[0] == 'B' ? Sign.Paper :
                    match[0] == 'C' ? Sign.Scissors :
                         throw new ArgumentException(match)
                let you = 
                    match[2] == 'X' ? Sign.Rock :
                    match[2] == 'Y' ? Sign.Paper :
                    match[2] == 'Z' ? Sign.Scissors :
                                      throw new ArgumentException(match)
            select Score(elf, you)
        ).Sum();

    public object PartTwo(string input) => (
            from match in input.Split('\n')
                let elf = 
                    match[0] == 'A' ? Sign.Rock :
                    match[0] == 'B' ? Sign.Paper :
                    match[0] == 'C' ? Sign.Scissors :
                                      throw new ArgumentException(match)
                let you = 
                    match[2] == 'X' ? Next(Next(elf)): // elf wins
                    match[2] == 'Y' ? elf :            // draw
                    match[2] == 'Z' ? Next(elf) :      // you win
                                      throw new ArgumentException(match)
            select Score(elf, you)
        ).Sum();
    
    int Score(Sign elfSign, Sign yourSign) =>
        yourSign == Next(elfSign)       ? 6 + (int)yourSign : // you win
        yourSign == elfSign             ? 3 + (int)yourSign : // draw
        yourSign == Next(Next(elfSign)) ? 0 + (int)yourSign : // elf wins
        throw new ArgumentException();

    Sign Next(Sign sign) => 
        sign == Sign.Rock     ? Sign.Paper : 
        sign == Sign.Paper    ? Sign.Scissors : 
        sign == Sign.Scissors ? Sign.Rock : 
        throw new ArgumentException();
}
