## --- Day 6: Wait For It ---
The ferry quickly brings you across Island Island. After asking around, you discover that there is indeed normally a large pile of sand somewhere near here, but you don't see anything besides lots of water and the small island where the ferry has docked.

As you try to figure out what to do next, you notice a poster on a wall near the ferry dock. "Boat races! Open to the public! Grand prize is an all-expenses-paid trip to <em>Desert Island</em>!" That must be where the sand comes from! Best of all, the boat races are starting in just a few minutes.

You manage to sign up as a competitor in the boat races just in time. The organizer explains that it's not really a traditional race - instead, you will get a fixed amount of time during which your boat has to travel as far as it can, and you win if your boat goes the farthest.

Read the [full puzzle](https://adventofcode.com/2023/day/6).

##  --- Notes ---
This has been a simple problem, could be solved even with brute forcing, but I went the 
maths path and implemented a quadratic equation solver instead. 

It's easy to compute how far our boat moves if we wait for `x` ms at the beginning. 
The solution to this equation tells us the `x`-es for which we break the record distance.

Part 2 is just Part 1 with bigger numbers.
