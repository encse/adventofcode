## --- Day 9: Mirage Maintenance ---
You ride the camel through the sandstorm and stop where the ghost's maps told you to stop. The sandstorm subsequently subsides, somehow seeing you standing at an <em>oasis</em>!

The camel goes to get some water and you stretch your neck. As you look up, you discover what must be yet another giant floating island, this one made of metal! That must be where the <em>parts to fix the sand machines</em> come from.

There's even a [hang glider](https://en.wikipedia.org/wiki/Hang_gliding) partially buried in the sand here; once the sun rises and heats up the sand, you might be able to use the glider and the hot air to get all the way up to the metal island!

Read the [full puzzle](https://adventofcode.com/2023/day/9).

##  --- Notes ---
We are implementing an _extrapolation_ algorithm that reminds me to my 
university years when we did Newton interpolation, or was it Lagrange? I don't 
remember anymore, but it was using a similar algorithm to this.

Part 1 and Part 2 differs only in the direction of the extrapolation, and one 
can be implemented using the other, just like I did it below.
