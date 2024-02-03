## --- Day 18: Lavaduct Lagoon ---
If you are not familiar with the problem, you can read it [here](https://adventofcode.com/2023/day/18).

Both parts ask for the integer area covered by some polygon. But it 
wouldn't be Advent of Code, if the polygon came in the form of coordinates. 
First we are dealing with some odd _dig_ instruction list with _hex colors_.

The polygon in Part 1 is much smaller and one can use any algorithm from flood fill
to ray casting, but Part 2 makes it clear that we need to pull out the bigger guns.

I heard about the [Shoelace formula](https://en.wikipedia.org/wiki/Shoelace_formula), but haven't used it in practice yet. I knew
that I can calculate the (signed) area of a polygon by summing up some determinants 
using the neighbouring vertices. But I haven't heard about [Pick's theorem](https://en.wikipedia.org/wiki/Pick%27s_theorem) before, so
it made me think for a while to extend this idea to return the _integer_ area instead
of the _real_ one.

Having solved Part 1 I could somehow guess the right formula involving _half the 
boundary plus 1_, which sounds just _right_ and was enough for Part 2, then still
being puzzled I went to the solution thread and read about Pick.

Pick's theorem connects the area returned by the Shoelace formula with the number 
of _interior points_ and _points in the boundary_. The problem asked for the sum. 

I give myself an extra ⭐ for learning something today.
