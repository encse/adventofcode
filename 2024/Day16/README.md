## --- Day 16: Reindeer Maze ---
It's time again for the [Reindeer Olympics](/2015/day/14)! This year, the big event is the <em>Reindeer Maze</em>, where the Reindeer compete for the <em>lowest score</em>.

You and The Historians arrive to search for the Chief right as the event is about to start. It wouldn't hurt to watch a little, right?

_Visit the website for the full story and [full puzzle](https://adventofcode.com/2024/day/16) description._

I spent hell a lot of time on this one. I’m not sure why, because I had a good understanding of what to do for both parts. `Part 1` went reasonably well: I quickly used a priority based approach to find the shortest path from the `start` state to the `goal`.

For `Part 2`, I initially tried a few dead ends, because I overcomplicate things as usual. But I found the right direction after about half an hour. The idea is to split the problem into two halves. First, we compute the optimal distances from every tile and direction to the goal node. This can be found using _Dijskstra's algorithm_.

Once I have the distances, I can start an other round, now working forward from the start position and using a flood-fill-like algorithm to discover the positions on the shortest path. This is easy to do with the distance map as a guide. I maintain the 'remaining score' along the path, and just need to check if the distance from a potential next state equals to the score I still have to use. This logic can be found in the `FindBestSpots` function.