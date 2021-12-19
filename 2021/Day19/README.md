original source: [https://adventofcode.com/2021/day/19](https://adventofcode.com/2021/day/19)
## --- Day 19: Beacon Scanner ---
As your [probe](17) drifted down through this area, it released an assortment of <em>beacons</em> and <em>scanners</em> into the water. It's difficult to navigate in the pitch black open waters of the ocean trench, but if you can build a map of the trench using data from the scanners, you should be able to safely reach the bottom.

The beacons and scanners float motionless in the water; they're designed to maintain the same position for long periods of time. Each scanner is capable of detecting all beacons in a large cube centered on the scanner; beacons that are at most 1000 units away from the scanner in each of the three axes (<code>x</code>, <code>y</code>, and <code>z</code>) have their precise position determined relative to the scanner. However, scanners cannot detect other scanners. The submarine has automatically summarized the relative positions of beacons detected by each scanner (your puzzle input).

For example, if a scanner is at <code>x,y,z</code> coordinates <code>500,0,-500</code> and there are beacons at <code>-500,1000,-1500</code> and <code>1501,0,-500</code>, the scanner could report that the first beacon is at <code>-1000,1000,-1000</code> (relative to the scanner) but would not detect the second beacon at all.

Unfortunately, while each scanner can report the positions of all detected beacons relative to itself, <em>the scanners do not know their own position</em>. You'll need to determine the positions of the beacons and scanners yourself.

The scanners and beacons map a single contiguous 3d region. This region can be reconstructed by finding pairs of scanners that have overlapping detection regions such that there are <em>at least 12 beacons</em> that both scanners detect within the overlap. By establishing 12 common beacons, you can precisely determine where the scanners are relative to each other, allowing you to reconstruct the beacon map one scanner at a time.

For a moment, consider only two dimensions. Suppose you have the following scanner reports:

<pre>
<code>--- scanner 0 ---
0,2
4,1
3,3

--- scanner 1 ---
-1,-1
-5,0
-2,1
</code>
</pre>

Drawing <code>x</code> increasing rightward, <code>y</code> increasing upward, scanners as <code>S</code>, and beacons as <code>B</code>, scanner <code>0</code> detects this:

<pre>
<code>...B.
B....
....B
S....
</code>
</pre>

Scanner <code>1</code> detects this:

<pre>
<code>...B..
B....S
....B.
</code>
</pre>

For this example, assume scanners only need 3 overlapping beacons. Then, the beacons visible to both scanners overlap to produce the following complete map:

<pre>
<code>...B..
B....S
....B.
S.....
</code>
</pre>

Unfortunately, there's a second problem: the scanners also don't know their <em>rotation or facing direction</em>. Due to magnetic alignment, each scanner is rotated some integer number of 90-degree turns around all of the <code>x</code>, <code>y</code>, and <code>z</code> axes. That is, one scanner might call a direction positive <code>x</code>, while another scanner might call that direction negative <code>y</code>. Or, two scanners might agree on which direction is positive <code>x</code>, but one scanner might be upside-down from the perspective of the other scanner. In total, each scanner could be in any of 24 different orientations: facing positive or negative <code>x</code>, <code>y</code>, or <code>z</code>, and considering any of four directions "up" from that facing.

For example, here is an arrangement of beacons as seen from a scanner in the same position but in different orientations:

<pre>
<code>--- scanner 0 ---
-1,-1,1
-2,-2,2
-3,-3,3
-2,-3,1
5,6,-4
8,0,7

--- scanner 0 ---
1,-1,1
2,-2,2
3,-3,3
2,-1,3
-5,4,-6
-8,-7,0

--- scanner 0 ---
-1,-1,-1
-2,-2,-2
-3,-3,-3
-1,-3,-2
4,6,5
-7,0,8

--- scanner 0 ---
1,1,-1
2,2,-2
3,3,-3
1,3,-2
-4,-6,5
7,0,8

--- scanner 0 ---
1,1,1
2,2,2
3,3,3
3,1,2
-6,-4,-5
0,7,-8
</code>
</pre>

By finding pairs of scanners that both see at least 12 of the same beacons, you can assemble the entire map. For example, consider the following report:

<pre>
<code>--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575
-876,649,763
-618,-824,-621
553,345,-567
474,580,667
-447,-329,318
-584,868,-557
544,-627,-890
564,392,-477
455,729,728
-892,524,684
-689,845,-530
423,-701,434
7,-33,-71
630,319,-379
443,580,662
-789,900,-551
459,-707,401

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600
729,430,532
-500,-761,534
-322,571,750
-466,-666,-811
-429,-592,574
-355,545,-477
703,-491,-529
-328,-685,520
413,935,-424
-391,539,-444
586,-435,557
-364,-763,-893
807,-499,-711
755,-354,-619
553,889,-390

--- scanner 2 ---
649,640,665
682,-795,504
-784,533,-524
-644,584,-595
-588,-843,648
-30,6,44
-674,560,763
500,723,-460
609,671,-379
-555,-800,653
-675,-892,-343
697,-426,-610
578,704,681
493,664,-388
-671,-858,530
-667,343,800
571,-461,-707
-138,-166,112
-889,563,-600
646,-828,498
640,759,510
-630,509,768
-681,-892,-333
673,-379,-804
-742,-814,-386
577,-820,562

--- scanner 3 ---
-589,542,597
605,-692,669
-500,565,-823
-660,373,557
-458,-679,-417
-488,449,543
-626,468,-788
338,-750,-386
528,-832,-391
562,-778,733
-938,-730,414
543,643,-506
-524,371,-870
407,773,750
-104,29,83
378,-903,-323
-778,-728,485
426,699,580
-438,-605,-362
-469,-447,-387
509,732,623
647,635,-688
-868,-804,481
614,-800,639
595,780,-596

--- scanner 4 ---
727,592,562
-293,-554,779
441,611,-461
-714,465,-776
-743,427,-804
-660,-479,-426
832,-632,460
927,-485,-438
408,393,-506
466,436,-512
110,16,151
-258,-428,682
-393,719,612
-211,-452,876
808,-476,-593
-575,615,604
-485,667,467
-680,325,-822
-627,-443,-432
872,-547,-609
833,512,582
807,604,487
839,-516,451
891,-625,532
-652,-548,-490
30,-46,-14
</code>
</pre>

Because all coordinates are relative, in this example, all "absolute" positions will be expressed relative to scanner <code>0</code> (using the orientation of scanner <code>0</code> and as if scanner <code>0</code> is at coordinates <code>0,0,0</code>).

Scanners <code>0</code> and <code>1</code> have overlapping detection cubes; the 12 beacons they both detect (relative to scanner <code>0</code>) are at the following coordinates:

<pre>
<code>-618,-824,-621
-537,-823,-458
-447,-329,318
404,-588,-901
544,-627,-890
528,-643,409
-661,-816,-575
390,-675,-793
423,-701,434
-345,-311,381
459,-707,401
-485,-357,347
</code>
</pre>

These same 12 beacons (in the same order) but from the perspective of scanner <code>1</code> are:

<pre>
<code>686,422,578
605,423,415
515,917,-361
-336,658,858
-476,619,847
-460,603,-452
729,430,532
-322,571,750
-355,545,-477
413,935,-424
-391,539,-444
553,889,-390
</code>
</pre>

Because of this, scanner <code>1</code> must be at <code>68,-1246,-43</code> (relative to scanner <code>0</code>).

Scanner <code>4</code> overlaps with scanner <code>1</code>; the 12 beacons they both detect (relative to scanner <code>0</code>) are:

<pre>
<code>459,-707,401
-739,-1745,668
-485,-357,347
432,-2009,850
528,-643,409
423,-701,434
-345,-311,381
408,-1815,803
534,-1912,768
-687,-1600,576
-447,-329,318
-635,-1737,486
</code>
</pre>

So, scanner <code>4</code> is at <code>-20,-1133,1061</code> (relative to scanner <code>0</code>).

Following this process, scanner <code>2</code> must be at <code>1105,-1205,1229</code> (relative to scanner <code>0</code>) and scanner <code>3</code> must be at <code>-92,-2380,-20</code> (relative to scanner <code>0</code>).

The full list of beacons (relative to scanner <code>0</code>) is:

<pre>
<code>-892,524,684
-876,649,763
-838,591,734
-789,900,-551
-739,-1745,668
-706,-3180,-659
-697,-3072,-689
-689,845,-530
-687,-1600,576
-661,-816,-575
-654,-3158,-753
-635,-1737,486
-631,-672,1502
-624,-1620,1868
-620,-3212,371
-618,-824,-621
-612,-1695,1788
-601,-1648,-643
-584,868,-557
-537,-823,-458
-532,-1715,1894
-518,-1681,-600
-499,-1607,-770
-485,-357,347
-470,-3283,303
-456,-621,1527
-447,-329,318
-430,-3130,366
-413,-627,1469
-345,-311,381
-36,-1284,1171
-27,-1108,-65
7,-33,-71
12,-2351,-103
26,-1119,1091
346,-2985,342
366,-3059,397
377,-2827,367
390,-675,-793
396,-1931,-563
404,-588,-901
408,-1815,803
423,-701,434
432,-2009,850
443,580,662
455,729,728
456,-540,1869
459,-707,401
465,-695,1988
474,580,667
496,-1584,1900
497,-1838,-617
527,-524,1933
528,-643,409
534,-1912,768
544,-627,-890
553,345,-567
564,392,-477
568,-2007,-577
605,-1665,1952
612,-1593,1893
630,319,-379
686,-3108,-505
776,-3184,-501
846,-3110,-434
1135,-1161,1235
1243,-1093,1063
1660,-552,429
1693,-557,386
1735,-437,1738
1749,-1800,1813
1772,-405,1572
1776,-675,371
1779,-442,1789
1780,-1548,337
1786,-1538,337
1847,-1591,415
1889,-1729,1762
1994,-1805,1792
</code>
</pre>

In total, there are <code><em>79</em></code> beacons.

Assemble the full map of beacons. <em>How many beacons are there?</em>


## --- Part Two ---
Sometimes, it's a good idea to appreciate just how big the ocean is. Using the [Manhattan distance](https://en.wikipedia.org/wiki/Taxicab_geometry), how far apart do the scanners get?

In the above example, scanners <code>2</code> (<code>1105,-1205,1229</code>) and <code>3</code> (<code>-92,-2380,-20</code>) are the largest Manhattan distance apart. In total, they are <code>1197 + 1175 + 1249 = <em>3621</em></code> units apart.

<em>What is the largest Manhattan distance between any two scanners?</em>


