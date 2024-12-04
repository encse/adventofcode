## --- Day 4: Ceres Search ---
"Looks like the Chief's not here. Next!" One of The Historians pulls out a device and pushes the only button on it. After a brief flash, you recognize the interior of the __Ceres monitoring station__!
As the search for the Chief continues, a small Elf who lives on the station tugs on your shirt; she'd like to know if you could help her with her <em>word search</em>. She only has to find one word: <code>XMAS</code>.

Read the [full puzzle](https://adventofcode.com/2024/day/4).

I employed my proven tactic of converting the input into a dictionary, using coordinates as keys. This approach makes it straightforward to iterate over the keys and check whether they fall within the bounds of the map.

Representing coordinates with complex numbers is another effective technique for handling steps in various directions.

The algorithm itself is a straightforward brute-force check of all starting positions and reading orders.
