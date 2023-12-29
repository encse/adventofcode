## --- Day 21: Step Counter ---
You manage to catch the [airship](7) right as it's dropping someone else off on their all-expenses-paid trip to Desert Island! It even helpfully drops you off near the [gardener](5) and his massive farm.

"You got the sand flowing again! Great work! Now we just need to wait until we have enough sand to filter the water for Snow Island and we'll have snow again in no time."

While you wait, one of the Elves that works with the gardener heard how good you are at solving problems and would like your help. He needs to get his [steps](https://en.wikipedia.org/wiki/Pedometer) in for the day, and so he'd like to know <em>which garden plots he can reach with exactly his remaining <code>64</code> steps</em>.

Read the [full puzzle](https://adventofcode.com/2023/day/21).

##  --- Notes ---
At first I solved this with carefully maintaining the number of different 
tiles (the 131x131 regions that repeat indefinitely) after each step. It 
turns out that there are only nine tile categories based on the direction 
closest to the starting point. The elf can go straight left, up, right 
and down and reach the next tile without obstacles. This is a special 
property of the input.

Each tile in a category can be in a few hundred different states. The 
first one (what I call the _seed_) is the point where the elf enters the 
tile. This can be the center of an edge or one of its corners. After 
seeding, the tile _ages_ on its own pace. Thanks to an other property of 
the input, tiles are not affected by their neighbourhood. Aging continues 
until a tile _grows_ up, when it starts to oscillate between just two 
states back and forth.

My first solution involved a 9 by 260 matrix containing the number of 
tiles in each state. I implemented the aging process and carefully 
computed when to seed new tiles for each category.

It turns out that if we are looking at only steps where `n = 131 * k + 65` 
we can compute how many tiles are in each position of the matrix.
I haven't gone through this whole process, just checked a few examples 
until I convinced myself that each and every item in the matrix is either 
constant or a linear or quadratic function of n.

This is not that hard to see as it sounds. After some lead in at the 
beginning, things start to work like this: in each batch of 131 steps a 
set of center tiles and a set of corner styles is generated. 
Always 4 center tiles come in, but corner tiles are linear in `n: 1,2,3...`
That is the grown up population for center tiles must be linear in `n`, 
and quadratic for the corners (can be computed using triangular numbers). 

If we know the active positions for each tile category and state, we
can multiply it with the number of tiles and sum it up to get the result.
This all means that if we reorganize the equations we get to a form of:

```
    a * n^2 + b * n + c      if n = k * 131 + 65
```

We just need to compute this polynom for 3 values and interpolate.
Finally evaluate for `n = 26501365` which happens to be `202300 * 131 + 65`
to get the final result.
