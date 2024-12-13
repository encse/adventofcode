## --- Day 8: Resonant Collinearity ---
You find yourselves on the _roof_ of a top-secret Easter Bunny installation.

While The Historians do their thing, you take a look at the familiar <em>huge antenna</em>. Much to your surprise, it seems to have been reconfigured to emit a signal that makes people 0.1% more likely to buy Easter Bunny brand Imitation Mediocre Chocolate as a Christmas gift! Unthinkable!

_Visit the website for the full story and [puzzle](https://adventofcode.com/2024/day/8) description._

Continuing the steps I started yesterday, I extracted a common function (`GetUniquePositions`) that takes a parameter to generate antinode positions, representing the difference between part one and part two.

`getAntinodes` returns the antinode positions of srcAntenna on the dstAntenna side. It doesn’t need to be symmetric — i.e., it doesn’t have to return the antinodes on the srcAntenna side — because GetUniquePositions will call it with the parameters swapped as well. This allows us to handle only one direction at a time.

The generators are fairly straightforward: I simply take steps in the direction determined by the antennas, starting from the destination. Since I represented coordinates using complex numbers again, there’s no need for any special handling on the algebra side, regular + and - operations work.

The `GetAntinodes` delegate is introduced only for documentation purposes. It looks better to my eyes than an ugly `Func<>` with four type parameters would in `GetUniquePositions`-s signature. 
