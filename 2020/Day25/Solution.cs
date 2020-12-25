using System.Linq;

namespace AdventOfCode.Y2020.Day25 {

    [ProblemName("Combo Breaker")]
    class Solution : Solver {

        public object PartOne(string input) {
            var numbers = input.Split("\n").Select(int.Parse).ToArray();
            var mod = 20201227;
            var loop = 0;
            var subj = 7L;
            var num = subj;
            while (num != numbers[0] && num != numbers[1]) {
                num = (num * subj) % mod;
                loop++;
            }

            subj = num == numbers[0] ? numbers[1] : numbers[0];
            num = subj;
            while (loop > 0) {
                num = (num * subj) % mod;
                loop--;
            }
            return num;
        }

    }
}