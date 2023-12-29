## --- Day 17: Clumsy Crucible ---
Visit the Advent of Code website for the problem statement [here](https://adventofcode.com/2023/day/17).

Part 1 and Part 2 differ only in the rules for the small and ultra crucibles. And it turns 
out those can be represented by just two integers: one for the minimum steps the crucible 
needs to move forward before it can make a turn (or stop), and an other one that puts an 
upper limit on the distance it can go in a straight line.

The algorithmic part is a pretty standard graph search implemented with a priority queue.
If you've seen one, you've seen them all. We are starting from the top left corner with the 
only `goal` state in the bottom right. Since we are minimizing for heatloss, we can use 
that as the priority of the queue items.
