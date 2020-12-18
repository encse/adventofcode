original source: [https://adventofcode.com/2019/day/15](https://adventofcode.com/2019/day/15)
## --- Day 15: Oxygen System ---
Out here in deep space, many things can go wrong. Fortunately, many of those things have indicator lights. Unfortunately, one of those lights is lit: the oxygen system for part of the ship has failed!

According to the readouts, the oxygen system must have failed days ago after a rupture in oxygen tank two; that section of the ship was automatically sealed once oxygen levels went dangerously low. A single remotely-operated <em>repair droid</em> is your only option for fixing the oxygen system.

The Elves' care package included an [Intcode](9) program (your puzzle input) that you can use to remotely control the repair droid. By running that program, you can direct the repair droid to the oxygen system and fix the problem.

The remote control program executes the following steps in a loop forever:


 - Accept a <em>movement command</em> via an input instruction.
 - Send the movement command to the repair droid.
 - Wait for the repair droid to finish the movement operation.
 - Report on the <em>status</em> of the repair droid via an output instruction.

Only four <em>movement commands</em> are understood: north (<code>1</code>), south (<code>2</code>), west (<code>3</code>), and east (<code>4</code>). Any other command is invalid. The movements differ in direction, but not in distance: in a long enough east-west hallway, a series of commands like <code>4,4,4,4,3,3,3,3</code> would leave the repair droid back where it started.

The repair droid can reply with any of the following <em>status</em> codes:


 - <code>0</code>: The repair droid hit a wall. Its position has not changed.
 - <code>1</code>: The repair droid has moved one step in the requested direction.
 - <code>2</code>: The repair droid has moved one step in the requested direction; its new position is the location of the oxygen system.

You don't know anything about the area around the repair droid, but you can figure it out by watching the status codes.

For example, we can draw the area using <code>D</code> for the droid, <code>#</code> for walls, <code>.</code> for locations the droid can traverse, and empty space for unexplored locations.  Then, the initial state looks like this:

<pre>
<code>      
      
   D  
      
      
</code>
</pre>

To make the droid go north, send it <code>1</code>. If it replies with <code>0</code>, you know that location is a wall and that the droid didn't move:

<pre>
<code>      
   #  
   D  
      
      
</code>
</pre>

To move east, send <code>4</code>; a reply of <code>1</code> means the movement was successful:

<pre>
<code>      
   #  
   .D 
      
      
</code>
</pre>

Then, perhaps attempts to move north (<code>1</code>), south (<code>2</code>), and east (<code>4</code>) are all met with replies of <code>0</code>:

<pre>
<code>      
   ## 
   .D#
    # 
      
</code>
</pre>

Now, you know the repair droid is in a dead end. Backtrack with <code>3</code> (which you already know will get a reply of <code>1</code> because you already know that location is open):

<pre>
<code>      
   ## 
   D.#
    # 
      
</code>
</pre>

Then, perhaps west (<code>3</code>) gets a reply of <code>0</code>, south (<code>2</code>) gets a reply of <code>1</code>, south again (<code>2</code>) gets a reply of <code>0</code>, and then west (<code>3</code>) gets a reply of <code>2</code>:

<pre>
<code>      
   ## 
  #..#
  D.# 
   #  
</code>
</pre>

Now, because of the reply of <code>2</code>, you know you've found the <em>oxygen system</em>! In this example, it was only <code><em>2</em></code> moves away from the repair droid's starting position.

<em>What is the fewest number of movement commands</em> required to move the repair droid from its starting position to the location of the oxygen system?


## --- Part Two ---
You quickly repair the oxygen system; oxygen gradually fills the area.

Oxygen starts in the location containing the repaired oxygen system. It takes <em>one minute</em> for oxygen to spread to all open locations that are adjacent to a location that already contains oxygen. Diagonal locations are <em>not</em> adjacent.

In the example above, suppose you've used the droid to explore the area fully and have the following map (where locations that currently contain oxygen are marked <code>O</code>):

<pre>
<code> ##   
#..## 
#.#..#
#.O.# 
 ###  
</code>
</pre>

Initially, the only location which contains oxygen is the location of the repaired oxygen system.  However, after one minute, the oxygen spreads to all open (<code>.</code>) locations that are adjacent to a location containing oxygen:

<pre>
<code> ##   
#..## 
#.#..#
#OOO# 
 ###  
</code>
</pre>

After a total of two minutes, the map looks like this:

<pre>
<code> ##   
#..## 
#O#O.#
#OOO# 
 ###  
</code>
</pre>

After a total of three minutes:

<pre>
<code> ##   
#O.## 
#O#OO#
#OOO# 
 ###  
</code>
</pre>

And finally, the whole region is full of oxygen after a total of four minutes:

<pre>
<code> ##   
#OO## 
#O#OO#
#OOO# 
 ###  
</code>
</pre>

So, in this example, all locations contain oxygen after <code><em>4</em></code> minutes.

Use the repair droid to get a complete map of the area. <em>How many minutes will it take to fill with oxygen?</em>


