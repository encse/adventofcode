## --- Day 13: Claw Contraption ---
Next up: the [lobby](/2020/day/24) of a resort on a tropical island. The Historians take a moment to admire the hexagonal floor tiles before spreading out.
 
Fortunately, it looks like the resort has a new [arcade](https://en.wikipedia.org/wiki/Amusement_arcade)! Maybe you can win some prizes from the [claw machines](https://en.wikipedia.org/wiki/Claw_machine)?

_Visit the website for the full story and [puzzle](https://adventofcode.com/2024/day/13) description._

We got a math problem today. The movements triggered by buttons A, B and the target position P form a linear equation that we can solve for the two unknowns: the number of button presses. Using math terms, if _i_ and _j_ marks the number of button presses, we are to solve `A * i + B * j = P`. This is simple enough to do using [Cramer's rule](https://en.wikipedia.org/wiki/Cramer%27s_rule), especially with two dimensional vectors. In this case the determinats can be computed with a simple cross product. We should not forget about the special cases: A and B can be parallel (didn't occur in my input), and the solution needs to be non negative integer for _i_ and _j_.