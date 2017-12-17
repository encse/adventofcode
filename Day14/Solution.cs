using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode2017.Day14 {

    class Solution : Solver {

        public string GetName() => "Disk Defragmentation";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Extract(input).Select(row => row.Count(ch => ch == '#')).Sum();

        int PartTwo(string input) {
            var mtx = Extract(input).Select(row => row.ToCharArray()).ToArray();
            var regions = 0;
            for (int irow = 0; irow < mtx.Count(); irow++) {
                for (int icol = 0; icol < mtx[0].Count(); icol++) {
                    if (mtx[irow][icol] == '#') {
                        regions++;
                        Fill(mtx, (irow, icol));
                    }
                }
            }
            return regions;
        }

        void Fill(char[][] mtx, (int, int) startCell) {
            var q = new Queue<(int irow, int icol)>();
            var ccol = mtx[0].Count();
            var crow = mtx.Count();
            q.Enqueue(startCell);

            while (q.Any()) {
                var (irowCurrent, icolCurrent) = q.Dequeue();
                mtx[irowCurrent][icolCurrent] = ' ';

                var neighbourCells =
                    from drow in new[] { -1, 0, 1 }
                    from dcol in new[] { -1, 0, 1 }
                    where Math.Abs(drow) + Math.Abs(dcol) == 1

                    let icolNeighbour = icolCurrent + dcol
                    let irowNeighbour = irowCurrent + drow

                    where icolNeighbour >= 0 &&
                        icolNeighbour < ccol &&
                        irowNeighbour >= 0 &&
                        irowNeighbour < crow &&
                        mtx[irowNeighbour][icolNeighbour] == '#'

                    select (irowNeighbour, icolNeighbour);

                foreach (var neighbourCell in neighbourCells) {
                    q.Enqueue(neighbourCell);
                }
            }
        }

        IEnumerable<string> Extract(string input) {
            for (var irow = 0; irow < 128; irow++) {
                var row = "";
                foreach (var n in KnotHash(input + "-" + irow)) {
                    var m = n;
                    for (var bit = 0; bit < 8; bit++) {
                        if ((m & (1 << (7 - bit))) != 0) {
                            row += "#";
                        } else {
                            row += ".";
                        }
                    }
                }
                yield return row;
            }
        }

        int[] KnotHash(string input) {
            var suffix = new[] { 17, 31, 73, 47, 23 };
            var chars = input.ToCharArray().Select(b => (int)b).Concat(suffix);
            var output = Enumerable.Range(0, 256).ToArray();

            var current = 0;
            var skip = 0;
            for (var round = 0; round < 64; round++) {
                foreach (var len in chars) {
                    for (int i = 0; i < len / 2; i++) {
                        var from = (current + i) % output.Length;
                        var to = (current + len - 1 - i) % output.Length;
                        (output[from], output[to]) = (output[to], output[from]);
                    }
                    current += len + skip;
                    skip++;
                }
            }
            var hash = output;
            return (
                from blockIdx in Enumerable.Range(0, 16)
                let block = hash.Skip(16 * blockIdx).Take(16)
                select block.Aggregate(0, (acc, ch) => acc ^ ch)
            ).ToArray();
        }
    }
}
