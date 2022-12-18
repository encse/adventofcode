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

        List<string> lines = new List<string>();
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
                var hash = string.Join("\n", lines);
                if (seen.TryGetValue(hash, out var cache)) {
                    // we have seen this pattern.
                    // compute length of the period, and how much does it
                    // add to the height of the cave:
                    var heightOfPeriod = this.Height - cache.height;
                    var periodLength = cache.rocksToAdd - rocksToAdd;

                    // advance forwad as much as possible
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

        public Tunnel AddRock() {
            // Adds one rock to the cave
            var rock = rocks[(int)irock++];

            // make room of 3 lines + the height of the rock
            for (var i = 0; i < rock.Length + 3; i++) {
                lines.Insert(0, "|       |");
            }

            // simulate falling
            var (rockX, rockY) = (3, 0);
            while (true) {
                var jet = jets[(int)ijet++];
                if (jet == '>' && !Hit(rock, rockX + 1, rockY)) {
                    rockX++;
                } else if (jet == '<' && !Hit(rock, rockX - 1, rockY)) {
                    rockX--;
                }
                if (Hit(rock, rockX, rockY + 1)) {
                    break;
                }
                rockY++;
            }

            Draw(rock, rockX, rockY);
            return this;
        }

        bool Hit(string[] rock, int rockX, int rockY) {
            // tells if a rock can be placed in the given location or hits something
            if (rock.Length + rockY > lines.Count) {
                return true;
            }

            var (crow, ccol) = (rock.Length, rock[0].Length);
            for (var irow = 0; irow < crow; irow++) {
                for (var icol = 0; icol < ccol; icol++) {
                    if (rock[irow][icol] == '#' && lines[irow + rockY][icol + rockX] != ' ') {
                        return true;
                    }
                }
            }
            return false;
        }

        void Draw(string[] rock, int rockX, int rockY) {
            // draws a rock pattern into the cave at the given x,y coordinates,

            var (crow, ccol) = (rock.Length, rock[0].Length);
            for (var irow = 0; irow < crow; irow++) {
                var chars = lines[irow + rockY].ToArray();
                for (var icol = 0; icol < ccol; icol++) {

                    if (rock[irow][icol] == '#') {
                        if (chars[icol + rockX] != ' ') {
                            throw new Exception();
                        }
                        chars[icol + rockX] = '#';
                    }
                }
                lines[rockY + irow] = string.Join("", chars);
            }

            // remove empty lines from the top
            while (!lines[0].Contains('#')) {
                lines.RemoveAt(0);
            }

            // keep the tail
            if (lines.Count > linesToStore) {
                var r = lines.Count - linesToStore;
                lines.RemoveRange(linesToStore, r);
                linesNotStored += r;
            }
        }

        char Get(IList<IList<char>> c, int x, int y){
            return ' ';
        }
    }

    record struct ModCounter(int index, int mod) {
        public static explicit operator int(ModCounter c) => c.index;
        public static ModCounter operator ++(ModCounter c) => 
            c with {index = c.index == c.mod - 1 ? 0 : c.index + 1};
    }
}
