original source: [https://adventofcode.com/2022/day/13](https://adventofcode.com/2022/day/13)
## --- Day 13: Distress Signal ---
You climb the hill and again try contacting the Elves. However, you instead receive a signal you weren't expecting: a <em>distress signal</em>.

Your handheld device must still not be working properly; the packets from the distress signal got decoded <em>out of order</em>. You'll need to re-order the list of received packets (your puzzle input) to decode the message.

Your list consists of pairs of packets; pairs are separated by a blank line. You need to identify <em>how many pairs of packets are in the right order</em>.

For example:

<pre>
<code>[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]
</code>
</pre>

Packet data consists of lists and integers. Each list starts with <code>[</code>, ends with <code>]</code>, and contains zero or more comma-separated values (either integers or other lists). Each packet is always a list and appears on its own line.

When comparing two values, the first value is called <em>left</em> and the second value is called <em>right</em>. Then:


 - If <em>both values are integers</em>, the <em>lower integer</em> should come first. If the left integer is lower than the right integer, the inputs are in the right order. If the left integer is higher than the right integer, the inputs are not in the right order. Otherwise, the inputs are the same integer; continue checking the next part of the input.
 - If <em>both values are lists</em>, compare the first value of each list, then the second value, and so on. If the left list runs out of items first, the inputs are in the right order. If the right list runs out of items first, the inputs are not in the right order. If the lists are the same length and no comparison makes a decision about the order, continue checking the next part of the input.
 - If <em>exactly one value is an integer</em>, convert the integer to a list which contains that integer as its only value, then retry the comparison. For example, if comparing <code>[0,0,0]</code> and <code>2</code>, convert the right value to <code>[2]</code> (a list containing <code>2</code>); the result is then found by instead comparing <code>[0,0,0]</code> and <code>[2]</code>.

Using these rules, you can determine which of the pairs in the example are in the right order:

<pre>
<code>== Pair 1 ==
- Compare [1,1,3,1,1] vs [1,1,5,1,1]
  - Compare 1 vs 1
  - Compare 1 vs 1
  - Compare 3 vs 5
    - Left side is smaller, so inputs are <em>in the right order</em>

== Pair 2 ==
- Compare [[1],[2,3,4]] vs [[1],4]
  - Compare [1] vs [1]
    - Compare 1 vs 1
  - Compare [2,3,4] vs 4
    - Mixed types; convert right to [4] and retry comparison
    - Compare [2,3,4] vs [4]
      - Compare 2 vs 4
        - Left side is smaller, so inputs are <em>in the right order</em>

== Pair 3 ==
- Compare [9] vs [[8,7,6]]
  - Compare 9 vs [8,7,6]
    - Mixed types; convert left to [9] and retry comparison
    - Compare [9] vs [8,7,6]
      - Compare 9 vs 8
        - Right side is smaller, so inputs are <em>not</em> in the right order

== Pair 4 ==
- Compare [[4,4],4,4] vs [[4,4],4,4,4]
  - Compare [4,4] vs [4,4]
    - Compare 4 vs 4
    - Compare 4 vs 4
  - Compare 4 vs 4
  - Compare 4 vs 4
  - Left side ran out of items, so inputs are <em>in the right order</em>

== Pair 5 ==
- Compare [7,7,7,7] vs [7,7,7]
  - Compare 7 vs 7
  - Compare 7 vs 7
  - Compare 7 vs 7
  - Right side ran out of items, so inputs are <em>not</em> in the right order

== Pair 6 ==
- Compare [] vs [3]
  - Left side ran out of items, so inputs are <em>in the right order</em>

== Pair 7 ==
- Compare [[[]]] vs [[]]
  - Compare [[]] vs []
    - Right side ran out of items, so inputs are <em>not</em> in the right order

== Pair 8 ==
- Compare [1,[2,[3,[4,[5,6,7]]]],8,9] vs [1,[2,[3,[4,[5,6,0]]]],8,9]
  - Compare 1 vs 1
  - Compare [2,[3,[4,[5,6,7]]]] vs [2,[3,[4,[5,6,0]]]]
    - Compare 2 vs 2
    - Compare [3,[4,[5,6,7]]] vs [3,[4,[5,6,0]]]
      - Compare 3 vs 3
      - Compare [4,[5,6,7]] vs [4,[5,6,0]]
        - Compare 4 vs 4
        - Compare [5,6,7] vs [5,6,0]
          - Compare 5 vs 5
          - Compare 6 vs 6
          - Compare 7 vs 0
            - Right side is smaller, so inputs are <em>not</em> in the right order
</code>
</pre>

What are the indices of the pairs that are already <em>in the right order</em>? (The first pair has index 1, the second pair has index 2, and so on.) In the above example, the pairs in the right order are 1, 2, 4, and 6; the sum of these indices is <code><em>13</em></code>.

Determine which pairs of packets are already in the right order. <em>What is the sum of the indices of those pairs?</em>


## --- Part Two ---
Now, you just need to put <em>all</em> of the packets in the right order. Disregard the blank lines in your list of received packets.

The distress signal protocol also requires that you include two additional <em>divider packets</em>:

<pre>
<code>[[2]]
[[6]]
</code>
</pre>

Using the same rules as before, organize all packets - the ones in your list of received packets as well as the two divider packets - into the correct order.

For the example above, the result of putting the packets in the correct order is:

<pre>
<code>[]
[[]]
[[[]]]
[1,1,3,1,1]
[1,1,5,1,1]
[[1],[2,3,4]]
[1,[2,[3,[4,[5,6,0]]]],8,9]
[1,[2,[3,[4,[5,6,7]]]],8,9]
[[1],4]
<em>[[2]]</em>
[3]
[[4,4],4,4]
[[4,4],4,4,4]
<em>[[6]]</em>
[7,7,7]
[7,7,7,7]
[[8,7,6]]
[9]
</code>
</pre>

Afterward, locate the divider packets. To find the <em>decoder key</em> for this distress signal, you need to determine the indices of the two divider packets and multiply them together. (The first packet is at index 1, the second packet is at index 2, and so on.) In this example, the divider packets are <em>10th</em> and <em>14th</em>, and so the decoder key is <code><em>140</em></code>.

Organize all of the packets into the correct order. <em>What is the decoder key for the distress signal?</em>


