using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Day22 {

    class Solution : Solver {

        enum State {
            Clean,
            Weakened,
            Infected,
            Flagged
        }
        public string GetName() => "Sporifica Virus";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var lines = input.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            var crow = lines.Length;
            var ccol = lines[0].Length;
            var infectedCells = new HashSet<(int irow, int icol)>();
            for (int irowT = 0; irowT < crow; irowT++) {
                for (int icolT = 0; icolT < ccol; icolT++) {
                    if (lines[irowT][icolT] == '#') {
                        infectedCells.Add((irowT, icolT));
                    }
                }
            }
            var (irow, icol) = ((crow) / 2, (ccol) / 2);
            var (drow, dcol) = (-1, 0);
            var infections = 0;
            for (int i = 0; i < 10000; i++) {
                if (infectedCells.Contains((irow, icol))) {
                    infectedCells.Remove((irow, icol));
                    (drow, dcol) = (dcol, -drow);
                } else {
                    infectedCells.Add((irow, icol));
                    infections++;
                    (drow, dcol) = (-dcol, drow);
                }
                (irow, icol) = (irow + drow, icol + dcol);
            }
            return infections;
        }

        int PartTwo(string input) {
            var lines = input.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            var crow = lines.Length;
            var ccol = lines[0].Length;
            var infectedCells = new Dictionary<(int irow, int icol), State>();
            for (int irowT = 0; irowT < crow; irowT++) {
                for (int icolT = 0; icolT < ccol; icolT++) {
                    if (lines[irowT][icolT] == '#') {
                        infectedCells.Add((irowT, icolT), State.Infected);
                    }
                }
            }
            var (irow, icol) = ((crow) / 2, (ccol) / 2);
            var (drow, dcol) = (-1, 0);
            var infections = 0;
            for (int i = 0; i < 10000000; i++) {
                var state = infectedCells.ContainsKey((irow, icol)) ? infectedCells[(irow, icol)] : State.Clean;
                State newState = state;
                switch(state){
                    case State.Clean:
                        newState = State.Weakened;
                        (drow, dcol) = (-dcol, drow);
                        break;
                    case State.Weakened:
                        newState = State.Infected;
                        infections++;
                        break;
                     case State.Infected:
                        newState = State.Flagged;
                        (drow, dcol) = (dcol, -drow);
                        break;
                    case State.Flagged:
                        newState = State.Clean;
                        (drow, dcol) = (-drow, -dcol);
                        break;
                   
                }
                infectedCells[(irow, icol)] = newState;
                (irow, icol) = (irow + drow, icol + dcol);
            }
            return infections;
        }
    }
}