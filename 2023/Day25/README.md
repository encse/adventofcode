## --- Day 25: Snowverload ---
Those who need a refresh can read the problem [here](https://adventofcode.com/2023/day/25).

This is our last day and these are historically not very hard. The puzzle is asking us 
to cut an undirected graph into two components by removing only three edges. I had 
absolutely no idea how to do that in an effective way, so it was time to consult the 
literature. Soon enough I found [Karger's algorithm](https://en.wikipedia.org/wiki/Karger%27s_algorithm) 
on Wikipedia.

It's a randomized algorithm that works on non-weighted, undirected graphs like ours.
It finds _some cut_ which is not necessarily minimal, but there is a good chance 
that it finds the minimal one in a few tries. _This is the Elf way!_

Karger's is not hard to implement, but I'm not used to do this kind of things lately, 
so I spent quite a lot of time getting it right. One mistake was that I didn't 
add the edges in the reverse direction: the input contains them only in one way. Then it was not obvious what to do with multiple edges between two nodes, because the algorithm needs to create these.
But it has started to work and I just let it do its thing in a loop and it really 
found a cut with 3 edges in a few milliseconds.

It was easy to extend the algorithm to return the sizes of the two components as well. 
Part 1 asks for the product of these. Part 2 is a meta puzzle, it requires to complete 
all other challenges from the previous days, but I already finished those, so I can 
conclude season today (Dec 25th). 

Thanks for joining me! If you find this work useful or want to get in touch 
(even just saying hello), find me on [Github](https://github.com/encse) or 
[Twitter](https://twitter.com/encse).

Psst! There is game [hidden](game) in this site.

