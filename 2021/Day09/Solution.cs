using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day09;

[ProblemName("Smoke Basin")]
class Solution : Solver {

    public object PartOne(string input) {
        var map = GetMap(input);

        // get the risk values of the low positions:
        var riskValue = (Pos pos) => 1 + map[pos];
        return GetLowPositions(map).Select(riskValue).Sum();
    }

    public object PartTwo(string input) {
        var map = GetMap(input);

        // find the 3 biggest basins and return a hash computed from their size:
        return GetLowPositions(map)
            .Select(p => BasinSize(map, p))
            .OrderByDescending(basinSize => basinSize)
            .Take(3)
            .Aggregate(1, (m, basinSize) => m * basinSize);
    }

    // store the positions in a dictionary so that we can iterate over them and 
    // to easily deal with positions outside the area (with GetValueOrDefault)
    ImmutableDictionary<Pos, int> GetMap(string input) {
        var lines = input.Split("\n");
        return (
            from irow in Enumerable.Range(0, lines[0].Length)
            from icol in Enumerable.Range(0, lines.Length)
            select new KeyValuePair<Pos, int>(new Pos(irow, icol), lines[irow][icol] - '0')
        ).ToImmutableDictionary();
    }

    IEnumerable<Pos> Neighbours(Pos pos) => 
        new [] {
           pos with {irow = pos.irow + 1},
           pos with {irow = pos.irow - 1},
           pos with {icol = pos.icol + 1},
           pos with {icol = pos.icol - 1},
        };

    // position is low if each of its neighbours is higher:
    public IEnumerable<Pos> GetLowPositions(ImmutableDictionary<Pos, int>  map) =>
        from pos in map.Keys 
        let height = map[pos]
        where Neighbours(pos).All(posN => height < map.GetValueOrDefault(posN, 9))
        select pos;

    public int BasinSize(ImmutableDictionary<Pos, int> map, Pos pos) {
        // flood fill algorithm
        var filled = new HashSet<Pos>{pos};
        var queue = new Queue<Pos>(filled);

        while (queue.Any()) {
            pos = queue.Dequeue();
            filled.Add(pos);
            foreach (var posN in Neighbours(pos)) {
                if (!filled.Contains(posN) && map.GetValueOrDefault(posN, 9) != 9) {
                    queue.Enqueue(posN);
                    filled.Add(posN);
                }
            }
        }

        return filled.Count;
    }
}

record Pos(int irow, int icol);
