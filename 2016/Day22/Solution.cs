using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2016.Day22;

[ProblemName("Grid Computing")]
class Solution : Solver {

    public object PartOne(string input) {
        var nodes = Parse(input);
        var r = 0;
        foreach (var nodeA in nodes) {
            if (nodeA.used > 0) {
                foreach (var nodeB in nodes) {
                    if ((nodeA.irow != nodeB.irow || nodeA.icol != nodeB.icol) && nodeB.avail > nodeA.used) {
                        r++;
                    }
                }
            }
        }
        return r;
    }

    public object PartTwo(string input) {
        var nodes = Parse(input);
        var grid = new Grid(nodes);
        
        while(grid.irowEmpty != 0){
            if (!grid.Wall(grid.irowEmpty - 1, grid.icolEmpty)) {
                grid.Move(-1, 0);
            } else {
                grid.Move(0, -1);
            }
        }
        while (grid.icolEmpty != grid.ccol -1) {
            grid.Move(0, 1);
        }
        while(!nodes[0,0].goal) {
            grid.Move(1, 0);
            grid.Move(0, -1);
            grid.Move(0, -1);
            grid.Move(-1, 0);
            grid.Move(0, 1);
        }
        return grid.moves;
    }

    Node[,] Parse(string input) {
        var nodes = (
            from line in input.Split('\n').Skip(2)
            let parts = Regex.Matches(line, @"(\d+)").Select(m => int.Parse(m.Groups[1].Value)).ToArray()
            select new Node { irow = parts[1], icol = parts[0], size = parts[2], used = parts[3] }
        ).ToArray();

        var (crow, ccol) = (nodes.Select(x => x.irow).Max() + 1, nodes.Select(x => x.icol).Max() + 1);
        var res = new Node[crow, ccol];
        foreach (var file in nodes) {
            res[file.irow, file.icol] = file;
        }
        res[0, ccol - 1].goal = true;
        return res;
    }

    class Grid {
        public int irowEmpty;
        public int icolEmpty;
        public Node[,] nodes;
        public int moves;

        public Grid(Node[,] nodes){
            this.nodes = nodes;
            foreach(var node in nodes){
                if(node.used == 0){
                    irowEmpty = node.irow;
                    icolEmpty = node.icol;
                    break;
                }
            }
        }

        public int crow { get { return nodes.GetLength(0); } }
        public int ccol { get { return nodes.GetLength(1); } }

        public void Tsto() {
            var sb = new StringBuilder();
            sb.AppendLine();
            for (var irowT = 0; irowT < crow; irowT++) {
                for (var icolT = 0; icolT < ccol; icolT++) {
                    if (nodes[irowT, icolT].goal) {
                        sb.Append("G");
                    } else if (irowT == 0 && icolT == 0) {
                        sb.Append("x");
                    } else if (nodes[irowT, icolT].used == 0) {
                        sb.Append("E");
                    } else if (Wall(irowT, icolT)) {
                        sb.Append("#");
                    } else {
                        sb.Append(".");
                    }
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
        }

        public bool Wall(int irow, int icol) =>
            nodes[irow, icol].used > nodes[irowEmpty, icolEmpty].size;

        public void Move(int drow, int dcol) {
            if (Math.Abs(drow) + Math.Abs(dcol) != 1) throw new Exception();

            var irowT = irowEmpty + drow;
            var icolT = icolEmpty + dcol;

            if (irowT < 0 || irowT >= crow) throw new Exception();
            if (icolT < 0 || icolT >= ccol) throw new Exception();
            if (nodes[irowT, icolT].used > nodes[irowEmpty, icolEmpty].avail) throw new Exception();

            nodes[irowEmpty, icolEmpty].used = nodes[irowT, icolT].used;
            nodes[irowEmpty, icolEmpty].goal = nodes[irowT, icolT].goal;

            (irowEmpty, icolEmpty) = (irowT, icolT);
            nodes[irowEmpty, icolEmpty].used = 0;
            nodes[irowEmpty, icolEmpty].goal = false;

            moves++;
        }
    }

    class Node {
        public bool goal = false;
        public int irow; 
        public int icol; 
        public int size; 
        public int used;
        public int avail { get { return size - used; } }
    }
}
