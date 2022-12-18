using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day17;

[ProblemName("Pyroclastic Flow")]
class Solution : Solver {

    public object PartOne(string input) {
        return new Tunnel(input, 100).AddRocks(2022).Height;
    }

    public object PartTwo(string input) {
        return new Tunnel(input, 100).AddRocks(1000000000000).Height;
    }

    class Tunnel {
        int linesToStore;

        List<char[]> lines = new List<char[]>();
        long linesNotStored;

        public long Height => lines.Count + linesNotStored;

        string[][] rocks;
        string jets;
        ModCounter irock;
        ModCounter ijet;

        // Simulation runs so that only the top N lines are kept in the tunnel. 
        // This is a practical constant, there is NO THEORY BEHIND it.
        public Tunnel(string jets, int linesToStore) {
            this.linesToStore = linesToStore;
            rocks = new string[][]{
                new []{"####"},
                new []{" # ", "###", " # "},
                new []{"  #", "  #", "###"},
                new []{"#", "#", "#", "#"},
                new []{"##", "##"}
            };
            this.irock = new ModCounter(0, rocks.Length);

            this.jets = jets;
            this.ijet = new ModCounter(0, jets.Length);
        }

        public Tunnel AddRocks(long rocksToAdd) {
            // We are adding rocks one by one until we find a recurring pattern.

            // Then we can jump forward full periods with just increasing the height 
            // of the cave: the top of the cave should look the same after a full period
            // so no need to simulate he rocks anymore. 

            // Then we just add the remaining rocks. 

            var seen = new Dictionary<string, (long rocksToAdd, long height)>();
            while (rocksToAdd > 0) {
                var hash = string.Join("", lines.SelectMany(ch => ch));
                if (seen.TryGetValue(hash, out var cache)) {
                    // we have seen this pattern, advance forwad as much as possible
                    var heightOfPeriod = this.Height - cache.height;
                    var periodLength = cache.rocksToAdd - rocksToAdd;
                    linesNotStored += (rocksToAdd / periodLength) * heightOfPeriod;
                    rocksToAdd = rocksToAdd % periodLength;
                    break;
                } else {
                    seen[hash] = (rocksToAdd, this.Height);
                    this.AddRock();
                    rocksToAdd--;
                }
            }

            while (rocksToAdd > 0) {
                this.AddRock();
                rocksToAdd--;
            }
            return this;
        }

        // Adds one rock to the cave
        public Tunnel AddRock() {
            var rock = rocks[(int)irock++];

            // make room of 3 lines + the height of the rock
            for (var i = 0; i < rock.Length + 3; i++) {
                lines.Insert(0, "|       |".ToArray());
            }

            // simulate falling
            var pos = new Pos(0, 3);
            while (true) {
                var jet = jets[(int)ijet++];
                if (jet == '>' && !Hit(rock, pos.Right)) {
                    pos = pos.Right;
                } else if (jet == '<' && !Hit(rock, pos.Left)) {
                    pos = pos.Left;
                }
                if (Hit(rock, pos.Below)) {
                    break;
                }
                pos = pos.Below;
            }

            Draw(rock, pos);
            return this;
        }

        // tells if a rock can be placed in the given location or hits something
        bool Hit(string[] rock, Pos pos) =>
            Area(rock).Any(pt =>
                Get(rock, pt) == '#' &&
                Get(lines, pt + pos) != ' '
            );

        void Draw(string[] rock, Pos pos) {
            // draws a rock pattern into the cave at the given x,y coordinates,
            foreach (var pt in Area(rock)) {
                if (Get(rock, pt) == '#') {
                    Set(lines, pt + pos, '#');
                }
            }
           
            // remove empty lines from the top
            while (!lines[0].Contains('#')) {
                lines.RemoveAt(0);
            }

            // keep the tail
            while (lines.Count > linesToStore) {
                lines.RemoveAt(lines.Count - 1);
                linesNotStored ++;
            }
        }
    }

    static IEnumerable<Pos> Area(string[] mat) =>
        from irow in Enumerable.Range(0, mat.Length)
        from icol in Enumerable.Range(0, mat[0].Length)
        select new Pos(irow, icol);

    static char Get(IEnumerable<IEnumerable<char>> mat, Pos pos) {
        return (mat.ElementAtOrDefault(pos.irow) ?? "#########").ElementAt(pos.icol);
    }

    static char Set(IList<char[]> mat, Pos pos, char ch) {
        return mat[pos.irow][pos.icol] = ch;
    }

    record struct Pos(int irow, int icol) {
        public Pos Left => new Pos(irow, icol - 1);
        public Pos Right => new Pos(irow, icol + 1);
        public Pos Below => new Pos(irow + 1, icol);
        public static Pos operator +(Pos posA, Pos posB) =>
            new Pos(posA.irow + posB.irow, posA.icol + posB.icol);
    }

    record struct ModCounter(int index, int mod) {
        public static explicit operator int(ModCounter c) => c.index;
        public static ModCounter operator ++(ModCounter c) =>
            c with { index = c.index == c.mod - 1 ? 0 : c.index + 1 };
    }
}
