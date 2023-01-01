using System;
using System.Linq;

namespace AdventOfCode.Y2022.Day02;

[ProblemName("Rock Paper Scissors")]
class Solution : Solver {

    // There are many obscure ways of solving this challenge. You can use 
    // mod 3 arithmetic or play with ASCII encoding. This approach is more 
    // explicit. I think it is as simple as it gets.

    // We parse the input lines into a pair of Rock/Paper/Scissors signs 
    // represented by 1,2,3 (the values from the problem description), 
    // calculate the score for each pair and sum it up.

    // Part one and two differs only in the decoding of the X, Y and Z signs.

    enum Sign {
        Rock = 1,
        Paper = 2,
        Scissors = 3,
    }

    public object PartOne(string input) => 
        Total(input, ElfParser, HumanParser1);
    
    public object PartTwo(string input) => 
        Total(input, ElfParser, HumanParser2);

    Sign ElfParser(string line) =>
        line[0] == 'A' ? Sign.Rock :
        line[0] == 'B' ? Sign.Paper :
        line[0] == 'C' ? Sign.Scissors :
                         throw new ArgumentException(line);

    Sign HumanParser1(string line) =>   
        line[2] == 'X' ? Sign.Rock :
        line[2] == 'Y' ? Sign.Paper :
        line[2] == 'Z' ? Sign.Scissors :
                         throw new ArgumentException(line);

    Sign HumanParser2(string line) =>   
        line[2] == 'X' ? Next(Next(ElfParser(line))): // elf wins
        line[2] == 'Y' ? ElfParser(line) :            // draw
        line[2] == 'Z' ? Next(ElfParser(line)) :      // you win
                         throw new ArgumentException(line);
          
    int Total(string input, Func<string, Sign> elf, Func<string, Sign> human) =>
        input
            .Split("\n")
            .Select(line => Score(elf(line), human(line)))
            .Sum();

    int Score(Sign elfSign, Sign humanSign) =>
        humanSign == Next(elfSign)       ? 6 + (int)humanSign : // human wins
        humanSign == elfSign             ? 3 + (int)humanSign : // draw
        humanSign == Next(Next(elfSign)) ? 0 + (int)humanSign : // elf wins
                                          throw new ArgumentException();

    Sign Next(Sign sign) => 
        sign == Sign.Rock     ? Sign.Paper : 
        sign == Sign.Paper    ? Sign.Scissors : 
        sign == Sign.Scissors ? Sign.Rock : 
                                throw new ArgumentException();
}
