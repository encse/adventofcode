using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day22;

[ProblemName("Crab Combat")]
class Solution : Solver {

    public object PartOne(string input) {
        var (deck1, deck2) = Parse(input);
        while (deck1.Any() && deck2.Any()) {

            var (card1, card2) = (deck1.Dequeue(), deck2.Dequeue());

            bool player1Wins = card1 > card2;

            if (player1Wins) {
                deck1.Enqueue(card1);
                deck1.Enqueue(card2);
            } else {
                deck2.Enqueue(card2);
                deck2.Enqueue(card1);
            }
        }
        return Answer(deck1, deck2);
    }

    public object PartTwo(string input) {

        var (deck1, deck2) = Parse(input);

        bool Game(Queue<int> deck1, Queue<int> deck2) {
            var seen = new HashSet<string>();

            while (deck1.Any() && deck2.Any()) {
                var hash = string.Join(",", deck1) + ";" + string.Join(",", deck2);
                if (seen.Contains(hash)) {
                    return true; // player 1 wins;
                }
                seen.Add(hash);

                var (card1, card2) = (deck1.Dequeue(), deck2.Dequeue());

                bool player1Wins;
                if (deck1.Count >= card1 && deck2.Count >= card2) {
                    player1Wins = Game(new Queue<int>(deck1.Take(card1)), new Queue<int>(deck2.Take(card2)));
                } else {
                    player1Wins = card1 > card2;
                }

                if (player1Wins) {
                    deck1.Enqueue(card1);
                    deck1.Enqueue(card2);
                } else {
                    deck2.Enqueue(card2);
                    deck2.Enqueue(card1);
                }
            }
            return deck1.Any(); // player1 wins?
        }

        Game(deck1, deck2);

        return Answer(deck1, deck2);
    }

    int Answer(Queue<int> deck1, Queue<int> deck2) => 
        deck1.Concat(deck2).Reverse().Select((c, i) => c * (i + 1)).Sum();

    (Queue<int> deck1, Queue<int> deck2) Parse(string input) {

        var decks = input.Split("\n\n");
        return (
            new Queue<int>(decks[0].Split("\n").Skip(1).Select(int.Parse)),
            new Queue<int>(decks[1].Split("\n").Skip(1).Select(int.Parse))
        );
    }
}
