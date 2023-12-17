using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Y2023.Day17;
using Map = Dictionary<Complex, int>;
record Crucible(Complex pos, Complex dir, int straight);

// Encode the difference between part 1 and 2 in a handy 'rules' object
record Rules(
    Func<Crucible, bool> canStop,
    Func<Crucible, bool> canGoForward,
    Func<Crucible, bool> canTurn
);

[ProblemName("Clumsy Crucible")]
class Solution : Solver {

    public object PartOne(string input) =>
        Heatloss(input,
            new Rules(
                canStop: crucible => true,
                canTurn: crucible => true,
                canGoForward: crucible => crucible.straight < 3
            ));

    public object PartTwo(string input) =>
        Heatloss(input,
            new Rules(
                canStop: crucible => crucible.straight >= 4,
                canTurn: crucible => crucible.straight == 0 || crucible.straight >= 4,
                canGoForward: crucible => crucible.straight < 10
            ));

    // Graph search using a priority queue. We can simply store the heatloss in 
    // the priority.
    int Heatloss(string input, Rules rules) {
        var map = ParseMap(input);
        var goal = map.Keys.MaxBy(pos => pos.Imaginary + pos.Real);

        var q = new PriorityQueue<Crucible, int>();
        q.Enqueue(new Crucible(pos: 0, dir: 1, straight: 0), 0);
        var seen = new HashSet<Crucible>();

        while (q.TryDequeue(out var crucible, out var heatloss)) {
            if (crucible.pos == goal && rules.canStop(crucible)) {
                return heatloss;
            }
            foreach (var next in Moves(crucible, rules)) {
                if (map.ContainsKey(next.pos) && !seen.Contains(next)) {
                    seen.Add(next);
                    q.Enqueue(next, heatloss + map[next.pos]);
                }
            }
        }
        throw new Exception();
    }

    // returns possible next states based on the rules
    public IEnumerable<Crucible> Moves(Crucible crucible, Rules rules) {
        if (rules.canGoForward(crucible)) {
            yield return crucible with { 
                pos = crucible.pos + crucible.dir, 
                straight = crucible.straight + 1 
            };
        }

        if (rules.canTurn(crucible)) {
            var dir = crucible.dir * Complex.ImaginaryOne;
            yield return new Crucible(crucible.pos + dir, dir, 1);
            yield return new Crucible(crucible.pos - dir, -dir, 1);
        }
    }

    // using a dictionary helps with bounds check (simply containskey):
    Map ParseMap(string input) {
        var lines = input.Split('\n');
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            let cell = int.Parse(lines[irow].Substring(icol, 1))
            let pos = new Complex(icol, irow)
            select new KeyValuePair<Complex, int>(pos, cell)
        ).ToDictionary();
    }
}