namespace AdventOfCode.Y2024.Day18;

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Numerics;

[ProblemName("RAM Run")]
class Solution : Solver {

    public object PartOne(string input) =>
        Distance(GetBlocks(input), 1024);

    public object PartTwo(string input) {
        // find the first block position that will cut off the goal position
        // we can use a binary search for this

        var blocks = GetBlocks(input);
        var (lo, hi) = (0, blocks.Length - 1);
        while (hi - lo > 1) {
            var m = (lo + hi) / 2;
            if (Distance(blocks, m) == null) {
                hi = m;
            } else {
                lo = m;
            }
        }
        return $"{blocks[lo].Real},{blocks[lo].Imaginary}";
    }

    int? Distance(Complex[] blocks, int take) {
        // our standard priority queue based path finding
        
        var blocked = blocks.Take(take).ToHashSet();
        var size = 70;
        var goal = size + size * Complex.ImaginaryOne;
        var q = new PriorityQueue<Complex, int>();
        q.Enqueue(0, 0);
        var seen = new HashSet<Complex>(0);

        while (q.TryDequeue(out var pos, out var dist)) {
            if (pos == goal) {
                return dist;
            } else {
                foreach (var dir in new[] { 1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne }) {
                    var posT = pos + dir;
                    if (!seen.Contains(posT) &&
                        !blocked.Contains(posT) &&
                        0 <= posT.Imaginary && posT.Imaginary <= size &&
                        0 <= posT.Real && posT.Real <= size
                    ) {
                        q.Enqueue(posT, dist + 1);
                        seen.Add(posT);
                    }
                }
            }
        }
        return null;
    }

    Complex[] GetBlocks(string input) => (
        from line in input.Split("\n")
        let nums = Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value)).ToArray()
        select nums[0] + nums[1] * Complex.ImaginaryOne
    ).ToArray();
}