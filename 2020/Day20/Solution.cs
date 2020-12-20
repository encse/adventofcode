using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day20 {

    [ProblemName("Jurassic Jigsaw")]
    class Solution : Solver {

        public object PartOne(string input) {
            var tiles = AssemblePuzzle(input);
            var size = tiles.GetLength(0);

            return (long)tiles[0, 0].id *
                tiles[size - 1, size - 1].id *
                tiles[0, size - 1].id *
                tiles[size - 1, 0].id;
        }

        public object PartTwo(string input) {
            var image = CombineTiles(-1, AssemblePuzzle(input));

            var monster = new string[]{
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "
            };

            while (true) {
                var monsterCount = MatchCount(image, monster);
                if (monsterCount > 0) {
                    var hashCountInImage = image.ToString().Count(ch => ch == '#');
                    var hashCountInMonster = string.Join("\n", monster).Count(ch => ch == '#');
                    return hashCountInImage - monsterCount * hashCountInMonster;
                }
                image.ChangeOrientation();
            }
        }

        private Tile[] Parse(string input) {
            return (
                from block in input.Split("\n\n")
                let lines = block.Split("\n")
                let id = lines[0].Trim(':').Split(" ")[1]
                let image = lines.Skip(1).Where(x => x != "").ToArray()
                select new Tile(int.Parse(id), image)
            ).ToArray();
        }

        private Tile[,] AssemblePuzzle(string input) {
            var tiles = Parse(input);

            // map the tiles sharing a common edge, it will be either one item for tiles on the edge or two for inner pieces
            var pairs = new Dictionary<string, List<Tile>>();
            foreach (var tile in tiles) {
                for (var i = 0; i < 8; i++) {
                    var pattern = tile.Top();
                    if (!pairs.ContainsKey(pattern)) {
                        pairs[pattern] = new List<Tile>();
                    }
                    pairs[pattern].Add(tile);
                    tile.ChangeOrientation();
                }
            }

            Tile getConnectingTile(Tile tile, string pattern) =>
                pairs[pattern].SingleOrDefault(tileB => tileB != tile);

            // once the corner is fixed we can always find a unique tile that matches the one to the left/above
            Tile putMatchingTileInPlace(Tile above, Tile left) {
                if (above == null && left == null) {
                    // find top-left corner
                    foreach (var tile in tiles) {
                        for (var i = 0; i < 8; i++) {
                            if (getConnectingTile(tile, tile.Top()) == null && getConnectingTile(tile, tile.Left()) == null) {
                                return tile;
                            }
                            tile.ChangeOrientation();
                        }
                    }
                } else {
                    // we know the tile from the inversion structure, just need to find its orientation
                    var tile = above != null ? getConnectingTile(above, above.Bottom()) : getConnectingTile(left, left.Right());
                    for (var i = 0; i < 8; i++) {
                        var topMatch = above == null ? getConnectingTile(tile, tile.Top()) == null : tile.Top() == above.Bottom();
                        var leftMatch = left == null ? getConnectingTile(tile, tile.Left()) == null : tile.Left() == left.Right();

                        if (topMatch && leftMatch) {
                            return tile;
                        }
                        tile.ChangeOrientation();
                    }
                }

                throw new Exception();
            }

            // just fill up the tileset one by one
            var size = (int)Math.Sqrt(tiles.Length);
            var tileset = new Tile[size, size];
            for (var irow = 0; irow < size; irow++)
                for (var icol = 0; icol < size; icol++) {
                    var tileAbove = irow == 0 ? null : tileset[irow - 1, icol];
                    var tileLeft = icol == 0 ? null : tileset[irow, icol - 1];
                    tileset[irow, icol] = putMatchingTileInPlace(tileAbove, tileLeft);
                }

            return tileset;
        }

        private Tile CombineTiles(int id, Tile[,] tiles) {
            // create a big tile leaving out the borders
            var image = new List<string>();
            var tileSize = tiles[0, 0].size;
            for (var irow = 0; irow < tiles.GetLength(0); irow++) {
                for (var i = 1; i < tileSize - 1; i++) {
                    var st = "";
                    for (var icol = 0; icol < tiles.GetLength(1); icol++) {
                        st += tiles[irow, icol].Row(i).Substring(1, tileSize - 2);
                    }
                    image.Add(st);
                }
            }
            return new Tile(id, image.ToArray());
        }

        int MatchCount(Tile image, params string[] pattern) {
            var res = 0;
            for (var irow = 0; irow < image.size; irow++) {
                for (var icol = 0; icol < image.size; icol++) {
                    if (Matches(image, pattern, irow, icol)) {
                        res++;
                    }
                }
            }
            return res;
        }

        bool Matches(Tile tile, string[] pattern, int irow, int icol) {
            var (ccolP, crowP) = (pattern[0].Length, pattern.Length);

            if (irow + crowP >= tile.size) {
                return false;
            }

            if (icol + ccolP >= tile.size) {
                return false;
            }

            for (var icolP = 0; icolP < ccolP; icolP++) {
                for (var irowP = 0; irowP < crowP; irowP++) {
                    if (pattern[irowP][icolP] == '#' && tile[irow + irowP, icol + icolP] != '#') {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    class Tile {
        public int id;
        public int size;

        string[] image;
        int orentation = 0;

        public Tile(int title, string[] image) {
            this.id = title;
            this.image = image;
            this.size = image.Length;
        }

        public void ChangeOrientation() {
            this.orentation++;
            this.orentation %= 8;
        }

        public char this[int irow, int icol] {
            get {
                for (var i = 0; i < orentation % 4; i++) {
                    (irow, icol) = (icol, size - 1 - irow);
                }

                if (orentation % 8 >= 4) {
                    icol = size - 1 - icol;
                }

                return this.image[irow][icol];
            }
        }

        public string Row(int irow) => GetSlice(irow, 0, 0, 1);
        public string Top() => GetSlice(0, 0, 0, 1);
        public string Right() => GetSlice(0, size - 1, 1, 0);
        public string Left() => GetSlice(0, 0, 1, 0);
        public string Bottom() => GetSlice(size - 1, 0, 0, 1);

        public override string ToString() {
            return $"Tile {id}:\n" + string.Join("\n", Enumerable.Range(0, size).Select(i => Row(i)));
        }

        string GetSlice(int irow, int icol, int drow, int dcol) {
            var st = "";
            for (var i = 0; i < size; i++) {
                st += this[irow, icol];
                irow += drow;
                icol += dcol;
            }
            return st;
        }
    }
}