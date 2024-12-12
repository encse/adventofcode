## --- Day 12: Garden Groups ---
Why not search for the Chief Historian near the _gardener_ and his _massive farm_? There's plenty of food, so The Historians grab something to eat while they search.

You're about to settle near a complex arrangement of garden plots when some Elves ask if you can lend a hand. They'd like to set up fences around each region of garden plots, but they can't figure out how much fence they need to order or how much it will cost. They hand you a map (your puzzle input) of the garden plots.

Read the [full puzzle](https://adventofcode.com/2024/day/12).

Here's an improved version with corrected grammar and flow:

---

One can sense that the difficulty has ramped up today with a more complex problem involving area and perimeter calculations. 

First we determine the connected components (regions) of plants of the 
same type. This is done by picking a position of the garden and applying a standard flood-fill algorithm to it. The result is a _region_.
As we process each region, we carefully remove all affected positions and then pick an untouched position to repeat the process. 
In a few iterations, the entire garden is associated with the correct regions. The size of the regions corresponds to the _area_ 
in question. This logic is implemented in the `GetRegions` function below.


The second part of the problem, however, is more intriguing: how do we calculate the lengths of the fences? In part 1, this is 
relatively simple because we just iterate through each region and count the neighboring cells that belong to a different regions. This tells how many fence is needed. Unfortunately, this logic doesn't work for part 2.

Initially, I attempted a "walk-around" approach, which involves finding a fence segment and tracing it like a line-following 
robot while counting the turns. The challenge with this approach is that some regions have holes, that require fences as well and I didnt want to implement hole finding.

I realized it is probably more straightforward to collect the fence segments along straight lines. For instance, if I start at the top-left corner and scan the map horizontally to the right, there is a long fence at the top of the map. A new segement starts when I step into a different region. On the next line, a similar check is performed: I determine if 
a fence is needed above (due to a different region) or if a new region has been entered. This horizontal scanning is repeated 
twice to detect fences above and below the positions, and a similar vertical scan is performed to identify fence segments on the left and right. You can find the implementation in the `GetFenceSegements` function.

`GetPerimeters` uses `GetFenceSegments` to calculate the total length of the fences for each region (in standard units 
for part 1 or as the number of different segments for part 2).

This concludes day 12. I really enjoyed this one, although the solution became longer than I like it to be...