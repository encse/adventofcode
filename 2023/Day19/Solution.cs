namespace AdventOfCode.Y2023.Day19;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Numerics;
using Rules = System.Collections.Generic.Dictionary<string, string>;
using Cube = System.Collections.Immutable.ImmutableArray<Range>;

record Range(int begin, int end);
record Cond(int dim, int num, string state);

[ProblemName("Aplenty")]
class Solution : Solver {

    // Part 1 can be understood in the context of Part 2. Part 2 asks to compute 
    // the accepted volume of a four dimensional hypercube. It has some elaborate 
    // way to slice up the cube parallel to its edges to smaller and smaller pieces 
    // and decide if the final sub-sub cubes are accepted or not. Our Part 2 
    // algorithm follows these rules and returns the 'accepted'volume we are 
    // looking for. 

    // We can use this algorithm to solve Part 1 starting from unit sized cubes
    // and checking if they are fully accepted or not.

    public object PartOne(string input) {
        var parts = input.Split("\n\n");
        var rules = ParseRules(parts[0]);
        return (
            from cube in ParseUnitCube(parts[1])
            where Accepted(rules, cube) == 1
            select cube.Select(r => r.begin).Sum()
        ).Sum();
    }

    public object PartTwo(string input) {
        var parts = input.Split("\n\n");
        var rules = ParseRules(parts[0]);
        var cube = Enumerable.Repeat(new Range(1, 4000), 4).ToImmutableArray();
        return Accepted(rules, cube);
    }

    BigInteger Volume(Cube cube) =>
        cube.Aggregate(BigInteger.One, (m, r) => m * (r.end - r.begin + 1));

    BigInteger Accepted(Rules rules, Cube cube) {
        var q = new Queue<(Cube cube, string state)>();
        q.Enqueue((cube, "in"));
        
        BigInteger res = 0;
        while (q.Any()) {
            (cube, var state) = q.Dequeue();
            if (cube.Any(coord => coord.end < coord.begin)) {
                continue; // cube is empty
            } else if (state == "A") {
                res += Volume(cube); // cube is accepted
            } else if (state == "R") {
                continue; // cube is rejected
            } else {
                foreach (var stm in rules[state].Split(",")) {
                    if (TryParseCond(stm, '<', out var cond)) {
                        var (cube1, cube2) = SplitCube(cube, cond.dim, cond.num);
                        q.Enqueue((cube1, cond.state));
                        cube = cube2;
                    } else if (TryParseCond(stm, '>', out cond)) {
                        var (cube1, cube2) = SplitCube(cube, cond.dim, cond.num + 1);
                        q.Enqueue((cube2, cond.state));
                        cube = cube1;
                    } else {
                        q.Enqueue((cube, stm));
                    }
                }
            }
        }
        return res;
    }

    // Cuts a cube at 'num' along the specified dimension, other dimensions are unaffected.
    (Cube lo, Cube hi) SplitCube(Cube cube, int dim, int num) {
        var r = cube[dim];
        return (
            cube.SetItem(dim, r with { end = Math.Min(num - 1, r.end) }),
            cube.SetItem(dim, r with { begin = Math.Max(r.begin, num) })
        );
    }

    int ParseDim(string st) => st switch { "x" => 0, "m" => 1, "a" => 2, _ => 3 };

    bool TryParseCond(string st, char ch, out Cond cond) {
        var parts = st.Split(ch, ':');
        var success = parts.Length == 3;
        cond = success ? new Cond(ParseDim(parts[0]), int.Parse(parts[1]), parts[2]) : null;
        return success;
    }

    Rules ParseRules(string input) => (
        from line in input.Split('\n')
        let parts = line.Split('{', '}')
        select new KeyValuePair<string, string>(parts[0], parts[1])
    ).ToDictionary();

    IEnumerable<Cube> ParseUnitCube(string input) =>
        from line in input.Split('\n')
        let nums = Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value))
        select nums.Select(n => new Range(n, n)).ToImmutableArray();
}