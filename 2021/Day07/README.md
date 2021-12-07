original source: [https://adventofcode.com/2021/day/7](https://adventofcode.com/2021/day/7)
## --- Day 7: The Treachery of Whales ---
A giant [whale](https://en.wikipedia.org/wiki/Sperm_whale) has decided your submarine is its next meal, and it's much faster than you are. There's nowhere to run!

Suddenly, a swarm of crabs (each in its own tiny submarine - it's too deep for them otherwise) zooms in to rescue you! They seem to be preparing to blast a hole in the ocean floor; sensors indicate a <em>massive underground cave system</em> just beyond where they're aiming!

The crab submarines all need to be aligned before they'll have enough power to blast a large enough hole for your submarine to get through. However, it doesn't look like they'll be aligned before the whale catches you! Maybe you can help?

There's one major catch - crab submarines can only move horizontally.

You quickly make a list of <em>the horizontal position of each crab</em> (your puzzle input). Crab submarines have limited fuel, so you need to find a way to make all of their horizontal positions match while requiring them to spend as little fuel as possible.

For example, consider the following horizontal positions:

<pre>
<code>16,1,2,0,4,2,7,1,2,14</code>
</pre>

This means there's a crab with horizontal position <code>16</code>, a crab with horizontal position <code>1</code>, and so on.

Each change of 1 step in horizontal position of a single crab costs 1 fuel. You could choose any horizontal position to align them all on, but the one that costs the least fuel is horizontal position <code>2</code>:


 - Move from <code>16</code> to <code>2</code>: <code>14</code> fuel
 - Move from <code>1</code> to <code>2</code>: <code>1</code> fuel
 - Move from <code>2</code> to <code>2</code>: <code>0</code> fuel
 - Move from <code>0</code> to <code>2</code>: <code>2</code> fuel
 - Move from <code>4</code> to <code>2</code>: <code>2</code> fuel
 - Move from <code>2</code> to <code>2</code>: <code>0</code> fuel
 - Move from <code>7</code> to <code>2</code>: <code>5</code> fuel
 - Move from <code>1</code> to <code>2</code>: <code>1</code> fuel
 - Move from <code>2</code> to <code>2</code>: <code>0</code> fuel
 - Move from <code>14</code> to <code>2</code>: <code>12</code> fuel

This costs a total of <code><em>37</em></code> fuel. This is the cheapest possible outcome; more expensive outcomes include aligning at position <code>1</code> (<code>41</code> fuel), position <code>3</code> (<code>39</code> fuel), or position <code>10</code> (<code>71</code> fuel).

Determine the horizontal position that the crabs can align to using the least fuel possible. <em>How much fuel must they spend to align to that position?</em>


## --- Part Two ---
The crabs don't seem interested in your proposed solution. Perhaps you misunderstand crab engineering?

As it turns out, crab submarine engines don't burn fuel at a constant rate. Instead, each change of 1 step in horizontal position costs 1 more unit of fuel than the last: the first step costs <code>1</code>, the second step costs <code>2</code>, the third step costs <code>3</code>, and so on.

As each crab moves, moving further becomes more expensive. This changes the best horizontal position to align them all on; in the example above, this becomes <code>5</code>:


 - Move from <code>16</code> to <code>5</code>: <code>66</code> fuel
 - Move from <code>1</code> to <code>5</code>: <code>10</code> fuel
 - Move from <code>2</code> to <code>5</code>: <code>6</code> fuel
 - Move from <code>0</code> to <code>5</code>: <code>15</code> fuel
 - Move from <code>4</code> to <code>5</code>: <code>1</code> fuel
 - Move from <code>2</code> to <code>5</code>: <code>6</code> fuel
 - Move from <code>7</code> to <code>5</code>: <code>3</code> fuel
 - Move from <code>1</code> to <code>5</code>: <code>10</code> fuel
 - Move from <code>2</code> to <code>5</code>: <code>6</code> fuel
 - Move from <code>14</code> to <code>5</code>: <code>45</code> fuel

This costs a total of <code><em>168</em></code> fuel. This is the new cheapest possible outcome; the old alignment position (<code>2</code>) now costs <code>206</code> fuel instead.

Determine the horizontal position that the crabs can align to using the least fuel possible so they can make you an escape route! <em>How much fuel must they spend to align to that position?</em>


