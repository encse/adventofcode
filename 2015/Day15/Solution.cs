using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2015.Day15 {

    [ProblemName("Science for Hungry People")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => Solve(input, null);
        long PartTwo(string input) => Solve(input, 500);

        long Solve(string input, int? calories) {
            var ingredients = Parse(input);
            var propsCount = ingredients[0].Length;

            var maxValue = 0L;
            foreach (var amounts in Partition(100, ingredients.Length)) {
                var props = new int[propsCount];
                for (int ingredient = 0; ingredient < ingredients.Length; ingredient++) {
                    for (int prop = 0; prop < 5; prop++) {
                        props[prop] += ingredients[ingredient][prop] * amounts[ingredient];
                    }
                }
                if (!calories.HasValue || calories.Value == props.Last()) {
                    var value = props.Take(propsCount - 1).Aggregate(1L, (acc, p) => acc * Math.Max(0, p));
                    maxValue = Math.Max(maxValue, value);
                }
            }
            return maxValue;
        }

        int[][] Parse(string input) =>
            (from line in input.Split('\n')
             let m = Regex.Match(line, @".*: capacity (.*), durability (.*), flavor (.*), texture (.*), calories (.*)")
             let nums = m.Groups.Cast<Group>().Skip(1).Select(g => int.Parse(g.Value)).ToArray()
             select nums).ToArray();

        IEnumerable<int[]> Partition(int n, int k) {
            if (k == 1) {
                yield return new int[] { n };
            } else {
                for (var i = 0; i <= n; i++) {
                    foreach (var rest in Partition(n - i, k - 1)) {
                        yield return rest.Select(x => x).Append(i).ToArray();
                    }
                }
            }
        }
    }
}