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
record Cond(int dim, int num, string jmp);

[ProblemName("Aplenty")]
class Solution : Solver {
    
    // Part 1 can be understood in the context of Part 2. Part 2 asks to compute the
    // 'accepted' volume of a four dimensional hypercube. It has some elaborate way to
    // recursively slice up the cube parallel to its edges to smaller and smaller pieces 
    // and decide if the sub-sub cubes at the bottom are accepted or not. Our Part 2 
    // algorithm follows these rules and returns the 'accepted' volume we are looking for. 

    // We can use this algorithm to solve Part 1 starting from small 1 x 1 x 1 x 1 cubes
    // and checking if they are fully accepted or not.

    public object PartOne(string input) {
        var parts = input.Split("\n\n");
        var rules = ParseRules(parts[0]);
        return (
            from cube in ParseUnitCube(parts[1])
            where AcceptedVolume(rules, cube, "in") == 1
            select cube.Select(dim => dim.begin).Sum()
        ).Sum();
    }

    public object PartTwo(string input) {
        var parts = input.Split("\n\n");
        var rules = ParseRules(parts[0]);
        var cube = Enumerable.Repeat(new Range(1, 4000), 4).ToImmutableArray();
        return AcceptedVolume(rules, cube, "in");
    }

    BigInteger Volume(Cube cube) =>
        cube.Aggregate(BigInteger.One, (m, dim) => m * (dim.end - dim.begin + 1));

    BigInteger AcceptedVolume(Rules rules, Cube cube, string state) {
        if (cube.Any(coord => coord.end < coord.begin)) {
            return 0; // cube is empty
        } else if (state == "A") {
            return Volume(cube); // cube is accepted as it is
        } else if (state == "R") {
            return 0; // cube is rejected
        } else {
            var res = BigInteger.Zero;
            foreach (var stm in rules[state].Split(",")) {
                // slicing happens here in the presence of < and > symbols.
                if (TryParseCond(stm, '<', out var cond)) {
                    // here we have a condition, something like 'x < 1000 : foo'
                    // so the cube is split along the right dimension (x) into two halves:
                    var (lo, hi) = SplitRange(cube[cond.dim], cond.num);
                    // recurse with the accepted half to 'foo':
                    res += AcceptedVolume(rules, cube.SetItem(cond.dim, lo), cond.jmp);
                    // and continue with the rejected half
                    cube = cube.SetItem(cond.dim, hi);
                } else if (TryParseCond(stm, '>', out cond)) {
                    // same as the other case with 'x > 1000'
                    var (lo, hi) = SplitRange(cube[cond.dim], cond.num + 1);
                    res += AcceptedVolume(rules, cube.SetItem(cond.dim, hi), cond.jmp);
                    cube = cube.SetItem(cond.dim, lo);
                } else {
                    res += AcceptedVolume(rules, cube, stm);
                }
            }
            return res;
        }
    }

    bool TryParseCond(string st, char ch, out Cond cond) {
        if (!st.Contains(ch)) {
            cond = null;
            return false;
        } else {
            // st has the form of "x<1000:foo"
            var parts = st.Split(ch, ':');
            cond = new Cond(
                dim: parts[0] switch { "x" => 0, "m" => 1, "a" => 2, _ => 3 },
                num: int.Parse(parts[1]),
                jmp: parts[2]
            );
            return true;
        }
    }

    // returns two ranges containing [r.begin .. num - 1], [num .. r.end], 
    // returns empty ranges where end < begin if num is outside of r
    (Range lo, Range hi) SplitRange(Range r, int num) => (
        r with { begin = r.begin, end = Math.Min(num - 1, r.end) },
        r with { begin = Math.Max(r.begin, num), end = r.end }
    );

    Rules ParseRules(string input) => (
        from line in input.Split('\n')
        let parts = line.Split('{', '}')
        select new KeyValuePair<string, string>(parts[0], parts[1])
    ).ToDictionary();

    IEnumerable<Cube> ParseUnitCube(string input) =>
        from line in input.Split('\n')
        let nums = Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value)).ToArray()
        select nums.Select(n => new Range(n, n)).ToImmutableArray();
}