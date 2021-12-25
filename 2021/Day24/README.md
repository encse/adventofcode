original source: [https://adventofcode.com/2021/day/24](https://adventofcode.com/2021/day/24)
## --- Day 24: Arithmetic Logic Unit ---
[Magic smoke](https://en.wikipedia.org/wiki/Magic_smoke) starts leaking from the submarine's [arithmetic logic unit](https://en.wikipedia.org/wiki/Arithmetic_logic_unit) (ALU). Without the ability to perform basic arithmetic and logic functions, the submarine can't produce cool patterns with its Christmas lights!

It also can't navigate. Or run the oxygen system.

Don't worry, though - you <em>probably</em> have enough oxygen left to give you enough time to build a new ALU.

The ALU is a four-dimensional processing unit: it has integer variables <code>w</code>, <code>x</code>, <code>y</code>, and <code>z</code>. These variables all start with the value <code>0</code>. The ALU also supports <em>six instructions</em>:


 - <code>inp a</code> - Read an input value and write it to variable <code>a</code>.
 - <code>add a b</code> - Add the value of <code>a</code> to the value of <code>b</code>, then store the result in variable <code>a</code>.
 - <code>mul a b</code> - Multiply the value of <code>a</code> by the value of <code>b</code>, then store the result in variable <code>a</code>.
 - <code>div a b</code> - Divide the value of <code>a</code> by the value of <code>b</code>, truncate the result to an integer, then store the result in variable <code>a</code>. (Here, "truncate" means to round the value toward zero.)
 - <code>mod a b</code> - Divide the value of <code>a</code> by the value of <code>b</code>, then store the <em>remainder</em> in variable <code>a</code>. (This is also called the [modulo](https://en.wikipedia.org/wiki/Modulo_operation) operation.)
 - <code>eql a b</code> - If the value of <code>a</code> and <code>b</code> are equal, then store the value <code>1</code> in variable <code>a</code>. Otherwise, store the value <code>0</code> in variable <code>a</code>.

In all of these instructions, <code>a</code> and <code>b</code> are placeholders; <code>a</code> will always be the variable where the result of the operation is stored (one of <code>w</code>, <code>x</code>, <code>y</code>, or <code>z</code>), while <code>b</code> can be either a variable or a number. Numbers can be positive or negative, but will always be integers.

The ALU has no <em>jump</em> instructions; in an ALU program, every instruction is run exactly once in order from top to bottom. The program halts after the last instruction has finished executing.

(Program authors should be especially cautious; attempting to execute <code>div</code> with <code>b=0</code> or attempting to execute <code>mod</code> with <code>a<0</code> or <code>b<=0</code>  will cause the program to crash and might even damage the ALU. These operations are never intended in any serious ALU program.)

For example, here is an ALU program which takes an input number, negates it, and stores it in <code>x</code>:

<pre>
<code>inp x
mul x -1
</code>
</pre>

Here is an ALU program which takes two input numbers, then sets <code>z</code> to <code>1</code> if the second input number is three times larger than the first input number, or sets <code>z</code> to <code>0</code> otherwise:

<pre>
<code>inp z
inp x
mul z 3
eql z x
</code>
</pre>

Here is an ALU program which takes a non-negative integer as input, converts it into binary, and stores the lowest (1's) bit in <code>z</code>, the second-lowest (2's) bit in <code>y</code>, the third-lowest (4's) bit in <code>x</code>, and the fourth-lowest (8's) bit in <code>w</code>:

<pre>
<code>inp w
add z w
mod z 2
div w 2
add y w
mod y 2
div w 2
add x w
mod x 2
div w 2
mod w 2
</code>
</pre>

Once you have built a replacement ALU, you can install it in the submarine, which will immediately resume what it was doing when the ALU failed: validating the submarine's <em>model number</em>. To do this, the ALU will run the MOdel Number Automatic Detector program (MONAD, your puzzle input).

Submarine model numbers are always <em>fourteen-digit numbers</em> consisting only of digits <code>1</code> through <code>9</code>. The digit <code>0</code> <em>cannot</em> appear in a model number.

When MONAD checks a hypothetical fourteen-digit model number, it uses fourteen separate <code>inp</code> instructions, each expecting a <em>single digit</em> of the model number in order of most to least significant. (So, to check the model number <code>13579246899999</code>, you would give <code>1</code> to the first <code>inp</code> instruction, <code>3</code> to the second <code>inp</code> instruction, <code>5</code> to the third <code>inp</code> instruction, and so on.) This means that when operating MONAD, each input instruction should only ever be given an integer value of at least <code>1</code> and at most <code>9</code>.

Then, after MONAD has finished running all of its instructions, it will indicate that the model number was <em>valid</em> by leaving a <code>0</code> in variable <code>z</code>. However, if the model number was <em>invalid</em>, it will leave some other non-zero value in <code>z</code>.

MONAD imposes additional, mysterious restrictions on model numbers, and legend says the last copy of the MONAD documentation was eaten by a [tanuki](https://en.wikipedia.org/wiki/Japanese_raccoon_dog). You'll need to <em>figure out what MONAD does</em> some other way.

To enable as many submarine features as possible, find the largest valid fourteen-digit model number that contains no <code>0</code> digits. <em>What is the largest model number accepted by MONAD?</em>


