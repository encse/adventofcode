## --- Day 3: Gear Ratios ---
You and the Elf eventually reach a [gondola lift](https://en.wikipedia.org/wiki/Gondola_lift) station; he says the gondola lift will take you up to the <em>water source</em>, but this is as far as he can bring you. You go inside.

It doesn't take long to find the gondolas, but there seems to be a problem: they're not moving.

"Aaah!"

Read the [full puzzle](https://adventofcode.com/2023/day/3).

## --- Notes ---
There are multiple ways to footgun ourselves with this one, if somebody is not
careful with the parser. I solved this problem using regular expressions. First I 
searched for all numbers and symbols, then collected those that were 
adjacent to each other.

I did the same trick in Part 2, but now searched for all gear symbols (`*`) and 
filtered out those that are adjacent to exactly two numbers.

Probably the hardest part is to tell if two matches are adjacent or not.
