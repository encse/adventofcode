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
        Cup currentCup;
        public Dictionary<long, Cup> cupsByLabel = new Dictionary<long, Cup>();
        int maxLabel;

        public Cups(string input, int maxLabel) {
            this.maxLabel = maxLabel;

            var numbers = 
                input.ToCharArray().Select(v => int.Parse(v.ToString()))
                    .Concat(Enumerable.Range(input.Length + 1, maxLabel - input.Length))
                    .Select(v => new Cup(v))
                    .ToArray();

            for (var i = 0; i < numbers.Length; i++) {
                numbers[i].next = numbers[(i + 1) % numbers.Length];
            }

            this.currentCup = numbers[0];
            var cup = numbers[0];
            for (var i = 0; i < maxLabel; i++) {
                cupsByLabel[cup.label] = cup;
                cup = cup.next;
            }
        }

        public IEnumerable<long> Labels() {
            var cup = cupsByLabel[1].next;
            while (true) {
                yield return cup.label;
                cup = cup.next;
            }
        }

        public void Rotate() {
            var removed = currentCup.next;
            currentCup.next = currentCup.next.next.next.next;
            var destinationCup = currentCup.label;
            destinationCup = destinationCup == 1 ? maxLabel : destinationCup - 1;
            while (destinationCup == removed.label ||
               destinationCup == removed.next.label ||
               destinationCup == removed.next.next.label
            ) {
                destinationCup = destinationCup == 1 ? maxLabel : destinationCup - 1;
            }
            var cup = cupsByLabel[destinationCup];
            removed.next.next.next = cup.next;
            cup.next = removed;
            currentCup = currentCup.next;
        }
    }

    class Cup {
        public long label;
        public Cup next;
        public Cup(int item) {
            this.label = item;
        }
    }
}