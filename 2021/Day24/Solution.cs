using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Y2021.Day24;

[ProblemName("Arithmetic Logic Unit")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input).max;
    public object PartTwo(string input) => Solve(input).min;

    (string min, string max) Solve(string input) {

        var max = Enumerable.Repeat(int.MinValue, 14).ToArray();
        var min = Enumerable.Repeat(int.MaxValue, 14).ToArray();
        var d = new int[14];
        var digits = Enumerable.Range(1, 9).ToArray();
        var stack = new Stack<int>();
        var blocks = input.Split("inp w\n").Skip(1).ToArray();

        for (var i = 0; i < 14; i++) {
            var block = blocks[i];
            var lines = block.Split('\n');

            if (block.Contains("div z 1")) {
                d[i] = int.Parse(lines[^4].Split(' ').Last());

                stack.Push(i);
            } else {
                d[i] = int.Parse(lines[4].Split(' ').Last());

                var pair = stack.Pop();
                foreach (var digit in digits) {
                    var digitPair = digit - d[pair] - d[i];
                    if (digits.Contains(digitPair)) {
                        if (digitPair > max[pair]) {
                            (max[pair], max[i]) = (digitPair, digit);
                        }
                        if (digitPair < min[pair]) {
                            (min[pair], min[i]) = (digitPair, digit);
                        }
                    }
                }
            }
        }

        return (string.Join("", min), string.Join("", max));
    }

}
