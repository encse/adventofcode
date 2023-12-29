## --- Day 13: Point of Incidence ---
Those who need a refresh can read the problem [here](https://adventofcode.com/2023/day/13).

A mirror is hidden somewhere in a rectangular board (our input). Some of the _rocks_ in the picture are just reflections. 
The problem doesn't specify if the mirror is put horizontal or vertical, but we know that it's across the
board from one end to the other and it is not necessarly in the middle.

Pretty much following the description, I created a function that tries all possible placements and computes how many errors (smudges - using the problem's terminology) were if the mirror was placed right there. Then just selected the one that had zero errors.

The second half of the problem asked for the very same thing but this time there was one `smudge` in the reflected image.

