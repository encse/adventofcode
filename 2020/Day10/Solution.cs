using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day10 {

    [ProblemName("Adapter Array")]
    class Solution : Solver {

        public object PartOne(string input) {
            var jolts = Parse(input);
            var window = jolts.Skip(1).Zip(jolts).Select(p => (current: p.First, prev: p.Second));

            return
                 window.Count(pair => pair.current - pair.prev == 1) *
                 window.Count(pair => pair.current - pair.prev == 3);
        }

        public object PartTwo(string input) {
            var jolts = Parse(input);

            // dynamic programming with rolling variables a, b, c for the function values at i + 1, i + 2 and i + 3.
            var (a, b, c) = (1L, 0L, 0L); 
            for (var i = jolts.Count - 2; i >= 0; i--) {
                var s =  
                    (i + 1 < jolts.Count && jolts[i + 1] - jolts[i] <= 3 ? a : 0) +
                    (i + 2 < jolts.Count && jolts[i + 2] - jolts[i] <= 3 ? b : 0) +
                    (i + 3 < jolts.Count && jolts[i + 3] - jolts[i] <= 3 ? c : 0);
                (a, b, c) = (s, a, b);
            }
            return a;
        }

        ImmutableList<int> Parse(string input) {
            var num = input.Split("\n").Select(int.Parse).OrderBy(x => x);
            return ImmutableList
                .Create(0)
                .AddRange(num)
                .Add(num.Last() + 3);
        }
    }
}