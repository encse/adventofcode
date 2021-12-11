original source: [https://adventofcode.com/2021/day/11](https://adventofcode.com/2021/day/11)
## --- Day 11: Dumbo Octopus ---
You enter a large cavern full of rare bioluminescent [dumbo octopuses](https://www.youtube.com/watch?v=eih-VSaS2g0)! They seem to not like the Christmas lights on your submarine, so you turn them off for now.

There are 100 octopuses arranged neatly in a 10 by 10 grid. Each octopus slowly gains <em>energy</em> over time and <em>flashes</em> brightly for a moment when its energy is full. Although your lights are off, maybe you could navigate through the cave without disturbing the octopuses if you could predict when the flashes of light will happen.

Each octopus has an <em>energy level</em> - your submarine can remotely measure the energy level of each octopus (your puzzle input). For example:

<pre>
<code>5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526
</code>
</pre>

The energy level of each octopus is a value between <code>0</code> and <code>9</code>. Here, the top-left octopus has an energy level of <code>5</code>, the bottom-right one has an energy level of <code>6</code>, and so on.

You can model the energy levels and flashes of light in <em>steps</em>. During a single step, the following occurs:


 - First, the energy level of each octopus increases by <code>1</code>.
 - Then, any octopus with an energy level greater than <code>9</code> <em>flashes</em>. This increases the energy level of all adjacent octopuses by <code>1</code>, including octopuses that are diagonally adjacent. If this causes an octopus to have an energy level greater than <code>9</code>, it <em>also flashes</em>. This process continues as long as new octopuses keep having their energy level increased beyond <code>9</code>. (An octopus can only flash <em>at most once per step</em>.)
 - Finally, any octopus that flashed during this step has its energy level set to <code>0</code>, as it used all of its energy to flash.

Adjacent flashes can cause an octopus to flash on a step even if it begins that step with very little energy. Consider the middle octopus with <code>1</code> energy in this situation:

<pre>
<code>Before any steps:
11111
19991
19191
19991
11111

After step 1:
34543
4<em>000</em>4
5<em>000</em>5
4<em>000</em>4
34543

After step 2:
45654
51115
61116
51115
45654
</code>
</pre>

An octopus is <em>highlighted</em> when it flashed during the given step.

Here is how the larger example above progresses:

<pre>
<code>Before any steps:
5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526

After step 1:
6594254334
3856965822
6375667284
7252447257
7468496589
5278635756
3287952832
7993992245
5957959665
6394862637

After step 2:
88<em>0</em>7476555
5<em>0</em>89<em>0</em>87<em>0</em>54
85978896<em>0</em>8
84857696<em>00</em>
87<em>00</em>9<em>0</em>88<em>00</em>
66<em>000</em>88989
68<em>0000</em>5943
<em>000000</em>7456
9<em>000000</em>876
87<em>0000</em>6848

After step 3:
<em>00</em>5<em>0</em>9<em>00</em>866
85<em>00</em>8<em>00</em>575
99<em>000000</em>39
97<em>000000</em>41
9935<em>0</em>8<em>00</em>63
77123<em>00000</em>
791125<em>000</em>9
221113<em>0000</em>
<em>0</em>421125<em>000</em>
<em>00</em>21119<em>000</em>

After step 4:
2263<em>0</em>31977
<em>0</em>923<em>0</em>31697
<em>00</em>3222115<em>0</em>
<em>00</em>41111163
<em>00</em>76191174
<em>00</em>53411122
<em>00</em>4236112<em>0</em>
5532241122
1532247211
113223<em>0</em>211

After step 5:
4484144<em>000</em>
2<em>0</em>44144<em>000</em>
2253333493
1152333274
11873<em>0</em>3285
1164633233
1153472231
6643352233
2643358322
2243341322

After step 6:
5595255111
3155255222
33644446<em>0</em>5
2263444496
2298414396
2275744344
2264583342
7754463344
3754469433
3354452433

After step 7:
67<em>0</em>7366222
4377366333
4475555827
34966557<em>0</em>9
35<em>00</em>6256<em>0</em>9
35<em>0</em>9955566
3486694453
8865585555
486558<em>0</em>644
4465574644

After step 8:
7818477333
5488477444
5697666949
46<em>0</em>876683<em>0</em>
473494673<em>0</em>
474<em>00</em>97688
69<em>0000</em>7564
<em>000000</em>9666
8<em>00000</em>4755
68<em>0000</em>7755

After step 9:
9<em>0</em>6<em>0000</em>644
78<em>00000</em>976
69<em>000000</em>8<em>0</em>
584<em>00000</em>82
5858<em>0000</em>93
69624<em>00000</em>
8<em>0</em>2125<em>000</em>9
222113<em>000</em>9
9111128<em>0</em>97
7911119976

After step 10:
<em>0</em>481112976
<em>00</em>31112<em>00</em>9
<em>00</em>411125<em>0</em>4
<em>00</em>811114<em>0</em>6
<em>00</em>991113<em>0</em>6
<em>00</em>93511233
<em>0</em>44236113<em>0</em>
553225235<em>0</em>
<em>0</em>53225<em>0</em>6<em>00</em>
<em>00</em>3224<em>0000</em>
</code>
</pre>


After step 10, there have been a total of <code>204</code> flashes. Fast forwarding, here is the same configuration every 10 steps:


<pre>
<code>After step 20:
3936556452
56865568<em>0</em>6
449655569<em>0</em>
444865558<em>0</em>
445686557<em>0</em>
568<em>00</em>86577
7<em>00000</em>9896
<em>0000000</em>344
6<em>000000</em>364
46<em>0000</em>9543

After step 30:
<em>0</em>643334118
4253334611
3374333458
2225333337
2229333338
2276733333
2754574565
5544458511
9444447111
7944446119

After step 40:
6211111981
<em>0</em>421111119
<em>00</em>42111115
<em>000</em>3111115
<em>000</em>3111116
<em>00</em>65611111
<em>0</em>532351111
3322234597
2222222976
2222222762

After step 50:
9655556447
48655568<em>0</em>5
448655569<em>0</em>
445865558<em>0</em>
457486557<em>0</em>
57<em>000</em>86566
6<em>00000</em>9887
8<em>000000</em>533
68<em>00000</em>633
568<em>0000</em>538

After step 60:
25333342<em>00</em>
274333464<em>0</em>
2264333458
2225333337
2225333338
2287833333
3854573455
1854458611
1175447111
1115446111

After step 70:
8211111164
<em>0</em>421111166
<em>00</em>42111114
<em>000</em>4211115
<em>0000</em>211116
<em>00</em>65611111
<em>0</em>532351111
7322235117
5722223475
4572222754

After step 80:
1755555697
59655556<em>0</em>9
448655568<em>0</em>
445865558<em>0</em>
457<em>0</em>86557<em>0</em>
57<em>000</em>86566
7<em>00000</em>8666
<em>0000000</em>99<em>0</em>
<em>0000000</em>8<em>00</em>
<em>0000000000</em>

After step 90:
7433333522
2643333522
2264333458
2226433337
2222433338
2287833333
2854573333
4854458333
3387779333
3333333333

After step 100:
<em>0</em>397666866
<em>0</em>749766918
<em>00</em>53976933
<em>000</em>4297822
<em>000</em>4229892
<em>00</em>53222877
<em>0</em>532222966
9322228966
7922286866
6789998766
</code>
</pre>

After 100 steps, there have been a total of <code><em>1656</em></code> flashes.

Given the starting energy levels of the dumbo octopuses in your cavern, simulate 100 steps. <em>How many total flashes are there after 100 steps?</em>


## --- Part Two ---
It seems like the individual flashes aren't bright enough to navigate. However, you might have a better option: the flashes seem to be <em>synchronizing</em>!

In the example above, the first time all octopuses flash simultaneously is step <code><em>195</em></code>:

<pre>
<code>After step 193:
5877777777
8877777777
7777777777
7777777777
7777777777
7777777777
7777777777
7777777777
7777777777
7777777777

After step 194:
6988888888
9988888888
8888888888
8888888888
8888888888
8888888888
8888888888
8888888888
8888888888
8888888888

After step 195:
<em>0000000000</em>
<em>0000000000</em>
<em>0000000000</em>
<em>0000000000</em>
<em>0000000000</em>
<em>0000000000</em>
<em>0000000000</em>
<em>0000000000</em>
<em>0000000000</em>
<em>0000000000</em>
</code>
</pre>

If you can calculate the exact moments when the octopuses will all flash simultaneously, you should be able to navigate through the cavern. <em>What is the first step during which all octopuses flash?</em>


