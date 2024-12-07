## --- Day 7: Bridge Repair ---
The Historians take you to a familiar [rope bridge](/2022/day/9) over a river in the middle of a jungle. The Chief isn't on this side of the bridge, though; maybe he's on the other side?

When you go to cross the bridge, you notice a group of engineers trying to repair it. (Apparently, it breaks pretty frequently.) You won't be able to cross until it's fixed.

You ask how long it'll take; the engineers tell you that it only needs final calibrations, but some young elephants were playing nearby and <em>stole all the operators</em> from their calibration equations! They could finish the calibrations if only someone could determine which test values could possibly be produced by placing any combination of operators into their calibration equations (your puzzle input).

Read the [full puzzle](https://adventofcode.com/2024/day/7).

It's time to pull out the recursion guns. I introduced a checker logic that goes through the numbers in one line of input and tries all possible operators on the accumulated result to reach the target.

The common logic that parses the input and executes the checker was extracted into a single `Solve` function, but I found it more readable to have distinct checkers for the two parts of the problem. 

Everything runs in about a second, but since it's just a single line, I couldn't stand and added an optimization in `Check2` to exit early when the accumulated result exceeds the target.
