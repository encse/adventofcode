original source: [https://adventofcode.com/2020/day/23](https://adventofcode.com/2020/day/23)
## --- Day 23: Crab Cups ---
The small crab challenges <em>you</em> to a game! The crab is going to mix up some cups, and you have to predict where they'll end up.

The cups will be arranged in a circle and labeled <em>clockwise</em> (your puzzle input). For example, if your labeling were <code>32415</code>, there would be five cups in the circle; going clockwise around the circle from the first cup, the cups would be labeled <code>3</code>, <code>2</code>, <code>4</code>, <code>1</code>, <code>5</code>, and then back to <code>3</code> again.

Before the crab starts, it will designate the first cup in your list as the <em>current cup</em>. The crab is then going to do <em>100 moves</em>.

Each <em>move</em>, the crab does the following actions:


 - The crab picks up the <em>three cups</em> that are immediately <em>clockwise</em> of the <em>current cup</em>. They are removed from the circle; cup spacing is adjusted as necessary to maintain the circle.
 - The crab selects a <em>destination cup</em>: the cup with a <em>label</em> equal to the <em>current cup's</em> label minus one. If this would select one of the cups that was just picked up, the crab will keep subtracting one until it finds a cup that wasn't just picked up. If at any point in this process the value goes below the lowest value on any cup's label, it <em>wraps around</em> to the highest value on any cup's label instead.
 - The crab places the cups it just picked up so that they are <em>immediately clockwise</em> of the destination cup. They keep the same order as when they were picked up.
 - The crab selects a new <em>current cup</em>: the cup which is immediately clockwise of the current cup.

For example, suppose your cup labeling were <code>389125467</code>. If the crab were to do merely 10 moves, the following changes would occur:

<pre>
<code>-- move 1 --
cups: (3) 8  9  1  2  5  4  6  7 
pick up: 8, 9, 1
destination: 2

-- move 2 --
cups:  3 (2) 8  9  1  5  4  6  7 
pick up: 8, 9, 1
destination: 7

-- move 3 --
cups:  3  2 (5) 4  6  7  8  9  1 
pick up: 4, 6, 7
destination: 3

-- move 4 --
cups:  7  2  5 (8) 9  1  3  4  6 
pick up: 9, 1, 3
destination: 7

-- move 5 --
cups:  3  2  5  8 (4) 6  7  9  1 
pick up: 6, 7, 9
destination: 3

-- move 6 --
cups:  9  2  5  8  4 (1) 3  6  7 
pick up: 3, 6, 7
destination: 9

-- move 7 --
cups:  7  2  5  8  4  1 (9) 3  6 
pick up: 3, 6, 7
destination: 8

-- move 8 --
cups:  8  3  6  7  4  1  9 (2) 5 
pick up: 5, 8, 3
destination: 1

-- move 9 --
cups:  7  4  1  5  8  3  9  2 (6)
pick up: 7, 4, 1
destination: 5

-- move 10 --
cups: (5) 7  4  1  8  3  9  2  6 
pick up: 7, 4, 1
destination: 3

-- final --
cups:  5 (8) 3  7  4  1  9  2  6 
</code>
</pre>

In the above example, the cups' values are the labels as they appear moving clockwise around the circle; the <em>current cup</em> is marked with <code>( )</code>.

After the crab is done, what order will the cups be in? Starting <em>after the cup labeled <code>1</code></em>, collect the other cups' labels clockwise into a single string with no extra characters; each number except <code>1</code> should appear exactly once. In the above example, after 10 moves, the cups clockwise from <code>1</code> are labeled <code>9</code>, <code>2</code>, <code>6</code>, <code>5</code>, and so on, producing <em><code>92658374</code></em>. If the crab were to complete all 100 moves, the order after cup <code>1</code> would be <em><code>67384529</code></em>.

Using your labeling, simulate 100 moves. <em>What are the labels on the cups after cup <code>1</code>?</em>


## --- Part Two ---
Due to what you can only assume is a mistranslation (you're not exactly fluent in Crab), you are quite surprised when the crab starts arranging <em>many</em> cups in a circle on your raft - <em>one million</em> (<code>1000000</code>) in total.

Your labeling is still correct for the first few cups; after that, the remaining cups are just numbered in an increasing fashion starting from the number after the highest number in your list and proceeding one by one until one million is reached. (For example, if your labeling were <code>54321</code>, the cups would be numbered <code>5</code>, <code>4</code>, <code>3</code>, <code>2</code>, <code>1</code>, and then start counting up from <code>6</code> until one million is reached.) In this way, every number from one through one million is used exactly once.

After discovering where you made the mistake in translating Crab Numbers, you realize the small crab isn't going to do merely 100 moves; the crab is going to do <em>ten million</em> (<code>10000000</code>) moves!

The crab is going to hide your <em>stars</em> - one each - under the <em>two cups that will end up immediately clockwise of cup <code>1</code></em>. You can have them if you predict what the labels on those cups will be when the crab is finished.

In the above example (<code>389125467</code>), this would be <code>934001</code> and then <code>159792</code>; multiplying these together produces <em><code>149245887792</code></em>.

Determine which two cups will end up immediately clockwise of cup <code>1</code>. <em>What do you get if you multiply their labels together?</em>


