original source: [https://adventofcode.com/2020/day/20](https://adventofcode.com/2020/day/20)
## --- Day 20: Jurassic Jigsaw ---
The high-speed train leaves the forest and quickly carries you south. You can even see a desert in the distance! Since you have some spare time, you might as well see if there was anything interesting in the image the Mythical Information Bureau satellite captured.

After decoding the satellite messages, you discover that the data actually contains many small images created by the satellite's <em>camera array</em>. The camera array consists of many cameras; rather than produce a single square image, they produce many smaller square image <em>tiles</em> that need to be <em>reassembled back into a single image</em>.

Each camera in the camera array returns a single monochrome <em>image tile</em> with a random unique <em>ID number</em>.  The tiles (your puzzle input) arrived in a random order.

Worse yet, the camera array appears to be malfunctioning: each image tile has been <em>rotated and flipped to a random orientation</em>. Your first task is to reassemble the original image by orienting the tiles so they fit together.

To show how the tiles should be reassembled, each tile's image data includes a border that should line up exactly with its adjacent tiles. All tiles have this border, and the border lines up exactly when the tiles are both oriented correctly. Tiles at the edge of the image also have this border, but the outermost edges won't line up with any other tiles.

For example, suppose you have the following nine tiles:

<pre>
<code>Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###

Tile 1951:
#.##...##.
#.####...#
.....#..##
#...######
.##.#....#
.###.#####
###.##.##.
.###....#.
..#.#..#.#
#...##.#..

Tile 1171:
####...##.
#..##.#..#
##.#..#.#.
.###.####.
..###.####
.##....##.
.#...####.
#.##.####.
####..#...
.....##...

Tile 1427:
###.##.#..
.#..#.##..
.#.##.#..#
#.#.#.##.#
....#...##
...##..##.
...#.#####
.#.####.#.
..#..###.#
..##.#..#.

Tile 1489:
##.#.#....
..##...#..
.##..##...
..#...#...
#####...#.
#..#.#.#.#
...#.#.#..
##.#...##.
..##.##.##
###.##.#..

Tile 2473:
#....####.
#..#.##...
#.##..#...
######.#.#
.#...#.#.#
.#########
.###.#..#.
########.#
##...##.#.
..###.#.#.

Tile 2971:
..#.#....#
#...###...
#.#.###...
##.##..#..
.#####..##
.#..####.#
#..#.#..#.
..####.###
..#.#.###.
...#.#.#.#

Tile 2729:
...#.#.#.#
####.#....
..#.#.....
....#..#.#
.##..##.#.
.#.####...
####.#.#..
##.####...
##..#.##..
#.##...##.

Tile 3079:
#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###...
</code>
</pre>

By rotating, flipping, and rearranging them, you can find a square arrangement that causes all adjacent borders to line up:

<pre>
<code>#...##.#.. ..###..### #.#.#####.
..#.#..#.# ###...#.#. .#..######
.###....#. ..#....#.. ..#.......
###.##.##. .#.#.#..## ######....
.###.##### ##...#.### ####.#..#.
.##.#....# ##.##.###. .#...#.##.
#...###### ####.#...# #.#####.##
.....#..## #...##..#. ..#.###...
#.####...# ##..#..... ..#.......
#.##...##. ..##.#..#. ..#.###...

#.##...##. ..##.#..#. ..#.###...
##..#.##.. ..#..###.# ##.##....#
##.####... .#.####.#. ..#.###..#
####.#.#.. ...#.##### ###.#..###
.#.####... ...##..##. .######.##
.##..##.#. ....#...## #.#.#.#...
....#..#.# #.#.#.##.# #.###.###.
..#.#..... .#.##.#..# #.###.##..
####.#.... .#..#.##.. .######...
...#.#.#.# ###.##.#.. .##...####

...#.#.#.# ###.##.#.. .##...####
..#.#.###. ..##.##.## #..#.##..#
..####.### ##.#...##. .#.#..#.##
#..#.#..#. ...#.#.#.. .####.###.
.#..####.# #..#.#.#.# ####.###..
.#####..## #####...#. .##....##.
##.##..#.. ..#...#... .####...#.
#.#.###... .##..##... .####.##.#
#...###... ..##...#.. ...#..####
..#.#....# ##.#.#.... ...##.....
</code>
</pre>

For reference, the IDs of the above tiles are:

<pre>
<code><em>1951</em>    2311    <em>3079</em>
2729    1427    2473
<em>2971</em>    1489    <em>1171</em>
</code>
</pre>

To check that you've assembled the image correctly, multiply the IDs of the four corner tiles together. If you do this with the assembled tiles from the example above, you get <code>1951 * 3079 * 2971 * 1171</code> = <em><code>20899048083289</code></em>.

Assemble the tiles into an image. <em>What do you get if you multiply together the IDs of the four corner tiles?</em>


## --- Part Two ---
Now, you're ready to <em>check the image for sea monsters</em>.

The borders of each tile are not part of the actual image; start by removing them.

In the example above, the tiles become:

<pre>
<code>.#.#..#. ##...#.# #..#####
###....# .#....#. .#......
##.##.## #.#.#..# #####...
###.#### #...#.## ###.#..#
##.#.... #.##.### #...#.##
...##### ###.#... .#####.#
....#..# ...##..# .#.###..
.####... #..#.... .#......

#..#.##. .#..###. #.##....
#.####.. #.####.# .#.###..
###.#.#. ..#.#### ##.#..##
#.####.. ..##..## ######.#
##..##.# ...#...# .#.#.#..
...#..#. .#.#.##. .###.###
.#.#.... #.##.#.. .###.##.
###.#... #..#.##. ######..

.#.#.### .##.##.# ..#.##..
.####.## #.#...## #.#..#.#
..#.#..# ..#.#.#. ####.###
#..####. ..#.#.#. ###.###.
#####..# ####...# ##....##
#.##..#. .#...#.. ####...#
.#.###.. ##..##.. ####.##.
...###.. .##...#. ..#..###
</code>
</pre>

Remove the gaps to form the actual image:

<pre>
<code>.#.#..#.##...#.##..#####
###....#.#....#..#......
##.##.###.#.#..######...
###.#####...#.#####.#..#
##.#....#.##.####...#.##
...########.#....#####.#
....#..#...##..#.#.###..
.####...#..#.....#......
#..#.##..#..###.#.##....
#.####..#.####.#.#.###..
###.#.#...#.######.#..##
#.####....##..########.#
##..##.#...#...#.#.#.#..
...#..#..#.#.##..###.###
.#.#....#.##.#...###.##.
###.#...#..#.##.######..
.#.#.###.##.##.#..#.##..
.####.###.#...###.#..#.#
..#.#..#..#.#.#.####.###
#..####...#.#.#.###.###.
#####..#####...###....##
#.##..#..#...#..####...#
.#.###..##..##..####.##.
...###...##...#...#..###
</code>
</pre>

Now, you're ready to search for sea monsters! Because your image is monochrome, a sea monster will look like this:

<pre>
<code>                  # 
#    ##    ##    ###
 #  #  #  #  #  #   
</code>
</pre>

When looking for this pattern in the image, <em>the spaces can be anything</em>; only the <code>#</code> need to match. Also, you might need to rotate or flip your image before it's oriented correctly to find sea monsters. In the above image, <em>after flipping and rotating it</em> to the appropriate orientation, there are <em>two</em> sea monsters (marked with <code><em>O</em></code>):

<pre>
<code>.####...#####..#...###..
#####..#..#.#.####..#.#.
.#.#...#.###...#.##.<em>O</em>#..
#.<em>O</em>.##.<em>O</em><em>O</em>#.#.<em>O</em><em>O</em>.##.<em>O</em><em>O</em><em>O</em>##
..#<em>O</em>.#<em>O</em>#.<em>O</em>##<em>O</em>..<em>O</em>.#<em>O</em>##.##
...#.#..##.##...#..#..##
#.##.#..#.#..#..##.#.#..
.###.##.....#...###.#...
#.####.#.#....##.#..#.#.
##...#..#....#..#...####
..#.##...###..#.#####..#
....#.##.#.#####....#...
..##.##.###.....#.##..#.
#...#...###..####....##.
.#.##...#.##.#.#.###...#
#.###.#..####...##..#...
#.###...#.##...#.##<em>O</em>###.
.<em>O</em>##.#<em>O</em><em>O</em>.###<em>O</em><em>O</em>##..<em>O</em><em>O</em><em>O</em>##.
..<em>O</em>#.<em>O</em>..<em>O</em>..<em>O</em>.#<em>O</em>##<em>O</em>##.###
#.#..##.########..#..##.
#.#####..#.#...##..#....
#....##..#.#########..##
#...#.....#..##...###.##
#..###....##.#...##.##.#
</code>
</pre>

Determine how rough the waters are in the sea monsters' habitat by counting the number of <code>#</code> that are <em>not</em> part of a sea monster. In the above example, the habitat's water roughness is <em><code>273</code></em>.

<em>How many <code>#</code> are not part of a sea monster?</em>


