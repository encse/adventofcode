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

        public int MinDistance(char chA, ImmutableHashSet<char> allKeys) {
            if (!minDistCache.ContainsKey(chA)) {
                var s = int.MaxValue;
                foreach (var keyB in allKeys) {
                    if (keyB != chA) {
                        s = Math.Min(s, Distance(chA, keyB));
                    }
                }
                minDistCache[chA] = s;
            }
            return minDistCache[chA];
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

        void Tsto() {
            for (var irow = 0; irow < crow; irow++) {
                for (var icol = 0; icol < ccol; icol++) {
                    Console.Write(Look((irow, icol)));
                }
                Console.WriteLine();
            }
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

            var keysToDoors = Explore(pos, maze);
            return Foo(maze, keysToDoors, ImmutableHashSet.CreateRange(keysToDoors.Keys), '@', new Dictionary<(char, string), int>());
            // var (path, length) = Astar(maze, keysToDoors);

            // Console.WriteLine(path + " " + length);
            // return length;

            // foreach (var (a, b) in Split(maze, keysToDoors, keysToDoors.Keys)) {
            //     foreach (var (c, d) in Split(maze, keysToDoors, a)) {
            //         foreach (var x in new[] { a, b }) {
            //             var d = 100000;
            //             foreach (var res in Paths(maze,
            //                     x,
            //                     keysToDoors,
            //                     ImmutableHashSet.CreateRange(keysToDoors.Keys),
            //                     ImmutableList.Create<char>(), d
            //                 )) {
            //                 d = res.Item2;
            //             }
            //             Console.Write(".");
            //         }
            //     }
            // }
            // return 0;
            // Func<int, bool> ok = (k) =>
            //     Paths(maze,
            //         ImmutableHashSet.CreateRange(keysToDoors.Keys),
            //         keysToDoors,
            //         ImmutableHashSet.CreateRange(keysToDoors.Keys),
            //         ImmutableList.Create<char>(), k
            //     ).Any();

            // // var hi = 1;
            // // while (!ok(hi)) {
            // //     hi *= 2;
            // // }

            // var hi = 7513;
            // var lo = 7511;
            // for (var i = 7512; i > 0; i--) {
            //     Console.WriteLine(i);
            //     var t = ok(i);
            //     if (!ok(i)) {
            //         return i;
            //     }
            // }
            // while (hi - lo > 1) {

            //     var s = (lo + hi) / 2;
            //     Console.Write(s);
            //     var t = ok(s);
            //     Console.WriteLine(" " + t);
            //     if (t) {
            //         hi = s;
            //     } else {
            //         lo = s;
            //     }
            // }
            // return hi;

        }
        int Foo(Maze maze,
            Dictionary<char, ImmutableHashSet<char>> keysToDoors,
            ImmutableHashSet<char> keys,
            char startKey,
            Dictionary<(char, string), int> cache
        ) {

            var cacheKey = (startKey, string.Join("", keys.OrderBy(x=>x)));
            if (cache.ContainsKey(cacheKey)){
                return cache[cacheKey];
            }

            if(keys.Count == 0){
                return 0;
            }
            var d = int.MaxValue;
            foreach(var key in keys){

                var dependencies = keysToDoors[key];
                if (dependencies.Intersect(keys).Count == 0){
                    var innen = maze.Distance(startKey, key) + Foo(maze, keysToDoors, keys.Remove(key), key, cache);
                    d = Math.Min(innen, d);
                }
            }

            cache[cacheKey] = d;
            return d;
        }

        IEnumerable<(char, ImmutableHashSet<char>, ImmutableHashSet<char>)> Split(
            Maze maze,
            Dictionary<char, ImmutableHashSet<char>> keysToDoors,
            ImmutableHashSet<char> keys
        ) {

            var bestKey = '\0';
            var m = -1;

            ImmutableHashSet<char> dependenciesOf(char key) {
                return keysToDoors[key].Intersect(keys);
            }

            foreach (var key in keys) {
                var balra = dependenciesOf(key).Count;
                var jobbra = 0;
                foreach (var keyB in keys) {
                    if (dependenciesOf(keyB).Contains(key)) {
                        jobbra++;
                    }
                }

                if (balra + jobbra > m) {
                    bestKey = key;
                    m = balra + jobbra;
                }
            }

            var left = ImmutableHashSet.Create<char>();
            var right = ImmutableHashSet.Create<char>();

            var rest = new List<char>();
            foreach (var key in keys) {
                if (dependenciesOf(key).Contains(bestKey)) {
                    right = right.Add(key);
                } else if (dependenciesOf(bestKey).Contains(key)) {
                    left = left.Add(key);
                } else if (key != bestKey) {
                    rest.Add(key);
                }
            }

            for (var mask = 0; mask < 1 << rest.Count; mask++) {
                var resLeft = left;
                var resRight = right;
                for (var i = 0; i < rest.Count; i++) {
                    if ((mask >> i) % 2 == 0) {
                        resLeft = resLeft.Add(rest[i]);
                    } else {
                        resRight = resRight.Add(rest[i]);
                    }
                }

                yield return (bestKey, resLeft, resRight);
            }

        }
        int Distance(IReadOnlyList<char> path, Maze maze) {

            if (path.Count == 0) {
                return 0;
            }
            var d = maze.Distance('@', path[0]);
            for (var i = 1; i < path.Count; i++) {
                d += maze.Distance(path[i - 1], path[i]);
            }
            return d;
        }
        int EstimatedRemainingDistance(ImmutableList<char> path, ImmutableHashSet<char> remainingKeys, Maze maze, ImmutableHashSet<char> allKeys) {

            if (remainingKeys.Count == 0) {
                return 0;
            }

            var d = 0;

            var m1 = 0;
            foreach (var keyA in remainingKeys) {
                m1 += maze.MinDistance(keyA, allKeys);
            }

            var m2 = 0;
            foreach (var keyA in remainingKeys) {
                foreach (var keyB in remainingKeys) {
                    if (keyA != keyB) {
                        m2 = Math.Max(m2, maze.Distance(keyA, keyB));
                    }
                }
            }
            // if(m2 > m1){
            //     Console.Write("c");
            // } else  {
            //     Console.Write(".");
            // }

            d += Math.Max(m1, m2);

            return d;

            // return d;
            // if (remainingKeys.Count == 0) {
            //     var d = maze.Distance('@', path[0]);
            //     for (var i = 1; i < path.Count; i++) {
            //         d += maze.Distance(path[i - 1], path[i]);
            //     }

            //     // var m = 0;
            //     // foreach (var keyA in remainingKeys) {
            //     //     foreach (var keyB in remainingKeys) {
            //     //         if (keyA != keyB) {
            //     //             m = Math.Max(m, maze.Distance(keyA, keyB));
            //     //         }
            //     //     }
            //     // }
            //     // d += m;
            //     return d;
            // } else {

            //     var d = 0;
            //     foreach (var keyA in remainingKeys) {
            //         var min = int.MaxValue;
            //         for (var j = 0; j <= path.Count; j++) {
            //             var pathT = path.Insert(j, keyA);

            //             var s = maze.Distance('@', pathT[0]);
            //             for (var i = 1; i < pathT.Count; i++) {
            //                 s += maze.Distance(pathT[i - 1], pathT[i]);
            //             }

            //             min = Math.Min(min, s);

            //         }
            //         d = Math.Max(0, min);
            //     }

            //     // var m = 0;
            //     // foreach (var keyA in remainingKeys) {
            //     //     foreach (var keyB in remainingKeys) {
            //     //         if (keyA != keyB) {
            //     //             m = Math.Max(m, maze.Distance(keyA, keyB));
            //     //         }
            //     //     }
            //     // }
            //     // d += m;
            //     return d;

            // }
        }


        // (string, int) Astar(Maze maze, Dictionary<char, ImmutableHashSet<char>> keysToDoors) {
        //     var allKeys = ImmutableHashSet.CreateRange(keysToDoors.Keys);
        //     var q = new PQueue<(ImmutableList<char> path, ImmutableHashSet<char> remainingKeys)>();
        //     q.Enqueue(0, (ImmutableList.Create<char>(), ImmutableHashSet.CreateRange(keysToDoors.Keys)));


        //     while (q.Any()) {
        //         var (path, remainingKeys) = q.Dequeue();

        //         if (!remainingKeys.Any()) {
        //             return (string.Join("", path), Distance(path, maze));
        //         }

        //         foreach (var key in remainingKeys) {
        //             var dependencies = keysToDoors[key].ToHashSet();

        //             var istart = -1;
        //             for (var i = 0; i < path.Count; i++) {

        //                 if (dependencies.Count == 0) {
        //                     istart = i;
        //                     break;
        //                 }
        //                 if (dependencies.Contains(path[i])) {
        //                     dependencies.Remove(path[i]);
        //                 }
        //             }

        //             if (istart == -1 && dependencies.Count == 0) {
        //                 istart = path.Count;
        //             }

        //             if (istart >= 0) {
        //                 for (var k = istart; k <= path.Count; k++) {
        //                     var pathT = path.Insert(k, key);
        //                     var remainingKeysT = remainingKeys.Remove(key);
        //                     var distT = Distance(pathT, maze);
        //                     // Console.WriteLine(distT);
        //                     var remT = EstimatedRemainingDistance(pathT, remainingKeysT, maze, allKeys);
        //                     q.Enqueue(distT + remT, (pathT, remainingKeysT));
        //                 }
        //                 break;
        //             }
        //         }

        //     }
        //     throw new Exception();

        // }


        Random r = new Random();
        IEnumerable<(string, int)> Paths(
            Maze maze,
            ImmutableHashSet<char> remainingKeys,
            Dictionary<char, ImmutableHashSet<char>> keysToDoors,
            ImmutableHashSet<char> allKeys,
            ImmutableList<char> path,
            int maxDist
        ) {

            var est = Distance(path, maze) + EstimatedRemainingDistance(path, remainingKeys, maze, allKeys);
            if (est > maxDist) {
                yield break;
            }

            if (remainingKeys.Count == 0) {
                yield return (string.Join("", path), Distance(path, maze));
                yield break;
            }

            var satisfiedKeys = new Dictionary<char, Dictionary<int, int>>();
            {
                var mpath = new List<char>(path);

                foreach (var key in remainingKeys) {
                    var dependencies = keysToDoors[key].ToHashSet();
                    if (remainingKeys.Intersect(dependencies).Count == 0) {

                        satisfiedKeys[key] = new Dictionary<int, int>();
                        for (var k = mpath.Count(); k >= 0; k--) {

                            if (k < mpath.Count() && keysToDoors[key].Contains(mpath[k])) {
                                break;
                            }

                            mpath.Insert(k, key);
                            satisfiedKeys[key][k] = Distance(mpath, maze);
                            mpath.RemoveAt(k);
                        }

                    };
                }
            }


            {
                var key = satisfiedKeys.Keys.OrderBy((keyT) => satisfiedKeys[keyT].Values.Min()).First();
                var distanceByIstart = satisfiedKeys[key];
                foreach (var k in distanceByIstart.Keys.OrderBy(istart => distanceByIstart[istart])) {
                    var pathT = path.Insert(k, key);
                    var remainingKeysT = remainingKeys.Remove(key);
                    foreach (var res in Paths(maze, remainingKeysT, keysToDoors, allKeys, pathT, maxDist)) {
                        maxDist = res.Item2 - 1;
                        yield return res;
                    }
                }
            }
        }


        IEnumerable<T[]> Permutations<T>(params T[] rgt) {
            void Swap(int i, int j) {
                (rgt[i], rgt[j]) = (rgt[j], rgt[i]);
            }

            IEnumerable<T[]> PermutationsRec(int i) {
                if (i == rgt.Length) {
                    yield return rgt.ToArray();
                }

                for (var j = i; j < rgt.Length; j++) {
                    Swap(i, j);
                    foreach (var perm in PermutationsRec(i + 1)) {
                        yield return perm;
                    }
                    Swap(i, j);
                }
            }

            return PermutationsRec(0);
        }



        int PartTwo(string input) {
            return 0;
        }


        Dictionary<char, ImmutableHashSet<char>> Explore((int irow, int icol) pos, Maze maze) {
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

    class PQueue<T> {
        SortedDictionary<int, Queue<T>> d = new SortedDictionary<int, Queue<T>>();
        public bool Any() {
            return d.Any();
        }

        public void Enqueue(int p, T t) {
            if (!d.ContainsKey(p)) {
                d[p] = new Queue<T>();
            }
            d[p].Enqueue(t);
        }

        public T Dequeue() {
            var p = d.Keys.First();
            var items = d[p];
            var t = items.Dequeue();
            if (!items.Any()) {
                d.Remove(p);
            }
            return t;
        }
    }
}