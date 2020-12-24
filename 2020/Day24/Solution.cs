using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day24 {

    [ProblemName("Lobby Layout")]
    class Solution : Solver {

        public object PartOne(string input) {
            var tiles = Parse(input);
            return tiles.Values.Sum();
        }

        public object PartTwo(string input) {
            var tiles = Parse(input);
            for (var i = 0; i < 100; i++) {
                tiles = Flip(tiles);
            }
            return tiles.Values.Sum();
        }

        Dictionary<(int, int, int), int> Flip(Dictionary<(int, int, int), int> tiles) {
            var res =  new Dictionary<(int, int, int), int>();
            var neighbours = new []{(0,1,-1), (1,0,-1), (1,-1,0), (0,-1,1),(-1,0,1),(-1,1,0)};

            var tilesX = new HashSet<(int,int,int)>();
            foreach(var tile in tiles.Keys){
                tilesX.Add(tile);
                foreach(var neighbour in  from dir in neighbours  select (tile.Item1 + dir.Item1, tile.Item2 + dir.Item2, tile.Item3 + dir.Item3)){
                    tilesX.Add(neighbour);
                }
            }

            foreach(var tile in tilesX){
                var color = tiles.GetValueOrDefault(tile);
                var blackNeighbours = 0;
                foreach(var neighbour in  from dir in neighbours  select (tile.Item1 + dir.Item1, tile.Item2 + dir.Item2, tile.Item3 + dir.Item3)){
                   // Console.WriteLine(tile+" "+neighbour);
                    if(tiles.GetValueOrDefault(neighbour) == 1){
                        blackNeighbours++;
                    }
                }

                if(color == 1){
                    res[tile] = blackNeighbours == 0 || blackNeighbours > 2 ? 0 : 1;
                } else {
                    res[tile] = blackNeighbours == 2 ? 1 : 0;
                }
            }
            return res;
        }

        Dictionary<(int, int, int), int> Parse(string input) {
            Dictionary<(int, int, int), int> tiles = new Dictionary<(int, int, int), int>();
            foreach (var line in input.Split("\n")) {
                var c = Wander(line).Last();
                tiles[c] = (tiles.GetValueOrDefault(c) + 1) % 2;
            }

            return tiles;
        }
        IEnumerable<(int x, int y, int z)> Wander(string input) {
            var (x, y, z) = (0, 0, 0);
            while (input != "") {
                if (input.StartsWith("e")) { (x, y, z) = (x + 0, y + 1, z - 1); input = input.Substring(1); } 
                else if (input.StartsWith("se")) { (x, y, z) = (x + 1, y + 0, z - 1); input = input.Substring(2); } 
                else if (input.StartsWith("sw")) { (x, y, z) = (x + 1, y - 1, z + 0); input = input.Substring(2); } 
                else if (input.StartsWith("w")) { (x, y, z) = (x + 0, y - 1, z + 1); input = input.Substring(1); } 
                else if (input.StartsWith("nw")) { (x, y, z) = (x - 1, y + 0, z + 1); input = input.Substring(2); } 
                else if (input.StartsWith("ne")) { (x, y, z) = (x - 1, y + 1, z + 0); input = input.Substring(2); }
                yield return (x, y, z);
            }
        }
    }
}