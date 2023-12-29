## --- Day 17: Clumsy Crucible ---
The lava starts flowing rapidly once the Lava Production Facility is operational. As you leave, the reindeer offers you a parachute, allowing you to quickly reach Gear Island.

As you descend, your bird's-eye view of Gear Island reveals why you had trouble finding anyone on your way up: half of Gear Island is empty, but the half below you is a giant factory city!

You land near the gradually-filling pool of lava at the base of your new <em>lavafall</em>. Lavaducts will eventually carry the lava throughout the city, but to make use of it immediately, Elves are loading it into large [crucibles](https://en.wikipedia.org/wiki/Crucible) on wheels.

Read the [full puzzle](https://adventofcode.com/2023/day/17).

##  --- Notes ---
Part 1 and Part 2 differ only in the rules for the small and ultra crucibles. And it turns 
out those can be represented by just two integers: one for the minimum steps the crucible 
needs to move forward before it can make a turn (or stop), and an other one that puts an 
upper limit on the distance it can go in a straight line.

The algorithmic part is a pretty standard graph search implemented with a priority queue.
If you've seen one, you've seen them all. We are starting from the top left corner with the 
only `goal` state in the bottom right. Since we are minimizing for heatloss, we can use 
that as the priority of the queue items.
