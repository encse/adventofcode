using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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
            var numbers = Numbers(input);
            var min = numbers.Min();
            numbers = numbers.Where(n => n + min <= 2020);
            foreach (var subset in Choose(numbers, k)) {
                if (subset.Sum() == 2020) {
                    return subset.Aggregate(1, (acc, t) => acc * t);
                }
            }
            throw new Exception();
        }

        IEnumerable<int> Numbers(string input) {
            return input.Split('\n').Select(int.Parse);
        }

        IEnumerable<ImmutableList<T>> Choose<T>(IEnumerable<T> items, int k) {

            IEnumerable<ImmutableList<T>> ChooseRec(LinkedList<T> ll, int k) {
                if (k == 0) {
                    yield return ImmutableList<T>.Empty;
                } else {
                    for (var i = 0; i < ll.Count; i++) {
                        var item = ll.First.Value;
                        ll.RemoveFirst();
                        foreach (var res in ChooseRec(ll, k - 1)) {
                            yield return res.Add(item);
                        }
                        ll.AddLast(item);
                    }
                }
            }

            return ChooseRec(new LinkedList<T>(items), k);

        }
    }
}