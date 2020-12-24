using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day24 {
    record Tile(int q, int r);

    [ProblemName("Lobby Layout")]
    class Solution : Solver {

        public object PartOne(string input) => ParseBlackTiles(input).Count();

        public object PartTwo(string input) =>
            Enumerable.Range(0, 100)
                .Aggregate(ParseBlackTiles(input), (blackTiles, _) => Flip(blackTiles))
                .Count();

        // https://www.redblobgames.com/grids/hexagons/#coordinates-axial
        Dictionary<string, (int q, int r)> HexDirections = new Dictionary<string, (int q, int r)>{
            {"o",  ( 0,  0)},
            {"e",  ( 1,  0)},
            {"se", ( 0,  1)},
            {"sw", (-1,  1)},
            {"w",  (-1,  0)},
            {"nw", ( 0, -1)},
            {"ne", ( 1, -1)}
        };

        IEnumerable<Tile> Neighbourhood(Tile point) =>
            from dir in HexDirections.Values select new Tile(point.q + dir.q, point.r + dir.r);

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
            var (q, r) = (0, 0);
            while (line != "") {
                foreach (var kvp in HexDirections) {
                    if (line.StartsWith(kvp.Key)) {
                        line = line.Substring(kvp.Key.Length);
                        (q, r) = (q + kvp.Value.q, r + kvp.Value.r);
                    }
                }
            }
            return new Tile(q, r);
        }
    }
}
