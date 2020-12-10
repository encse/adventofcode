using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day10 {

    [ProblemName("Adapter Array")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var jolts = Parse(input);
            var window = jolts.Skip(1).Zip(jolts).Select(p => (current: p.First, prev: p.Second));

            return
                 window.Count(pair => pair.current - pair.prev == 1) *
                 window.Count(pair => pair.current - pair.prev == 3);
        }

        long PartTwo(string input) {
            var jolts = Parse(input);

            var result = new Dictionary<int, long>(){
                { jolts.Count - 1, 1 }
            };

            for (var i = jolts.Count - 2; i >= 0; i--) {
                long get(int di) => i + di < jolts.Count && jolts[i + di] - jolts[i] <= 3 ? result[i + di] : 0;
                result[i] = get(1) + get(2) + get(3);
            }
            return result[0];
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