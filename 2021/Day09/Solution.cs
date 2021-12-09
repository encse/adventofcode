using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day09;

[ProblemName("Smoke Basin")]
class Solution : Solver {

    public object PartOne(string input) {
        var lines = input.Split("\n");

        return GetLowPoints(lines).Select(pos => lines[pos.irow][pos.icol] - '0' + 1).Sum();
    }


    public object PartTwo(string input) {
        var lines = input.Split("\n");
        var m = 1;
        foreach(var x in GetLowPoints(lines).Select(p => BasinSize(lines, p)).OrderBy(x=>-x).Take(3)){
            m *= x;
        }
        return m;
    }

    public int BasinSize(string[] lines, Pos pos) {
        var ccol = lines[0].Length;
        var crow = lines.Length;
        var q = new Queue<Pos>();
        var seen = new HashSet<Pos>();
        seen.Add(pos);
        q.Enqueue(pos);

        var res = 0;
        while (q.Any()) {
            pos = q.Dequeue();

            res++;
            foreach (var (dcol, drow) in new[] { (1, 0), (-1, 0), (0, 1), (0, -1) }) {
                var (irowN, icolN) = (pos.irow + drow, pos.icol + dcol);
                if (irowN >= 0 && irowN < crow && icolN >= 0 && icolN < ccol && lines[irowN][icolN] != '9') {
                    var posN = new Pos(irowN, icolN);
                    if(!seen.Contains(posN)){
                        q.Enqueue(posN);
                        seen.Add(posN);
                    }
                }
            }

        }
        return res;
    }

    public IEnumerable<Pos> GetLowPoints(string[] lines) {
        var ccol = lines[0].Length;
        var crow = lines.Length;
        for (var irow = 0; irow < crow; irow++) {
            for (var icol = 0; icol < ccol; icol++) {
                var m = true;
                foreach (var (dcol, drow) in new[] { (1, 0), (-1, 0), (0, 1), (0, -1) }) {
                    var (irowN, icolN) = (irow + drow, icol + dcol);
                    if (irowN >= 0 && irowN < crow && icolN >= 0 && icolN < ccol) {
                        if (lines[irowN][icolN] <= lines[irow][icol]) {
                            m = false;
                            break;
                        }
                    }
                }
                if (m) {
                    yield return new Pos(irow, icol);
                }
            }
        }
    }
}

record Pos(int irow, int icol);
