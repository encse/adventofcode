using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day01;

[ProblemName("Sonar Sweep")]
class Solution : Solver {

    public object PartOne(string input) => CountIncreases(Numbers(input));

    public object PartTwo(string input) => CountIncreases(Blocks(Numbers(input)));

    int[] Blocks(int[] ns) => (from r in Window(3, ns) select r.Sum()).ToArray();

    int CountIncreases(int[] ns) => Window(2, ns).Count(p => p[0] < p[1]);

    IEnumerable<int[]> Window(int w, int[] ns) => Enumerable.Range(0, ns.Length - w + 1).Select(k => ns[k..(k + w)]);

    int[] Numbers(string input) => (from n in input.Split('\n') select int.Parse(n)).ToArray();
}
