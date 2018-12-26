original source: [https://adventofcode.com/2015/day/18](https://adventofcode.com/2015/day/18)
## --- Day 18: Like a GIF For Your Yard ---
After the [million lights incident](6), the fire code has gotten stricter: now, at most ten thousand lights are allowed.  You arrange them in a 100x100 grid.

Never one to let you down, Santa again mails you instructions on the ideal lighting configuration.  With so few lights, he says, you'll have to resort to *animation*.

Start by setting your lights to the included initial configuration (your puzzle input).  A `#` means "on", and a `.` means "off".

Then, animate your grid in steps, where each step decides the next configuration based on the current one.  Each light's next state (either on or off) depends on its current state and the current states of the eight lights adjacent to it (including diagonals).  Lights on the edge of the grid might have fewer than eight neighbors; the missing ones always count as "off".

For example, in a simplified 6x6 grid, the light marked `A` has the neighbors numbered `1` through `8`, and the light marked `B`, which is on an edge, only has the neighbors marked `1` through `5`:

```
1B5...
234...
......
..123.
..8A4.
..765.
```

The state a light should have next is based on its current state (on or off) plus the *number of neighbors that are on*:


 - A light which is *on* stays on when `2` or `3` neighbors are on, and turns off otherwise.
 - A light which is *off* turns on if exactly `3` neighbors are on, and stays off otherwise.

All of the lights update simultaneously; they all consider the same current state before moving to the next.

Here's a few steps from an example configuration of another 6x6 grid:

```
Initial state:
.#.#.#
...##.
#....#
..#...
#.#..#
####..

After 1 step:
..##..
..##.#
...##.
......
#.....
#.##..

After 2 steps:
..###.
......
..###.
......
.#....
.#....

After 3 steps:
...#..
......
...#..
..##..
......
......

After 4 steps:
......
......
..##..
..##..
......
......
```

After `4` steps, this example has four lights on.

In your grid of 100x100 lights, given your initial configuration, *how many lights are on after 100 steps*?


## --- Part Two ---
You flip the instructions over; Santa goes on to point out that this is all just an implementation of [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway's_Game_of_Life).  At least, it was, until you notice that something's wrong with the grid of lights you bought: four lights, one in each corner, are *stuck on* and can't be turned off.  The example above will actually run like this:

```
Initial state:
##.#.#
...##.
#....#
..#...
#.#..#
####.#

After 1 step:
#.##.#
####.#
...##.
......
#...#.
#.####

After 2 steps:
#..#.#
#....#
.#.##.
...##.
.#..##
##.###

After 3 steps:
#...##
####.#
..##.#
......
##....
####.#

After 4 steps:
#.####
#....#
...#..
.##...
#.....
#.#..#

After 5 steps:
##.###
.##..#
.##...
.##...
#.#...
##...#
```

After `5` steps, this example now has `17` lights on.

In your grid of 100x100 lights, given your initial configuration, but with the four corners always in the *on* state, *how many lights are on after 100 steps*?


