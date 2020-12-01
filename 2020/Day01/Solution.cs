using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day01 {

    class Solution : Solver {

        public string GetName() => "Report Repair";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => Solve(input, 2);
        long PartTwo(string input) => Solve(input, 3);

        long Solve(string input, int k) {
            foreach (var pair in Choose(Numbers(input), k)) {
                if (pair.Sum() == 2020) {
                    return pair.Aggregate(1, (acc, t) => acc * t);
                }
            }
            throw new Exception();
        }

        List<int> Numbers(string input) {
            return input.Split('\n').Select(int.Parse).ToList();
        }

        IEnumerable<ImmutableList<T>> Choose<T>(List<T> items, int k) {

            if (k == 0) {
                yield return ImmutableList<T>.Empty;
            } else {
                for (var i = 0; i < items.Count; i++) {
                    var item = items[i];
                    items.RemoveAt(i);
                    foreach (var res in Choose(items, k - 1)) {
                        yield return res.Add(item);
                    }

                    items.Insert(i, item);
                }
            }
        }
    }
}