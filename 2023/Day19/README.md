## --- Day 19: Aplenty ---
Those who need a refresh can read the problem [here](https://adventofcode.com/2023/day/19).

Part 1 is an _implementation_ challenge, where you need to model some virtual 
machine following certain rules. I jumped on that and wrote it while sipping my 
morning coffee. But this is Day 19 and there _has to be a twist_. We got a totally 
different challenge for the second half. It's like opening your calendar and 
finding two chocolates instead of one. Yay!

Part 2 looks frightening first, but not for a seasoned Advent of Coder with
multiple dimension travels behind his back. It's asking for the volume of a 
hypercube. Don't believe? Think about it. We start from a 4000 x 4000 x 4000 x 4000
cube and slice it up to smaller parts based on the conditions we are given. 
(It becomes more of a hyperectangle during this process, but let's not be picky 
about names.) At the end we get to some smallish cubes which are either 
_Accepted_ or fully _Rejected_. This algorithm is a bit 
similar to what we did in Day 5 and I tried to code it like that.

Just follow the instructions precisely and Part 2 is tamed. To clean things up a
bit, we can even reuse this to implement Part 1. (So much about our nice 
interpreter from the morning.) Go over the list of parts as they were tiny 
1 x 1 x 1 x 1 cubes and check if they are accepted or not.

The code is the longest I've written so far, but it's hopefully readable
after this introduction. 

