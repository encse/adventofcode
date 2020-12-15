using System.Linq;

namespace AdventOfCode.Y2020.Day15 {

    [ProblemName("Rambunctious Recitation")]
    class Solution : Solver {

        public object PartOne(string input) => NumberAt(input, 2020);
        public object PartTwo(string input) => NumberAt(input, 30000000);

        public int NumberAt(string input, int count) {
            var numbers = input.Split(",").Select(int.Parse).ToArray();
            var lastSeen = numbers.Select((num, i) => (num, i)).ToDictionary(kvp => kvp.num, kvp => kvp.i + 1);
            var number = numbers.Last();
            for (var round = lastSeen.Count; round < count; round++) {
                (lastSeen[number], number) = (round, lastSeen.ContainsKey(number) ? round - lastSeen[number] : 0);
            }
            return number;
        }
    }
}