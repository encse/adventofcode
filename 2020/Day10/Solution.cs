using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day10 {

    [ProblemName("Adapter Array")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var num = input.Split("\n").Select(int.Parse).OrderBy(x => x).ToList();
            num.Insert(0, 0);
            num.Add(num[^1] + 3);
            var a = Enumerable.Range(1, num.Count - 1).Aggregate(0, (t, i) => t + (num[i] - num[i - 1] == 1 ? 1 : 0));
            var b = Enumerable.Range(1, num.Count - 1).Aggregate(0, (t, i) => t + (num[i] - num[i - 1] == 3 ? 1 : 0));
            return a * b;
        }

        long PartTwo(string input) {
            var num = input.Split("\n").Select(int.Parse).OrderBy(x => x).ToList();
            num.Insert(0, 0);
            num.Add(num[^1] + 3);

            var cache = new Dictionary<int, long>();
            long Rec(int prevI, int i) =>
                i >= num.Count          ? 0 :
                num[i] - num[prevI] > 3 ? 0 :
                i == num.Count - 1      ? 1 :
                /* otherwise */         cache.GetOrCompute(i, () => Rec(i, i + 1) + Rec(i, i + 2) + Rec(i, i + 3));

            return Rec(0,0);
        }
    }

    static class Cache {
        public static B GetOrCompute<A,B>(this Dictionary<A,B> dict, A a, Func<B> compute){
            if (!dict.ContainsKey(a)) {
                dict[a] = compute();
            }
            return dict[a];
        }
    }
}