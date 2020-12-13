original source: [https://adventofcode.com/2020/day/11](https://adventofcode.com/2020/day/11)
## --- Day 11: Seating System ---
Your plane lands with plenty of time to spare. The final leg of your journey is a ferry that goes directly to the tropical island where you can finally start your vacation. As you reach the waiting area to board the ferry, you realize you're so early, nobody else has even arrived yet!

By modeling the process people use to choose (or abandon) their seat in the waiting area, you're pretty sure you can predict the best place to sit. You make a quick map of the seat layout (your puzzle input).

The seat layout fits neatly on a grid. Each position is either floor (<code>.</code>), an empty seat (<code>L</code>), or an occupied seat (<code>#</code>). For example, the initial seat layout might look like this:

<pre>
<code>L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL
</code>
</pre>

Now, you just need to model the people who will be arriving shortly. Fortunately, people are entirely predictable and always follow a simple set of rules. All decisions are based on the <em>number of occupied seats</em> adjacent to a given seat (one of the eight positions immediately up, down, left, right, or diagonal from the seat). The following rules are applied to every seat simultaneously:


 - If a seat is <em>empty</em> (<code>L</code>) and there are <em>no</em> occupied seats adjacent to it, the seat becomes <em>occupied</em>.
 - If a seat is <em>occupied</em> (<code>#</code>) and <em>four or more</em> seats adjacent to it are also occupied, the seat becomes <em>empty</em>.
 - Otherwise, the seat's state does not change.

Floor (<code>.</code>) never changes; seats don't move, and nobody sits on the floor.

After one round of these rules, every seat in the example layout becomes occupied:

<pre>
<code>#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##
</code>
</pre>

After a second round, the seats with four or more occupied adjacent seats become empty again:

<pre>
<code>#.LL.L#.##
#LLLLLL.L#
L.L.L..L..
#LLL.LL.L#
#.LL.LL.LL
#.LLLL#.##
..L.L.....
#LLLLLLLL#
#.LLLLLL.L
#.#LLLL.##
</code>
</pre>

This process continues for three more rounds:

<pre>
<code>#.##.L#.##
#L###LL.L#
L.#.#..#..
#L##.##.L#
#.##.LL.LL
#.###L#.##
..#.#.....
#L######L#
#.LL###L.L
#.#L###.##
</code>
</pre>

<pre>
<code>#.#L.L#.##
#LLL#LL.L#
L.L.L..#..
#LLL.##.L#
#.LL.LL.LL
#.LL#L#.##
..L.L.....
#L#LLLL#L#
#.LLLLLL.L
#.#L#L#.##
</code>
</pre>

<pre>
<code>#.#L.L#.##
#LLL#LL.L#
L.#.L..#..
#L##.##.L#
#.#L.LL.LL
#.#L#L#.##
..L.L.....
#L#L##L#L#
#.LLLLLL.L
#.#L#L#.##
</code>
</pre>

At this point, something interesting happens: the chaos stabilizes and further applications of these rules cause no seats to change state! Once people stop moving around, you count <em><code>37</code></em> occupied seats.

Simulate your seating area by applying the seating rules repeatedly until no seats change state. <em>How many seats end up occupied?</em>


## --- Part Two ---
As soon as people start to arrive, you realize your mistake. People don't just care about adjacent seats - they care about <em>the first seat they can see</em> in each of those eight directions!

Now, instead of considering just the eight immediately adjacent seats, consider the <em>first seat</em> in each of those eight directions. For example, the empty seat below would see <em>eight</em> occupied seats:

<pre>
<code>.......#.
...#.....
.#.......
.........
..#L....#
....#....
.........
#........
...#.....
</code>
</pre>

The leftmost empty seat below would only see <em>one</em> empty seat, but cannot see any of the occupied ones:

<pre>
<code>.............
.L.L.#.#.#.#.
.............
</code>
</pre>

The empty seat below would see <em>no</em> occupied seats:

<pre>
<code>.##.##.
#.#.#.#
##...##
...L...
##...##
#.#.#.#
.##.##.
</code>
</pre>

Also, people seem to be more tolerant than you expected: it now takes <em>five or more</em> visible occupied seats for an occupied seat to become empty (rather than <em>four or more</em> from the previous rules). The other rules still apply: empty seats that see no occupied seats become occupied, seats matching no rule don't change, and floor never changes.

Given the same starting layout as above, these new rules cause the seating area to shift around as follows:

<pre>
<code>L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL
</code>
</pre>

<pre>
<code>#.##.##.##
#######.##
#.#.#..#..
####.##.##
#.##.##.##
#.#####.##
..#.#.....
##########
#.######.#
#.#####.##
</code>
</pre>

<pre>
<code>#.LL.LL.L#
#LLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLL#
#.LLLLLL.L
#.LLLLL.L#
</code>
</pre>

<pre>
<code>#.L#.##.L#
#L#####.LL
L.#.#..#..
##L#.##.##
#.##.#L.##
#.#####.#L
..#.#.....
LLL####LL#
#.L#####.L
#.L####.L#
</code>
</pre>

<pre>
<code>#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##LL.LL.L#
L.LL.LL.L#
#.LLLLL.LL
..L.L.....
LLLLLLLLL#
#.LLLLL#.L
#.L#LL#.L#
</code>
</pre>

<pre>
<code>#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##L#.#L.L#
L.L#.#L.L#
#.L####.LL
..#.#.....
LLL###LLL#
#.LLLLL#.L
#.L#LL#.L#
</code>
</pre>

<pre>
<code>#.L#.L#.L#
#LLLLLL.LL
L.L.L..#..
##L#.#L.L#
L.L#.LL.L#
#.LLLL#.LL
..#.L.....
LLL###LLL#
#.LLLLL#.L
#.L#LL#.L#
</code>
</pre>

Again, at this point, people stop shifting around and the seating area reaches equilibrium. Once this occurs, you count <em><code>26</code></em> occupied seats.

Given the new visibility method and the rule change for occupied seats becoming empty, once equilibrium is reached, <em>how many seats end up occupied?</em>


