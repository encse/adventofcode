using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day20;

record Pos2(int irow, int icol);
record Pos3(int irow, int icol, int level);
record PosD(int irow, int icol, int dlevel);

[ProblemName("Donut Maze")]
class Solution : Solver {

    public object PartOne(string input) {
        return Solve(input, false);
    }

    public object PartTwo(string input) {
        return Solve(input, true);
    }

    int Solve(string input, bool part2) {
        var mx = input.Split("\n").Select(x => x.ToCharArray()).ToArray();
        var (portals, start, end) = Explore(mx);

        var pos = start;
        var dist = 0;
        var q = new Queue<(Pos3, int dist)>();
        q.Enqueue((pos, dist));

        var seen = new HashSet<Pos3>();
        seen.Add(pos);

        IEnumerable<Pos3> Neighbours(Pos3 pos) {
            foreach (var (drow, dcol) in new[] { (0, -1), (0, 1), (-1, 0), (1, 0) }) {
                yield return new (pos.irow + drow, pos.icol + dcol, pos.level);
            }

            if (portals.ContainsKey(new (pos.irow, pos.icol))) {
                var (irowT, icolT, dlevel) = portals[new (pos.irow, pos.icol)];

                if (!part2) {
                    dlevel = 0;
                }

                if (pos.level + dlevel >= 0) {
                    yield return new (irowT, icolT, pos.level + dlevel);
                }
            }
        }

        while (q.Any()) {
            (pos, dist) = q.Dequeue();
            if (pos == end) {
                return dist;
            }

            foreach (var posT in Neighbours(pos)) {
                if (!seen.Contains(posT)) {
                    var distT = dist + 1;
                    if (mx[posT.irow][posT.icol] == '.') {
                        seen.Add(posT);
                        q.Enqueue((posT, distT));
                    }

                }
            }
        }
        throw new Exception();
    }

    (Dictionary<Pos2, PosD> portals, Pos3 start, Pos3 goal) Explore(char[][] mx) {
        var portals = new Dictionary<Pos2, PosD>();
        var tmp = new Dictionary<string, Pos2>();
        var ccol = mx[0].Length;
        var crow = mx.Length;
        for (var irow = 0; irow < crow - 1; irow++) {
            for (var icol = 0; icol < ccol - 1; icol++) {
                foreach (var (drow, dcol) in new[] { (0, 1), (1, 0) }) {
                    var st = $"{mx[irow][icol]}{mx[irow + drow][icol + dcol]}";
                    if (st.All(char.IsLetter)) {
                        var portal = irow - drow >= 0 && icol - dcol >= 0 && mx[irow - drow][icol - dcol] == '.' ?
                            new Pos2(irow - drow, icol - dcol) :
                            new Pos2(irow + 2 * drow, icol + 2 * dcol);

                        if (tmp.ContainsKey(st)) {
                            var dlevel = portal.icol == 2 || portal.icol == ccol - 3 || portal.irow == 2 || portal.irow == crow - 3 ? -1 : 1;
                            portals[portal] = new (tmp[st].irow, tmp[st].icol, dlevel);
                            portals[tmp[st]] = new (portal.irow, portal.icol, -dlevel);
                        } else {
                            tmp[st] = portal;
                        }
                        mx[irow][icol] = ' ';
                        mx[irow + drow][icol + dcol] = ' ';
                    }
                }
            }
        }

        return (portals, new (tmp["AA"].irow, tmp["AA"].icol, 0), new (tmp["ZZ"].irow, tmp["ZZ"].icol, 0));
    }
}
