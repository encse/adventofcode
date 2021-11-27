using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Day06;

[ProblemName("Memory Reallocation")]
class Solution : Solver {

    public object PartOne(string input) => GetStepCount(Parse(input));

    public object PartTwo(string input) {
        var numbers = Parse(input);
        GetStepCount(numbers);
        return GetStepCount(numbers);
    }

    List<int> Parse(string input) => input.Split('\t').Select(int.Parse).ToList();

    int GetStepCount(List<int> numbers) {
        var stepCount = 0;
        var seen = new HashSet<string>();
        while (true) {
            var key = string.Join(";", numbers.Select(x => x.ToString()));
            if (seen.Contains(key)) {
                return stepCount;
            }
            seen.Add(key);
            Redistribute(numbers);
            stepCount++;
        }
    }

    void Redistribute(List<int> numbers) {
        var max = numbers.Max();
        var i = numbers.IndexOf(max);
        numbers[i] = 0;
        while (max > 0) {
            i++;
            numbers[i % numbers.Count]++;
            max--;
        }
    }
}
