using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day21;

[ProblemName("Dirac Dice")]
class Solution : Solver {

    public object PartOne(string input) {
        var die = Die();

        var pos = Parse(input);
        var point = new int[] { 0, 0 };
        var roll = 0;
        while (true) {
            foreach (var i in new[] { 0, 1 }) {
                var s = 0;
                for (var k = 0; k < 3; k++) {
                    die.MoveNext();
                    roll++;
                    s += die.Current;
                }
                pos[i] = (pos[i] - 1 + s) % 10 + 1;
                point[i] += pos[i];
                if (point[i] >= 1000) {
                    return point[1 - i] * roll;
                }
            }
        }
    }

    public object PartTwo(string input) {

        var cache = new Dictionary<(Player, Player), (long, long)>();

        (long win1, long win2) winner(Player player1, Player player2) {
            if (player2.score >= 21) {
                return (1, 0);
            }

            var key = (player1, player2);
            if (!cache.ContainsKey(key)) {
                var res = (win1: 0L, win2: 0L);
                foreach (var steps in DiracThrows()) {
                    var w = winner(player2, player1.Move(steps));
                    res = (win1: res.win1 + w.win2, win2: res.win2 + w.win1);
                }
                cache[key] = res;
            }
            return cache[key];
        }

        var foo = Parse(input);
        var player1 = new Player(0, foo[0]);
        var player2 = new Player(0, foo[1]);

        var wins = winner(player1, player2);
        return Math.Max(wins.win1, wins.win2);
    }

    IEnumerator<int> Die() {
        var i = 1;
        while (true) {
            yield return i;
            i++;
            if (i > 100) {
                i = 1;
            }
        }
    }

    IEnumerable<int> DiracThrows() =>
        from i in new[] { 1, 2, 3 }
        from j in new[] { 1, 2, 3 }
        from k in new[] { 1, 2, 3 }
        select i + j + k;

    int[] Parse(string input) => (
        from line in input.Split("\n")
        let parts = line.Split(": ")
        select int.Parse(parts[1])
    ).ToArray();
}

record Player(int score, int pos) {
    public Player Move(int steps) {
        var newPos = (this.pos - 1 + steps) % 10 + 1;
        return new Player(this.score + newPos, newPos);
    }
}