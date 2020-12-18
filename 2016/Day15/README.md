original source: [https://adventofcode.com/2016/day/15](https://adventofcode.com/2016/day/15)
## --- Day 15: Timing is Everything ---
The halls open into an interior plaza containing a large kinetic sculpture. The sculpture is in a sealed enclosure and seems to involve a set of identical spherical capsules that are carried to the top and allowed to [bounce through the maze](https://youtu.be/IxDoO9oODOk?t=177) of spinning pieces.

Part of the sculpture is even interactive! When a button is pressed, a capsule is dropped and tries to fall through slots in a set of rotating discs to finally go through a little hole at the bottom and come out of the sculpture. If any of the slots aren't aligned with the capsule as it passes, the capsule bounces off the disc and soars away. You feel compelled to get one of those capsules.

The discs pause their motion each second and come in different sizes; they seem to each have a fixed number of positions at which they stop.  You decide to call the position with the slot <code>0</code>, and count up for each position it reaches next.

Furthermore, the discs are spaced out so that after you push the button, one second elapses before the first disc is reached, and one second elapses as the capsule passes from one disc to the one below it.  So, if you push the button at <code>time=100</code>, then the capsule reaches the top disc at <code>time=101</code>, the second disc at <code>time=102</code>, the third disc at <code>time=103</code>, and so on.

The button will only drop a capsule at an integer time - no fractional seconds allowed.

For example, at <code>time=0</code>, suppose you see the following arrangement:

<pre>
<code>Disc #1 has 5 positions; at time=0, it is at position 4.
Disc #2 has 2 positions; at time=0, it is at position 1.
</code>
</pre>

If you press the button exactly at <code>time=0</code>, the capsule would start to fall; it would reach the first disc at <code>time=1</code>. Since the first disc was at position <code>4</code> at <code>time=0</code>, by <code>time=1</code> it has ticked one position forward.  As a five-position disc, the next position is <code>0</code>, and the capsule falls through the slot.

Then, at <code>time=2</code>, the capsule reaches the second disc. The second disc has ticked forward two positions at this point: it started at position <code>1</code>, then continued to position <code>0</code>, and finally ended up at position <code>1</code> again.  Because there's only a slot at position <code>0</code>, the capsule bounces away.

If, however, you wait until <code>time=5</code> to push the button, then when the capsule reaches each disc, the first disc will have ticked forward <code>5+1 = 6</code> times (to position <code>0</code>), and the second disc will have ticked forward <code>5+2 = 7</code> times (also to position <code>0</code>). In this case, the capsule would fall through the discs and come out of the machine.

However, your situation has more than two discs; you've noted their positions in your puzzle input. What is the <em>first time you can press the button</em> to get a capsule?


## --- Part Two ---
After getting the first capsule (it contained a star! what great fortune!), the machine detects your success and begins to rearrange itself.

When it's done, the discs are back in their original configuration as if it were <code>time=0</code> again, but a new disc with <code>11</code> positions and starting at position <code>0</code> has appeared exactly one second below the previously-bottom disc.

With this new disc, and counting again starting from <code>time=0</code> with the configuration in your puzzle input, what is the <em>first time you can press the button</em> to get another capsule?


