using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Day22 {

    enum State {
        Clean,
        Weakened,
        Infected,
        Flagged
    }
    
    class Solution : Solver {

        public string GetName() => "Sporifica Virus";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) =>
            Iterate(input, 10000, (state, drow, dcol) => {
                switch (state) {
                    case State.Clean:
                        return (State.Infected, -dcol, drow);
                    case State.Infected:
                        return (State.Clean, dcol, -drow);
                    default:
                        throw new Exception();
                }
            });

        int PartTwo(string input) =>
            Iterate(input, 10000000, (state, drow, dcol) => {
                switch (state) {
                    case State.Clean:
                        return (State.Weakened, -dcol, drow);
                    case State.Weakened:
                        return (State.Infected, drow, dcol);
                    case State.Infected:
                        return (State.Flagged, dcol, -drow);
                    case State.Flagged:
                        return (State.Clean, -drow, -dcol);
                    default:
                        throw new Exception();
                }
            });


        int Iterate(string input, int iterations, Func<State, int, int, (State State, int irow, int icol)> update) {
            var lines = input.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            var crow = lines.Length;
            var ccol = lines[0].Length;
            var cells = new Dictionary<(int irow, int icol), State>();
            for (int irowT = 0; irowT < crow; irowT++) {
                for (int icolT = 0; icolT < ccol; icolT++) {
                    if (lines[irowT][icolT] == '#') {
                        cells.Add((irowT, icolT), State.Infected);
                    }
                }
            }
            var (irow, icol) = (crow / 2, ccol/ 2);
            var (drow, dcol) = (-1, 0);
            var infections = 0;
            for (int i = 0; i < iterations; i++) {
                var state = cells.TryGetValue((irow, icol), out var s) ? s : State.Clean;
                
                (state, drow, dcol) = update(state, drow, dcol);
                
                if (state == State.Infected) {
                    infections++;
                }
                if (state == State.Clean) {
                    cells.Remove((irow, icol));
                } else {
                    cells[(irow, icol)] = state;
                }
                (irow, icol) = (irow + drow, icol + dcol);
            }
            return infections;
        }
    }
}