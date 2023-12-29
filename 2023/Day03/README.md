## --- Day 3: Gear Ratios ---
Let's revisit the problem description [here](https://adventofcode.com/2023/day/3).

There are multiple ways to footgun ourselves with this one, if somebody is not
careful with the parser. I solved this problem using regular expressions. First I 
searched for all numbers and symbols, then collected those that were 
adjacent to each other.

I did the same trick in Part 2, but now searched for all gear symbols (`*`) and 
filtered out those that are adjacent to exactly two numbers.

Probably the hardest part is to tell if two matches are adjacent or not.
