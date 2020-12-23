using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day23 {

    [ProblemName("Crab Cups")]
    class Solution : Solver {

        public object PartOne(string input) {

            var cups = new Cups(input, 9);
            for (var i = 0; i < 100; i++) {
                cups.Rotate();
            }

            return string.Join("", cups.Labels().Take(8));
        }

        public object PartTwo(string input) {

            var cups = new Cups(input, 1000000);
            for (var i = 0; i < 10000000; i++) {
                cups.Rotate();
            }

            var labels = cups.Labels().Take(2).ToArray();
            return labels[0] * labels[1];
        }
    }

    class Cups {
        int currentCup;
        public int[] next;

        public Cups(string input, int maxLabel) {
            next = Enumerable.Range(1, maxLabel + 1).ToArray();
            next[0] = -1;

            var digits = input.Select(d => int.Parse(d.ToString())).ToArray();
            for (var i = 0; i < digits.Length - 1; i++) {
                next[digits[i]] = digits[i + 1];
            }
            next[digits.Last()] = digits.First();

            if (maxLabel > input.Length) {
                (next[digits.Last()], next[maxLabel]) = (input.Length + 1, next[digits.Last()]);
            }

            currentCup = digits.First();
        }

        public IEnumerable<long> Labels() {
            var cup = next[1];
            while (true) {
                yield return cup;
                cup = next[cup];
            }
        }

        public void Rotate() {
            var removed = next[currentCup];
            next[currentCup] = next[next[next[removed]]];
            var destinationCup = currentCup == 1 ? next.Length - 1: currentCup - 1;

            while (destinationCup == removed ||
               destinationCup == next[removed] ||
               destinationCup == next[next[removed]]
            ) {
                destinationCup = destinationCup == 1 ? next.Length - 1 : destinationCup - 1;
            }

            (next[destinationCup], next[next[next[removed]]]) = (removed, next[destinationCup]);
            currentCup = next[currentCup];
        }
    }
}