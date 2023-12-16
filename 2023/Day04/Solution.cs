using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2023.Day04;
record Card(int matches);

[ProblemName("Scratchcards")]
class Solution : Solver {

    public object PartOne(string input) => (
         from line in input.Split("\n")
         let card = ParseCard(line)
         where card.matches > 0
         select Math.Pow(2, card.matches - 1)
    ).Sum();

    // Quite imperatively, just walk over the cards keeping track of the counts.
    public object PartTwo(string input) {
        var cards = input.Split("\n").Select(ParseCard).ToArray();
        var counts = cards.Select(_ => 1).ToArray();

        for (var i = 0; i < cards.Length; i++) {
            var (card, count) = (cards[i], counts[i]);
            for (var j = 0; j < card.matches; j++) {
                counts[i + j + 1] += count;
            }
        }
        return counts.Sum();
    }

    // Only the match count is relevant for a card
    Card ParseCard(string line) {
        var parts = line.Split(':', '|');
        var l = from m in Regex.Matches(parts[1], @"\d+") select m.Value;
        var r = from m in Regex.Matches(parts[2], @"\d+") select m.Value;
        return new Card(l.Intersect(r).Count());
    }
}
