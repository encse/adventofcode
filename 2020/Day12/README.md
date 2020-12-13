original source: [https://adventofcode.com/2020/day/12](https://adventofcode.com/2020/day/12)
## --- Day 12: Rain Risk ---
Your ferry made decent progress toward the island, but the storm came in faster than anyone expected. The ferry needs to take <em>evasive actions</em>!

Unfortunately, the ship's navigation computer seems to be malfunctioning; rather than giving a route directly to safety, it produced extremely circuitous instructions. When the captain uses the [PA system](https://en.wikipedia.org/wiki/Public_address_system) to ask if anyone can help, you quickly volunteer.

The navigation instructions (your puzzle input) consists of a sequence of single-character <em>actions</em> paired with integer input <em>values</em>. After staring at them for a few minutes, you work out what they probably mean:


 - Action <em><code>N</code></em> means to move <em>north</em> by the given value.
 - Action <em><code>S</code></em> means to move <em>south</em> by the given value.
 - Action <em><code>E</code></em> means to move <em>east</em> by the given value.
 - Action <em><code>W</code></em> means to move <em>west</em> by the given value.
 - Action <em><code>L</code></em> means to turn <em>left</em> the given number of degrees.
 - Action <em><code>R</code></em> means to turn <em>right</em> the given number of degrees.
 - Action <em><code>F</code></em> means to move <em>forward</em> by the given value in the direction the ship is currently facing.

The ship starts by facing <em>east</em>. Only the <code>L</code> and <code>R</code> actions change the direction the ship is facing. (That is, if the ship is facing east and the next instruction is <code>N10</code>, the ship would move north 10 units, but would still move east if the following action were <code>F</code>.)

For example:

<pre>
<code>F10
N3
F7
R90
F11
</code>
</pre>

These instructions would be handled as follows:


 - <code>F10</code> would move the ship 10 units east (because the ship starts by facing east) to <em>east 10, north 0</em>.
 - <code>N3</code> would move the ship 3 units north to <em>east 10, north 3</em>.
 - <code>F7</code> would move the ship another 7 units east (because the ship is still facing east) to <em>east 17, north 3</em>.
 - <code>R90</code> would cause the ship to turn right by 90 degrees and face <em>south</em>; it remains at <em>east 17, north 3</em>.
 - <code>F11</code> would move the ship 11 units south to <em>east 17, south 8</em>.

At the end of these instructions, the ship's [Manhattan distance](https://en.wikipedia.org/wiki/Manhattan_distance) (sum of the absolute values of its east/west position and its north/south position) from its starting position is <code>17 + 8</code> = <em><code>25</code></em>.

Figure out where the navigation instructions lead. <em>What is the Manhattan distance between that location and the ship's starting position?</em>


## --- Part Two ---
Before you can give the destination to the captain, you realize that the actual action meanings were printed on the back of the instructions the whole time.

Almost all of the actions indicate how to move a <em>waypoint</em> which is relative to the ship's position:


 - Action <em><code>N</code></em> means to move the waypoint <em>north</em> by the given value.
 - Action <em><code>S</code></em> means to move the waypoint <em>south</em> by the given value.
 - Action <em><code>E</code></em> means to move the waypoint <em>east</em> by the given value.
 - Action <em><code>W</code></em> means to move the waypoint <em>west</em> by the given value.
 - Action <em><code>L</code></em> means to rotate the waypoint around the ship <em>left</em> (<em>counter-clockwise</em>) the given number of degrees.
 - Action <em><code>R</code></em> means to rotate the waypoint around the ship <em>right</em> (<em>clockwise</em>) the given number of degrees.
 - Action <em><code>F</code></em> means to move <em>forward</em> to the waypoint a number of times equal to the given value.

The waypoint starts <em>10 units east and 1 unit north</em> relative to the ship. The waypoint is relative to the ship; that is, if the ship moves, the waypoint moves with it.

For example, using the same instructions as above:


 - <code>F10</code> moves the ship to the waypoint 10 times (a total of <em>100 units east and 10 units north</em>), leaving the ship at <em>east 100, north 10</em>. The waypoint stays 10 units east and 1 unit north of the ship.
 - <code>N3</code> moves the waypoint 3 units north to <em>10 units east and 4 units north of the ship</em>. The ship remains at <em>east 100, north 10</em>.
 - <code>F7</code> moves the ship to the waypoint 7 times (a total of <em>70 units east and 28 units north</em>), leaving the ship at <em>east 170, north 38</em>. The waypoint stays 10 units east and 4 units north of the ship.
 - <code>R90</code> rotates the waypoint around the ship clockwise 90 degrees, moving it to <em>4 units east and 10 units south of the ship</em>. The ship remains at <em>east 170, north 38</em>.
 - <code>F11</code> moves the ship to the waypoint 11 times (a total of <em>44 units east and 110 units south</em>), leaving the ship at <em>east 214, south 72</em>. The waypoint stays 4 units east and 10 units south of the ship.

After these operations, the ship's Manhattan distance from its starting position is <code>214 + 72</code> = <em><code>286</code></em>.

Figure out where the navigation instructions actually lead. <em>What is the Manhattan distance between that location and the ship's starting position?</em>


