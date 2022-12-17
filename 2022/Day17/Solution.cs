using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day17;

[ProblemName("Pyroclastic Flow")]
class Solution : Solver {

    public object PartOne(string input) {
        return new Tunnel(input).AddRocks(2022).height;
    }

    public object PartTwo(string input) {
        return new Tunnel(input).AddRocks(1000000000000).height;
    }

    class Tunnel {
        // preserve just the top of the whole cave this is a practical 
        // constant, there is NO THEORY BEHIND it.
        const int linesToStore = 100;
        List<string> lines;
        long linesNotStored;

        public long height => lines.Count + linesNotStored - 1;

        IEnumerator<string[]> rocks;
        IEnumerator<char> jets;

        public Tunnel(string jets) {
            var rocks = new[]{
                "####".Split("\n"),
                " # \n###\n # ".Split("\n"),
                "  #\n  #\n###".Split("\n"),
                "#\n#\n#\n#".Split("\n"),
                "##\n##".Split("\n")
            };

            this.rocks = Loop(rocks).GetEnumerator();
            this.jets = Loop(jets.Trim()).GetEnumerator();
            this.lines = new List<string>() { "+-------+" };
        }

        public Tunnel AddRocks(long rocks) {
            // We are adding rocks one by one until we find a recurring pattern.

            // Then we can jump forward full periods with just increasing the height 
            // of the cave: the top of the cave should look the same after a full period
            // so no need to simulate he rocks anymore. 

            // Then we just add the remaining rocks. 

            var seen = new Dictionary<string, (long rocks, long height)>();
            while (rocks > 0) {
                var hash = string.Join("\n", lines);
                if (seen.TryGetValue(hash, out var cache)) {
                    // we have seen this pattern.
                    // compute length of the period, and how much does it
                    // add to the height of the cave:
                    var heightOfPeriod = this.height - cache.height;
                    var periodLength = cache.rocks - rocks;

                    // advance forwad as much as possible
                    linesNotStored += (rocks / periodLength) * heightOfPeriod;
                    rocks = rocks % periodLength;
                    break;
                } else {
                    seen[hash] = (rocks, this.height);
                    this.AddRock();
                    rocks--;
                }
            }

            while (rocks > 0) {
                this.AddRock();
                rocks--;
            }
            return this;
        }

        public Tunnel AddRock() {
            // Adds one rock to the cave
            rocks.MoveNext();
            var rock = rocks.Current;

            // make room: 3 lines + the height of the rock
            for (var i = 0; i < rock.Length + 3; i++) {
                lines.Insert(0, "|       |");
            }

            // simulate falling
            var (rockX, rockY) = (3, 0);
            while (true) {
                jets.MoveNext();
                if (jets.Current == '>' && !Hit(rock, rockX + 1, rockY)) {
                    rockX++;
                } else if (jets.Current == '<' && !Hit(rock, rockX - 1, rockY)) {
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

        bool Hit(string[] rock, int x, int y) {
            // tells if a rock hits the walls of the cave or some other rock

            var (crow, ccol) = (rock.Length, rock[0].Length);
            for (var irow = 0; irow < crow; irow++) {
                for (var icol = 0; icol < ccol; icol++) {
                    if (rock[irow][icol] == '#' && lines[irow + y][icol + x] != ' ') {
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
                var r = lines.Count - linesToStore - 1;
                lines.RemoveRange(linesToStore, r);
                linesNotStored += r;
            }
        }

        IEnumerable<T> Loop<T>(IEnumerable<T> items) {
            while (true) {
                foreach (var item in items) {
                    yield return item;
                }
            }
        }

    }
}
