## --- Day 4: Scratchcards ---
The original problem description is available [here](https://adventofcode.com/2023/day/4).

An other day! In Part 1 we need to determine how many _winning_ numbers we have 
in a scratch card, this is simple enough. 

Part 2 can be treated in a very-very bad way if somebody doesn't notice that it's
nothing else but a loop!. We start with a single card #1 and see how many 
cards we are winning, this will generate some new cards of id 2, 3 and so. Then move 
to card(s) #2 and continue this process until the end of the list. Return 
the number of all cards we dealt with.
