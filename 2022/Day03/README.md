original source: [https://adventofcode.com/2022/day/3](https://adventofcode.com/2022/day/3)
## --- Day 3: Rucksack Reorganization ---
One Elf has the important job of loading all of the [rucksacks](https://en.wikipedia.org/wiki/Rucksack) with supplies for the jungle journey. Unfortunately, that Elf didn't quite follow the packing instructions, and so a few items now need to be rearranged.

Each rucksack has two large <em>compartments</em>. All items of a given type are meant to go into exactly one of the two compartments. The Elf that did the packing failed to follow this rule for exactly one item type per rucksack.

The Elves have made a list of all of the items currently in each rucksack (your puzzle input), but they need your help finding the errors. Every item type is identified by a single lowercase or uppercase letter (that is, <code>a</code> and <code>A</code> refer to different types of items).

The list of items for each rucksack is given as characters all on a single line. A given rucksack always has the same number of items in each of its two compartments, so the first half of the characters represent items in the first compartment, while the second half of the characters represent items in the second compartment.

For example, suppose you have the following list of contents from six rucksacks:

<pre>
<code>vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw
</code>
</pre>


 - The first rucksack contains the items <code>vJrwpWtwJgWrhcsFMMfFFhFp</code>, which means its first compartment contains the items <code>vJrwpWtwJgWr</code>, while the second compartment contains the items <code>hcsFMMfFFhFp</code>. The only item type that appears in both compartments is lowercase <code><em>p</em></code>.
 - The second rucksack's compartments contain <code>jqHRNqRjqzjGDLGL</code> and <code>rsFMfFZSrLrFZsSL</code>. The only item type that appears in both compartments is uppercase <code><em>L</em></code>.
 - The third rucksack's compartments contain <code>PmmdzqPrV</code> and <code>vPwwTWBwg</code>; the only common item type is uppercase <code><em>P</em></code>.
 - The fourth rucksack's compartments only share item type <code><em>v</em></code>.
 - The fifth rucksack's compartments only share item type <code><em>t</em></code>.
 - The sixth rucksack's compartments only share item type <code><em>s</em></code>.

To help prioritize item rearrangement, every item type can be converted to a <em>priority</em>:


 - Lowercase item types <code>a</code> through <code>z</code> have priorities 1 through 26.
 - Uppercase item types <code>A</code> through <code>Z</code> have priorities 27 through 52.

In the above example, the priority of the item type that appears in both compartments of each rucksack is 16 (<code>p</code>), 38 (<code>L</code>), 42 (<code>P</code>), 22 (<code>v</code>), 20 (<code>t</code>), and 19 (<code>s</code>); the sum of these is <code><em>157</em></code>.

Find the item type that appears in both compartments of each rucksack. <em>What is the sum of the priorities of those item types?</em>


## --- Part Two ---
As you finish identifying the misplaced items, the Elves come to you with another issue.

For safety, the Elves are divided into groups of three. Every Elf carries a badge that identifies their group. For efficiency, within each group of three Elves, the badge is the <em>only item type carried by all three Elves</em>. That is, if a group's badge is item type <code>B</code>, then all three Elves will have item type <code>B</code> somewhere in their rucksack, and at most two of the Elves will be carrying any other item type.

The problem is that someone forgot to put this year's updated authenticity sticker on the badges. All of the badges need to be pulled out of the rucksacks so the new authenticity stickers can be attached.

Additionally, nobody wrote down which item type corresponds to each group's badges. The only way to tell which item type is the right one is by finding the one item type that is <em>common between all three Elves</em> in each group.

Every set of three lines in your list corresponds to a single group, but each group can have a different badge item type. So, in the above example, the first group's rucksacks are the first three lines:

<pre>
<code>vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
</code>
</pre>

And the second group's rucksacks are the next three lines:

<pre>
<code>wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw
</code>
</pre>

In the first group, the only item type that appears in all three rucksacks is lowercase <code>r</code>; this must be their badges. In the second group, their badge item type must be <code>Z</code>.

Priorities for these items must still be found to organize the sticker attachment efforts: here, they are 18 (<code>r</code>) for the first group and 52 (<code>Z</code>) for the second group. The sum of these is <code><em>70</em></code>.

Find the item type that corresponds to the badges of each three-Elf group. <em>What is the sum of the priorities of those item types?</em>


