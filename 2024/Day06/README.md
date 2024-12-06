## --- Day 6: Guard Gallivant ---
The Historians use their fancy [device](4) again, this time to whisk you all away to the North Pole prototype suit manufacturing lab... in the year __1518__! It turns out that having direct access to history is very convenient for a group of historians.

You still have to be careful of time paradoxes, and so it will be important to avoid anyone from 1518 while The Historians search for the Chief. Unfortunately, a single <em>guard</em> is patrolling this part of the lab.

Read the [full puzzle](https://adventofcode.com/2024/day/6).

This was a straightforward implementation challenge. I wrote a `Walk` function that tracks the guard's movement and returns the visited locations. It also determines whether the guard enters a loop or exits the grid. `Part1` utilizes only the location information, while `Part2` adds blockers along the guard's path and counts the instances where he starts walking in a cycle.
