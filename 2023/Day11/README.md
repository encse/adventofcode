## --- Day 11: Cosmic Expansion ---
Visit the Advent of Code website for the problem statement[here](https://adventofcode.com/2023/day/11).

A pretty simple problem for today. We had to compute the sum of the pairwise Manhattan distances 
of each galaxies (`#` symbols) in a map.

The twist is that moving accross some columns or rows of the map is worths double distance points 
(one million in Part 2). But it was not hard to incorporate this into our distance function.

I did it this way, but we should mention that since we are computing the distance over each pair, and all operations are commutative and associative, it's probably possible to reorder things a bit which can result in a more efficient algorithm. 

