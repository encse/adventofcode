namespace AdventOfCode.Y2024.Day20;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

[ProblemName("Race Condition")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, 2);
    public object PartTwo(string input) => Solve(input, 20);

    int Solve(string input, int cheat) {
        var path = GetPath(input);

        // this nested loop is 6x times faster then the same thing with LINQ  ¯\_(ツ)_/¯
        var res = 0;
        for (var i = 0; i < path.Length; i++) {
            for (var j = i + 1; j < path.Length; j++) {
                var dist = Manhattan(path[i], path[j]);

                // the index of an element in the path equals to its distance 
                // from the finish line

                var saving = j - (i + dist);
                if (dist <= cheat && saving >= 100) {
                    res++;
                }
            }
        }
        return res;
    }

    int Manhattan(Complex a, Complex b) =>
        (int)(Math.Abs(a.Imaginary - b.Imaginary) + Math.Abs(a.Real - b.Real));

    // follow the path from start to finish, supposed that there is a single track in the input
    Complex[] GetPath(string input) {
        var lines = input.Split("\n");
        var map = (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Complex.ImaginaryOne, lines[y][x])
        ).ToDictionary();

        var start = map.Keys.Single(k => map[k] == 'S');
        var goal = map.Keys.Single(k => map[k] == 'E');

        var (prev, cur, dir) = ((Complex?)null, start, Complex.ImaginaryOne);

        var res = new List<Complex> { start };
        while (cur != goal) {
            if (map[cur + dir] == '#' || cur + dir == prev) {
                dir *= Complex.ImaginaryOne;
            } else {
                (prev, cur) = (cur, cur + dir);
                res.Add(cur);
            }
        }
        return res.ToArray();
    }
}