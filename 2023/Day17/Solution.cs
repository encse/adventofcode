namespace AdventOfCode.Y2023.Day17;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, int>;

record Crucible(Complex pos, Complex dir, int straight);

[ProblemName("Clumsy Crucible")]
class Solution : Solver {

    public object PartOne(string input) => Heatloss(input, 0, 3);
    public object PartTwo(string input) => Heatloss(input, 4, 10);

    // Graph search using a priority queue. We can simply store the heatloss in 
    // the priority.
    int Heatloss(string input, int minStraight, int maxStraight) {
        var map = ParseMap(input);
        var goal = map.Keys.MaxBy(pos => pos.Imaginary + pos.Real);
        var q = new PriorityQueue<Crucible, int>();

        // initial direction: right or down
        q.Enqueue(new Crucible(pos: 0, dir: 1, straight: 0), 0);
        q.Enqueue(new Crucible(pos: 0, dir: Complex.ImaginaryOne, straight: 0), 0);

        var seen = new HashSet<Crucible>();
        while (q.TryDequeue(out var crucible, out var heatloss)) {
            if (crucible.pos == goal && crucible.straight >= minStraight) {
                return heatloss;
            }
            foreach (var next in Moves(crucible, minStraight, maxStraight)) {
                if (map.ContainsKey(next.pos) && !seen.Contains(next)) {
                    seen.Add(next);
                    q.Enqueue(next, heatloss + map[next.pos]);
                }
            }
        }
        throw new Exception();
    }

    // returns possible next states based on the rules
    IEnumerable<Crucible> Moves(Crucible c, int minStraight, int maxStraight) {
        if (c.straight < maxStraight) {
            yield return c with { 
                pos = c.pos + c.dir, 
                straight = c.straight + 1 
            };
        }

        if (c.straight >= minStraight) {
            var dir = c.dir * Complex.ImaginaryOne;
            yield return new Crucible(c.pos + dir, dir, 1);
            yield return new Crucible(c.pos - dir, -dir, 1);
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