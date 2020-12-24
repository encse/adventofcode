using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day24 {
    record Tile(int x, int y, int z);

    [ProblemName("Lobby Layout")]
    class Solution : Solver {

        public object PartOne(string input) => ParseBlackTiles(input).Count();

        public object PartTwo(string input) =>
            Enumerable.Range(0, 100)
                .Aggregate(ParseBlackTiles(input), (blackTiles, _) => Flip(blackTiles))
                .Count();

        Dictionary<string, (int x, int y, int z)> HexDirections = new Dictionary<string, (int x, int y, int z)>{
            {"x", (0, 0, 0)},
            {"e", (0, 1, -1)},
            {"se", (1, 0, -1)},
            {"sw", (1, -1, 0)},
            {"w", (0, -1, 1)},
            {"nw", (-1, 0, 1)},
            {"ne", (-1, 1, 0)}
        };

        IEnumerable<Tile> Neighbourhood(Tile point) =>
            from dir in HexDirections.Values
            select new Tile(point.x + dir.x, point.y + dir.y, point.z + dir.z);

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
            var (x, y, z) = (0, 0, 0);
            while (line != "") {
                foreach (var kvp in HexDirections) {
                    if (line.StartsWith(kvp.Key)) {
                        line = line.Substring(kvp.Key.Length);
                        (x, y, z) = (x + kvp.Value.x, y + kvp.Value.y, z + kvp.Value.z);
                    }
                }
            }
            return new Tile(x, y, z);
        }

    }
}
