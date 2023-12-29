## --- Day 13: Point of Incidence ---
With your help, the hot springs team locates an appropriate spring which launches you neatly and precisely up to the edge of <em>Lava Island</em>.

There's just one problem: you don't see any <em>lava</em>.

You <em>do</em> see a lot of ash and igneous rock; there are even what look like gray mountains scattered around. After a while, you make your way to a nearby cluster of mountains only to discover that the valley between them is completely full of large <em>mirrors</em>.  Most of the mirrors seem to be aligned in a consistent way; perhaps you should head in that direction?

Read the [full puzzle](https://adventofcode.com/2023/day/13).

##  --- Notes ---
A mirror is hidden somewhere in a rectangular board (our input). Some of the _rocks_ in the picture are just reflections. 
The problem doesn't specify if the mirror is put horizontal or vertical, but we know that it's across the
board from one end to the other and it is not necessarly in the middle.

Pretty much following the description, I created a function that tries all possible placements and computes how many errors (smudges - using the problem's terminology) were if the mirror was placed right there. Then just selected the one that had zero errors.

The second half of the problem asked for the very same thing but this time there was one `smudge` in the reflected image.

