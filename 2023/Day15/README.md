## --- Day 15: Lens Library ---
Let's revisit the problem description [here](https://adventofcode.com/2023/day/15).

Part 1 was super simple. What's funny is that I saw a similar hash algorithm yesterday
in someone else's solution, where he stored the hashes of the visited states instead
of serializing it as a whole.

For the second part, I created a function that applies one statement to
an array of boxes at hand. The signature is set up so that I can use it to Aggregate 
all steps seeded with an initial set of 256 empty boxes. We are transforming boxes
to boxes while applying the steps in a row. The function passed as the third argument 
is used to extract the computing power from the final state of the box array.

I'm describing it in a functional way, but there is no purity under the hood. We 
are working with and modifying the same box objects during the process.
