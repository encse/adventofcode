original source: [https://adventofcode.com/2022/day/2](https://adventofcode.com/2022/day/2)
## --- Day 2: Rock Paper Scissors ---
The Elves begin to set up camp on the beach. To decide whose tent gets to be closest to the snack storage, a giant [Rock Paper Scissors](https://en.wikipedia.org/wiki/Rock_paper_scissors) tournament is already in progress.

Rock Paper Scissors is a game between two players. Each game contains many rounds; in each round, the players each simultaneously choose one of Rock, Paper, or Scissors using a hand shape. Then, a winner for that round is selected: Rock defeats Scissors, Scissors defeats Paper, and Paper defeats Rock. If both players choose the same shape, the round instead ends in a draw.

Appreciative of your help yesterday, one Elf gives you an <em>encrypted strategy guide</em> (your puzzle input) that they say will be sure to help you win. "The first column is what your opponent is going to play: <code>A</code> for Rock, <code>B</code> for Paper, and <code>C</code> for Scissors. The second column--" Suddenly, the Elf is called away to help with someone's tent.

The second column, you reason, must be what you should play in response: <code>X</code> for Rock, <code>Y</code> for Paper, and <code>Z</code> for Scissors. Winning every time would be suspicious, so the responses must have been carefully chosen.

The winner of the whole tournament is the player with the highest score. Your <em>total score</em> is the sum of your scores for each round. The score for a single round is the score for the <em>shape you selected</em> (1 for Rock, 2 for Paper, and 3 for Scissors) plus the score for the <em>outcome of the round</em> (0 if you lost, 3 if the round was a draw, and 6 if you won).

Since you can't be sure if the Elf is trying to help you or trick you, you should calculate the score you would get if you were to follow the strategy guide.

For example, suppose you were given the following strategy guide:

<pre>
<code>A Y
B X
C Z
</code>
</pre>

This strategy guide predicts and recommends the following:


 - In the first round, your opponent will choose Rock (<code>A</code>), and you should choose Paper (<code>Y</code>). This ends in a win for you with a score of <em>8</em> (2 because you chose Paper + 6 because you won).
 - In the second round, your opponent will choose Paper (<code>B</code>), and you should choose Rock (<code>X</code>). This ends in a loss for you with a score of <em>1</em> (1 + 0).
 - The third round is a draw with both players choosing Scissors, giving you a score of 3 + 3 = <em>6</em>.

In this example, if you were to follow the strategy guide, you would get a total score of <code><em>15</em></code> (8 + 1 + 6).

<em>What would your total score be if everything goes exactly according to your strategy guide?</em>


## --- Part Two ---
The Elf finishes helping with the tent and sneaks back over to you. "Anyway, the second column says how the round needs to end: <code>X</code> means you need to lose, <code>Y</code> means you need to end the round in a draw, and <code>Z</code> means you need to win. Good luck!"

The total score is still calculated in the same way, but now you need to figure out what shape to choose so the round ends as indicated. The example above now goes like this:


 - In the first round, your opponent will choose Rock (<code>A</code>), and you need the round to end in a draw (<code>Y</code>), so you also choose Rock. This gives you a score of 1 + 3 = <em>4</em>.
 - In the second round, your opponent will choose Paper (<code>B</code>), and you choose Rock so you lose (<code>X</code>) with a score of 1 + 0 = <em>1</em>.
 - In the third round, you will defeat your opponent's Scissors with Rock for a score of 1 + 6 = <em>7</em>.

Now that you're correctly decrypting the ultra top secret strategy guide, you would get a total score of <code><em>12</em></code>.

Following the Elf's instructions for the second column, <em>what would your total score be if everything goes exactly according to your strategy guide?</em>


