original source: [https://adventofcode.com/2015/day/1](https://adventofcode.com/2015/day/1)
## --- Day 1: Not Quite Lisp ---
Santa was hoping for a white Christmas, but his weather machine's "snow" function is powered by stars, and he's fresh out!  To save Christmas, he needs you to collect <em>fifty stars</em> by December 25th.

Collect stars by helping Santa solve puzzles.  Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first.  Each puzzle grants <em>one star</em>. Good luck!

Here's an easy puzzle to warm you up.

Santa is trying to deliver presents in a large apartment building, but he can't find the right floor - the directions he got are a little confusing. He starts on the ground floor (floor <code>0</code>) and then follows the instructions one character at a time.

An opening parenthesis, <code>(</code>, means he should go up one floor, and a closing parenthesis, <code>)</code>, means he should go down one floor.

The apartment building is very tall, and the basement is very deep; he will never find the top or bottom floors.

For example:


 - <code>(())</code> and <code>()()</code> both result in floor <code>0</code>.
 - <code>(((</code> and <code>(()(()(</code> both result in floor <code>3</code>.
 - <code>))(((((</code> also results in floor <code>3</code>.
 - <code>())</code> and <code>))(</code> both result in floor <code>-1</code> (the first basement level).
 - <code>)))</code> and <code>)())())</code> both result in floor <code>-3</code>.

To <em>what floor</em> do the instructions take Santa?


## --- Part Two ---
Now, given the same instructions, find the <em>position</em> of the first character that causes him to enter the basement (floor <code>-1</code>).  The first character in the instructions has position <code>1</code>, the second character has position <code>2</code>, and so on.

For example:


 - <code>)</code> causes him to enter the basement at character position <code>1</code>.
 - <code>()())</code> causes him to enter the basement at character position <code>5</code>.

What is the <em>position</em> of the character that causes Santa to first enter the basement?


