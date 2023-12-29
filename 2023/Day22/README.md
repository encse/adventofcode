## --- Day 22: Sand Slabs ---
Enough sand has fallen; it can finally filter water for Snow Island.

Well, <em>almost</em>.

The sand has been falling as large compacted <em>bricks</em> of sand, piling up to form an impressive stack here near the edge of Island Island. In order to make use of the sand to filter water, some of the bricks will need to be broken apart - nay, <em>disintegrated</em> - back into freely flowing sand.

Read the [full puzzle](https://adventofcode.com/2023/day/22).

##  --- Notes ---
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
