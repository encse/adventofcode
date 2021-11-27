using System;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day06;

[ProblemName("Custom Customs")]      
class Solution : Solver {

    public object PartOne(string input) => Solve(input, (a,b) => a.Union(b));
    public object PartTwo(string input) => Solve(input, (a,b) => a.Intersect(b));

    int Solve(string input, Func<ImmutableHashSet<char>, ImmutableHashSet<char>, ImmutableHashSet<char>> combine) {
        return (
            from grp in input.Split("\n\n")
            let answers = from line in grp.Split("\n") select line.ToImmutableHashSet()
            select answers.Aggregate(combine).Count
        ).Sum();
    }
}
