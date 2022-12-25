original source: [https://adventofcode.com/2022/day/25](https://adventofcode.com/2022/day/25)
## --- Day 25: Full of Hot Air ---
As the expedition finally reaches the extraction point, several large [hot air balloons](https://en.wikipedia.org/wiki/Hot_air_balloon) drift down to meet you. Crews quickly start unloading the equipment the balloons brought: many hot air balloon kits, some fuel tanks, and a <em>fuel heating machine</em>.

The fuel heating machine is a new addition to the process. When this mountain was a volcano, the ambient temperature was more reasonable; now, it's so cold that the fuel won't work at all without being warmed up first.

The Elves, seemingly in an attempt to make the new machine feel welcome, have already attached a pair of [googly eyes](https://en.wikipedia.org/wiki/Googly_eyes) and started calling it "Bob".

To heat the fuel, Bob needs to know the total amount of fuel that will be processed ahead of time so it can correctly calibrate heat output and flow rate. This amount is simply the <em>sum</em> of the fuel requirements of all of the hot air balloons, and those fuel requirements are even listed clearly on the side of each hot air balloon's burner.

You assume the Elves will have no trouble adding up some numbers and are about to go back to figuring out which balloon is yours when you get a tap on the shoulder. Apparently, the fuel requirements use numbers written in a format the Elves don't recognize; predictably, they'd like your help deciphering them.

You make a list of all of the fuel requirements (your puzzle input), but you don't recognize the number format either. For example:

<pre>
<code>1=-0-2
12111
2=0=
21
2=01
111
20012
112
1=-1=
1-12
12
1=
122
</code>
</pre>

Fortunately, Bob is labeled with a support phone number. Not to be deterred, you call and ask for help.

"That's right, just supply the fuel amount to the-- oh, for more than one burner? No problem, you just need to add together our Special Numeral-Analogue Fuel Units. Patent pending! They're way better than normal numbers for--"

You mention that it's quite cold up here and ask if they can skip ahead.

"Okay, our Special Numeral-Analogue Fuel Units - SNAFU for short - are sort of like normal numbers. You know how starting on the right, normal numbers have a ones place, a tens place, a hundreds place, and so on, where the digit in each place tells you how many of that value you have?"

"SNAFU works the same way, except it uses powers of five instead of ten. Starting from the right, you have a ones place, a fives place, a twenty-fives place, a one-hundred-and-twenty-fives place, and so on. It's that easy!"

You ask why some of the digits look like <code>-</code> or <code>=</code> instead of "digits".

"You know, I never did ask the engineers why they did that. Instead of using digits four through zero, the digits are <code><em>2</em></code>, <code><em>1</em></code>, <code><em>0</em></code>, <em>minus</em> (written <code>-</code>), and <em>double-minus</em> (written <code>=</code>). Minus is worth -1, and double-minus is worth -2."

"So, because ten (in normal numbers) is two fives and no ones, in SNAFU it is written <code>20</code>. Since eight (in normal numbers) is two fives minus two ones, it is written <code>2=</code>."

"You can do it the other direction, too. Say you have the SNAFU number <code>2=-01</code>. That's <code>2</code> in the 625s place, <code>=</code> (double-minus) in the 125s place, <code>-</code> (minus) in the 25s place, <code>0</code> in the 5s place, and <code>1</code> in the 1s place. (2 times 625) plus (-2 times 125) plus (-1 times 25) plus (0 times 5) plus (1 times 1). That's 1250 plus -250 plus -25 plus 0 plus 1. <em>976</em>!"

"I see here that you're connected via our premium uplink service, so I'll transmit our handy SNAFU brochure to you now. Did you need anything else?"

You ask if the fuel will even work in these temperatures.

"Wait, it's <em>how</em> cold? There's no <em>way</em> the fuel - or <em>any</em> fuel - would work in those conditions! There are only a few places in the-- where did you say you are again?"

Just then, you notice one of the Elves pour a few drops from a snowflake-shaped container into one of the fuel tanks, thank the support representative for their time, and disconnect the call.

The SNAFU brochure contains a few more examples of decimal ("normal") numbers and their SNAFU counterparts:

<pre>
<code>  Decimal          SNAFU
        1              1
        2              2
        3             1=
        4             1-
        5             10
        6             11
        7             12
        8             2=
        9             2-
       10             20
       15            1=0
       20            1-0
     2022         1=11-2
    12345        1-0---0
314159265  1121-1110-1=0
</code>
</pre>

Based on this process, the SNAFU numbers in the example above can be converted to decimal numbers as follows:

<pre>
<code> SNAFU  Decimal
1=-0-2     1747
 12111      906
  2=0=      198
    21       11
  2=01      201
   111       31
 20012     1257
   112       32
 1=-1=      353
  1-12      107
    12        7
    1=        3
   122       37
</code>
</pre>

In decimal, the sum of these numbers is <code>4890</code>.

As you go to input this number on Bob's console, you discover that some buttons you expected are missing. Instead, you are met with buttons labeled <code>=</code>, <code>-</code>, <code>0</code>, <code>1</code>, and <code>2</code>. Bob needs the input value expressed as a SNAFU number, not in decimal.

Reversing the process, you can determine that for the decimal number <code>4890</code>, the SNAFU number you need to supply to Bob's console is <code><em>2=-1=0</em></code>.

The Elves are starting to get cold. <em>What SNAFU number do you supply to Bob's console?</em>


## --- Part Two ---
The hot air balloons quickly carry you to the North Pole. As soon as you land, most of the expedition is escorted directly to a small building attached to the reindeer stables.

The <em>head smoothie chef</em> has just finished warming up the industrial-grade smoothie blender as you arrive. It will take <em>50 stars</em> to fill the blender. The expedition Elves turn their attention to you, and you begin emptying the fruit from your pack onto the table.

As you do, a very young Elf - one you recognize from the expedition team - approaches the table and holds up a single <em>star</em> fruit he found. The head smoothie chef places it in the blender.

Only <em>49 stars</em> to go.


