using System.Linq;

namespace AdventOfCode.Y2020.Day25 {

    [ProblemName("Combo Breaker")]
    class Solution : Solver {

        public object PartOne(string input) {
            // https://en.wikipedia.org/wiki/Diffie%E2%80%93Hellman_key_exchange
            var numbers = input.Split("\n").Select(int.Parse).ToArray();
            var mod = 20201227;
            var pow = 0;
            var subj = 7L;
            var num = subj;
            while (num != numbers[0] && num != numbers[1]) {
                num = (num * subj) % mod;
                pow++;
            }

            subj = num == numbers[0] ? numbers[1] : numbers[0];
            num = subj;
            while (pow > 0) {
                num = (num * subj) % mod;
                pow--;
            }
            return num;
        }

    }
}