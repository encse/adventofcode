## --- Day 8: Haunted Wasteland ---
The task description is copyrighted, but it's available [here](https://adventofcode.com/2023/day/8).

We need to implement some process that is called _wandering around the desert_ and it's essentially a 
series of dictionary lookups that lead to other dictionary lookups. 

Pretty dry as I'm writing it down, but the point is that after some iterations we get from AAA to ZZZ. 
Part 1 asks for the number of steps needed for that.

Part 2 gives it a spin and is asking us to start from all nodes that end with the letter A at once, 
and continue _wandering around_ in parallel until we reach Z nodes simultanously in every path!

Obviously, this would take ages to wait out, but fortunately the input is specially crafted so that we can take the least common multiplier of the length of the individual loops and return just that.
