## --- Day 14: Parabolic Reflector Dish ---
You reach the place where all of the mirrors were pointing: a massive [parabolic reflector dish](https://en.wikipedia.org/wiki/Parabolic_reflector) attached to the side of another large mountain.

The dish is made up of many small mirrors, but while the mirrors themselves are roughly in the shape of a parabolic reflector dish, each individual mirror seems to be pointing in slightly the wrong direction. If the dish is meant to focus light, all it's doing right now is sending it in a vague direction.

This system must be what provides the energy for the lava! If you focus the reflector dish, maybe you can go where it's pointing and use the light to fix the lava production.

Read the [full puzzle](https://adventofcode.com/2023/day/14).

##  --- Notes ---
We are playing Boulder Dash today, but instead of moving a character on the screen
we tilt the `screen` itself and move all the boulders at once. The task asks us to implement tilting 
in each directions (North, South, East and West), but I just implemented North and kept rotating 
the board by 90 degrees.

Then we start tilting like crazy - four billion times. When you see _iterate for &lt;a big number&gt;_ you immediately start looking for some repetition. This is not different with today's problem either. In just about a hundred steps the board enters into a loop, so we can jump
over the rest of the tilting work and just read the result out from the list of states we collected.


