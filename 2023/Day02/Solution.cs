using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day02;

[ProblemName("Cube Conundrum")]
class Solution : Solver {

    public object PartOne(string input) => (
        from line in input.Split("\n")
        let game = ParseGame(line)
        where game.red <= 12 &&  game.green <= 13 && game.blue <= 14
        select game.id
    ).Sum();

    public object PartTwo(string input) => (
        from line in input.Split("\n")
        let game = ParseGame(line)
        select game.red * game.green * game.blue
    ).Sum();

    // no need to keep track of the individual rounds in a game, just return 
    // the maximum of the red, green, blue boxes
    Game ParseGame(string line) => 
        new Game(
            ParseInts(line, @"Game (\d+)").First(), 
            ParseInts(line, @"(\d+) red").Max(),
            ParseInts(line, @"(\d+) green").Max(),
            ParseInts(line, @"(\d+) blue").Max()
        );

    // extracts integers from a string identified by the a single regex group.
    IEnumerable<int> ParseInts(string st, string rx) =>
        from m in Regex.Matches(st, rx) 
        select int.Parse(m.Groups[1].Value);
}

record Game(int id, int red, int green, int blue);