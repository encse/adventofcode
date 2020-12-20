using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day20 {

    class Tile {
        public int id;
        string[] image;
        public int size;
        // int orientation = 0;
        // int flip = 0;

        int position = 0;

        public string[] edges;

        public Tile(int title, string[] image) {
            this.id = title;
            this.image = image;
            this.size = image.Length;

            if (image.Length == 11) {
                Console.WriteLine("x");
            }
            this.edges = new[] {
                edge(0,0,0,1),
                edge(0,0,1,0),
                edge(size-1,0,0,1),
                edge(size-1,0,-1,0),
                edge(0,size-1,0,-1),
                edge(0,size-1,1,0),
                edge(size-1,size-1,0,-1),
                edge(size-1,size-1,-1,0),
            };
        }

        public void ChangePosition() {
            this.position++;
            this.position %= 8;
        }

        // public void Rotate() {
        //     this.orientation++;
        //     this.orientation %= 4;
        // }

        // public void Flip() {
        //     this.flip++;
        //     this.orientation %= 2;
        // }

        public char this[int irow, int icol] {
            get {


                for (var i = 0; i < position % 4; i++) {
                    (irow, icol) = (icol, size - 1 - irow);
                }

                if (position % 8 >= 4) {
                    icol = size - 1 - icol;
                }

                return this.image[irow][icol];
            }
        }

        string edge(int irow, int icol, int drow, int dcol) {
            var st = "";
            for (var i = 0; i < size; i++) {
                st += this[irow, icol];
                irow += drow;
                icol += dcol;
            }
            return st;
        }

        public string row(int irow) => edge(irow, 0, 0, 1);
        public string top() => edge(0, 0, 0, 1);
        public string bottom() => edge(size - 1, 0, 0, 1);
        public string left() => edge(0, 0, 1, 0);
        public string right() => edge(0, size - 1, 1, 0);
    }

    [ProblemName("Jurassic Jigsaw")]
    class Solution : Solver {

        public object PartOne(string input) {
            var tiles = RestoreTiles(input);

            return (long)tiles[0,0].id * tiles[11,11].id * tiles[0,11].id *tiles[11,0].id;
        }

        private Tile[] Parse(string input) {
            return (
                from block in input.Split("\n\n")
                let lines = block.Split("\n")
                select new Tile(int.Parse(lines[0].Trim(':').Split(" ")[1]), lines.Skip(1).Where(x => x != "").ToArray())
            ).ToArray();
        }

        private Tile[,] RestoreTiles(string input) {
            var tiles = Parse(input).ToList();

            Tile findTile(string topPattern, string leftPattern) {
                foreach (var tile in tiles) {

                    for (var i = 0; i < 8; i++) {
                        var topMatch = topPattern != null ? tile.top() == topPattern :
                            !tiles.Any(tileB => tileB.id != tile.id && tileB.edges.Contains(tile.top()));
                        var leftMatch = leftPattern != null ? tile.left() == leftPattern :
                            !tiles.Any(tileB => tileB.id != tile.id && tileB.edges.Contains(tile.left()));

                        if (topMatch && leftMatch) {
                            return tile;
                        }
                        tile.ChangePosition();
                    }
                }
                throw new Exception();
            }

            var mtx = new Tile[12, 12];
            for (var irow = 0; irow < 12; irow++) {
                for (var icol = 0; icol < 12; icol++) {
                    var topPattern = irow == 0 ? null : mtx[irow - 1, icol].bottom();
                    var leftPattern = icol == 0 ? null : mtx[irow, icol - 1].right();

                    var tile = findTile(topPattern, leftPattern);
                    mtx[irow, icol] = tile;
                    tiles.Remove(tile);
                }
            }

            return mtx;
        }

        public object PartTwo(string input) {
            var mtx = RestoreTiles(input);
            var image = new List<string>();
            for (var irow = 0; irow < 12; irow++) {
                for (var i = 1; i < 9; i++) {
                    var st = "";
                    for (var icol = 0; icol < 12; icol++) {
                        st += mtx[irow, icol].row(i).Substring(1, 8);
                    }
                    image.Add(st);
                }
            }
            var bigTile = new Tile(-1, image.ToArray());

            var monster = new string[]{
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "
            };

            for (var i = 0; i < 9; i++) {
                int matches() {
                    var res = 0;
                    for (var irow = 0; irow < bigTile.size; irow++) {
                        for (var icol = 0; icol < bigTile.size; icol++) {
                            bool match() {
                                var ccolM = monster[0].Length;
                                var crowM = monster.Length;
                                if (icol + ccolM >= bigTile.size) {
                                    return false;
                                }
                                if (irow + crowM >= bigTile.size) {
                                    return false;
                                }

                                for (var icolM = 0; icolM < ccolM; icolM++) {
                                    for (var irowM = 0; irowM < crowM; irowM++) {
                                        if (monster[irowM][icolM] == '#' && bigTile[irow + irowM, icol + icolM] != '#') {
                                            return false;
                                        }
                                    }
                                }
                                return true;
                            }

                            if (match()) {
                                res++;
                            }
                        }
                    }
                    return res;
                }

                var cmatch = matches();
                if (cmatch > 0) {
                    var hashCount = 0;
                    for (var irow = 0; irow < bigTile.size; irow++) {
                        for (var icol = 0; icol < bigTile.size; icol++) {
                            if (bigTile[irow, icol] == '#')
                                hashCount++;
                        }
                    }

                    return hashCount - cmatch * 15;
                }

                bigTile.ChangePosition();
            }

            throw new Exception();
        }
    }
}