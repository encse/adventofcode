## --- Day 22: Sand Slabs ---
The original problem description is available
[here](https://adventofcode.com/2023/day/22).

We deserved a simple one today. I started with a function that applies gravity to 
the blocks. It orders them in ascending Z order then just runs over the list and
pushes each block down as much as possible. The result is a nicely packed jenga tower.

Several helper strucures were introduced to make things easier. I have a `Range` and 
a `Block` with some helpers like `IntersectsXY` that tells if the X-Y projection of 
two blocks are in cover.

I also created a function that returns the _support structure_ of our model at hand, 
so that I can tell the upper and lower neighbours of each block easily.

The eye catching _Kaboom_ function goes over the input and selects each block for 
desintegration. It calculates the number of blocks that start falling when this 
single block disappears. The result is a list of integers, which can be used 
to answer both questions for today.
