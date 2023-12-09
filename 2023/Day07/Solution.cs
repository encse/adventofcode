using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day07;

[ProblemName("Camel Cards")]
class Solution : Solver {

    // Each 'hand' gets points based on the card's individual value and  
    // pattern value.

    public object PartOne(string input) => Solve(input, Part1Points);
    public object PartTwo(string input) => Solve(input, Part2Points);

    (long, long) Part1Points(string hand) =>
        (PatternValue(hand), CardValue(hand, "123456789TJQKA"));

    (long, long) Part2Points(string hand) {
        var cards = "J123456789TQKA";
        var patternValue = cards.Select(ch => PatternValue(hand.Replace('J', ch))).Max();
        return (patternValue, CardValue(hand, cards));
    }

    // map cards to their indices in cardOrder. E.g. for 123456789TJQKA
    // A8A8A becomes (13)(7)(13)(7)(13), 9A34Q becomes (8)(13)(2)(3)(11)
    long CardValue(string hand, string cardOrder) =>
        Pack(hand.Select(card => cardOrder.IndexOf(card)));

    // map cards to the number of their occurrences in the hand then order them such that
    // A8A8A becomes 33322, 9A34Q becomes 11111 and K99AA becomes 22221
    long PatternValue(string hand) =>
        Pack(hand.Select(card => hand.Count(x => x == card)).OrderDescending());

    long Pack(IEnumerable<int> numbers) => 
        numbers.Aggregate(1L, (a, v) => (a * 256) + v);

    int Solve(string input, Func<string, (long, long)> getPoints) {
        var bidsByRanking = (
            from line in input.Split("\n")
            let hand = line.Split(" ")[0]
            let bid = int.Parse(line.Split(" ")[1])
            orderby getPoints(hand)
            select bid
        );

        return bidsByRanking.Select((bid, rank) => (rank + 1) * bid).Sum();
    }
}
