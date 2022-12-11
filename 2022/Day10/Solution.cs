using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day10;

[ProblemName("Cathode-Ray Tube")]
class Solution : Solver {

    public object PartOne(string input) {
        var sample = new[] { 20, 60, 100, 140, 180, 220 };
        return Signal(input)
            .Where(signal => sample.Contains(signal.cycle))
            .Select(signal => signal.x * signal.cycle)
            .Sum();
    }

    public object PartTwo(string input) => 
        Signal(input)
            .Select(signal => {
                var spriteMiddle = signal.x;
                var screenColumn = (signal.cycle - 1) % 40;
                return Math.Abs(spriteMiddle - screenColumn) < 2 ? '#' : ' ';
            })
            .Chunk(40)
            .Select(line => new string(line))
            .Aggregate("", (screen, line) => screen + line + "\n")
            .Ocr();

    IEnumerable<(int cycle, int x)> Signal(string input) {
        var (cycle, x) = (1, 1);
        foreach (var line in input.Split("\n")) {
            var parts = line.Split(" ");
            switch (parts[0]) {
                case "noop":
                    yield return (cycle++, x);
                    break;
                case "addx":
                    yield return (cycle++, x);
                    yield return (cycle++, x);
                    x += int.Parse(parts[1]);
                    break;
                default:
                    throw new ArgumentException(parts[0]);
            }
        }
    }
}