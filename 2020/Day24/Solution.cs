using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day24;
record Tile(int x, int y);

[ProblemName("Lobby Layout")]
class Solution : Solver {

    public object PartOne(string input) => ParseBlackTiles(input).Count();

    public object PartTwo(string input) =>
        Enumerable.Range(0, 100)
            .Aggregate(ParseBlackTiles(input), (blackTiles, _) => Flip(blackTiles))
            .Count();

    Dictionary<string, (int x, int y)> HexDirections = new Dictionary<string, (int x, int y)> {
        {"o",  ( 0,  0)},
        {"ne", ( 1,  1)},
        {"nw", (-1,  1)},
        {"e",  ( 2,  0)},
        {"w",  (-2,  0)},
        {"se", ( 1, -1)},
        {"sw", (-1, -1)},
    };

    IEnumerable<Tile> Neighbourhood(Tile tile) =>
        from dir in HexDirections.Values select new Tile(tile.x + dir.x, tile.y + dir.y);

    HashSet<Tile> Flip(HashSet<Tile> blackTiles) {
        var tiles = (
            from black in blackTiles
            from tile in Neighbourhood(black)
            select tile
        ).ToHashSet();

        return (
            from tile in tiles
            let blacks = Neighbourhood(tile).Count(n => blackTiles.Contains(n))
            where blacks == 2 || blacks == 3 && blackTiles.Contains(tile)
            select tile
        ).ToHashSet();
    }

    HashSet<Tile> ParseBlackTiles(string input) {
        var tiles = new Dictionary<Tile, bool>();

        foreach (var line in input.Split("\n")) {
            var tile = Walk(line);
            tiles[tile] = !tiles.GetValueOrDefault(tile);
        }

        return (from kvp in tiles where kvp.Value select kvp.Key).ToHashSet();
    }

    Tile Walk(string line) {
        var (x, y) = (0, 0);
        while (line != "") {
            foreach (var kvp in HexDirections) {
                if (line.StartsWith(kvp.Key)) {
                    line = line.Substring(kvp.Key.Length);
                    (x, y) = (x + kvp.Value.x, y + kvp.Value.y);
                }
            }
        }
        return new Tile(x, y);
    }
}
