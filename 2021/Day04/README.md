original source: [https://adventofcode.com/2021/day/4](https://adventofcode.com/2021/day/4)
## --- Day 4: Giant Squid ---
You're already almost 1.5km (almost a mile) below the surface of the ocean, already so deep that you can't see any sunlight. What you <em>can</em> see, however, is a giant squid that has attached itself to the outside of your submarine.

Maybe it wants to play [bingo](https://en.wikipedia.org/wiki/Bingo_(American_version))?

Bingo is played on a set of boards each consisting of a 5x5 grid of numbers. Numbers are chosen at random, and the chosen number is <em>marked</em> on all boards on which it appears. (Numbers may not appear on all boards.) If all numbers in any row or any column of a board are marked, that board <em>wins</em>. (Diagonals don't count.)

The submarine has a <em>bingo subsystem</em> to help passengers (currently, you and the giant squid) pass the time. It automatically generates a random order in which to draw numbers and a random set of boards (your puzzle input). For example:

<pre>
<code>7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7
</code>
</pre>

After the first five numbers are drawn (<code>7</code>, <code>4</code>, <code>9</code>, <code>5</code>, and <code>11</code>), there are no winners, but the boards are marked as follows (shown here adjacent to each other to save space):

<pre>
<code>22 13 17 <em>11</em>  0         3 15  0  2 22        14 21 17 24  <em>4</em>
 8  2 23  <em>4</em> 24         <em>9</em> 18 13 17  <em>5</em>        10 16 15  <em>9</em> 19
21  <em>9</em> 14 16  <em>7</em>        19  8  <em>7</em> 25 23        18  8 23 26 20
 6 10  3 18  <em>5</em>        20 <em>11</em> 10 24  <em>4</em>        22 <em>11</em> 13  6  <em>5</em>
 1 12 20 15 19        14 21 16 12  6         2  0 12  3  <em>7</em>
</code>
</pre>

After the next six numbers are drawn (<code>17</code>, <code>23</code>, <code>2</code>, <code>0</code>, <code>14</code>, and <code>21</code>), there are still no winners:

<pre>
<code>22 13 <em>17</em> <em>11</em>  <em>0</em>         3 15  <em>0</em>  <em>2</em> 22        <em>14</em> <em>21</em> <em>17</em> 24  <em>4</em>
 8  <em>2</em> <em>23</em>  <em>4</em> 24         <em>9</em> 18 13 <em>17</em>  <em>5</em>        10 16 15  <em>9</em> 19
<em>21</em>  <em>9</em> <em>14</em> 16  <em>7</em>        19  8  <em>7</em> 25 <em>23</em>        18  8 <em>23</em> 26 20
 6 10  3 18  <em>5</em>        20 <em>11</em> 10 24  <em>4</em>        22 <em>11</em> 13  6  <em>5</em>
 1 12 20 15 19        <em>14</em> <em>21</em> 16 12  6         <em>2</em>  <em>0</em> 12  3  <em>7</em>
</code>
</pre>

Finally, <code>24</code> is drawn:

<pre>
<code>22 13 <em>17</em> <em>11</em>  <em>0</em>         3 15  <em>0</em>  <em>2</em> 22        <em>14</em> <em>21</em> <em>17</em> <em>24</em>  <em>4</em>
 8  <em>2</em> <em>23</em>  <em>4</em> <em>24</em>         <em>9</em> 18 13 <em>17</em>  <em>5</em>        10 16 15  <em>9</em> 19
<em>21</em>  <em>9</em> <em>14</em> 16  <em>7</em>        19  8  <em>7</em> 25 <em>23</em>        18  8 <em>23</em> 26 20
 6 10  3 18  <em>5</em>        20 <em>11</em> 10 <em>24</em>  <em>4</em>        22 <em>11</em> 13  6  <em>5</em>
 1 12 20 15 19        <em>14</em> <em>21</em> 16 12  6         <em>2</em>  <em>0</em> 12  3  <em>7</em>
</code>
</pre>

At this point, the third board <em>wins</em> because it has at least one complete row or column of marked numbers (in this case, the entire top row is marked: <code><em>14 21 17 24  4</em></code>).

The <em>score</em> of the winning board can now be calculated. Start by finding the <em>sum of all unmarked numbers</em> on that board; in this case, the sum is <code>188</code>. Then, multiply that sum by <em>the number that was just called</em> when the board won, <code>24</code>, to get the final score, <code>188 * 24 = <em>4512</em></code>.

To guarantee victory against the giant squid, figure out which board will win first. <em>What will your final score be if you choose that board?</em>


## --- Part Two ---
On the other hand, it might be wise to try a different strategy: let the giant squid win.

You aren't sure how many bingo boards a giant squid could play at once, so rather than waste time counting its arms, the safe thing to do is to <em>figure out which board will win last</em> and choose that one. That way, no matter which boards it picks, it will win for sure.

In the above example, the second board is the last to win, which happens after <code>13</code> is eventually called and its middle column is completely marked. If you were to keep playing until this point, the second board would have a sum of unmarked numbers equal to <code>148</code> for a final score of <code>148 * 13 = <em>1924</em></code>.

Figure out which board will win last. <em>Once it wins, what would its final score be?</em>


