original source: [https://adventofcode.com/2018/day/15](https://adventofcode.com/2018/day/15)
## --- Day 15: Beverage Bandits ---
Having perfected their hot chocolate, the Elves have a new problem: the [Goblins](https://en.wikipedia.org/wiki/Goblin) that live in these caves will do anything to steal it. Looks like they're here for a fight.

You scan the area, generating a map of the walls (<code>#</code>), open cavern (<code>.</code>), and starting position of every Goblin (<code>G</code>) and Elf (<code>E</code>) (your puzzle input).

Combat proceeds in <em>rounds</em>; in each round, each unit that is still alive takes a <em>turn</em>, resolving all of its actions before the next unit's turn begins. On each unit's turn, it tries to <em>move</em> into range of an enemy (if it isn't already) and then <em>attack</em> (if it is in range).

All units are very disciplined and always follow very strict combat rules. Units never move or attack diagonally, as doing so would be dishonorable. When multiple choices are equally valid, ties are broken in <em>reading order</em>: top-to-bottom, then left-to-right.  For instance, the order in which units take their turns within a round is the <em>reading order of their starting positions</em> in that round, regardless of the type of unit or whether other units have moved after the round started.  For example:

<pre>
<code>                 would take their
These units:   turns in this order:
  #######           #######
  #.G.E.#           #.1.2.#
  #E.G.E#           #3.4.5#
  #.G.E.#           #.6.7.#
  #######           #######
</code>
</pre>

Each unit begins its turn by identifying all possible <em>targets</em> (enemy units). If no targets remain, combat ends.

Then, the unit identifies all of the open squares (<code>.</code>) that are <em>in range</em> of each target; these are the squares which are <em>adjacent</em> (immediately up, down, left, or right) to any target and which aren't already occupied by a wall or another unit. Alternatively, the unit might <em>already</em> be in range of a target. If the unit is not already in range of a target, and there are no open squares which are in range of a target, the unit ends its turn.

If the unit is already in range of a target, it does not <em>move</em>, but continues its turn with an <em>attack</em>. Otherwise, since it is not in range of a target, it <em>moves</em>.

To <em>move</em>, the unit first considers the squares that are <em>in range</em> and determines <em>which of those squares it could reach in the fewest steps</em>. A <em>step</em> is a single movement to any <em>adjacent</em> (immediately up, down, left, or right) open (<code>.</code>) square. Units cannot move into walls or other units. The unit does this while considering the <em>current positions of units</em> and does <em>not</em> do any prediction about where units will be later. If the unit cannot reach (find an open path to) any of the squares that are in range, it ends its turn. If multiple squares are in range and <em>tied</em> for being reachable in the fewest steps, the square which is first in <em>reading order</em> is chosen. For example:

<pre>
<code>Targets:      In range:     Reachable:    Nearest:      Chosen:
#######       #######       #######       #######       #######
#E..G.#       #E.?G?#       #E.@G.#       #E.!G.#       #E.+G.#
#...#.#  -->  #.?.#?#  -->  #.@.#.#  -->  #.!.#.#  -->  #...#.#
#.G.#G#       #?G?#G#       #@G@#G#       #!G.#G#       #.G.#G#
#######       #######       #######       #######       #######
</code>
</pre>

In the above scenario, the Elf has three targets (the three Goblins):


 - Each of the Goblins has open, adjacent squares which are <em>in range</em> (marked with a <code>?</code> on the map).
 - Of those squares, four are <em>reachable</em> (marked <code>@</code>); the other two (on the right) would require moving through a wall or unit to reach.
 - Three of these reachable squares are <em>nearest</em>, requiring the fewest steps (only <code>2</code>) to reach (marked <code>!</code>).
 - Of those, the square which is first in reading order is <em>chosen</em> (<code>+</code>).

The unit then takes a single <em>step</em> toward the chosen square along the <em>shortest path</em> to that square. If multiple steps would put the unit equally closer to its destination, the unit chooses the step which is first in reading order. (This requires knowing when there is <em>more than one shortest path</em> so that you can consider the first step of each such path.) For example:

<pre>
<code>In range:     Nearest:      Chosen:       Distance:     Step:
#######       #######       #######       #######       #######
#.E...#       #.E...#       #.E...#       #4E<em>2</em>12#       #..E..#
#...?.#  -->  #...!.#  -->  #...+.#  -->  #3<em>2</em>101#  -->  #.....#
#..?G?#       #..!G.#       #...G.#       #432G2#       #...G.#
#######       #######       #######       #######       #######
</code>
</pre>

The Elf sees three squares in range of a target (<code>?</code>), two of which are nearest (<code>!</code>), and so the first in reading order is chosen (<code>+</code>). Under "Distance", each open square is marked with its distance from the destination square; the two squares to which the Elf could move on this turn (down and to the right) are both equally good moves and would leave the Elf <code>2</code> steps from being in range of the Goblin. Because the step which is first in reading order is chosen, the Elf moves <em>right</em> one square.

Here's a larger example of movement:

<pre>
<code>Initially:
#########
#G..G..G#
#.......#
#.......#
#G..E..G#
#.......#
#.......#
#G..G..G#
#########

After 1 round:
#########
#.G...G.#
#...G...#
#...E..G#
#.G.....#
#.......#
#G..G..G#
#.......#
#########

After 2 rounds:
#########
#..G.G..#
#...G...#
#.G.E.G.#
#.......#
#G..G..G#
#.......#
#.......#
#########

After 3 rounds:
#########
#.......#
#..GGG..#
#..GEG..#
#G..G...#
#......G#
#.......#
#.......#
#########
</code>
</pre>

Once the Goblins and Elf reach the positions above, they all are either in range of a target or cannot find any square in range of a target, and so none of the units can move until a unit dies.

After moving (or if the unit began its turn in range of a target), the unit <em>attacks</em>.

To <em>attack</em>, the unit first determines <em>all</em> of the targets that are <em>in range</em> of it by being immediately <em>adjacent</em> to it. If there are no such targets, the unit ends its turn. Otherwise, the adjacent target with the <em>fewest hit points</em> is selected; in a tie, the adjacent target with the fewest hit points which is first in reading order is selected.

The unit deals damage equal to its <em>attack power</em> to the selected target, reducing its hit points by that amount. If this reduces its hit points to <code>0</code> or fewer, the selected target <em>dies</em>: its square becomes <code>.</code> and it takes no further turns.

Each <em>unit</em>, either Goblin or Elf, has <code>3</code> <em>attack power</em> and starts with <code>200</code> <em>hit points</em>.

For example, suppose the only Elf is about to attack:

<pre>
<code>       HP:            HP:
G....  9       G....  9  
..G..  4       ..G..  4  
..E<em>G</em>.  2  -->  ..E..     
..G..  2       ..G..  2  
...G.  1       ...G.  1  
</code>
</pre>

The "HP" column shows the hit points of the Goblin to the left in the corresponding row. The Elf is in range of three targets: the Goblin above it (with <code>4</code> hit points), the Goblin to its right (with <code>2</code> hit points), and the Goblin below it (also with <code>2</code> hit points). Because three targets are in range, the ones with the lowest hit points are selected: the two Goblins with <code>2</code> hit points each (one to the right of the Elf and one below the Elf). Of those, the Goblin first in reading order (the one to the right of the Elf) is selected. The selected Goblin's hit points (<code>2</code>) are reduced by the Elf's attack power (<code>3</code>), reducing its hit points to <code>-1</code>, killing it.

After attacking, the unit's turn ends.  Regardless of how the unit's turn ends, the next unit in the round takes its turn.  If all units have taken turns in this round, the round ends, and a new round begins.

The Elves look quite outnumbered.  You need to determine the <em>outcome</em> of the battle: the <em>number of full rounds that were completed</em> (not counting the round in which combat ends) multiplied by <em>the sum of the hit points of all remaining units</em> at the moment combat ends. (Combat only ends when a unit finds no targets during its turn.)

Below is an entire sample combat. Next to each map, each row's units' hit points are listed from left to right.

<pre>
<code>Initially:
#######   
#.G...#   G(200)
#...EG#   E(200), G(200)
#.#.#G#   G(200)
#..G#E#   G(200), E(200)
#.....#   
#######   

After 1 round:
#######   
#..G..#   G(200)
#...EG#   E(197), G(197)
#.#G#G#   G(200), G(197)
#...#E#   E(197)
#.....#   
#######   

After 2 rounds:
#######   
#...G.#   G(200)
#..GEG#   G(200), E(188), G(194)
#.#.#G#   G(194)
#...#E#   E(194)
#.....#   
#######   

Combat ensues; eventually, the top Elf dies:

After 23 rounds:
#######   
#...G.#   G(200)
#..G.G#   G(200), G(131)
#.#.#G#   G(131)
#...#E#   E(131)
#.....#   
#######   

After 24 rounds:
#######   
#..G..#   G(200)
#...G.#   G(131)
#.#G#G#   G(200), G(128)
#...#E#   E(128)
#.....#   
#######   

After 25 rounds:
#######   
#.G...#   G(200)
#..G..#   G(131)
#.#.#G#   G(125)
#..G#E#   G(200), E(125)
#.....#   
#######   

After 26 rounds:
#######   
#G....#   G(200)
#.G...#   G(131)
#.#.#G#   G(122)
#...#E#   E(122)
#..G..#   G(200)
#######   

After 27 rounds:
#######   
#G....#   G(200)
#.G...#   G(131)
#.#.#G#   G(119)
#...#E#   E(119)
#...G.#   G(200)
#######   

After 28 rounds:
#######   
#G....#   G(200)
#.G...#   G(131)
#.#.#G#   G(116)
#...#E#   E(113)
#....G#   G(200)
#######   

More combat ensues; eventually, the bottom Elf dies:

After 47 rounds:
#######   
#G....#   G(200)
#.G...#   G(131)
#.#.#G#   G(59)
#...#.#   
#....G#   G(200)
#######   
</code>
</pre>

Before the 48th round can finish, the top-left Goblin finds that there are no targets remaining, and so combat ends. So, the number of <em>full rounds</em> that were completed is <code><em>47</em></code>, and the sum of the hit points of all remaining units is <code>200+131+59+200 = <em>590</em></code>. From these, the <em>outcome</em> of the battle is <code>47 * 590 = <em>27730</em></code>.

Here are a few example summarized combats:

<pre>
<code>#######       #######
#G..#E#       #...#E#   E(200)
#E#E.E#       #E#...#   E(197)
#G.##.#  -->  #.E##.#   E(185)
#...#E#       #E..#E#   E(200), E(200)
#...E.#       #.....#
#######       #######

Combat ends after 37 full rounds
Elves win with 982 total hit points left
Outcome: 37 * 982 = <em>36334</em>
</code>
</pre>

<pre>
<code>#######       #######   
#E..EG#       #.E.E.#   E(164), E(197)
#.#G.E#       #.#E..#   E(200)
#E.##E#  -->  #E.##.#   E(98)
#G..#.#       #.E.#.#   E(200)
#..E#.#       #...#.#   
#######       #######   

Combat ends after 46 full rounds
Elves win with 859 total hit points left
Outcome: 46 * 859 = <em>39514</em>
</code>
</pre>

<pre>
<code>#######       #######   
#E.G#.#       #G.G#.#   G(200), G(98)
#.#G..#       #.#G..#   G(200)
#G.#.G#  -->  #..#..#   
#G..#.#       #...#G#   G(95)
#...E.#       #...G.#   G(200)
#######       #######   

Combat ends after 35 full rounds
Goblins win with 793 total hit points left
Outcome: 35 * 793 = <em>27755</em>
</code>
</pre>

<pre>
<code>#######       #######   
#.E...#       #.....#   
#.#..G#       #.#G..#   G(200)
#.###.#  -->  #.###.#   
#E#G#G#       #.#.#.#   
#...#G#       #G.G#G#   G(98), G(38), G(200)
#######       #######   

Combat ends after 54 full rounds
Goblins win with 536 total hit points left
Outcome: 54 * 536 = <em>28944</em>
</code>
</pre>

<pre>
<code>#########       #########   
#G......#       #.G.....#   G(137)
#.E.#...#       #G.G#...#   G(200), G(200)
#..##..G#       #.G##...#   G(200)
#...##..#  -->  #...##..#   
#...#...#       #.G.#...#   G(200)
#.G...G.#       #.......#   
#.....G.#       #.......#   
#########       #########   

Combat ends after 20 full rounds
Goblins win with 937 total hit points left
Outcome: 20 * 937 = <em>18740</em>
</code>
</pre>

<em>What is the outcome</em> of the combat described in your puzzle input?


## --- Part Two ---
According to your calculations, the Elves are going to lose badly. Surely, you won't mess up the timeline too much if you give them just a little advanced technology, right?

You need to make sure the Elves not only <em>win</em>, but also suffer <em>no losses</em>: even the death of a single Elf is unacceptable.

However, you can't go too far: larger changes will be more likely to permanently alter spacetime.

So, you need to <em>find the outcome</em> of the battle in which the Elves have the <em>lowest integer attack power</em> (at least <code>4</code>) that allows them to <em>win without a single death</em>. The Goblins always have an attack power of <code>3</code>.

In the first summarized example above, the lowest attack power the Elves need to win without losses is <code>15</code>:

<pre>
<code>#######       #######
#.G...#       #..E..#   E(158)
#...EG#       #...E.#   E(14)
#.#.#G#  -->  #.#.#.#
#..G#E#       #...#.#
#.....#       #.....#
#######       #######

Combat ends after 29 full rounds
Elves win with 172 total hit points left
Outcome: 29 * 172 = <em>4988</em>
</code>
</pre>

In the second example above, the Elves need only <code>4</code> attack power:

<pre>
<code>#######       #######
#E..EG#       #.E.E.#   E(200), E(23)
#.#G.E#       #.#E..#   E(200)
#E.##E#  -->  #E.##E#   E(125), E(200)
#G..#.#       #.E.#.#   E(200)
#..E#.#       #...#.#
#######       #######

Combat ends after 33 full rounds
Elves win with 948 total hit points left
Outcome: 33 * 948 = <em>31284</em>
</code>
</pre>

In the third example above, the Elves need <code>15</code> attack power:

<pre>
<code>#######       #######
#E.G#.#       #.E.#.#   E(8)
#.#G..#       #.#E..#   E(86)
#G.#.G#  -->  #..#..#
#G..#.#       #...#.#
#...E.#       #.....#
#######       #######

Combat ends after 37 full rounds
Elves win with 94 total hit points left
Outcome: 37 * 94 = <em>3478</em>
</code>
</pre>

In the fourth example above, the Elves need <code>12</code> attack power:

<pre>
<code>#######       #######
#.E...#       #...E.#   E(14)
#.#..G#       #.#..E#   E(152)
#.###.#  -->  #.###.#
#E#G#G#       #.#.#.#
#...#G#       #...#.#
#######       #######

Combat ends after 39 full rounds
Elves win with 166 total hit points left
Outcome: 39 * 166 = <em>6474</em>
</code>
</pre>

In the last example above, the lone Elf needs <code>34</code> attack power:

<pre>
<code>#########       #########   
#G......#       #.......#   
#.E.#...#       #.E.#...#   E(38)
#..##..G#       #..##...#   
#...##..#  -->  #...##..#   
#...#...#       #...#...#   
#.G...G.#       #.......#   
#.....G.#       #.......#   
#########       #########   

Combat ends after 30 full rounds
Elves win with 38 total hit points left
Outcome: 30 * 38 = <em>1140</em>
</code>
</pre>

After increasing the Elves' attack power until it is just barely enough for them to win without any Elves dying, <em>what is the outcome</em> of the combat described in your puzzle input?


