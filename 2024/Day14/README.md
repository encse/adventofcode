## --- Day 14: Restroom Redoubt ---
One of The Historians needs to use the bathroom; fortunately, you know there's a bathroom near an unvisited location on their list, and so you're all quickly teleported directly to the lobby of Easter Bunny Headquarters.

Unfortunately, EBHQ seems to have "improved" bathroom security <em>again</em> after your last [visit](/2016/day/2). The area outside the bathroom is swarming with robots!

_Visit the website for the full story and [full puzzle](https://adventofcode.com/2024/day/14) description._

A nice simulation challenge for today. `Part 1` was straightforward: iterate 100 times and count the robots in the different quadrants. 

I’d bet many of us anticipated some `least common multiple` or `Chinese Remainder Theorem` magic for `Part 2`, but Eric threw us a curveball by making us search for a Christmas tree pattern in the robot’s movement.

The expected output wasn’t clearly specified — other than the fact that it should resemble a Christmas tree. I wrote a plot function to display the robot’s locations on the screen, dumped everything into a long file, and manually inspected it in my editor.

Later, to automate this process, decided to search for a longer horizontal '####' pattern in the output.
