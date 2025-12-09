## --- Day 9: Movie Theater ---
<em>
You slide down the [firepole](https://en.wikipedia.org/wiki/Fireman%27s_pole) in the corner of the playground and land in the North Pole base movie theater!

The movie theater has a big tile floor with an interesting pattern. Elves here are redecorating the theater by switching out some of the square tiles in the big grid they form. Some of the tiles are <em>red</em>; the Elves would like to find the largest rectangle that uses red tiles for two of its opposite corners. They even have a list of where the red tiles are located in the grid (your puzzle input).

Visit the website for the full story and [full puzzle](https://adventofcode.com/2025/day/9) description.
</em>

This one really caused me a headache. I made a bug in the area function, at the beginning, which was not triggered
in part one. Then came the strugling for hours which overloaded my mind. My head was full of intersection functions and 
ray casting and eye balling for errors, but it just didn't work.

I made a little python script that draws the input, which is a circle with a slot in the middle. Looked really special, 
I started to add heuristics to my solution, but of course this didn't help due to the bogus area function.

After the 200th read, I finally spotted my mistake and got the second star, but I got tired of the whole thing and couldn't 
really put this into a nice form.  

At the end, I started to read the solution thread and spotted that a simple `AabbCollision` function is enough. And 
somehow it clicked. Without the hint, I would have been on the wrong track for a much longer time for sure.

But at least the final result looks good in my opinion.

