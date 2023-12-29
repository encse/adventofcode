## --- Day 5: If You Give A Seed A Fertilizer ---
Visit the Advent of Code website for the problem statement [here](https://adventofcode.com/2023/day/5).

A more tricky problem today. We have a set of numbers that need to be transformed by a series of maps, then we need to return the minimum of all outcomes.

This might sound simple, but Part 2 transforms it to a more interesting problem where the inputs are not numbers but ranges. Our maps split the ranges into other ranges, so a lot of special cases and possible off-by-one errors appear on the horizon.

I created a function that deals with just one map but multiple ranges. The ranges are fed into a `queue` and I process them one by one. There are just three possiblites: the map tells nothing about the range, it maps the range to a single range or some _cutting_ is required. The first cases are simply enough, only the last one is interesting. It can happen that our range need to be cut into 2, 3, 4 or even more parts, so this would be hard to handle in a single step. So what I did was simply just make a single cut that generates two new ranges and add these back to the queue. They will be taken care about later. At the end they will be cut enough times so that the parts can be processed by one of the two simple cases above. 

Having a function to do the heavy lifting, I can push all ranges though all the maps easily, then take the minimum of the result.
