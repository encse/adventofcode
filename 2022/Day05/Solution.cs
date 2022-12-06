using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day05;

[ProblemName("Supply Stacks")]
class Solution : Solver {

    public object PartOne(string input) => MoveCrates(input, CrateMover9000);
    public object PartTwo(string input) => MoveCrates(input, CrateMover9001);

    record struct Move(int count, Stack<char> source, Stack<char> target);

    void CrateMover9000(Move move) {
        for (var i = 0; i < move.count; i++) {
            move.target.Push(move.source.Pop());
        }
    }

    void CrateMover9001(Move move) {
        // same as CrateMover9000 but keeps element order
        var helper = new Stack<char>();
        CrateMover9000(move with {target=helper});
        CrateMover9000(move with {source=helper});
    }

    string MoveCrates(string input, Action<Move> crateMover)  {
        var parts = input.Split("\n\n");

        var stackDefs = parts[0].Split("\n");

        // process each line by 4 character wide columns
        // last line defines the number of stacks:
        var stacks = stackDefs.Last().Chunk(4).Select(i => new Stack<char>()).ToArray();
        // bottom-up: push the next element to the the correspoing stack (' ' means no more elements).
        foreach (var line in stackDefs.Reverse().Skip(1)) {
            foreach (var (stack, item) in stacks.Zip(line.Chunk(4))) {
                // item is ~ "[A] "
                if (item[1] != ' ') {
                    stack.Push(item[1]);
                }
            }
        }

        // parse the 'moves' with regex, and use 'crateMover' on them:
        foreach (var line in parts[1].Split("\n")) {
            var m = Regex.Match(line, @"move (.*) from (.*) to (.*)");
            var count = int.Parse(m.Groups[1].Value);
            var from = int.Parse(m.Groups[2].Value) - 1;
            var to = int.Parse(m.Groups[3].Value) - 1;
            crateMover(new Move(count:count, source: stacks[from], target: stacks[to]));
        }

        // assuming that the stacks are not empty at the end, concatenate the top of each:
        return string.Join("", stacks.Select(stack => stack.Pop()));
    }
}
