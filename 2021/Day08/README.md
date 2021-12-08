original source: [https://adventofcode.com/2021/day/8](https://adventofcode.com/2021/day/8)
## --- Day 8: Seven Segment Search ---
You barely reach the safety of the cave when the whale smashes into the cave mouth, collapsing it. Sensors indicate another exit to this cave at a much greater depth, so you have no choice but to press on.

As your submarine slowly makes its way through the cave system, you notice that the four-digit [seven-segment displays](https://en.wikipedia.org/wiki/Seven-segment_display) in your submarine are malfunctioning; they must have been damaged during the escape. You'll be in a lot of trouble without them, so you'd better figure out what's wrong.

Each digit of a seven-segment display is rendered by turning on or off any of seven segments named <code>a</code> through <code>g</code>:

<pre>
<code>  0:      1:      2:      3:      4:
 <em>aaaa</em>    ....    <em>aaaa    aaaa</em>    ....
<em>b    c</em>  .    <em>c</em>  .    <em>c</em>  .    <em>c  b    c</em>
<em>b    c</em>  .    <em>c</em>  .    <em>c</em>  .    <em>c  b    c</em>
 ....    ....    <em>dddd    dddd    dddd</em>
<em>e    f</em>  .    <em>f  e</em>    .  .    <em>f</em>  .    <em>f</em>
<em>e    f</em>  .    <em>f  e</em>    .  .    <em>f</em>  .    <em>f</em>
 <em>gggg</em>    ....    <em>gggg    gggg</em>    ....

  5:      6:      7:      8:      9:
 <em>aaaa    aaaa    aaaa    aaaa    aaaa</em>
<em>b</em>    .  <em>b</em>    .  .    <em>c  b    c  b    c</em>
<em>b</em>    .  <em>b</em>    .  .    <em>c  b    c  b    c</em>
 <em>dddd    dddd</em>    ....    <em>dddd    dddd</em>
.    <em>f  e    f</em>  .    <em>f  e    f</em>  .    <em>f</em>
.    <em>f  e    f</em>  .    <em>f  e    f</em>  .    <em>f</em>
 <em>gggg    gggg</em>    ....    <em>gggg    gggg</em>
</code>
</pre>

So, to render a <code>1</code>, only segments <code>c</code> and <code>f</code> would be turned on; the rest would be off. To render a <code>7</code>, only segments <code>a</code>, <code>c</code>, and <code>f</code> would be turned on.

The problem is that the signals which control the segments have been mixed up on each display. The submarine is still trying to display numbers by producing output on signal wires <code>a</code> through <code>g</code>, but those wires are connected to segments <em>randomly</em>. Worse, the wire/segment connections are mixed up separately for each four-digit display! (All of the digits <em>within</em> a display use the same connections, though.)

So, you might know that only signal wires <code>b</code> and <code>g</code> are turned on, but that doesn't mean <em>segments</em> <code>b</code> and <code>g</code> are turned on: the only digit that uses two segments is <code>1</code>, so it must mean segments <code>c</code> and <code>f</code> are meant to be on. With just that information, you still can't tell which wire (<code>b</code>/<code>g</code>) goes to which segment (<code>c</code>/<code>f</code>). For that, you'll need to collect more information.

For each display, you watch the changing signals for a while, make a note of <em>all ten unique signal patterns</em> you see, and then write down a single <em>four digit output value</em> (your puzzle input). Using the signal patterns, you should be able to work out which pattern corresponds to which digit.

For example, here is what you might see in a single entry in your notes:

<pre>
<code>acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |
cdfeb fcadb cdfeb cdbaf</code>
</pre>

(The entry is wrapped here to two lines so it fits; in your notes, it will all be on a single line.)

Each entry consists of ten <em>unique signal patterns</em>, a <code>|</code> delimiter, and finally the <em>four digit output value</em>. Within an entry, the same wire/segment connections are used (but you don't know what the connections actually are). The unique signal patterns correspond to the ten different ways the submarine tries to render a digit using the current wire/segment connections. Because <code>7</code> is the only digit that uses three segments, <code>dab</code> in the above example means that to render a <code>7</code>, signal lines <code>d</code>, <code>a</code>, and <code>b</code> are on. Because <code>4</code> is the only digit that uses four segments, <code>eafb</code> means that to render a <code>4</code>, signal lines <code>e</code>, <code>a</code>, <code>f</code>, and <code>b</code> are on.

Using this information, you should be able to work out which combination of signal wires corresponds to each of the ten digits. Then, you can decode the four digit output value. Unfortunately, in the above example, all of the digits in the output value (<code>cdfeb fcadb cdfeb cdbaf</code>) use five segments and are more difficult to deduce.

For now, <em>focus on the easy digits</em>. Consider this larger example:

<pre>
<code>be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb |
<em>fdgacbe</em> cefdb cefbgd <em>gcbe</em>
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec |
fcgedb <em>cgb</em> <em>dgebacf</em> <em>gc</em>
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef |
<em>cg</em> <em>cg</em> fdcagb <em>cbg</em>
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega |
efabcd cedba gadfec <em>cb</em>
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga |
<em>gecf</em> <em>egdcabf</em> <em>bgf</em> bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf |
<em>gebdcfa</em> <em>ecba</em> <em>ca</em> <em>fadegcb</em>
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf |
<em>cefg</em> dcbef <em>fcge</em> <em>gbcadfe</em>
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd |
<em>ed</em> bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg |
<em>gbdfcae</em> <em>bgc</em> <em>cg</em> <em>cgb</em>
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc |
<em>fgae</em> cfgab <em>fg</em> bagce
</code>
</pre>

Because the digits <code>1</code>, <code>4</code>, <code>7</code>, and <code>8</code> each use a unique number of segments, you should be able to tell which combinations of signals correspond to those digits. Counting <em>only digits in the output values</em> (the part after <code>|</code> on each line), in the above example, there are <code><em>26</em></code> instances of digits that use a unique number of segments (highlighted above).

<em>In the output values, how many times do digits <code>1</code>, <code>4</code>, <code>7</code>, or <code>8</code> appear?</em>


## --- Part Two ---
Through a little deduction, you should now be able to determine the remaining digits. Consider again the first example above:

<pre>
<code>acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab |
cdfeb fcadb cdfeb cdbaf</code>
</pre>

After some careful analysis, the mapping between signal wires and segments only make sense in the following configuration:

<pre>
<code> dddd
e    a
e    a
 ffff
g    b
g    b
 cccc
</code>
</pre>

So, the unique signal patterns would correspond to the following digits:


 - <code>acedgfb</code>: <code>8</code>
 - <code>cdfbe</code>: <code>5</code>
 - <code>gcdfa</code>: <code>2</code>
 - <code>fbcad</code>: <code>3</code>
 - <code>dab</code>: <code>7</code>
 - <code>cefabd</code>: <code>9</code>
 - <code>cdfgeb</code>: <code>6</code>
 - <code>eafb</code>: <code>4</code>
 - <code>cagedb</code>: <code>0</code>
 - <code>ab</code>: <code>1</code>

Then, the four digits of the output value can be decoded:


 - <code>cdfeb</code>: <code><em>5</em></code>
 - <code>fcadb</code>: <code><em>3</em></code>
 - <code>cdfeb</code>: <code><em>5</em></code>
 - <code>cdbaf</code>: <code><em>3</em></code>

Therefore, the output value for this entry is <code><em>5353</em></code>.

Following this same process for each entry in the second, larger example above, the output value of each entry can be determined:


 - <code>fdgacbe cefdb cefbgd gcbe</code>: <code>8394</code>
 - <code>fcgedb cgb dgebacf gc</code>: <code>9781</code>
 - <code>cg cg fdcagb cbg</code>: <code>1197</code>
 - <code>efabcd cedba gadfec cb</code>: <code>9361</code>
 - <code>gecf egdcabf bgf bfgea</code>: <code>4873</code>
 - <code>gebdcfa ecba ca fadegcb</code>: <code>8418</code>
 - <code>cefg dcbef fcge gbcadfe</code>: <code>4548</code>
 - <code>ed bcgafe cdgba cbgef</code>: <code>1625</code>
 - <code>gbdfcae bgc cg cgb</code>: <code>8717</code>
 - <code>fgae cfgab fg bagce</code>: <code>4315</code>

Adding all of the output values in this larger example produces <code><em>61229</em></code>.

For each entry, determine all of the wire/segment connections and decode the four-digit output values. <em>What do you get if you add up all of the output values?</em>


