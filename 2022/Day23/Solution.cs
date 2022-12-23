using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Y2022.Day23;

[ProblemName("Unstable Diffusion")]
class Solution : Solver {

    // I used complex numbers for a change. The map is represented with a hashset of positions.

    public object PartOne(string input) {
        var state = Parse(input);
        for (var i = 0; i < 10; i++) {
            state = Step(state);
        }

        // smallest enclosing rectangle
        var width = state.elves.Select(p => p.Real).Max() - 
                    state.elves.Select(p => p.Real).Min() + 1;

        var height = state.elves.Select(p => p.Imaginary).Max() - 
                     state.elves.Select(p => p.Imaginary).Min() + 1;

        return width * height - state.elves.Count;
    }

    public object PartTwo(string input) {
        // interate until fixpoint is reached
        var steps = 0;

        var state = Parse(input);
        while (!state.fixpoint) {
            state = Step(state);
            steps++;
        }

        return steps;
    }

    State Step(State state) {

        bool occupied(Complex pos) => state.elves.Contains(pos);
        bool lonely(Complex pos) => directions.All(dir => !occupied(pos + dir));

        // elf proposes a postion if nobody is nearby in that direction
        bool proposes(Complex elf, Complex dir) => ExtendDir(dir).All(d => !occupied(elf + d));

        // for each position (key) it has a list of the elves who wants to step there
        var proposals = new Dictionary<Complex, List<Complex>>();

        foreach (var elf in state.elves) {
            if (lonely(elf)) {
                continue;
            }

            foreach (var dir in state.directions) {
                if (proposes(elf, dir)) {
                    var pos = elf + dir;
                    if (!proposals.ContainsKey(pos)) {
                        proposals[pos] = new List<Complex>();
                    }
                    proposals[pos].Add(elf);
                    break;
                }
            }
        }

        // move elves, compute fixpoint flag
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
            state.elves,
            state.directions.Skip(1).Concat(state.directions.Take(1)).ToList(),
            fixpoint
        );
    }

    State Parse(string input) {
        var lines = input.Split("\n");

        var elves = (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            where lines[irow][icol] == '#'
            select new Complex(icol, irow)
        ).ToHashSet();

        var directions = new List<Complex>() { N, S, W, E };

        return new State(elves, directions, fixpoint: false);
    }

    ///  -------

    record State(HashSet<Complex> elves, List<Complex> directions, bool fixpoint);

    static Complex N = new Complex(0, -1);
    static Complex E = new Complex(1, 0);
    static Complex S = new Complex(0, 1);
    static Complex W = new Complex(-1, 0);
    static Complex NW = N + W;
    static Complex NE = N + E;
    static Complex SE = S + E;
    static Complex SW = S + W;

    static Complex[] directions = new[] { NW, N, NE, E, SE, S, SW, W };

    // Extends an ordinal position with its intercardinal neighbours
    Complex[] ExtendDir(Complex dir) =>
        dir == N ? new[] { NW, N, NE } :
        dir == E ? new[] { NE, E, SE } :
        dir == S ? new[] { SW, S, SE } :
        dir == W ? new[] { NW, W, SW } :
                   throw new Exception();

}
