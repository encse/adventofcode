original source: [https://adventofcode.com/2020/day/6](https://adventofcode.com/2020/day/6)
## --- Day 6: Custom Customs ---
As your flight approaches the regional airport where you'll switch to a much larger plane, [customs declaration forms](https://en.wikipedia.org/wiki/Customs_declaration) are distributed to the passengers.

The form asks a series of 26 yes-or-no questions marked <code>a</code> through <code>z</code>. All you need to do is identify the questions for which <em>anyone in your group</em> answers "yes". Since your group is just you, this doesn't take very long.

However, the person sitting next to you seems to be experiencing a language barrier and asks if you can help. For each of the people in their group, you write down the questions for which they answer "yes", one per line.  For example:

<pre>
<code>abcx
abcy
abcz
</code>
</pre>

In this group, there are <em><code>6</code></em> questions to which anyone answered "yes": <code>a</code>, <code>b</code>, <code>c</code>, <code>x</code>, <code>y</code>, and <code>z</code>. (Duplicate answers to the same question don't count extra; each question counts at most once.)

Another group asks for your help, then another, and eventually you've collected answers from every group on the plane (your puzzle input). Each group's answers are separated by a blank line, and within each group, each person's answers are on a single line. For example:

<pre>
<code>abc

a
b
c

ab
ac

a
a
a
a

b
</code>
</pre>

This list represents answers from five groups:


 - The first group contains one person who answered "yes" to <em><code>3</code></em> questions: <code>a</code>, <code>b</code>, and <code>c</code>.
 - The second group contains three people; combined, they answered "yes" to <em><code>3</code></em> questions: <code>a</code>, <code>b</code>, and <code>c</code>.
 - The third group contains two people; combined, they answered "yes" to <em><code>3</code></em> questions: <code>a</code>, <code>b</code>, and <code>c</code>.
 - The fourth group contains four people; combined, they answered "yes" to only <em><code>1</code></em> question, <code>a</code>.
 - The last group contains one person who answered "yes" to only <em><code>1</code></em> question, <code>b</code>.

In this example, the sum of these counts is <code>3 + 3 + 3 + 1 + 1</code> = <em><code>11</code></em>.

For each group, count the number of questions to which anyone answered "yes". <em>What is the sum of those counts?</em>


## --- Part Two ---
As you finish the last group's customs declaration, you notice that you misread one word in the instructions:

You don't need to identify the questions to which <em>anyone</em> answered "yes"; you need to identify the questions to which <em>everyone</em> answered "yes"!

Using the same  example as above:

<pre>
<code>abc

a
b
c

ab
ac

a
a
a
a

b
</code>
</pre>

This list represents answers from five groups:


 - In the first group, everyone (all 1 person) answered "yes" to <em><code>3</code></em> questions: <code>a</code>, <code>b</code>, and <code>c</code>.
 - In the second group, there is <em>no</em> question to which everyone answered "yes".
 - In the third group, everyone answered yes to only <em><code>1</code></em> question, <code>a</code>. Since some people did not answer "yes" to <code>b</code> or <code>c</code>, they don't count.
 - In the fourth group, everyone answered yes to only <em><code>1</code></em> question, <code>a</code>.
 - In the fifth group, everyone (all 1 person) answered "yes" to <em><code>1</code></em> question, <code>b</code>.

In this example, the sum of these counts is <code>3 + 0 + 1 + 1 + 1</code> = <em><code>6</code></em>.

For each group, count the number of questions to which <em>everyone</em> answered "yes". <em>What is the sum of those counts?</em>


