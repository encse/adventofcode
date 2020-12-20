using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day20 {

    class Tile {
        public string id;
        string[] image;
        int size;
        // int orientation = 0;
        // int flip = 0;

        int position = 0;

        public string[] edges;

        public Tile(string title, string[] image) {
            this.id = title;
            this.image = image;
            this.size = image.Length;

            if(image.Length == 11){
                Console.WriteLine("x");
            }
            this.edges = new [] {
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

        public void ChangePosition(){
            this.position ++;
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

        public string top() => edge(0, 0, 0, 1);
        public string bottom() => edge(size - 1, 0, 0, 1);
        public string left() => edge(0, 0, 1, 0);
        public string right() => edge(0, size - 1, 1, 0);
    }

    [ProblemName("Jurassic Jigsaw")]
    class Solution : Solver {

        public object PartOne(string input) {
            // var tiles = input.Split("\n\n");

            // var edgeMap = new Dictionary<string, string[]>();
            // foreach (var tile in tiles) {
            //     var image = tile.Split("\n").Skip(1).ToArray();

            //     string extractEdge(int irow, int icol, int drow, int dcol) {
            //         var st = "";
            //         for (var i = 0; i < 10; i++) {
            //             st += image[irow][icol];
            //             irow += drow;
            //             icol += dcol;
            //         }
            //         return st;
            //     }
            //    edgeMap[tile.Split("\n")[0]] = new [] {
            //         extractEdge(0,0,0,1),
            //         extractEdge(0,0,1,0),
            //         extractEdge(9,0,0,1),
            //         extractEdge(9,0,-1,0),
            //         extractEdge(0,9,0,-1),
            //         extractEdge(0,9,1,0),
            //         extractEdge(9,9,0,-1),
            //         extractEdge(9,9,-1,0),
            //     };

            // }


            // foreach(var tileA in edgeMap.Keys){
            //     var c = 0;
            //      foreach(var tileB in edgeMap.Keys){
            //          if(tileA == tileB){
            //              continue;
            //          }
            //          if(edgeMap[tileA].Any(edgeA => edgeMap[tileB].Contains(edgeA))){
            //              c++;
            //          }
            //      }
            //      if(c == 2) {
            //          Console.WriteLine(tileA);
            //      }
            // }
            // return 0;
            return 0;
        }

        private Tile[] Parse(string input) {
            return (
                from block in input.Split("\n\n")
                let lines = block.Split("\n")
                select new Tile(lines[0], lines.Skip(1).Where(x => x != "").ToArray())
            ).ToArray();
        }
        public object PartTwo(string input) {
            var tiles = Parse(input).ToList();

            Tile findCorner(){
                foreach (var tile in tiles) {
                    for(var i = 0;i<8;i++){
                        var topEdge = tiles.Any(tileB => tileB.id != tile.id && tileB.edges.Contains(tile.top()));
                        var leftEdge = tiles.Any(tileB => tileB.id != tile.id && tileB.edges.Contains(tile.left()));

                        if (topEdge && leftEdge) {
                            return tile;
                        }
                        tile.ChangePosition();
                    }
                }
                return null;
            }

            IEnumerable<Tile> findTile(string topPattern, string leftPattern){
                foreach (var tile in tiles) {
                   // var tile = tiles.Find(tile => tile.id.Contains("2857"));
                    
                    for(var i = 0;i<8;i++){
                        //Console.WriteLine(tile.top() + " " + tile.left());
                        var topMatch = topPattern != null ? tile.top() == topPattern : 
                            !tiles.Any(tileB => tileB.id != tile.id && tileB.edges.Contains(tile.top()));
                        var leftMatch = leftPattern != null ? tile.left() == leftPattern : 
                            !tiles.Any(tileB => tileB.id != tile.id && tileB.edges.Contains(tile.left()));

                        if(topMatch && leftMatch){
                            yield return tile;
                            break;
                        }
                        tile.ChangePosition();
                    }
                }
            }

            var mtx = new Tile[12,12];
            for (var irow=0;irow<12;irow++) {
                for(var icol=0;icol<12;icol++){
                    var topPattern = irow == 0 ? null : mtx[irow-1, icol].bottom();
                    var leftPattern = icol == 0 ? null : mtx[irow, icol-1].right();
                    
                    var q = findTile(topPattern, leftPattern).ToArray();
                    var tile = q[0];
                    if(tile == null){
                        throw new Exception();
                    }
                    mtx[irow, icol] = tile;
                    tiles.Remove(tile);
                }
            }
            Console.WriteLine(mtx);
            //edgeMap.Remove(corner.title);
            return 0;
        }
    }
}