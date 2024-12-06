## --- Day 6: Guard Gallivant ---
The Historians use their fancy __device__ again, this time to whisk you all away to the North Pole prototype suit manufacturing lab... in the year __1518__! It turns out that having direct access to history is very convenient for a group of historians.

You still have to be careful of time paradoxes, and so it will be important to avoid anyone from 1518 while The Historians search for the Chief. Unfortunately, a single <em>guard</em> is patrolling this part of the lab.

Read the [full puzzle](https://adventofcode.com/2024/day/6).

This has been a straightforward implementation challenge. I wrote a `Walk` function that tracks the guard's movement and returns the visited locations. It also determines whether the guard enters a loop or exits the grid. `Part1` utilizes only the location information, while `Part2` adds blockers along the guard's path and counts the instances where he starts walking in a cycle.

To make a 90º turn in 2D you need swap the coordinates and multiply _one_ of them by -1. The turn can be clockwise or counterclockwise, it depends on which coordinate was multiplied. 

Here we use complex numbers to represent coordinates, and we get the same effect by simply multiplying with ImaginaryOne or `i`. `-i` turns right, and `i` to left (but this depends on how you draw your coordinate system of course, 'i' points upwards in mine). If this sounds a bit magical to You, I suggest trying it out on a few vectors by hand.
