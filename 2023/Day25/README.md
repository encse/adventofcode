## --- Day 25: Snowverload ---
<em>Still</em> somehow without snow, you go to the last place you haven't checked: the center of Snow Island, directly below the waterfall.

Here, someone has clearly been trying to fix the problem. Scattered everywhere are hundreds of weather machines, almanacs, communication modules, hoof prints, machine parts, mirrors, lenses, and so on.

Somehow, everything has been <em>wired together</em> into a massive snow-producing apparatus, but nothing seems to be running. You check a tiny screen on one of the communication modules: <code>Error 2023</code>. It doesn't say what <code>Error 2023</code> means, but it <em>does</em> have the phone number for a support line printed on it.

Read the [full puzzle](https://adventofcode.com/2023/day/25).

##  --- Notes ---
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

