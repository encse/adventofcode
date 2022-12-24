using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day24;

[ProblemName("Blizzard Basin")]
class Solution : Solver {
    
    // We do a standard A* algorithm, the only trick is that
    // the 'map' always changes as blizzards move. Luckily blizzards get 
    // into a loop pretty soon, so we have to deal with only a few 
    // different maps. These are precomputed in the Parse() phase and
    // stored in a Maps structure. Later it's very cheap to check if
    // a position is free or not at a given time.

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

        void enqueue(SearchState state) {
            // estimate the remaining step count with Manhattan distance
            var dist =
                Math.Abs(goal.irow - state.pos.irow) +
                Math.Abs(goal.icol - state.pos.icol);
            q.Enqueue(state, state.time + dist);
        }

        enqueue(intialState);
        HashSet<SearchState> seen = new HashSet<SearchState>();

        while (q.Count > 0) {
            var state = q.Dequeue();
            if (state.pos == goal) {
                return state;
            }

            foreach (var stateNeighbour in NextStates(state, maps)) {
                if (!seen.Contains(stateNeighbour)) {
                    seen.Add(stateNeighbour);
                    enqueue(stateNeighbour);
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

    // Create a list of maps from the input containing all blizzard positions as
    // time goes. It will loop pretty quickly.
    (Pos entry, Pos exit, Maps maps) Parse(string input) {
        var mapList = new List<string[]>();
        var blizzards = ParseBlizzards(input);
        var map = MarkBlizzards(input, blizzards);
        while (true) {
            mapList.Add(map);
            blizzards = blizzards.Select(b => b.Step()).ToList();
            map = MarkBlizzards(input, blizzards);
            if (string.Join("\n", map) == string.Join("\n", mapList[0])) {
                break;
            }
        }

        var maps = new Maps(mapList.ToArray());
        var entry = new Pos(0, 1);
        var exit = new Pos(maps.crow - 1, maps.ccol - 2);
        return (entry, exit, maps);
    }

    List<Blizzard> ParseBlizzards(string input) {
        var lines = input.Split("\n");
        var crow = lines.Length;
        var ccol = lines[0].Length;

        var result = new List<Blizzard>();
        for (var irow = 0; irow < crow; irow++) {
            for (var icol = 0; icol < ccol; icol++) {
                var ch = lines[irow][icol];
                var dir =
                    ch == '>' ? new Pos(0, 1) :
                    ch == '<' ? new Pos(0, -1) :
                    ch == '^' ? new Pos(-1, 0) :
                    ch == 'v' ? new Pos(1, 0) :
                                new Pos(0, 0);
                if (dir != new Pos(0, 0)) {
                    result.Add(new Blizzard(new Pos(irow, icol), dir, crow, ccol));
                }
            }
        }
        return result;
    }


    string[] MarkBlizzards(string input, IEnumerable<Blizzard> blizzards) {
        var map = input
            .Replace(">", ".")
            .Replace("<", ".")
            .Replace("^", ".")
            .Replace("v", ".")
            .Split("\n")
            .Select(line => line.ToCharArray())
            .ToArray();

        foreach (var b in blizzards) {
            map[b.pos.irow][b.pos.icol] = 'B';
        }

        return map.Select(line => string.Join("", line)).ToArray();
    }

    record SearchState(int time, Pos pos);

    record Pos(int irow, int icol);

    record Blizzard(Pos pos, Pos dir, int crow, int ccol) {
        public Blizzard Step() {
            var irow = pos.irow + dir.irow;
            if (irow == 0) {
                irow = crow - 2;
            } else if (irow == crow - 1) {
                irow = 1;
            }

            var icol = pos.icol + dir.icol;
            if (icol == 0) {
                icol = ccol - 2;
            } else if (icol == ccol - 1) {
                icol = 1;
            }

            return this with { pos = new Pos(irow, icol) };
        }
    }

    // time indexable list of maps
    record Maps(string[][] maps) {
        public int crow => maps[0].Length;
        public int ccol => maps[0][0].Length;

        public char Get(int time, Pos pos) {
            var map = maps[time % maps.Length];

            if (pos.irow >= 0 && pos.irow < crow &&
                pos.icol >= 0 && pos.icol < ccol
            ) {
                return map[pos.irow][pos.icol];
            } else {
                return '#';
            }
        }
    }

}
