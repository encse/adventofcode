original source: [https://adventofcode.com/2021/day/9](https://adventofcode.com/2021/day/9)
## --- Day 9: Smoke Basin ---
These caves seem to be [lava tubes](https://en.wikipedia.org/wiki/Lava_tube). Parts are even still volcanically active; small hydrothermal vents release smoke into the caves that slowly settles like rain.

If you can model how the smoke flows through the caves, you might be able to avoid it and be that much safer. The submarine generates a heightmap of the floor of the nearby caves for you (your puzzle input).

Smoke flows to the lowest point of the area it's in. For example, consider the following heightmap:

<pre>
<code>2<em>1</em>9994321<em>0</em>
3987894921
98<em>5</em>6789892
8767896789
989996<em>5</em>678
</code>
</pre>

Each number corresponds to the height of a particular location, where <code>9</code> is the highest and <code>0</code> is the lowest a location can be.

Your first goal is to find the <em>low points</em> - the locations that are lower than any of its adjacent locations. Most locations have four adjacent locations (up, down, left, and right); locations on the edge or corner of the map have three or two adjacent locations, respectively. (Diagonal locations do not count as adjacent.)

In the above example, there are <em>four</em> low points, all highlighted: two are in the first row (a <code>1</code> and a <code>0</code>), one is in the third row (a <code>5</code>), and one is in the bottom row (also a <code>5</code>). All other locations on the heightmap have some lower adjacent location, and so are not low points.

The <em>risk level</em> of a low point is <em>1 plus its height</em>. In the above example, the risk levels of the low points are <code>2</code>, <code>1</code>, <code>6</code>, and <code>6</code>. The sum of the risk levels of all low points in the heightmap is therefore <code><em>15</em></code>.

Find all of the low points on your heightmap. <em>What is the sum of the risk levels of all low points on your heightmap?</em>


## --- Part Two ---
Next, you need to find the largest basins so you know what areas are most important to avoid.

A <em>basin</em> is all locations that eventually flow downward to a single low point. Therefore, every low point has a basin, although some basins are very small. Locations of height <code>9</code> do not count as being in any basin, and all other locations will always be part of exactly one basin.

The <em>size</em> of a basin is the number of locations within the basin, including the low point. The example above has four basins.

The top-left basin, size <code>3</code>:

<pre>
<code><em>21</em>99943210
<em>3</em>987894921
9856789892
8767896789
9899965678
</code>
</pre>

The top-right basin, size <code>9</code>:

<pre>
<code>21999<em>43210</em>
398789<em>4</em>9<em>21</em>
985678989<em>2</em>
8767896789
9899965678
</code>
</pre>

The middle basin, size <code>14</code>:

<pre>
<code>2199943210
39<em>878</em>94921
9<em>85678</em>9892
<em>87678</em>96789
9<em>8</em>99965678
</code>
</pre>

The bottom-right basin, size <code>9</code>:

<pre>
<code>2199943210
3987894921
9856789<em>8</em>92
876789<em>678</em>9
98999<em>65678</em>
</code>
</pre>

Find the three largest basins and multiply their sizes together. In the above example, this is <code>9 * 14 * 9 = <em>1134</em></code>.

<em>What do you get if you multiply together the sizes of the three largest basins?</em>


