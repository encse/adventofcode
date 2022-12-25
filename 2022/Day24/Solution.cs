using System;
using System.Collections.Generic;

namespace AdventOfCode.Y2022.Day24;

[ProblemName("Blizzard Basin")]
class Solution : Solver {

    // We do a standard A* algorithm, the only trick is that
    // the 'map' always changes as blizzards move, so our position
    // is now a space time coordinate. 
    // I used an efficent Maps class that can be queried with these.

    record Pos(int time, int irow, int icol);

    public object PartOne(string input) {
        var (entry, exit, maps) = Parse(input);
        return WalkTo(entry, exit, maps).time;
    }

    public object PartTwo(string input) {
        var (entry, exit, maps) = Parse(input);
        var pos = WalkTo(entry, exit, maps);
        pos = WalkTo(pos, entry, maps);
        pos = WalkTo(pos, exit, maps);
        return pos.time;
    }

    // Standard A* algorithm
    Pos WalkTo(Pos start, Pos goal, Maps maps) {

        var q = new PriorityQueue<Pos, int>();

        int f(Pos pos) {
            // estimate the remaining step count with Manhattan distance
            var dist =
                Math.Abs(goal.irow - pos.irow) +
                Math.Abs(goal.icol - pos.icol);
            return pos.time + dist;
        }

        q.Enqueue(start, f(start));
        HashSet<Pos> seen = new HashSet<Pos>();

        while (q.Count > 0) {
            var pos = q.Dequeue();
            if (pos.irow == goal.irow && pos.icol == goal.icol) {
                return pos;
            }

            foreach (var nextPos in NextPositions(pos, maps)) {
                if (!seen.Contains(nextPos)) {
                    seen.Add(nextPos);
                    q.Enqueue(nextPos, f(nextPos));
                }
            }
        }

        throw new Exception();
    }

    // Increase time, look for free neighbours
    IEnumerable<Pos> NextPositions(Pos pos, Maps maps) {
        pos = pos with {time = pos.time + 1};
        foreach (var nextPos in new Pos[]{
            pos,
            pos with {irow=pos.irow -1},
            pos with {irow=pos.irow +1},
            pos with {icol=pos.icol -1},
            pos with {icol=pos.icol +1},
        }) {
            if (maps.Get(nextPos) == '.') {
                yield return nextPos;
            }
        }
    }

    (Pos entry, Pos exit, Maps maps) Parse(string input) {
        var maps = new Maps(input);
        var entry = new Pos(0, 0, 1);
        var exit = new Pos(int.MaxValue, maps.crow - 1, maps.ccol - 2);
        return (entry, exit, maps);
    }

    // Space-time indexable map
    class Maps {
        private string[] map;
        public readonly int crow;
        public readonly int ccol;

        public Maps(string input) {
            map = input.Split("\n");
            this.crow = map.Length;
            this.ccol = map[0].Length;
        }

        public char Get(Pos pos) {
            if (pos.irow == 0 && pos.icol == 1) {
                return '.';
            }
            if (pos.irow == crow - 1 && pos.icol == ccol - 2) {
                return '.';
            }

            if (pos.irow <= 0 || pos.irow >= crow - 1 || 
                pos.icol <= 0 || pos.icol >= ccol - 1
            ) {
                return '#';
            }

            // blizzards have a horizontal and a vertical loop
            // it's easy to check the original postions with going back in time
            // using modular arithmetic
            var hmod = ccol - 2;
            var vmod = crow - 2;

            var icolW = (pos.icol - 1 + hmod - (pos.time % hmod)) % hmod + 1;
            var icolE = (pos.icol - 1 + hmod + (pos.time % hmod)) % hmod + 1;
            var icolN = (pos.irow - 1 + vmod - (pos.time % vmod)) % vmod + 1;
            var icolS = (pos.irow - 1 + vmod + (pos.time % vmod)) % vmod + 1;

            return 
                map[pos.irow][icolW] == '>' ? '>':
                map[pos.irow][icolE] == '<' ? '<':
                map[icolN][pos.icol] == 'v' ? 'v':
                map[icolS][pos.icol] == '^' ? '^':
                                              '.';
        }
    }
}
