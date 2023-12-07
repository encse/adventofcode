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
        // The most frequent card ignoring J with a special case for "JJJJJ":
        var replacement = (
            from ch in hand
            where ch != 'J'
            group ch by ch into g
            orderby g.Count() descending
            select g.Key
        ).FirstOrDefault('J');

        var cv = CardValue(hand, "J123456789TQKA");
        var pv = PatternValue(hand.Replace('J', replacement));
        return (pv << 64) + cv;
    }

    // replace cards with with their indices in cardOrder. E.g. for 123456789TJQKA
    // A8A8A becomes (13)(7)(13)(7)(13), 9A34Q becomes (8)(13)(2)(3)(11)
    BigInteger CardValue(string hand, string cardOrder) =>
         new BigInteger(hand.Select(ch => (byte)cardOrder.IndexOf(ch)).Reverse().ToArray());

    // replace cards with the number of their occurrences in the hand then order them such as
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