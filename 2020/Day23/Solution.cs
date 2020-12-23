using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day23 {

    class Cup {
        public int label;
        public Cup next;
        public Cup(int item, Cup next) {
            this.label = item;
            this.next = next;
        }
    };


    [ProblemName("Crab Cups")]
    class Solution : Solver {

        public object PartOne(string input) {


            var currentCup = Parse(input);
            var maxLabel = 9;
            for (var i = 0; i < 100; i++) {
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
                var cup = currentCup;
                while (cup.label != destinationCup) {
                    cup = cup.next;
                }
                removed.next.next.next = cup.next;
                cup.next = removed;
                currentCup = currentCup.next;
            }

            while (currentCup.label != 1) {
                currentCup = currentCup.next;
            }
            var res = "";
            for (var i = 0; i < 8; i++) {
                currentCup = currentCup.next;
                res += currentCup.label;
            }
            return res;
        }

        Cup Parse(string input) {
            var numbers = input.ToCharArray().Select(v => new Cup(int.Parse(v.ToString()), null)).ToArray();
            var prev = numbers.Last();
            for (var i = 0; i < numbers.Length; i++) {
                numbers[i].next = numbers[(i + 1) % numbers.Length];
            }
            return numbers[0];
        }

        public object PartTwo(string input) {
           
            // var currentCup = Parse(input);
            // var maxLabel = 1000000;
            // for (var i = 0; i < 100; i++) {
            //     var removed = currentCup.next;
            //     currentCup.next = currentCup.next.next.next.next;
            //     var destinationCup = currentCup.label;
            //     destinationCup = destinationCup == 1 ? maxLabel : destinationCup - 1;
            //     while (destinationCup == removed.label ||
            //        destinationCup == removed.next.label ||
            //        destinationCup == removed.next.next.label
            //     ) {
            //         destinationCup = destinationCup == 1 ? maxLabel : destinationCup - 1;
            //     }
            //     var cup = currentCup;
            //     while (cup.label != destinationCup) {
            //         cup = cup.next;
            //     }
            //     removed.next.next.next = cup.next;
            //     cup.next = removed;
            //     currentCup = currentCup.next;
            // }

            // while (currentCup.label != 1) {
            //     currentCup = currentCup.next;
            // }
            // var res = "";
            // for (var i = 0; i < 8; i++) {
            //     currentCup = currentCup.next;
            //     res += currentCup.label;
            // }
            // return res;
        }
    }
}