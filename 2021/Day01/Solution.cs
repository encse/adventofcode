using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day01;

[ProblemName("Sonar Sweep")]
class Solution : Solver {

    public object PartOne(string input) => DepthIncreaseCount(Numbers(input));

    public object PartTwo(string input) => DepthIncreaseCount(ThreeMeasurements(Numbers(input)));

    int DepthIncreaseCount(int[] ns) => Window(2, ns).Count(p => p[0] < p[1]);

    // the sum of elements in a sliding window of 3
    int[] ThreeMeasurements(int[] ns) => (from r in Window(3, ns) select r.Sum()).ToArray();

    // parse input to array of numbers
    int[] Numbers(string input) => (from n in input.Split('\n') select int.Parse(n)).ToArray();

    // create a w wide sliding window from the elements of the array
    IEnumerable<int[]> Window(int w, int[] ns) => Enumerable.Range(0, ns.Length - w + 1).Select(k => ns[k..(k + w)]);
}
