using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day02;

[ProblemName("Dive!")]
class Solution : Solver {

    public object PartOne(string input) {
        return Parse(input)
           .Aggregate(
               new State1(0, 0),
               (state, step) => step.dir switch {
                   'f' => state with { x = state.x + step.amount },
                   'u' => state with { y = state.y - step.amount },
                   'd' => state with { y = state.y + step.amount },
                   _ => throw new Exception(),
               },
               res => res.x * res.y
           );
    }

    public object PartTwo(string input) {
        return Parse(input)
           .Aggregate(
               new State2(0, 0, 0),
               (state, step) => step.dir switch {
                   'f' => state with { 
                              x = state.x + step.amount, 
                              y = state.y + step.amount * state.aim 
                          },
                   'u' => state with { aim = state.aim - step.amount },
                   'd' => state with { aim = state.aim + step.amount },
                   _ => throw new Exception(),
               },
               res => res.x * res.y
           );
    }

    IEnumerable<Input> Parse(string st) => 
        from 
            line in st.Split('\n')
            let parts = line.Split()
        select 
            new Input(parts[0][0], int.Parse(parts[1]));
}

record Input(char dir, int amount);
record State1(int x, int y);
record State2(int x, int y, int aim);