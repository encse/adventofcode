using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day05;

[ProblemName("Supply Stacks")]
class Solution : Solver {

    // The input is parsed into some stacks of 'crates', and move operations 
    // that is to be applied on them. There is a crane which takes some number 
    // of crates from one stack and puts them on the top of an other stack. 
    // Part one and two differs in how this crane works, which is implemented 
    // by the two 'crateMover' functions.
    record struct Move(int count, Stack<char> source, Stack<char> target);

    public object PartOne(string input) => MoveCrates(input, CrateMover9000);
    public object PartTwo(string input) => MoveCrates(input, CrateMover9001);

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

    string MoveCrates(string input, Action<Move> crateMover) {
        var parts = input.Split("\n\n");
        
        var stackDefs = parts[0].Split("\n");
        // [D]
        // [N] [C]
        // [Z] [M] [P]
        //  1   2   3 
      
        // last line defines the number of stacks:
        var stacks = stackDefs
            .Last()
            .Chunk(4)
            .Select(_ => new Stack<char>())
            .ToArray();
        
        // Each input line is processed in 4 character long chunks in bottom up
        // order. Push the next element into the next stack (note how the chunk 
        // and the stack is paired up using the zip function). ' ' means no more 
        // elements to add, just go to the next chunk.
        foreach (var line in stackDefs.Reverse().Skip(1)) {
            foreach (var (stack, item) in stacks.Zip(line.Chunk(4))) {
                if (item[1] != ' ') {
                    stack.Push(item[1]);
                }
            }
        }

        // now parse the move operations and crateMover on them:
        foreach (var line in parts[1].Split("\n")) {
            // e.g. "move 6 from 4 to 3"
            var m = Regex.Match(line, @"move (.*) from (.*) to (.*)");
            var count = int.Parse(m.Groups[1].Value);
            var from = int.Parse(m.Groups[2].Value) - 1;
            var to = int.Parse(m.Groups[3].Value) - 1;
            crateMover(
                new Move(
                    count:count, 
                    source: stacks[from], target: stacks[to]
                ));
        }

        // collect the top of each stack:
        return string.Join("", stacks.Select(stack => stack.Pop()));
    }
}
