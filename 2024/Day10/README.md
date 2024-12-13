## --- Day 10: Hoof It ---
You all arrive at a _Lava Production Facility_ on a floating island in the sky. As the others begin to search the massive industrial complex, you feel a small nose boop your leg and look down to discover a reindeer wearing a hard hat.

The reindeer is holding a book titled "Lava Island Hiking Guide". However, when you open the book, you discover that most of it seems to have been scorched by lava! As you're about to ask how you can help, the reindeer brings you a blank [topographic map](https://en.wikipedia.org/wiki/Topographic_map) of the surrounding area (your puzzle input) and looks up at you excitedly.

_Visit the website for the full story and [puzzle](https://adventofcode.com/2024/day/10) description._

Today's problem is surprisingly straightforward compared to yesterday's pointer juggling. We finally get to use our favorite queue data structure to implement a flood fill. I saw this coming...  

As usual, we use a dictionary with complex numbers to parse the input. The meat of the solution is in `GetTrailsFrom`, which returns all trails starting at a specific trailhead. 

The difference between `Part 1` and `Part 2` lies in how distinct trails are defined. I decided to return all trails keyed by their trailheads in `GetAllTrails` and delegate the distinctness logic to `PartOne` and `PartTwo`.

A nice and easy task for today!
