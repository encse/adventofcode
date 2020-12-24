original source: [https://adventofcode.com/2020/day/24](https://adventofcode.com/2020/day/24)
## --- Day 24: Lobby Layout ---
Your raft makes it to the tropical island; it turns out that the small crab was an excellent navigator. You make your way to the resort.

As you enter the lobby, you discover a small problem: the floor is being renovated. You can't even reach the check-in desk until they've finished installing the <em>new tile floor</em>.

The tiles are all <em>hexagonal</em>; they need to be arranged in a [hex grid](https://en.wikipedia.org/wiki/Hexagonal_tiling) with a very specific color pattern. Not in the mood to wait, you offer to help figure out the pattern.

The tiles are all <em>white</em> on one side and <em>black</em> on the other. They start with the white side facing up. The lobby is large enough to fit whatever pattern might need to appear there.

A member of the renovation crew gives you a <em>list of the tiles that need to be flipped over</em> (your puzzle input). Each line in the list identifies a single tile that needs to be flipped by giving a series of steps starting from a <em>reference tile</em> in the very center of the room. (Every line starts from the same reference tile.)

Because the tiles are hexagonal, every tile has <em>six neighbors</em>: east, southeast, southwest, west, northwest, and northeast. These directions are given in your list, respectively, as <code>e</code>, <code>se</code>, <code>sw</code>, <code>w</code>, <code>nw</code>, and <code>ne</code>. A tile is identified by a series of these directions with <em>no delimiters</em>; for example, <code>esenee</code> identifies the tile you land on if you start at the reference tile and then move one tile east, one tile southeast, one tile northeast, and one tile east.

Each time a tile is identified, it flips from white to black or from black to white. Tiles might be flipped more than once. For example, a line like <code>esew</code> flips a tile immediately adjacent to the reference tile, and a line like <code>nwwswee</code> flips the reference tile itself.

Here is a larger example:

<pre>
<code>sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew
</code>
</pre>

In the above example, 10 tiles are flipped once (to black), and 5 more are flipped twice (to black, then back to white). After all of these instructions have been followed, a total of <em><code>10</code></em> tiles are <em>black</em>.

Go through the renovation crew's list and determine which tiles they need to flip. After all of the instructions have been followed, <em>how many tiles are left with the black side up?</em>


## --- Part Two ---
The tile floor in the lobby is meant to be a living art exhibit. Every day, the tiles are all flipped according to the following rules:


 - Any <em>black</em> tile with <em>zero</em> or <em>more than 2</em> black tiles immediately adjacent to it is flipped to <em>white</em>.
 - Any <em>white</em> tile with <em>exactly 2</em> black tiles immediately adjacent to it is flipped to <em>black</em>.

Here, <em>tiles immediately adjacent</em> means the six tiles directly touching the tile in question.

The rules are applied <em>simultaneously</em> to every tile; put another way, it is first determined which tiles need to be flipped, then they are all flipped at the same time.

In the above example, the number of black tiles that are facing up after the given number of days has passed is as follows:

<pre>
<code>Day 1: 15
Day 2: 12
Day 3: 25
Day 4: 14
Day 5: 23
Day 6: 28
Day 7: 41
Day 8: 37
Day 9: 49
Day 10: 37

Day 20: 132
Day 30: 259
Day 40: 406
Day 50: 566
Day 60: 788
Day 70: 1106
Day 80: 1373
Day 90: 1844
Day 100: 2208
</code>
</pre>

After executing this process a total of 100 times, there would be <em><code>2208</code></em> black tiles facing up.

<em>How many tiles will be black after 100 days?</em>


