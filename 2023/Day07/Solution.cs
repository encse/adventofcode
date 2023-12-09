using System;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Y2023.Day07;

[ProblemName("Camel Cards")]
class Solution : Solver {

    // Each 'hand' gets points based on the card's individual value and their 
    // pattern value. These are combined into a single BigInteger for easy comparison.

    public object PartOne(string input) => Solve(input, Part1Points);
    public object PartTwo(string input) => Solve(input, Part2Points);

    BigInteger Part1Points(string hand) =>
        (PatternValue(hand) << 64) + CardValue(hand, "123456789TJQKA");

    BigInteger Part2Points(string hand) {
        var cards = "J123456789TQKA";
        var cv = CardValue(hand, cards);
        // try all combinations, no fancy stuff
        var pv = cards.Select(ch => PatternValue(hand.Replace('J', ch))).Max();
        return (pv << 64) + cv;
    }

    // map cards to their indices in cardOrder. E.g. for 123456789TJQKA
    // A8A8A becomes (13)(7)(13)(7)(13), 9A34Q becomes (8)(13)(2)(3)(11)
    BigInteger CardValue(string hand, string cardOrder) =>
         new BigInteger(hand.Select(ch => (byte)cardOrder.IndexOf(ch)).Reverse().ToArray());

    // map cards to the number of their occurrences in the hand then order them such that
    // A8A8A becomes 33322, 9A34Q becomes 11111 and K99AA becomes 22221
    BigInteger PatternValue(string hand) =>
        new BigInteger(hand.Select(ch => (byte)hand.Count(x => x == ch)).Order().ToArray());

    int Solve(string input, Func<string, BigInteger> getPoints) {
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
