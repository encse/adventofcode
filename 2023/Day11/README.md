## --- Day 11: Cosmic Expansion ---
You continue following signs for "Hot Springs" and eventually come across an [observatory](https://en.wikipedia.org/wiki/Observatory). The Elf within turns out to be a researcher studying cosmic expansion using the giant telescope here.

He doesn't know anything about the missing machine parts; he's only visiting for this research project. However, he confirms that the hot springs are the next-closest area likely to have people; he'll even take you straight there once he's done with today's observation analysis.

Maybe you can help him with the analysis to speed things up?

Read the [full puzzle](https://adventofcode.com/2023/day/11).

##  --- Notes ---
A pretty simple problem for today. We had to compute the sum of the pairwise Manhattan distances 
of each galaxies (`#` symbols) in a map.

The twist is that moving accross some columns or rows of the map is worths double distance points 
(one million in Part 2). But it was not hard to incorporate this into our distance function.

I did it this way, but we should mention that since we are computing the distance over each pair, and all operations are commutative and associative, it's probably possible to reorder things a bit which can result in a more efficient algorithm. 

