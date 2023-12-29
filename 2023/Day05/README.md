## --- Day 5: If You Give A Seed A Fertilizer ---
You take the boat and find the gardener right where you were told he would be: managing a giant "garden" that looks more to you like a farm.

"A water source? Island Island <em>is</em> the water source!" You point out that Snow Island isn't receiving any water.

"Oh, we had to stop the water because we <em>ran out of sand</em> to [filter](https://en.wikipedia.org/wiki/Sand_filter) it with! Can't make snow with dirty water. Don't worry, I'm sure we'll get more sand soon; we only turned off the water a few days... weeks... oh no." His face sinks into a look of horrified realization.

Read the [full puzzle](https://adventofcode.com/2023/day/5).

##  --- Notes ---
A more tricky problem today. We have a set of numbers that need to be transformed by a series of maps, then we need to return the minimum of all outcomes.

This might sound simple, but Part 2 transforms it to a more interesting problem where the inputs are not numbers but ranges. Our maps split the ranges into other ranges, so a lot of special cases and possible off-by-one errors appear on the horizon.

I created a function that deals with just one map but multiple ranges. The ranges are fed into a `queue` and I process them one by one. There are just three possiblites: the map tells nothing about the range, it maps the range to a single range or some _cutting_ is required. The first cases are simply enough, only the last one is interesting. It can happen that our range need to be cut into 2, 3, 4 or even more parts, so this would be hard to handle in a single step. So what I did was simply just make a single cut that generates two new ranges and add these back to the queue. They will be taken care about later. At the end they will be cut enough times so that the parts can be processed by one of the two simple cases above. 

Having a function to do the heavy lifting, I can push all ranges though all the maps easily, then take the minimum of the result.
