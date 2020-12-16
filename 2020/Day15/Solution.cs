using System.Linq;

namespace AdventOfCode.Y2020.Day15 {

    [ProblemName("Rambunctious Recitation")]
    class Solution : Solver {

        public object PartOne(string input) => NumberAt(input, 2020);
        public object PartTwo(string input) => NumberAt(input, 30000000);

        public int NumberAt(string input, int count) {
            var numbers = input.Split(",").Select(int.Parse).ToArray();
            var (lastSeen, number) = (new int[count], numbers[0]);
            for (var round = 1; round <= count; round++) {
                (lastSeen[number], number) = (round, 
                    round <= numbers.Length ? numbers[round-1] : 
                    lastSeen[number] == 0 ? 0 : 
                    round - lastSeen[number]);
            }
            return number;
        }
    }
}