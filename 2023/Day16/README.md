## --- Day 16: The Floor Will Be Lava ---
With the beam of light completely focused <em>somewhere</em>, the reindeer leads you deeper still into the Lava Production Facility. At some point, you realize that the steel facility walls have been replaced with cave, and the doorways are just cave, and the floor is cave, and you're pretty sure this is actually just a giant cave.

Finally, as you approach what must be the heart of the mountain, you see a bright light in a cavern up ahead. There, you discover that the beam of light you so carefully focused is emerging from the cavern wall closest to the facility and pouring all of its energy into a contraption on the opposite side.

Upon closer inspection, the contraption appears to be a flat, two-dimensional square grid containing <em>empty space</em> (<code>.</code>), <em>mirrors</em> (<code>/</code> and <code>\</code>), and <em>splitters</em> (<code>|</code> and <code>-</code>).

Read the [full puzzle](https://adventofcode.com/2023/day/16).

##  --- Notes ---
I was a bit worried when I saw Part 1, because it let the window open for a complicated optimization
for Part 2. But it just turned out to be the same thing as Part 1 iterated along the edges of the map. 

I went with the proven strategy and represented the map as a dictionary indexed by complex numbers. It's
easy to check the bounds, and changing positions is just complex arithmetic.

At first I created a long switch case to determine how a beam changes its way when encountering
mirrors and splitters, but it turns out that in many cases it just continues in the same direction.
Splitting can be handled in just two lines for the vertical and horizontal case. Finally my choice of 
the coordinate system makes turning around mirrors very simple: the coordinates flip when the mirror 
is facing `\` and just an additional multiplication by -1 is needed for the `/` case.

