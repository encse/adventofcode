## --- Day 8: Playground ---
<em>
Equipped with a new understanding of teleporter maintenance, you confidently step onto the repaired teleporter pad.

You rematerialize on an unfamiliar teleporter pad and find yourself in a vast underground space which contains a giant playground!

Visit the website for the full story and [full puzzle](https://adventofcode.com/2025/day/8) description.
</em>

I decided to go with two implementations of Kruskal's algorithm, as part one and two are different enough. I didn't use a
disjoint set representation. It's fast enough this way as well, and switching to disjoint sets would just make the code longer.

This is how my graph looks with the spanning tree:

<video src="graph.mp4" controls autoplay loop muted playsinline width="640">
</video>

I created a small [renderer](graph.html) for this. (Well, ChatGPT created the render, with my guidance.)