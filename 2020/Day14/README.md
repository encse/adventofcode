original source: [https://adventofcode.com/2020/day/14](https://adventofcode.com/2020/day/14)
## --- Day 14: Docking Data ---
As your ferry approaches the sea port, the captain asks for your help again. The computer system that runs this port isn't compatible with the docking program on the ferry, so the docking parameters aren't being correctly initialized in the docking program's memory.

After a brief inspection, you discover that the sea port's computer system uses a strange [bitmask](https://en.wikipedia.org/wiki/Mask_(computing)) system in its initialization program. Although you don't have the correct decoder chip handy, you can emulate it in software!

The initialization program (your puzzle input) can either update the bitmask or write a value to memory.  Values and memory addresses are both 36-bit unsigned integers.  For example, ignoring bitmasks for a moment, a line like <code>mem[8] = 11</code> would write the value <code>11</code> to memory address <code>8</code>.

The bitmask is always given as a string of 36 bits, written with the most significant bit (representing <code>2^35</code>) on the left and the least significant bit (<code>2^0</code>, that is, the <code>1</code>s bit) on the right. The current bitmask is applied to values immediately before they are written to memory: a <code>0</code> or <code>1</code> overwrites the corresponding bit in the value, while an <code>X</code> leaves the bit in the value unchanged.

For example, consider the following program:

<pre>
<code>mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0
</code>
</pre>

This program starts by specifying a bitmask (<code>mask = ....</code>). The mask it specifies will overwrite two bits in every written value: the <code>2</code>s bit is overwritten with <code>0</code>, and the <code>64</code>s bit is overwritten with <code>1</code>.

The program then attempts to write the value <code>11</code> to memory address <code>8</code>. By expanding everything out to individual bits, the mask is applied as follows:

<pre>
<code>value:  000000000000000000000000000000001011  (decimal 11)
mask:   XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
result: 00000000000000000000000000000<em>1</em>0010<em>0</em>1  (decimal 73)
</code>
</pre>

So, because of the mask, the value <code>73</code> is written to memory address <code>8</code> instead. Then, the program tries to write <code>101</code> to address <code>7</code>:

<pre>
<code>value:  000000000000000000000000000001100101  (decimal 101)
mask:   XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
result: 00000000000000000000000000000<em>1</em>1001<em>0</em>1  (decimal 101)
</code>
</pre>

This time, the mask has no effect, as the bits it overwrote were already the values the mask tried to set. Finally, the program tries to write <code>0</code> to address <code>8</code>:

<pre>
<code>value:  000000000000000000000000000000000000  (decimal 0)
mask:   XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
result: 00000000000000000000000000000<em>1</em>0000<em>0</em>0  (decimal 64)
</code>
</pre>

<code>64</code> is written to address <code>8</code> instead, overwriting the value that was there previously.

To initialize your ferry's docking program, you need the sum of all values left in memory after the initialization program completes. (The entire 36-bit address space begins initialized to the value <code>0</code> at every address.) In the above example, only two values in memory are not zero - <code>101</code> (at address <code>7</code>) and <code>64</code> (at address <code>8</code>) - producing a sum of <em><code>165</code></em>.

Execute the initialization program. <em>What is the sum of all values left in memory after it completes?</em>


## --- Part Two ---
For some reason, the sea port's computer system still can't communicate with your ferry's docking program. It must be using <em>version 2</em> of the decoder chip!

A version 2 decoder chip doesn't modify the values being written at all.  Instead, it acts as a [memory address decoder](https://www.youtube.com/watch?v=PvfhANgLrm4). Immediately before a value is written to memory, each bit in the bitmask modifies the corresponding bit of the destination <em>memory address</em> in the following way:


 - If the bitmask bit is <code>0</code>, the corresponding memory address bit is <em>unchanged</em>.
 - If the bitmask bit is <code>1</code>, the corresponding memory address bit is <em>overwritten with <code>1</code></em>.
 - If the bitmask bit is <code>X</code>, the corresponding memory address bit is <em>floating</em>.

A <em>floating</em> bit is not connected to anything and instead fluctuates unpredictably. In practice, this means the floating bits will take on <em>all possible values</em>, potentially causing many memory addresses to be written all at once!

For example, consider the following program:

<pre>
<code>mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1
</code>
</pre>

When this program goes to write to memory address <code>42</code>, it first applies the bitmask:

<pre>
<code>address: 000000000000000000000000000000101010  (decimal 42)
mask:    000000000000000000000000000000X1001X
result:  000000000000000000000000000000<em>X1</em>10<em>1X</em>
</code>
</pre>

After applying the mask, four bits are overwritten, three of which are different, and two of which are <em>floating</em>. Floating bits take on every possible combination of values; with two floating bits, four actual memory addresses are written:

<pre>
<code>000000000000000000000000000000<em>0</em>1101<em>0</em>  (decimal 26)
000000000000000000000000000000<em>0</em>1101<em>1</em>  (decimal 27)
000000000000000000000000000000<em>1</em>1101<em>0</em>  (decimal 58)
000000000000000000000000000000<em>1</em>1101<em>1</em>  (decimal 59)
</code>
</pre>

Next, the program is about to write to memory address <code>26</code> with a different bitmask:

<pre>
<code>address: 000000000000000000000000000000011010  (decimal 26)
mask:    00000000000000000000000000000000X0XX
result:  00000000000000000000000000000001<em>X</em>0<em>XX</em>
</code>
</pre>

This results in an address with three floating bits, causing writes to <em>eight</em> memory addresses:

<pre>
<code>00000000000000000000000000000001<em>0</em>0<em>00</em>  (decimal 16)
00000000000000000000000000000001<em>0</em>0<em>01</em>  (decimal 17)
00000000000000000000000000000001<em>0</em>0<em>10</em>  (decimal 18)
00000000000000000000000000000001<em>0</em>0<em>11</em>  (decimal 19)
00000000000000000000000000000001<em>1</em>0<em>00</em>  (decimal 24)
00000000000000000000000000000001<em>1</em>0<em>01</em>  (decimal 25)
00000000000000000000000000000001<em>1</em>0<em>10</em>  (decimal 26)
00000000000000000000000000000001<em>1</em>0<em>11</em>  (decimal 27)
</code>
</pre>

The entire 36-bit address space still begins initialized to the value 0 at every address, and you still need the sum of all values left in memory at the end of the program.  In this example, the sum is <em><code>208</code></em>.

Execute the initialization program using an emulator for a version 2 decoder chip. <em>What is the sum of all values left in memory after it completes?</em>


