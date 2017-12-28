using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Day06 {

    class Solution : Solver {

        public string GetName() => "Memory Reallocation"; 

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input.Split('\t').Select(int.Parse).ToList());
            yield return PartTwo(input.Split('\t').Select(int.Parse).ToList());
        }

        int PartOne(List<int> numbers) => GetStepCount(numbers);

        int PartTwo(List<int> numbers) {
            GetStepCount(numbers);
            return GetStepCount(numbers);
        }

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
}
