## --- Day 4: Ceres Search ---
"Looks like the Chief's not here. Next!" One of The Historians pulls out a device and pushes the only button on it. After a brief flash, you recognize the interior of the __Ceres monitoring station__!
As the search for the Chief continues, a small Elf who lives on the station tugs on your shirt; she'd like to know if you could help her with her <em>word search</em> (your puzzle input). She only has to find one word: <code>XMAS</code>.

Read the [full puzzle](https://adventofcode.com/2024/day/4).

I used my proven tactic and converted the input to a Dictionary keyed by coordinates. It's easy to iterate over the keys and checking if we are in the bounds of the map as well.

Representing the coordinates with Complex numbers is also a useful way to deal with stepping in various directions.

The algorithm itself is simply a bruteforce check of all starting positions and reading orders.
