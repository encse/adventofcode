## --- Day 7: Bridge Repair ---
The Historians take you to a familiar _rope bridge_ over a river in the middle of a jungle. The Chief isn't on this side of the bridge, though; maybe he's on the other side?

When you go to cross the bridge, you notice a group of engineers trying to repair it. (Apparently, it breaks pretty frequently.) You won't be able to cross until it's fixed.

_Visit the website for the full story and [puzzle](https://adventofcode.com/2024/day/7) description._

It's time to pull out the recursion guns. I introduced a checker logic that goes through the numbers in one line of input and tries all possible operators on the accumulated result to reach the target.

The common logic that parses the input and executes the checker was extracted into a single `Solve` function, but I found it more readable to have distinct checkers for the two parts of the problem. 

Everything runs in about a second, but since it's just a single line, I couldn't stand and added an optimization in `Check2` to exit early when the accumulated result exceeds the target.
