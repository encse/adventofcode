## --- Day 12: Hot Springs ---
If you are not familiar with the problem, you can read i [here](https://adventofcode.com/2023/day/12).

A day of memoized functions / dynamic programming. Each line of the input has some pattern (string) and constraints (numbers). Going over the pattern we need to figure out what should be put in the place of the ? symbols so that it satisfies the constraints on the right. How many ways are to satisfy all conditions?

This cries out for recursion, you can find the details below.

In Part 2 the input is transformed to something bigger with a process
called `unfolding`. It's the same question as before, the sole 
purpose of the unfolding is to force us adding memoization to the 
algorithm we came up with in Part 1.

