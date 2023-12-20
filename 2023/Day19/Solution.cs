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
record Cond(int dim, char op, int num, string state);

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
            where AcceptedVolume(rules, cube) == 1
            select cube.Select(r => r.begin).Sum()
        ).Sum();
    }

    public object PartTwo(string input) {
        var parts = input.Split("\n\n");
        var rules = ParseRules(parts[0]);
        var cube = Enumerable.Repeat(new Range(1, 4000), 4).ToImmutableArray();
        return AcceptedVolume(rules, cube);
    }

    BigInteger AcceptedVolume(Rules rules, Cube cube) {
        var q = new Queue<(Cube cube, string state)>();
        q.Enqueue((cube, "in"));

        BigInteger res = 0;
        while (q.Any()) {
            (cube, var state) = q.Dequeue();
            if (cube.Any(coord => coord.end < coord.begin)) {
                continue; // cube is empty
            } else if (state == "R") {
                continue; // cube is rejected
            } else if (state == "A") {
                res += Volume(cube); // cube is accepted
            } else {
                foreach (var stm in rules[state].Split(",")) {
                    Cond cond = TryParseCond(stm);
                    if (cond == null) {
                        q.Enqueue((cube, stm));
                    } else if (cond.op == '<') {
                        var (cube1, cube2) = CutCube(cube, cond.dim, cond.num - 1);
                        q.Enqueue((cube1, cond.state));
                        cube = cube2;
                    } else if (cond?.op == '>') {
                        var (cube1, cube2) = CutCube(cube, cond.dim, cond.num);
                        cube = cube1;
                        q.Enqueue((cube2, cond.state));
                    } 
                }
            }
        }
        return res;
    }

    BigInteger Volume(Cube cube) =>
        cube.Aggregate(BigInteger.One, (m, r) => m * (r.end - r.begin + 1));
  
    // Cuts a cube along the specified dimension, other dimensions are unaffected.
    (Cube lo, Cube hi) CutCube(Cube cube, int dim, int num) {
        var r = cube[dim];
        return (
            cube.SetItem(dim, r with { end = Math.Min(num, r.end) }),
            cube.SetItem(dim, r with { begin = Math.Max(r.begin, num + 1) })
        );
    }

    Cond TryParseCond(string st) =>
        st.Split('<', '>', ':') switch {
            ["x", var num, var state] => new Cond(0, st[1], int.Parse(num), state),
            ["m", var num, var state] => new Cond(1, st[1], int.Parse(num), state),
            ["a", var num, var state] => new Cond(2, st[1], int.Parse(num), state),
            ["s", var num, var state] => new Cond(3, st[1], int.Parse(num), state),
            _ => null
        };

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