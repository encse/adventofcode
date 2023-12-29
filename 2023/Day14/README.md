## --- Day 14: Parabolic Reflector Dish ---
The task description is copyrighted, but it's available [here](https://adventofcode.com/2023/day/14).

We are playing Boulder Dash today, but instead of moving a character on the screen
we tilt the `screen` itself and move all the boulders at once. The task asks us to implement tilting 
in each directions (North, South, East and West), but I just implemented North and kept rotating 
the board by 90 degrees.

Then we start tilting like crazy - four billion times. When you see _iterate for &lt;a big number&gt;_ you immediately start looking for some repetition. This is not different with today's problem either. In just about a hundred steps the board enters into a loop, so we can jump
over the rest of the tilting work and just read the result out from the list of states we collected.


