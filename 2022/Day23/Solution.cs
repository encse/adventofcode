using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day23;

[ProblemName("Unstable Diffusion")]
class Solution : Solver {

    public object PartOne(string input) {
        var state = Parse(input);
        for (var i = 0; i < 10; i++) {
            state = Step(state);
        }
        var w = state.elves.MaxBy(p => p.icol).icol - state.elves.MinBy(p => p.icol).icol + 1;
        var h = state.elves.MaxBy(p => p.irow).irow - state.elves.MinBy(p => p.irow).irow + 1;
        return w * h - state.elves.Count;
    }

    public object PartTwo(string input) {
        var state = Parse(input);
        var steps = 0;
        while (!state.fixpoint) {
            state = Step(state);
            steps++;
        }
        return steps;
    }


    State Step(State state) {

        bool occupied(Pos pos) => state.elves.Contains(pos);

        bool lonely(Pos elf) => directions.All(dir => !occupied(elf + dir));

        // elf proposes a postion if nobody is nearby in that direction
        bool proposes(Pos elf, string dir) => extendDir(dir).All(d => !occupied(elf + d));

        // adds intercardinal positions to an ordinal postion, e.g for N it returns NW, N, NE; 
        string[] extendDir(string dir) => directions.Where(d => d.Contains(dir)).ToArray();

        // for each position (key) it has a list of the elves who wants to step there
        var proposals = new Dictionary<Pos, List<Pos>>();

        foreach (var elf in state.elves) {
            if (lonely(elf)) {
                continue;
            }

            foreach (var dir in state.directions) {
                if (proposes(elf, dir)) {
                    var pos = elf + dir;
                    if (!proposals.ContainsKey(pos)) {
                        proposals[pos] = new List<Pos>();
                    }
                    proposals[pos].Add(elf);
                    break;
                }
            }
        }

        var fixpoint = true;
        foreach (var p in proposals) {
            var (to, from) = p;
            if (from.Count == 1) {
                state.elves.Remove(from.Single());
                state.elves.Add(to);
                fixpoint = false;
            }
        }
        
        return new State(
            fixpoint,
            state.elves,
            state.directions.Skip(1).Concat(state.directions.Take(1)).ToList()
        );
    }

    State Parse(string input) {
        var lines = input.Split("\n");
        var elves = (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            where lines[irow][icol] == '#'
            select new Pos(irow, icol)
        ).ToHashSet();
        var directions = new List<string>() { "N", "S", "W", "E" };

        return new State(fixpoint:false, elves, directions: directions);
    }

    string[] directions = "NW N NE E SE S SW W".Split(" ");

    record Pos(int irow, int icol) {
        public static Pos operator +(Pos pos, string dir) =>
            dir switch {
                "N" => pos with { irow = pos.irow - 1 },
                "S" => pos with { irow = pos.irow + 1 },
                "W" => pos with { icol = pos.icol - 1 },
                "E" => pos with { icol = pos.icol + 1 },
                "NW" => pos + "N" + "W",
                "NE" => pos + "N" + "E",
                "SW" => pos + "S" + "W",
                "SE" => pos + "S" + "E",
                _ => throw new Exception()
            };
    }

    record State(bool fixpoint, HashSet<Pos> elves, List<string> directions);

}
