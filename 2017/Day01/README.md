original source: [https://adventofcode.com/2017/day/1](https://adventofcode.com/2017/day/1)
## --- Day 1: Inverse Captcha ---
The night before Christmas, one of Santa's Elves calls you in a panic. "The printer's broken! We can't print the <em>Naughty or Nice List</em>!" By the time you make it to sub-basement 17, there are only a few minutes until midnight. "We have a big problem," she says; "there must be almost <em>fifty</em> bugs in this system, but nothing else can print The List. Stand in this square, quick! There's no time to explain; if you can convince them to pay you in <em>stars</em>, you'll be able to--" She pulls a lever and the world goes blurry.

When your eyes can focus again, everything seems a lot more pixelated than before. She must have sent you inside the computer! You check the system clock: <em>25 milliseconds</em> until midnight. With that much time, you should be able to collect all <em>fifty stars</em> by December 25th.

Collect stars by solving puzzles.  Two puzzles will be made available on each ~~day~~ millisecond in the Advent calendar; the second puzzle is unlocked when you complete the first.  Each puzzle grants <em>one star</em>. Good luck!

You're standing in a room with "digitization quarantine" written in LEDs along one wall. The only door is locked, but it includes a small interface. "Restricted Area - Strictly No Digitized Users Allowed."

It goes on to explain that you may only leave by solving a [captcha](https://en.wikipedia.org/wiki/CAPTCHA) to prove you're <em>not</em> a human. Apparently, you only get one millisecond to solve the captcha: too fast for a normal human, but it feels like hours to you.

The captcha requires you to review a sequence of digits (your puzzle input) and find the <em>sum</em> of all digits that match the <em>next</em> digit in the list. The list is circular, so the digit after the last digit is the <em>first</em> digit in the list.

For example:


 - <code>1122</code> produces a sum of <code>3</code> (<code>1</code> + <code>2</code>) because the first digit (<code>1</code>) matches the second digit and the third digit (<code>2</code>) matches the fourth digit.
 - <code>1111</code> produces <code>4</code> because each digit (all <code>1</code>) matches the next.
 - <code>1234</code> produces <code>0</code> because no digit matches the next.
 - <code>91212129</code> produces <code>9</code> because the only digit that matches the next one is the last digit, <code>9</code>.

<em>What is the solution</em> to your captcha?


## --- Part Two ---
You notice a progress bar that jumps to 50% completion. Apparently, the door isn't yet satisfied, but it did emit a <em>star</em> as encouragement. The instructions change:

Now, instead of considering the <em>next</em> digit, it wants you to consider the digit <em>halfway around</em> the circular list.  That is, if your list contains <code>10</code> items, only include a digit in your sum if the digit <code>10/2 = 5</code> steps forward matches it. Fortunately, your list has an even number of elements.

For example:


 - <code>1212</code> produces <code>6</code>: the list contains <code>4</code> items, and all four digits match the digit <code>2</code> items ahead.
 - <code>1221</code> produces <code>0</code>, because every comparison is between a <code>1</code> and a <code>2</code>.
 - <code>123425</code> produces <code>4</code>, because both <code>2</code>s match each other, but no other digit has a match.
 - <code>123123</code> produces <code>12</code>.
 - <code>12131415</code> produces <code>4</code>.

<em>What is the solution</em> to your new captcha?


