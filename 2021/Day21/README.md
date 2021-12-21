original source: [https://adventofcode.com/2021/day/21](https://adventofcode.com/2021/day/21)
## --- Day 21: Dirac Dice ---
There's not much to do as you slowly descend to the bottom of the ocean. The submarine computer challenges you to a nice game of <em>Dirac Dice</em>.

This game consists of a single [die](https://en.wikipedia.org/wiki/Dice), two [pawns](https://en.wikipedia.org/wiki/Glossary_of_board_games#piece), and a game board with a circular track containing ten spaces marked <code>1</code> through <code>10</code> clockwise. Each player's <em>starting space</em> is chosen randomly (your puzzle input). Player 1 goes first.

Players take turns moving. On each player's turn, the player rolls the die <em>three times</em> and adds up the results. Then, the player moves their pawn that many times <em>forward</em> around the track (that is, moving clockwise on spaces in order of increasing value, wrapping back around to <code>1</code> after <code>10</code>). So, if a player is on space <code>7</code> and they roll <code>2</code>, <code>2</code>, and <code>1</code>, they would move forward 5 times, to spaces <code>8</code>, <code>9</code>, <code>10</code>, <code>1</code>, and finally stopping on <code>2</code>.

After each player moves, they increase their <em>score</em> by the value of the space their pawn stopped on. Players' scores start at <code>0</code>. So, if the first player starts on space <code>7</code> and rolls a total of <code>5</code>, they would stop on space <code>2</code> and add <code>2</code> to their score (for a total score of <code>2</code>). The game immediately ends as a win for any player whose score reaches <em>at least <code>1000</code></em>.

Since the first game is a practice game, the submarine opens a compartment labeled <em>deterministic dice</em> and a 100-sided die falls out. This die always rolls <code>1</code> first, then <code>2</code>, then <code>3</code>, and so on up to <code>100</code>, after which it starts over at <code>1</code> again. Play using this die.

For example, given these starting positions:

<pre>
<code>Player 1 starting position: 4
Player 2 starting position: 8
</code>
</pre>

This is how the game would go:


 - Player 1 rolls <code>1</code>+<code>2</code>+<code>3</code> and moves to space <code>10</code> for a total score of <code>10</code>.
 - Player 2 rolls <code>4</code>+<code>5</code>+<code>6</code> and moves to space <code>3</code> for a total score of <code>3</code>.
 - Player 1 rolls <code>7</code>+<code>8</code>+<code>9</code> and moves to space <code>4</code> for a total score of <code>14</code>.
 - Player 2 rolls <code>10</code>+<code>11</code>+<code>12</code> and moves to space <code>6</code> for a total score of <code>9</code>.
 - Player 1 rolls <code>13</code>+<code>14</code>+<code>15</code> and moves to space <code>6</code> for a total score of <code>20</code>.
 - Player 2 rolls <code>16</code>+<code>17</code>+<code>18</code> and moves to space <code>7</code> for a total score of <code>16</code>.
 - Player 1 rolls <code>19</code>+<code>20</code>+<code>21</code> and moves to space <code>6</code> for a total score of <code>26</code>.
 - Player 2 rolls <code>22</code>+<code>23</code>+<code>24</code> and moves to space <code>6</code> for a total score of <code>22</code>.

...after many turns...


 - Player 2 rolls <code>82</code>+<code>83</code>+<code>84</code> and moves to space <code>6</code> for a total score of <code>742</code>.
 - Player 1 rolls <code>85</code>+<code>86</code>+<code>87</code> and moves to space <code>4</code> for a total score of <code>990</code>.
 - Player 2 rolls <code>88</code>+<code>89</code>+<code>90</code> and moves to space <code>3</code> for a total score of <code>745</code>.
 - Player 1 rolls <code>91</code>+<code>92</code>+<code>93</code> and moves to space <code>10</code> for a final score, <code>1000</code>.

Since player 1 has at least <code>1000</code> points, player 1 wins and the game ends. At this point, the losing player had <code>745</code> points and the die had been rolled a total of <code>993</code> times; <code>745 * 993 = <em>739785</em></code>.

Play a practice game using the deterministic 100-sided die. The moment either player wins, <em>what do you get if you multiply the score of the losing player by the number of times the die was rolled during the game?</em>


## --- Part Two ---
Now that you're warmed up, it's time to play the real game.

A second compartment opens, this time labeled <em>Dirac dice</em>. Out of it falls a single three-sided die.

As you experiment with the die, you feel a little strange. An informational brochure in the compartment explains that this is a <em>quantum die</em>: when you roll it, the universe <em>splits into multiple copies</em>, one copy for each possible outcome of the die. In this case, rolling the die always splits the universe into <em>three copies</em>: one where the outcome of the roll was <code>1</code>, one where it was <code>2</code>, and one where it was <code>3</code>.

The game is played the same as before, although to prevent things from getting too far out of hand, the game now ends when either player's score reaches at least <code><em>21</em></code>.

Using the same starting positions as in the example above, player 1 wins in <code><em>444356092776315</em></code> universes, while player 2 merely wins in <code>341960390180808</code> universes.

Using your given starting positions, determine every possible outcome. <em>Find the player that wins in more universes; in how many universes does that player win?</em>


