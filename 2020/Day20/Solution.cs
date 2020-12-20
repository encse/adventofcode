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

            for (var i = 0; i < 9; i++) {
                var monsterCount = MatchCount(image, monster);
                if (monsterCount > 0) {
                    var hashCountInImage = image.ToString().Count(ch => ch == '#');
                    var hashCountInMonster = string.Join("\n", monster).Count(ch => ch == '#');
                    return hashCountInImage - monsterCount * hashCountInMonster;
                }
                image.ChangeOrientation();
            }

            throw new Exception();
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
            var tiles = Parse(input).ToList();

            var edges = new Dictionary<Tile, string[]>(
                from tile in tiles select
                    new KeyValuePair<Tile, string[]>(
                        tile,
                        new string[] {
                            tile.Top(),
                            tile.Right(),
                            tile.Bottom(),
                            tile.Left(),
                            string.Join("", tile.Top().Reverse()),
                            string.Join("", tile.Right().Reverse()),
                            string.Join("", tile.Bottom().Reverse()),
                            string.Join("", tile.Left().Reverse()),
                        }
                    ));

            Tile findMatchingTile(string topPattern, string leftPattern) {
                foreach (var tile in tiles) {
                    for (var i = 0; i < 8; i++) {
                        var topMatch = topPattern != null ? tile.Top() == topPattern :
                            !tiles.Any(tileB => tileB.id != tile.id && edges[tileB].Contains(tile.Top()));
                        var leftMatch = leftPattern != null ? tile.Left() == leftPattern :
                            !tiles.Any(tileB => tileB.id != tile.id && edges[tileB].Contains(tile.Left()));

                        if (topMatch && leftMatch) {
                            return tile;
                        }
                        tile.ChangeOrientation();
                    }
                }
                throw new Exception();
            }

            var tilesetSize = (int)Math.Sqrt(tiles.Count);
            var tileset = new Tile[tilesetSize, tilesetSize];
            for (var irow = 0; irow < tilesetSize; irow++)
                for (var icol = 0; icol < tilesetSize; icol++) {
                    var topPattern = irow == 0 ? null : tileset[irow - 1, icol].Bottom();
                    var leftPattern = icol == 0 ? null : tileset[irow, icol - 1].Right();

                    var tile = findMatchingTile(topPattern, leftPattern);
                    tiles.Remove(tile);
                    tileset[irow, icol] = tile;
                }

            return tileset;
        }

        private Tile CombineTiles(int id, Tile[,] tiles) {
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