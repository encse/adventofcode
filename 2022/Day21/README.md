original source: [https://adventofcode.com/2022/day/21](https://adventofcode.com/2022/day/21)
## --- Day 21: Monkey Math ---
The [monkeys](11) are back! You're worried they're going to try to steal your stuff again, but it seems like they're just holding their ground and making various monkey noises at you.

Eventually, one of the elephants realizes you don't speak monkey and comes over to interpret. As it turns out, they overheard you talking about trying to find the grove; they can show you a shortcut if you answer their <em>riddle</em>.

Each monkey is given a <em>job</em>: either to <em>yell a specific number</em> or to <em>yell the result of a math operation</em>. All of the number-yelling monkeys know their number from the start; however, the math operation monkeys need to wait for two other monkeys to yell a number, and those two other monkeys might <em>also</em> be waiting on other monkeys.

Your job is to <em>work out the number the monkey named <code>root</code> will yell</em> before the monkeys figure it out themselves.

For example:

<pre>
<code>root: pppw + sjmn
dbpl: 5
cczh: sllz + lgvd
zczc: 2
ptdq: humn - dvpt
dvpt: 3
lfqf: 4
humn: 5
ljgn: 2
sjmn: drzm * dbpl
sllz: 4
pppw: cczh / lfqf
lgvd: ljgn * ptdq
drzm: hmdt - zczc
hmdt: 32
</code>
</pre>

Each line contains the name of a monkey, a colon, and then the job of that monkey:


 - A lone number means the monkey's job is simply to yell that number.
 - A job like <code>aaaa + bbbb</code> means the monkey waits for monkeys <code>aaaa</code> and <code>bbbb</code> to yell each of their numbers; the monkey then yells the sum of those two numbers.
 - <code>aaaa - bbbb</code> means the monkey yells <code>aaaa</code>'s number minus <code>bbbb</code>'s number.
 - Job <code>aaaa * bbbb</code> will yell <code>aaaa</code>'s number multiplied by <code>bbbb</code>'s number.
 - Job <code>aaaa / bbbb</code> will yell <code>aaaa</code>'s number divided by <code>bbbb</code>'s number.

So, in the above example, monkey <code>drzm</code> has to wait for monkeys <code>hmdt</code> and <code>zczc</code> to yell their numbers. Fortunately, both <code>hmdt</code> and <code>zczc</code> have jobs that involve simply yelling a single number, so they do this immediately: <code>32</code> and <code>2</code>. Monkey <code>drzm</code> can then yell its number by finding <code>32</code> minus <code>2</code>: <code><em>30</em></code>.

Then, monkey <code>sjmn</code> has one of its numbers (<code>30</code>, from monkey <code>drzm</code>), and already has its other number, <code>5</code>, from <code>dbpl</code>. This allows it to yell its own number by finding <code>30</code> multiplied by <code>5</code>: <code><em>150</em></code>.

This process continues until <code>root</code> yells a number: <code><em>152</em></code>.

However, your actual situation involves considerably more monkeys. <em>What number will the monkey named <code>root</code> yell?</em>


## --- Part Two ---
Due to some kind of monkey-elephant-human mistranslation, you seem to have misunderstood a few key details about the riddle.

First, you got the wrong job for the monkey named <code>root</code>; specifically, you got the wrong math operation. The correct operation for monkey <code>root</code> should be <code>=</code>, which means that it still listens for two numbers (from the same two monkeys as before), but now checks that the two numbers <em>match</em>.

Second, you got the wrong monkey for the job starting with <code>humn:</code>. It isn't a monkey - it's <em>you</em>. Actually, you got the job wrong, too: you need to figure out <em>what number you need to yell</em> so that <code>root</code>'s equality check passes. (The number that appears after <code>humn:</code> in your input is now irrelevant.)

In the above example, the number you need to yell to pass <code>root</code>'s equality test is <code><em>301</em></code>. (This causes <code>root</code> to get the same number, <code>150</code>, from both of its monkeys.)

<em>What number do you yell to pass <code>root</code>'s equality test?</em>


