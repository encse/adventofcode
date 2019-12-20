using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day20 {


    class Pos2 {
        readonly (int x, int y) repr;
        public int x => repr.x;
        public int y => repr.y;
        private Pos2((int x, int y) pos) {
            this.repr = pos;
        }
        public static implicit operator Pos2((int, int) pos){
            return new Pos2(pos);
        }
        public override bool Equals(object obj) => repr.Equals(obj);
        public override int GetHashCode() => repr.GetHashCode();
    }

    class Pos3 {
        readonly (int x, int y, int z) repr;
        public int x => repr.x;
        public int y => repr.y;
        public int z => repr.z;
        private Pos3((int x, int y, int z) pos) {
            this.repr = pos;
        }
        public static implicit operator Pos3((int, int, int) pos){
            return new Pos3(pos);
        }
        public override bool Equals(object obj) => repr.Equals(obj);
        public override int GetHashCode() => repr.GetHashCode();
    }

    class Solution : Solver {

        public string GetName() => "Donut Maze";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            return Solve(input, false);
        }

        int PartTwo(string input) {
            return Solve(input, true);
        }

        int Solve(string input, bool part2) {
            var mx = input.Split("\n").Select(x => x.ToCharArray()).ToArray();
            var (portals, start, end) = Explore(mx);

            var pos = start;
            var dist = 0;
            var q = new Queue<((int irow, int icol, int level), int dist)>();
            q.Enqueue((pos, dist));

            var seen = new HashSet<(int irow, int icol, int level)>();
            seen.Add(pos);

            while (q.Any()) {
                (pos, dist) = q.Dequeue();
                if (pos == end) {
                    return dist;
                }
                foreach (var (drow, dcol) in new[] { (0, -1), (0, 1), (-1, 0), (1, 0) }) {
                    var (irowT, icolT, levelT) = (pos.irow + drow, pos.icol + dcol, pos.level);
                    var posT = (irowT, icolT, levelT);
                    if (!seen.Contains(posT)) {

                        var distT = dist + 1;

                        if (mx[irowT][icolT] == '.') {
                            seen.Add(posT);
                            q.Enqueue((posT, distT));
                        }

                    }


                }

                if (portals.ContainsKey((pos.irow, pos.icol))) {
                    var (irowT, icolT, dlevel) = portals[(pos.irow, pos.icol)];
                    var posT = (irowT, icolT, pos.level + dlevel);
                    if (!seen.Contains(posT) && pos.level + dlevel >= 0) {

                        var distT = dist + 1;

                        if (mx[irowT][icolT] == '.') {
                            seen.Add(posT);
                            q.Enqueue((posT, distT));
                        }

                    }
                }


            }
            throw new Exception();
        }

        (Dictionary<(int irow, int icol), (int irow, int icol, int dlevel)> portals, (int irow, int icol, int level) start, (int irow, int icol, int level) goal) Explore(char[][] mx) {
            var portals = new Dictionary<(int irow, int icol), (int irow, int icol, int dlevel)>();
            var tmp = new Dictionary<string, (int irow, int icol)>();
            var ccol = mx[0].Length;
            var crow = mx.Length;
            for (var irow = 0; irow < crow - 1; irow++) {
                for (var icol = 0; icol < ccol - 1; icol++) {
                    foreach (var (drow, dcol) in new[] { (0, 1), (1, 0) }) {
                        var st = $"{mx[irow][icol]}{mx[irow + drow][icol + dcol]}";
                        if (st.All(char.IsLetter)) {
                            (int irow, int icol) portal = irow - drow >= 0 && icol - dcol >= 0 && mx[irow - drow][icol - dcol] == '.' ?
                                (irow - drow, icol - dcol) :
                                (irow + 2 * drow, icol + 2 * dcol);

                            if (tmp.ContainsKey(st)) {
                                var dlevel = portal.icol == 2 || portal.icol == ccol - 3 || portal.irow == 2 || portal.irow == crow - 3 ? -1 : 1;
                                portals[portal] = (tmp[st].irow, tmp[st].icol, dlevel);
                                portals[tmp[st]] = (portal.irow, portal.icol, -dlevel);
                            } else {
                                tmp[st] = portal;
                            }
                            mx[irow][icol] = ' ';
                            mx[irow + drow][icol + dcol] = ' ';
                        }
                    }
                }
            }

            return (portals, (tmp["AA"].irow, tmp["AA"].icol, 0), (tmp["ZZ"].irow, tmp["ZZ"].icol, 0));
        }
    }
}