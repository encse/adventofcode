namespace AdventOfCode.Y2024.Day25;

using System;
using System.Linq;

[ProblemName("Code Chronicle")]
class Solution : Solver {

    public object PartOne(string input) {
        int[] parsePattern(string[] lines) => 
            Enumerable.Range(0, lines[0].Length).Select(x => 
                Enumerable.Range(0, lines.Length).Count(y => lines[y][x] == '#')
            ).ToArray();
        
        bool match(int[] k, int[] l) => 
            Enumerable.Range(0, k.Length).All(i => k[i] + l[i] <= 7);

        var patterns = input.Split("\n\n").Select(b=>b.Split("\n"));
        var keys = patterns.Where(p => p[0][0] == '.').Select(parsePattern).ToList();
        var locks = patterns.Where(p => p[0][0] == '#').Select(parsePattern).ToList();

        return keys.Sum(k => locks.Count(l => match(l,k)));
    }
}