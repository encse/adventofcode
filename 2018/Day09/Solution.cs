using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day09 {

    class Solution : Solver {

        public string GetName() => "Marble Mania";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) {
            var p = Parse(input);
            return Solver(p.players, p.points);
        }

        long PartTwo(string input) {
            var p = Parse(input);
            return Solver(p.players, 100 * p.points);
        }

        (int players, int points) Parse(string input) {
            var rx = new Regex(@"(?<players>\d+) players; last marble is worth (?<points>\d+) points");
            var m = rx.Match(input);
            return (int.Parse(m.Groups["players"].Value), int.Parse(m.Groups["points"].Value));
        }

        long Solver(int playerCount, int points) {
            var players = new long[playerCount];
            var current = new Node() { value = 0 };
            current.left = current;
            current.right = current;
            var nextPoints = 1;
            var iplayer = 0;
            while (nextPoints <= points) {
                iplayer = (iplayer + 1) % players.Length;

                if (nextPoints % 23 == 0) {
                    players[iplayer] += nextPoints;

                    for (var i = 0; i < 7; i++) {
                        current = current.left;
                    }

                    players[iplayer] += current.value;
                    var left = current.left;
                    var right = current.right;
                    right.left = left;
                    left.right = right;
                    current = right;
                    
                } else {
                    var left = current.right;
                    var right = current.right.right;
                    current = new Node { value = nextPoints, left = left, right = right };
                    left.right = current;
                    right.left = current;
                }
                nextPoints++;
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