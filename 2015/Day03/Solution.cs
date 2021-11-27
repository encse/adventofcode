using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2015.Day03;

[ProblemName("Perfectly Spherical Houses in a Vacuum")]
class Solution : Solver {

    public object PartOne(string input) => Run(input, 1);

    public object PartTwo(string input) => Run(input, 2);

    int Run(string input, int actors) {

        var seen = new HashSet<(int, int)>();
        var pos = new(int irow, int icol)[actors];
        for (var i = 0; i < actors; i++) {
            pos[i] = (0, 0);
        }
        seen.Add((0,0));
        
        var actor = 0;
        foreach (var ch in input) {
            switch (ch) {
                case 'v': pos[actor].irow++; break;
                case '<': pos[actor].icol--; break;
                case '>': pos[actor].icol++; break;
                case '^': pos[actor].irow--; break;
            }
            seen.Add(pos[actor]);
            actor = (actor + 1) % actors;
        }
        return seen.Count();
    }
}
