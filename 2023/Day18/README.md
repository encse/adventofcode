## --- Day 18: Lavaduct Lagoon ---
Thanks to your efforts, the machine parts factory is one of the first factories up and running since the lavafall came back. However, to catch up with the large backlog of parts requests, the factory will also need a <em>large supply of lava</em> for a while; the Elves have already started creating a large lagoon nearby for this purpose.

However, they aren't sure the lagoon will be big enough; they've asked you to take a look at the <em>dig plan</em> (your puzzle input). For example:

<pre>
<code>R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 2 (#015232)
U 2 (#7a21e3)
</code>
</pre>

Read the [full puzzle](https://adventofcode.com/2023/day/18).

##  --- Notes ---
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
of _interior points_ and _points in the boundary_. The problem asked for the sum 
of latter two. 

I give myself an extra ⭐ for learning something today.
