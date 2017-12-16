original source: [http://adventofcode.com/2017/day/16](http://adventofcode.com/2017/day/16)
## --- Day 16: Permutation Promenade ---
You come upon a very unusual sight; a group of programs here appear to be [dancing](https://www.youtube.com/watch?v=lyZQPjUT5B4&t=53).

There are sixteen programs in total, named `a` through `p`. They start by standing in a line: `a` stands in position `0`, `b` stands in position `1`, and so on until `p`, which stands in position `15`.

The programs' *dance* consists of a sequence of *dance moves*:


 - *Spin*, written `sX`, makes `X` programs move from the end to the front, but maintain their order otherwise. (For example, `s3` on `abcde` produces `cdeab`).
 - *Exchange*, written `xA/B`, makes the programs at positions `A` and `B` swap places.
 - *Partner*, written `pA/B`, makes the programs named `A` and `B` swap places.

For example, with only five programs standing in a line (`abcde`), they could do the following dance:


 - `s1`, a spin of size `1`: `eabcd`.
 - `x3/4`, swapping the last two programs: `eabdc`.
 - `pe/b`, swapping programs `e` and `b`: `baedc`.

After finishing their dance, the programs end up in order `baedc`.
You watch the dance for a while and record their dance moves (your puzzle input). *In what order are the programs standing* after their dance?




## --- Part Two ---
Now that you're starting to get a feel for the dance moves, you turn your attention to *the dance as a whole*.

Keeping the positions they ended up in from their previous dance, the programs perform it again and again: including the first dance, a total of *one billion* (`1000000000`) times.

In the example above, their second dance would *begin* with the order `baedc`, and use the same dance moves:


 - `s1`, a spin of size `1`: `cbaed`.
 - `x3/4`, swapping the last two programs: `cbade`.
 - `pe/b`, swapping programs `e` and `b`: `ceadb`.

*In what order are the programs standing* after their billion dances?


