original source: [https://adventofcode.com/2021/day/10](https://adventofcode.com/2021/day/10)
## --- Day 10: Syntax Scoring ---
You ask the submarine to determine the best route out of the deep-sea cave, but it only replies:

<pre>
<code>Syntax error in navigation subsystem on line: all of them</code>
</pre>

<em>All of them?!</em> The damage is worse than you thought. You bring up a copy of the navigation subsystem (your puzzle input).

The navigation subsystem syntax is made of several lines containing <em>chunks</em>. There are one or more chunks on each line, and chunks contain zero or more other chunks. Adjacent chunks are not separated by any delimiter; if one chunk stops, the next chunk (if any) can immediately start. Every chunk must <em>open</em> and <em>close</em> with one of four legal pairs of matching characters:


 - If a chunk opens with <code>(</code>, it must close with <code>)</code>.
 - If a chunk opens with <code>[</code>, it must close with <code>]</code>.
 - If a chunk opens with <code>{</code>, it must close with <code>}</code>.
 - If a chunk opens with <code><</code>, it must close with <code>></code>.

So, <code>()</code> is a legal chunk that contains no other chunks, as is <code>[]</code>. More complex but valid chunks include <code>([])</code>, <code>{()()()}</code>, <code><([{}])></code>, <code>[<>({}){}[([])<>]]</code>, and even <code>(((((((((())))))))))</code>.

Some lines are <em>incomplete</em>, but others are <em>corrupted</em>. Find and discard the corrupted lines first.

A corrupted line is one where a chunk <em>closes with the wrong character</em> - that is, where the characters it opens and closes with do not form one of the four legal pairs listed above.

Examples of corrupted chunks include <code>(]</code>, <code>{()()()></code>, <code>(((()))}</code>, and <code><([]){()}[{}])</code>. Such a chunk can appear anywhere within a line, and its presence causes the whole line to be considered corrupted.

For example, consider the following navigation subsystem:

<pre>
<code>[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]
</code>
</pre>

Some of the lines aren't corrupted, just incomplete; you can ignore these lines for now. The remaining five lines are corrupted:


 - <code>{([(<{}[<>[]}>{[]{[(<()></code> - Expected <code>]</code>, but found <code>}</code> instead.
 - <code>[[<[([]))<([[{}[[()]]]</code> - Expected <code>]</code>, but found <code>)</code> instead.
 - <code>[{[{({}]{}}([{[{{{}}([]</code> - Expected <code>)</code>, but found <code>]</code> instead.
 - <code>[<(<(<(<{}))><([]([]()</code> - Expected <code>></code>, but found <code>)</code> instead.
 - <code><{([([[(<>()){}]>(<<{{</code> - Expected <code>]</code>, but found <code>></code> instead.

Stop at the first incorrect closing character on each corrupted line.

Did you know that syntax checkers actually have contests to see who can get the high score for syntax errors in a file? It's true! To calculate the syntax error score for a line, take the <em>first illegal character</em> on the line and look it up in the following table:


 - <code>)</code>: <code>3</code> points.
 - <code>]</code>: <code>57</code> points.
 - <code>}</code>: <code>1197</code> points.
 - <code>></code>: <code>25137</code> points.

In the above example, an illegal <code>)</code> was found twice (<code>2*3 = <em>6</em></code> points), an illegal <code>]</code> was found once (<code><em>57</em></code> points), an illegal <code>}</code> was found once (<code><em>1197</em></code> points), and an illegal <code>></code> was found once (<code><em>25137</em></code> points). So, the total syntax error score for this file is <code>6+57+1197+25137 = <em>26397</em></code> points!

Find the first illegal character in each corrupted line of the navigation subsystem. <em>What is the total syntax error score for those errors?</em>


## --- Part Two ---
Now, discard the corrupted lines.  The remaining lines are <em>incomplete</em>.

Incomplete lines don't have any incorrect characters - instead, they're missing some closing characters at the end of the line. To repair the navigation subsystem, you just need to figure out <em>the sequence of closing characters</em> that complete all open chunks in the line.

You can only use closing characters (<code>)</code>, <code>]</code>, <code>}</code>, or <code>></code>), and you must add them in the correct order so that only legal pairs are formed and all chunks end up closed.

In the example above, there are five incomplete lines:


 - <code>[({(<(())[]>[[{[]{<()<>></code> - Complete by adding <code>}}]])})]</code>.
 - <code>[(()[<>])]({[<{<<[]>>(</code> - Complete by adding <code>)}>]})</code>.
 - <code>(((({<>}<{<{<>}{[]{[]{}</code> - Complete by adding <code>}}>}>))))</code>.
 - <code>{<[[]]>}<{[{[{[]{()[[[]</code> - Complete by adding <code>]]}}]}]}></code>.
 - <code><{([{{}}[<[[[<>{}]]]>[]]</code> - Complete by adding <code>])}></code>.

Did you know that autocomplete tools <em>also</em> have contests? It's true! The score is determined by considering the completion string character-by-character. Start with a total score of <code>0</code>. Then, for each character, multiply the total score by 5 and then increase the total score by the point value given for the character in the following table:


 - <code>)</code>: <code>1</code> point.
 - <code>]</code>: <code>2</code> points.
 - <code>}</code>: <code>3</code> points.
 - <code>></code>: <code>4</code> points.

So, the last completion string above - <code>])}></code> - would be scored as follows:


 - Start with a total score of <code>0</code>.
 - Multiply the total score by 5 to get <code>0</code>, then add the value of <code>]</code> (2) to get a new total score of <code>2</code>.
 - Multiply the total score by 5 to get <code>10</code>, then add the value of <code>)</code> (1) to get a new total score of <code>11</code>.
 - Multiply the total score by 5 to get <code>55</code>, then add the value of <code>}</code> (3) to get a new total score of <code>58</code>.
 - Multiply the total score by 5 to get <code>290</code>, then add the value of <code>></code> (4) to get a new total score of <code>294</code>.

The five lines' completion strings have total scores as follows:


 - <code>}}]])})]</code> - <code>288957</code> total points.
 - <code>)}>]})</code> - <code>5566</code> total points.
 - <code>}}>}>))))</code> - <code>1480781</code> total points.
 - <code>]]}}]}]}></code> - <code>995444</code> total points.
 - <code>])}></code> - <code>294</code> total points.

Autocomplete tools are an odd bunch: the winner is found by <em>sorting</em> all of the scores and then taking the <em>middle</em> score. (There will always be an odd number of scores to consider.) In this example, the middle score is <code><em>288957</em></code> because there are the same number of scores smaller and larger than it.

Find the completion string for each incomplete line, score the completion strings, and sort the scores. <em>What is the middle score?</em>


