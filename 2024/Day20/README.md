## --- Day 20: Race Condition ---
The Historians are quite pixelated again. This time, a massive, black building looms over you - you're _right outside_ the CPU!

While The Historians get to work, a nearby program sees that you're idle and challenges you to a <em>race</em>. Apparently, you've arrived just in time for the frequently-held <em>race condition</em> festival!

_Visit the website for the full story and [full puzzle](https://adventofcode.com/2024/day/20) description._

Here is your corrected text with improved grammar and flow:  

The problem included a small but crucial hint: _there is only a single path from the start to the end_. Moreover, there are no dead ends in the input; it's just a single, continuous trace. 

I created a function that returns the points of the track in from finish to start. This way, the index of an item in the array corresponds to its distance to the finish line. Then, I go over the path. For each position, the number of possible cheats is calculated by checking what happens if we are trying to make a shortcut to any other positions that is closer to the finish line. 

There are a number of cases to consider:
- the target position is too far away. This happens when its Manhattan distance is greater than the allowed _cheat_ limit
- the target is within range, but the saving is less than 100
- the target is within range, and the saving is at least 100

We need to determine the number of good cheats for each position add them up. I used Parallel LINQ here, as the regular sequential one took significantly more time.
