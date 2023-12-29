## --- Day 24: Never Tell Me The Odds ---
If you are not familiar with the problem, you can read it  [here](https://adventofcode.com/2023/day/24).

A bit unexpectedly we are given a geometry problem that requires floating
point numbers. I don't remember if this was ever needed in Advent of Code. 

Part 1 asks to find the intesection of two 2 dimensional lines. This is
simple enough, but .Net doesn't have a built in matrix library, so I had to 
inline everything that was needed. Part 2 was much more interesting. We have 
to find a position and velocity of a _stone_ that hits all particles provided in 
the input. The particles move in a 3D line now.

I solved this first using the Chinese Remainder Theorem, but I didn't like it
because it was totally independent of Part 1, almost like two different problems.
I went looking around in others' solutions until I found a good one that is easy 
to follow.

The idea is that we try to guess the speed of our stone (a for loop), then supposing
that it is the right velocity we create a new reference frame that moves with 
that speed. The stone doesn't move in this frame, it has some fixed coordinates 
somewhere. Now transform each particle into this reference frame as well. Since the 
stone is not moving, if we properly guessed the speed, we find that each particle 
meets the stone's line at the _same_ point. This must be the stone's location.

We can reuse code from Part 1, just need to project everything to the XY
plane first to compute the stone's `(x,y)` position, then do the same in the XZ
or YZ plane to get `z` as well.
