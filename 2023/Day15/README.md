## --- Day 15: Lens Library ---
The newly-focused parabolic reflector dish is sending all of the collected light to a point on the side of yet another mountain - the largest mountain on Lava Island. As you approach the mountain, you find that the light is being collected by the wall of a large facility embedded in the mountainside.

You find a door under a large sign that says "Lava Production Facility" and next to a smaller sign that says "Danger - Personal Protective Equipment required beyond this point".

As you step inside, you are immediately greeted by a somewhat panicked reindeer wearing goggles and a loose-fitting [hard hat](https://en.wikipedia.org/wiki/Hard_hat). The reindeer leads you to a shelf of goggles and hard hats (you quickly find some that fit) and then further into the facility. At one point, you pass a button with a faint snout mark and the label "PUSH FOR HELP". No wonder you were loaded into that [trebuchet](1) so quickly!

Read the [full puzzle](https://adventofcode.com/2023/day/15).

##  --- Notes ---
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
