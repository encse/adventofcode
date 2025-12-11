## --- Day 10: Factory ---

*Just across the hall, you find a large factory. Fortunately, the Elves here have plenty of time to decorate. Unfortunately, it’s because the factory machines are all offline, and none of the Elves can figure out the initialization procedure.*

*The Elves do have the manual for the machines, but the section detailing the initialization procedure was eaten by a  requirements for each machine.*

*Visit the website for the full story and the [full puzzle](https://adventofcode.com/2025/day/10) description.*

The first half of the problem was totally fine: we just had to find the combination with the minimal number of button presses.

However, Part 2 was, in my view, against the spirit of Advent of Code.

It’s clearly a linear algebra problem. The buttons form the **matrix A**. Each button gets a column in **A** with zeros and ones corresponding to the bits of the button.

The desired joltage becomes the vector **b**.

Then one needs to solve **Ax = b** such that the components of **x** are non-negative integers and **Σ xᵢ** is minimal.

This is a textbook integer linear programming problem, trivially solved by an ILP solver, or in [my case](solve.py) z3. But this is not how Advent of Code has worked in the last 10+ years, so I was hoping for some shortcut, maybe a special structure of the input, but no luck. This really seems to be the intended way.

I did find another path, though, which is not that good-looking. One can notice that **A** is almost always full rank, and the kernel space has at most 2–3 dimensions.

This means we can solve the equation using Gaussian elimination, which will give us a solution with the columns in the kernel set to 0. There is no guarantee that the solution is integer, or that all xᵢ are non-negative, though.

But once the columns in the base and the kernel are identified, we form a square matrix **B** and move the remaining columns to **K**, then deal with **Bx = b − Ky**.

Say that **K** has 3 columns. Now we can start brute-forcing by setting the values in **y** to some low integers: [1,0,0], [0,1,0], [0,0,1], then [1,1,0], [1,0,1], [0,1,1], [2,0,0], [0,2,0], [0,0,2], slowly increasing the sum of the values and solving for each combination.

This way we eventually get a feasible solution for the original problem, say with the total button-press count equal to 100 or so. Then it’s enough to continue the above iteration while the sum of yᵢ is ≤ 100. That gives us a termination condition. During the search we might run into better solutions, which further lowers the upper bound.

I [prototyped this idea](gauss.py) in Python with ChatGPT. I think, if anything, this could be ported to C#, but I don’t feel the urge. Although it uses first principles that one could potentially implement, it’s a much slower solution than the one using z3.
