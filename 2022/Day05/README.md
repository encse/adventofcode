original source: [https://adventofcode.com/2022/day/5](https://adventofcode.com/2022/day/5)
## --- Day 5: Supply Stacks ---
The expedition can depart as soon as the final supplies have been unloaded from the ships. Supplies are stored in stacks of marked <em>crates</em>, but because the needed supplies are buried under many other crates, the crates need to be rearranged.

The ship has a <em>giant cargo crane</em> capable of moving crates between stacks. To ensure none of the crates get crushed or fall over, the crane operator will rearrange them in a series of carefully-planned steps. After the crates are rearranged, the desired crates will be at the top of each stack.

The Elves don't want to interrupt the crane operator during this delicate procedure, but they forgot to ask her <em>which</em> crate will end up where, and they want to be ready to unload them as soon as possible so they can embark.

They do, however, have a drawing of the starting stacks of crates <em>and</em> the rearrangement procedure (your puzzle input). For example:

<pre>
<code>    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2
</code>
</pre>

In this example, there are three stacks of crates. Stack 1 contains two crates: crate <code>Z</code> is on the bottom, and crate <code>N</code> is on top. Stack 2 contains three crates; from bottom to top, they are crates <code>M</code>, <code>C</code>, and <code>D</code>. Finally, stack 3 contains a single crate, <code>P</code>.

Then, the rearrangement procedure is given. In each step of the procedure, a quantity of crates is moved from one stack to a different stack. In the first step of the above rearrangement procedure, one crate is moved from stack 2 to stack 1, resulting in this configuration:

<pre>
<code>[D]        
[N] [C]    
[Z] [M] [P]
 1   2   3 
</code>
</pre>

In the second step, three crates are moved from stack 1 to stack 3. Crates are moved <em>one at a time</em>, so the first crate to be moved (<code>D</code>) ends up below the second and third crates:

<pre>
<code>        [Z]
        [N]
    [C] [D]
    [M] [P]
 1   2   3
</code>
</pre>

Then, both crates are moved from stack 2 to stack 1. Again, because crates are moved <em>one at a time</em>, crate <code>C</code> ends up below crate <code>M</code>:

<pre>
<code>        [Z]
        [N]
[M]     [D]
[C]     [P]
 1   2   3
</code>
</pre>

Finally, one crate is moved from stack 1 to stack 2:

<pre>
<code>        [<em>Z</em>]
        [N]
        [D]
[<em>C</em>] [<em>M</em>] [P]
 1   2   3
</code>
</pre>

The Elves just need to know <em>which crate will end up on top of each stack</em>; in this example, the top crates are <code>C</code> in stack 1, <code>M</code> in stack 2, and <code>Z</code> in stack 3, so you should combine these together and give the Elves the message <code><em>CMZ</em></code>.

<em>After the rearrangement procedure completes, what crate ends up on top of each stack?</em>


## --- Part Two ---
As you watch the crane operator expertly rearrange the crates, you notice the process isn't following your prediction.

Some mud was covering the writing on the side of the crane, and you quickly wipe it away. The crane isn't a CrateMover 9000 - it's a <em>CrateMover 9001</em>.

The CrateMover 9001 is notable for many new and exciting features: air conditioning, leather seats, an extra cup holder, and <em>the ability to pick up and move multiple crates at once</em>.

Again considering the example above, the crates begin in the same configuration:

<pre>
<code>    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 
</code>
</pre>

Moving a single crate from stack 2 to stack 1 behaves the same as before:

<pre>
<code>[D]        
[N] [C]    
[Z] [M] [P]
 1   2   3 
</code>
</pre>

However, the action of moving three crates from stack 1 to stack 3 means that those three moved crates <em>stay in the same order</em>, resulting in this new configuration:

<pre>
<code>        [D]
        [N]
    [C] [Z]
    [M] [P]
 1   2   3
</code>
</pre>

Next, as both crates are moved from stack 2 to stack 1, they <em>retain their order</em> as well:

<pre>
<code>        [D]
        [N]
[C]     [Z]
[M]     [P]
 1   2   3
</code>
</pre>

Finally, a single crate is still moved from stack 1 to stack 2, but now it's crate <code>C</code> that gets moved:

<pre>
<code>        [<em>D</em>]
        [N]
        [Z]
[<em>M</em>] [<em>C</em>] [P]
 1   2   3
</code>
</pre>

In this example, the CrateMover 9001 has put the crates in a totally different order: <code><em>MCD</em></code>.

Before the rearrangement process finishes, update your simulation so that the Elves know where they should stand to be ready to unload the final supplies. <em>After the rearrangement procedure completes, what crate ends up on top of each stack?</em>


