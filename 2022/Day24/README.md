original source: [https://adventofcode.com/2022/day/24](https://adventofcode.com/2022/day/24)
## --- Day 24: Blizzard Basin ---
With everything replanted for next year (and with elephants and monkeys to tend the grove), you and the Elves leave for the extraction point.

Partway up the mountain that shields the grove is a flat, open area that serves as the extraction point. It's a bit of a climb, but nothing the expedition can't handle.

At least, that would normally be true; now that the mountain is covered in snow, things have become more difficult than the Elves are used to.

As the expedition reaches a valley that must be traversed to reach the extraction site, you find that strong, turbulent winds are pushing small <em>blizzards</em> of snow and sharp ice around the valley. It's a good thing everyone packed warm clothes! To make it across safely, you'll need to find a way to avoid them.

Fortunately, it's easy to see all of this from the entrance to the valley, so you make a map of the valley and the blizzards (your puzzle input). For example:

<pre>
<code>#.#####
#.....#
#>....#
#.....#
#...v.#
#.....#
#####.#
</code>
</pre>

The walls of the valley are drawn as <code>#</code>; everything else is ground. Clear ground - where there is currently no blizzard - is drawn as <code>.</code>. Otherwise, blizzards are drawn with an arrow indicating their direction of motion: up (<code>^</code>), down (<code>v</code>), left (<code><</code>), or right (<code>></code>).

The above map includes two blizzards, one moving right (<code>></code>) and one moving down (<code>v</code>). In one minute, each blizzard moves one position in the direction it is pointing:

<pre>
<code>#.#####
#.....#
#.>...#
#.....#
#.....#
#...v.#
#####.#
</code>
</pre>

Due to conservation of blizzard energy, as a blizzard reaches the wall of the valley, a new blizzard forms on the opposite side of the valley moving in the same direction. After another minute, the bottom downward-moving blizzard has been replaced with a new downward-moving blizzard at the top of the valley instead:

<pre>
<code>#.#####
#...v.#
#..>..#
#.....#
#.....#
#.....#
#####.#
</code>
</pre>

Because blizzards are made of tiny snowflakes, they pass right through each other. After another minute, both blizzards temporarily occupy the same position, marked <code>2</code>:

<pre>
<code>#.#####
#.....#
#...2.#
#.....#
#.....#
#.....#
#####.#
</code>
</pre>

After another minute, the situation resolves itself, giving each blizzard back its personal space:

<pre>
<code>#.#####
#.....#
#....>#
#...v.#
#.....#
#.....#
#####.#
</code>
</pre>

Finally, after yet another minute, the rightward-facing blizzard on the right is replaced with a new one on the left facing the same direction:

<pre>
<code>#.#####
#.....#
#>....#
#.....#
#...v.#
#.....#
#####.#
</code>
</pre>

This process repeats at least as long as you are observing it, but probably forever.

Here is a more complex example:

<pre>
<code>#.######
#>>.<^<#
#.<..<<#
#>v.><>#
#<^v^^>#
######.#
</code>
</pre>

Your expedition begins in the only non-wall position in the top row and needs to reach the only non-wall position in the bottom row. On each minute, you can <em>move</em> up, down, left, or right, or you can <em>wait</em> in place. You and the blizzards act <em>simultaneously</em>, and you cannot share a position with a blizzard.

In the above example, the fastest way to reach your goal requires <code><em>18</em></code> steps. Drawing the position of the expedition as <code>E</code>, one way to achieve this is:

<pre>
<code>Initial state:
#<em>E</em>######
#>>.<^<#
#.<..<<#
#>v.><>#
#<^v^^>#
######.#

Minute 1, move down:
#.######
#<em>E</em>>3.<.#
#<..<<.#
#>2.22.#
#>v..^<#
######.#

Minute 2, move down:
#.######
#.2>2..#
#<em>E</em>^22^<#
#.>2.^>#
#.>..<.#
######.#

Minute 3, wait:
#.######
#<^<22.#
#<em>E</em>2<.2.#
#><2>..#
#..><..#
######.#

Minute 4, move up:
#.######
#<em>E</em><..22#
#<<.<..#
#<2.>>.#
#.^22^.#
######.#

Minute 5, move right:
#.######
#2<em>E</em>v.<>#
#<.<..<#
#.^>^22#
#.2..2.#
######.#

Minute 6, move right:
#.######
#>2<em>E</em><.<#
#.2v^2<#
#>..>2>#
#<....>#
######.#

Minute 7, move down:
#.######
#.22^2.#
#<v<em>E</em><2.#
#>>v<>.#
#>....<#
######.#

Minute 8, move left:
#.######
#.<>2^.#
#.<em>E</em><<.<#
#.22..>#
#.2v^2.#
######.#

Minute 9, move up:
#.######
#<<em>E</em>2>>.#
#.<<.<.#
#>2>2^.#
#.v><^.#
######.#

Minute 10, move right:
#.######
#.2<em>E</em>.>2#
#<2v2^.#
#<>.>2.#
#..<>..#
######.#

Minute 11, wait:
#.######
#2^<em>E</em>^2>#
#<v<.^<#
#..2.>2#
#.<..>.#
######.#

Minute 12, move down:
#.######
#>>.<^<#
#.<<em>E</em>.<<#
#>v.><>#
#<^v^^>#
######.#

Minute 13, move down:
#.######
#.>3.<.#
#<..<<.#
#>2<em>E</em>22.#
#>v..^<#
######.#

Minute 14, move right:
#.######
#.2>2..#
#.^22^<#
#.>2<em>E</em>^>#
#.>..<.#
######.#

Minute 15, move right:
#.######
#<^<22.#
#.2<.2.#
#><2><em>E</em>.#
#..><..#
######.#

Minute 16, move right:
#.######
#.<..22#
#<<.<..#
#<2.>><em>E</em>#
#.^22^.#
######.#

Minute 17, move down:
#.######
#2.v.<>#
#<.<..<#
#.^>^22#
#.2..2<em>E</em>#
######.#

Minute 18, move down:
#.######
#>2.<.<#
#.2v^2<#
#>..>2>#
#<....>#
######<em>E</em>#
</code>
</pre>

<em>What is the fewest number of minutes required to avoid the blizzards and reach the goal?</em>


## --- Part Two ---
As the expedition reaches the far side of the valley, one of the Elves looks especially dismayed:

He <em>forgot his snacks</em> at the entrance to the valley!

Since you're so good at dodging blizzards, the Elves humbly request that you go back for his snacks. From the same initial conditions, how quickly can you make it from the start to the goal, then back to the start, then back to the goal?

In the above example, the first trip to the goal takes <code>18</code> minutes, the trip back to the start takes <code>23</code> minutes, and the trip back to the goal again takes <code>13</code> minutes, for a total time of <code><em>54</em></code> minutes.

<em>What is the fewest number of minutes required to reach the goal, go back to the start, then reach the goal again?</em>


