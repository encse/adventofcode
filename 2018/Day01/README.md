original source: [https://adventofcode.com/2018/day/1](https://adventofcode.com/2018/day/1)
## --- Day 1: Chronal Calibration ---
"We've detected some temporal anomalies," one of Santa's Elves at the Temporal Anomaly Research and Detection Instrument Station tells you. She sounded pretty worried when she called you down here. "At 500-year intervals into the past, someone has been changing Santa's history!"

"The good news is that the changes won't propagate to our time stream for another 25 days, and we have a device" - she attaches something to your wrist - "that will let you fix the changes with no such propagation delay. It's configured to send you 500 years further into the past every few days; that was the best we could do on such short notice."

"The bad news is that we are detecting roughly <em>fifty</em> anomalies throughout time; the device will indicate fixed anomalies with <em>stars</em>. The other bad news is that we only have one device and you're the best person for the job! Good lu--" She taps a button on the device and you suddenly feel like you're falling. To save Christmas, you need to get all <em>fifty stars</em> by December 25th.

Collect stars by solving puzzles.  Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first.  Each puzzle grants <em>one star</em>. Good luck!

After feeling like you've been falling for a few minutes, you look at the device's tiny screen. "Error: Device must be calibrated before first use. Frequency drift detected. Cannot maintain destination lock." Below the message, the device shows a sequence of changes in frequency (your puzzle input). A value like <code>+6</code> means the current frequency increases by <code>6</code>; a value like <code>-3</code> means the current frequency decreases by <code>3</code>.

For example, if the device displays frequency changes of <code>+1, -2, +3, +1</code>, then starting from a frequency of zero, the following changes would occur:


 - Current frequency <code> 0</code>, change of <code>+1</code>; resulting frequency <code> 1</code>.
 - Current frequency <code> 1</code>, change of <code>-2</code>; resulting frequency <code>-1</code>.
 - Current frequency <code>-1</code>, change of <code>+3</code>; resulting frequency <code> 2</code>.
 - Current frequency <code> 2</code>, change of <code>+1</code>; resulting frequency <code> 3</code>.

In this example, the resulting frequency is <code>3</code>.

Here are other example situations:


 - <code>+1, +1, +1</code> results in <code> 3</code>
 - <code>+1, +1, -2</code> results in <code> 0</code>
 - <code>-1, -2, -3</code> results in <code>-6</code>

Starting with a frequency of zero, <em>what is the resulting frequency</em> after all of the changes in frequency have been applied?


## --- Part Two ---
You notice that the device repeats the same frequency change list over and over. To calibrate the device, you need to find the first frequency it reaches <em>twice</em>.

For example, using the same list of changes above, the device would loop as follows:


 - Current frequency <code> 0</code>, change of <code>+1</code>; resulting frequency <code> 1</code>.
 - Current frequency <code> 1</code>, change of <code>-2</code>; resulting frequency <code>-1</code>.
 - Current frequency <code>-1</code>, change of <code>+3</code>; resulting frequency <code> 2</code>.
 - Current frequency <code> 2</code>, change of <code>+1</code>; resulting frequency <code> 3</code>.
 - (At this point, the device continues from the start of the list.)
 - Current frequency <code> 3</code>, change of <code>+1</code>; resulting frequency <code> 4</code>.
 - Current frequency <code> 4</code>, change of <code>-2</code>; resulting frequency <code> 2</code>, which has already been seen.

In this example, the first frequency reached twice is <code>2</code>. Note that your device might need to repeat its list of frequency changes many times before a duplicate frequency is found, and that duplicates might be found while in the middle of processing the list.

Here are other examples:


 - <code>+1, -1</code> first reaches <code>0</code> twice.
 - <code>+3, +3, +4, -2, -4</code> first reaches <code>10</code> twice.
 - <code>-6, +3, +8, +5, -6</code> first reaches <code>5</code> twice.
 - <code>+7, +7, -2, -7, -4</code> first reaches <code>14</code> twice.

<em>What is the first frequency your device reaches twice?</em>


