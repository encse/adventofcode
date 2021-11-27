using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.Day20;

[ProblemName("Jurassic Jigsaw")]
class Solution : Solver {

    public object PartOne(string input) {
        var tiles = AssemblePuzzle(input);
        return 
            tiles.First().First().id *
            tiles.First().Last().id *
            tiles.Last().First().id *
            tiles.Last().Last().id;
    }

    public object PartTwo(string input) {
        var image = MergeTiles(AssemblePuzzle(input));

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

    private Tile[] ParseTiles(string input) {
        return (
            from block in input.Split("\n\n")
            let lines = block.Split("\n")
            let id = Regex.Match(lines[0], "\\d+").Value
            let image = lines.Skip(1).Where(x => x != "").ToArray()
            select new Tile(int.Parse(id), image)
        ).ToArray();
    }

    private Tile[][] AssemblePuzzle(string input) {
        var tiles = ParseTiles(input);

        // Collects tiles sharing a common edge. 
        // Due to the way the input is created, the list contains
        // - one item for tiles on the edge or 
        // - two for inner pieces.
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

        bool isEdge(string pattern) => pairs[pattern].Count == 1;
        Tile getNeighbour(Tile tile, string pattern) => pairs[pattern].SingleOrDefault(other => other != tile);

        Tile putTileInPlace(Tile above, Tile left) {
            if (above == null && left == null) {
                // find top-left corner
                foreach (var tile in tiles) {
                    for (var i = 0; i < 8; i++) {
                        if (isEdge(tile.Top()) && isEdge(tile.Left())) {
                            return tile;
                        }
                        tile.ChangeOrientation();
                    }
                }
            } else {
                // we know the tile from the inversion structure, just need to find its orientation
                var tile = above != null ? getNeighbour(above, above.Bottom()) : getNeighbour(left, left.Right());
                while (true) {
                    var topMatch = above == null ? isEdge(tile.Top())  : tile.Top() == above.Bottom();
                    var leftMatch = left == null ? isEdge(tile.Left()) : tile.Left() == left.Right();

                    if (topMatch && leftMatch) {
                        return tile;
                    }
                    tile.ChangeOrientation();
                }
            }

            throw new Exception();
        }

        // once the corner is fixed we can always find a unique tile that matches the one to the left & above
        // just fill up the tileset one by one
        var size = (int)Math.Sqrt(tiles.Length);
        var puzzle = new Tile[size][];
        for (var irow = 0; irow < size; irow++) {
            puzzle[irow] = new Tile[size];
            for (var icol = 0; icol < size; icol++) {
                var above = irow == 0 ? null : puzzle[irow - 1][icol];
                var left  = icol == 0 ? null : puzzle[irow][icol - 1];
                puzzle[irow][icol] = putTileInPlace(above, left);
            }
        }
        return puzzle;
    }

    private Tile MergeTiles(Tile[][] tiles) {
        // create a big tile leaving out the borders
        var image = new List<string>();
        var tileSize = tiles[0][0].size;
        var tileCount = tiles.Length;
        for (var irow = 0; irow < tileCount; irow++) {
            for (var i = 1; i < tileSize - 1; i++) {
                var st = "";
                for (var icol = 0; icol < tileCount; icol++) {
                    st += tiles[irow][icol].Row(i).Substring(1, tileSize - 2);
                }
                image.Add(st);
            }
        }
        return new Tile(42, image.ToArray());
    }

    int MatchCount(Tile image, params string[] pattern) {
        var res = 0;
        var (ccolP, crowP) = (pattern[0].Length, pattern.Length);
        for (var irow = 0; irow < image.size - crowP; irow++) 
        for (var icol = 0; icol < image.size - ccolP ; icol++) {
            bool match() {
                for (var icolP = 0; icolP < ccolP; icolP++)
                for (var irowP = 0; irowP < crowP; irowP++) {
                    if (pattern[irowP][icolP] == '#' && image[irow + irowP, icol + icolP] != '#') {
                        return false;
                    }
                }
                return true;
            }
            if(match()) {
                res++;
            }
        }
        return res;
    }
}

class Tile {
    public long id;
    public int size;
    string[] image;

    // This is a bit tricky, but makes operations fast and easy to implement.
    //
    // - orentation % 4 specifies the rotation of the tile
    // - orientation % 8 >= 4 means the tile is flipped.
    //
    // The actual rotation and flipping happens in the indexer, 
    // where the input coordinates are adjusted accordingly.
    //
    // Checking each 8 possible orientation for a tile requires just 7 incrementation of this value.
    int orentation = 0;

    public Tile(long id, string[] image) {
        this.id = id;
        this.image = image;
        this.size = image.Length;
    }

    public void ChangeOrientation() {
        this.orentation++;
    }

    public char this[int irow, int icol] {
        get {
            for (var i = 0; i < orentation % 4; i++) {
                (irow, icol) = (icol, size - 1 - irow); // rotate
            }

            if (orentation % 8 >= 4) {
                icol = size - 1 - icol; // flip vertical axis
            }

            return this.image[irow][icol];
        }
    }

    public string Row(int irow) => GetSlice(irow, 0, 0, 1);
    public string Col(int icol) => GetSlice(0, icol, 1, 0);
    public string Top() => Row(0);
    public string Bottom() => Row(size - 1);
    public string Left() => Col(0);
    public string Right() => Col(size - 1);

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
