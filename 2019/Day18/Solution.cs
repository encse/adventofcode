using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2019.Day18 {


    class Maze {
        string[] maze;
        public Maze(string st) {
            this.maze = st.Split("\n");
        }
        int ccol => maze[0].Length;
        int crow => maze.Length;
        Dictionary<char, (int, int)> positionCache = new Dictionary<char, (int, int)>();
        Dictionary<(char, char), int> distanceCache = new Dictionary<(char, char), int>();
        Dictionary<char, int> minDistCache = new Dictionary<char, int>();

        public char Look((int irow, int icol) pos) {
            var (irow, icol) = pos;
            if (irow < 0 || irow >= crow || icol < 0 || icol >= ccol) {
                return '#';
            }
            return maze[irow][icol];

        }

        public (int irow, int icol) Find(char ch) {
            if (!positionCache.ContainsKey(ch)) {
                for (var irow = 0; irow < crow; irow++) {
                    for (var icol = 0; icol < ccol; icol++) {
                        if (maze[irow][icol] == ch) {
                            positionCache[ch] = (irow, icol);
                            return positionCache[ch];
                        }
                    }
                }
                throw new Exception();
            } else {
                return positionCache[ch];
            }
        }


        public int Distance(char chA, char chB) {
            var key = (chA, chB);
            if (!distanceCache.ContainsKey(key)) {
                distanceCache[key] = ComputeDistance(chA, chB);
            }
            return distanceCache[key];
        }

        int ComputeDistance(char chA, char chB) {
            var pos = Find(chA);
            if (chA == chB) {
                return 0;
            }
            var q = new Queue<((int irow, int icol) pos, int dist)>();
            int dist = 0;
            q.Enqueue((pos, dist));

            var seen = new HashSet<(int irow, int icol)>();
            seen.Add(pos);
            while (q.Any()) {
                (pos, dist) = q.Dequeue();

                foreach (var (drow, dcol) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1) }) {
                    var posT = (pos.irow + drow, pos.icol + dcol);
                    var ch = Look(posT);

                    if (seen.Contains(posT) || ch == '#') {
                        continue;
                    }

                    seen.Add(posT);
                    var distT = dist + 1;

                    if (ch == chB) {
                        return distT;
                    } else {
                        q.Enqueue((posT, distT));
                    }
                }
            }
            throw new Exception();
        }
    }
    
    class Solution : Solver {

        public string GetName() => "Many-Worlds Interpretation";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var maze = new Maze(input);

            var pos = maze.Find('@');

            var dependencies = GenerateDependencies(pos, maze);
            return Solve(maze, dependencies, ImmutableHashSet.CreateRange(dependencies.Keys), '@', new Dictionary<(char, string), int>());
        }


        int PartTwo(string input) {
            var d = 0;
            foreach (var subMaze in GenerateSubMazes(input)) {
                var maze = new Maze(subMaze);

                var pos = maze.Find('@');

                var dependencies = GenerateDependencies(pos, maze);
                d += Solve(maze, dependencies, ImmutableHashSet.CreateRange(dependencies.Keys), '@', new Dictionary<(char, string), int>());
            }
            return d;
        }

        IEnumerable<string> GenerateSubMazes(string input) {
            var mx = input.Split("\n").Select(x => x.ToCharArray()).ToArray();
            var crow = mx.Length;
            var ccol = mx[0].Length;
            var hrow = crow / 2;
            var hcol = ccol / 2;
            var pattern = "@#@\n###\n@#@".Split();
            foreach (var drow in new[] { -1, 0, 1 }) {
                foreach (var dcol in new[] { -1, 0, 1 }) {
                    mx[hrow + drow][hcol + dcol] = pattern[1 + drow][1 + dcol];
                }
            }

            foreach (var (drow, dcol) in new[] { (0, 0), (0, hcol + 1), (hrow + 1, 0), (hrow + 1, hcol + 1) }) {
                var res = "";
                for (var irow = 0; irow < hrow; irow++) {
                    res += string.Join("", mx[irow + drow].Skip(dcol).Take(hcol)) + "\n";
                }

                for (var ch = 'A'; ch <= 'Z'; ch++) {
                    if (!res.Contains(char.ToLower(ch))) {
                        res = res.Replace(ch, '.');
                    }
                }
                res = res.Substring(0, res.Length - 1);
                yield return res;
            }
        }

        int Solve(Maze maze,
            Dictionary<char, ImmutableHashSet<char>> dependencies,
            ImmutableHashSet<char> keys,
            char startKey,
            Dictionary<(char, string), int> cache
        ) {

            var cacheKey = (startKey, string.Join("", keys.OrderBy(x => x)));
            if (cache.ContainsKey(cacheKey)) {
                return cache[cacheKey];
            }

            if (keys.Count == 0) {
                return 0;
            }
            var d = int.MaxValue;
            foreach (var key in keys) {
                if (dependencies[key].Intersect(keys).Count == 0) {
                    var innen = maze.Distance(startKey, key) + Solve(maze, dependencies, keys.Remove(key), key, cache);
                    d = Math.Min(innen, d);
                }
            }

            cache[cacheKey] = d;
            return d;
        }



        Dictionary<char, ImmutableHashSet<char>> GenerateDependencies((int irow, int icol) pos, Maze maze) {
            var q = new Queue<((int irow, int icol) pos, string doors)>();
            var doors = "";
            q.Enqueue((pos, doors));

            var res = new Dictionary<char, ImmutableHashSet<char>>();
            var seen = new HashSet<(int irow, int icol)>();
            seen.Add(pos);
            while (q.Any()) {
                (pos, doors) = q.Dequeue();

                foreach (var (drow, dcol) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1) }) {
                    var posT = (pos.irow + drow, pos.icol + dcol);
                    var ch = maze.Look(posT);

                    if (seen.Contains(posT) || ch == '#') {
                        continue;
                    }

                    seen.Add(posT);
                    var doorsT = doors;

                    if (ch >= 'a' && ch <= 'z') {
                        res[ch] = ImmutableHashSet.CreateRange(doors.ToLower());
                        doorsT += ch;
                    }

                    if (ch >= 'A' && ch <= 'Z') {
                        doorsT += ch;
                    }
                    q.Enqueue((posT, doorsT));
                }
            }
            return res;
        }
    }
}