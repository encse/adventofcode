using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2018.Day09 {

    class Solution : Solver {

        public string GetName() => "Marble Mania";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => Solve(input, 1);

        long PartTwo(string input) => Solve(input, 100);

        long Solve(string input, int mul) {

            var match = Regex.Match(input, @"(?<players>\d+) players; last marble is worth (?<points>\d+) points");
            var players = new long[int.Parse(match.Groups["players"].Value)];
            var targetPoints = int.Parse(match.Groups["points"].Value) * mul;

            var current = new Node { value = 0 };
            current.left = current;
            current.right = current;

            var points = 1;
            var iplayer = 1;
            while (points <= targetPoints) {

                if (points % 23 == 0) {
                    for (var i = 0; i < 7; i++) {
                        current = current.left;
                    }

                    players[iplayer] += points + current.value;

                    var left = current.left;
                    var right = current.right;
                    right.left = left;
                    left.right = right;
                    current = right;

                } else {
                    var left = current.right;
                    var right = current.right.right;
                    current = new Node { value = points, left = left, right = right };
                    left.right = current;
                    right.left = current;
                }

                points++;
                iplayer = (iplayer + 1) % players.Length;
            }

            return players.Max();
        }
    }

    class Node {
        public int value;
        public Node left;
        public Node right;
    }
}