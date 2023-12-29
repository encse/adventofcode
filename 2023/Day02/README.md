## --- Day 2: Cube Conundrum ---
The task description is copyrighted, but it's available [here](https://adventofcode.com/2023/day/2).

Ok, now we are on track. The hardest part of this problem is the parsing, but I introduced a helper that can extract a number in the context of some regular expression which works like a breeze. What's more, we only need to keep track of the maximum of the red, green and blue boxes, so our `Game` struct becomes just four integers.

The actual _algorithm_ for Part 1 and Part 2 is very simple, and linq makes it quite readable as well.
