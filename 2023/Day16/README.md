## --- Day 16: The Floor Will Be Lava ---
The original problem description is available [here](https://adventofcode.com/2023/day/16).

I was a bit worried when I saw Part 1, because it let the window open for a complicated optimization
for Part 2. But it just turned out to be the same thing as Part 1 iterated along the edges of the map. 

I went with the proven strategy and represented the map as a dictionary indexed by complex numbers. It's
easy to check the bounds, and changing positions is just complex arithmetic.

At first I created a long switch case to determine how a beam changes its way when encountering
mirrors and splitters, but it turns out that in many cases it just continues in the same direction.
Splitting can be handled in just two lines for the vertical and horizontal case. Finally my choice of 
the coordinate system makes turning around mirrors very simple: the coordinates flip when the mirror 
is facing `\` and just an additional multiplication by -1 is needed for the `/` case.

