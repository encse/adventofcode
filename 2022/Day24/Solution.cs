using System;
using System.Collections.Generic;

namespace AdventOfCode.Y2022.Day24;

[ProblemName("Blizzard Basin")]
class Solution : Solver {

    // We do a standard A* algorithm, the only trick is that
    // the 'map' always changes as blizzards move, I used a Maps
    // class that can be queried with time + irow + icol.

    public object PartOne(string input) {
        var (entry, exit, maps) = Parse(input);
        var state = new SearchState(0, entry);
        return WalkTo(state, exit, maps).time;
    }

    public object PartTwo(string input) {
        var (entry, exit, maps) = Parse(input);
        var state = new SearchState(0, entry);
        state = WalkTo(state, exit, maps);
        state = WalkTo(state, entry, maps);
        state = WalkTo(state, exit, maps);
        return state.time;
    }

    // Standard A* algorithm
    SearchState WalkTo(SearchState intialState, Pos goal, Maps maps) {

        var q = new PriorityQueue<SearchState, int>();

        int f(SearchState state) {
            // estimate the remaining step count with Manhattan distance
            var dist =
                Math.Abs(goal.irow - state.pos.irow) +
                Math.Abs(goal.icol - state.pos.icol);
            return state.time + dist;
        }

        q.Enqueue(intialState, f(intialState));
        HashSet<SearchState> seen = new HashSet<SearchState>();

        while (q.Count > 0) {
            var state = q.Dequeue();
            if (state.pos == goal) {
                return state;
            }

            foreach (var stateNeighbour in NextStates(state, maps)) {
                if (!seen.Contains(stateNeighbour)) {
                    seen.Add(stateNeighbour);
                    q.Enqueue(stateNeighbour, f(stateNeighbour));
                }
            }
        }

        throw new Exception();
    }

    // Increase time with one, look for free neighbour positions
    IEnumerable<SearchState> NextStates(SearchState state, Maps maps) {
        foreach (var pos in new Pos[]{
            state.pos,
            state.pos with {irow=state.pos.irow -1},
            state.pos with {irow=state.pos.irow +1},
            state.pos with {icol=state.pos.icol -1},
            state.pos with {icol=state.pos.icol +1},
        }) {
            if (maps.Get(state.time + 1, pos) == '.') {
                yield return state with {
                    time = state.time + 1,
                    pos = pos
                };
            }
        }
    }

    (Pos entry, Pos exit, Maps maps) Parse(string input) {
        var maps = new Maps(input);
        var entry = new Pos(0, 1);
        var exit = new Pos(maps.crow - 1, maps.ccol - 2);
        return (entry, exit, maps);
    }

    record SearchState(int time, Pos pos);
    record Pos(int irow, int icol);

    // Time indexable list of maps
    class Maps {
        private string[] lines;
        public readonly int crow;
        public readonly int ccol;

        public Maps(string input) {
            lines = input.Split("\n");
            this.crow = lines.Length;
            this.ccol = lines[0].Length;
        }

        public char Get(int time, Pos pos) {
            if (pos.irow == 0 && pos.icol == 1) {
                return '.';
            }
            if (pos.irow == crow - 1 && pos.icol == ccol - 2) {
                return '.';
            }

            if (pos.irow <= 0 || pos.irow >= crow - 1 || pos.icol <= 0 || pos.icol >= ccol - 1) {
                return '#';
            }

            var icolLeft =  (pos.icol - 1 - time + 1000 * (ccol - 2)) % (ccol - 2) + 1;
            var icolRight = (pos.icol - 1 + time + 1000 * (ccol - 2)) % (ccol - 2) + 1;
            var irowUp =    (pos.irow - 1 - time + 1000 * (crow - 2)) % (crow - 2) + 1;
            var irowDown =  (pos.irow - 1 + time + 1000 * (crow - 2)) % (crow - 2) + 1;

            if (
                lines[pos.irow][icolLeft] == '>' ||
                lines[pos.irow][icolRight] == '<' ||
                lines[irowUp][pos.icol] == 'v' ||
                lines[irowDown][pos.icol] == '^'
            ) {
                return '#';
            } else {
                return '.';
            }
        }
    }
}
