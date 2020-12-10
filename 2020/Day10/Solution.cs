using System;
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
            var cache = new Dictionary<int, long>();
            long Rec(int prevI, int i) =>
                i >= jolts.Count            ? 0 :
                jolts[i] - jolts[prevI] > 3 ? 0 :
                i == jolts.Count - 1        ? 1 :
                /* otherwise */             cache.GetOrCompute(i, () => Rec(i, i + 1) + Rec(i, i + 2) + Rec(i, i + 3));

            return Rec(0, 0);
        }

        ImmutableList<int> Parse(string input) {
            var num = input.Split("\n").Select(int.Parse).OrderBy(x => x);
            return ImmutableList
                .Create(0)
                .AddRange(num)
                .Add(num.Last() + 3);
        }
    }

    static class Cache {
        public static B GetOrCompute<A, B>(this Dictionary<A, B> dict, A a, Func<B> compute) {
            if (!dict.ContainsKey(a)) {
                dict[a] = compute();
            }
            return dict[a];
        }
    }
}