original source: [https://adventofcode.com//2016/day/17](https://adventofcode.com//2016/day/17)
## --- Day 17: Two Steps Forward ---
You're trying to access a secure vault protected by a `4x4` grid of small rooms connected by doors. You start in the top-left room (marked `S`), and you can access the vault (marked `V`) once you reach the bottom-right room:

```
#########
#S| | | #
#-#-#-#-#
# | | | #
#-#-#-#-#
# | | | #
#-#-#-#-#
# | | |  
####### V
```

Fixed walls are marked with `#`, and doors are marked with `-` or `|`.

The doors in your *current room* are either open or closed (and locked) based on the hexadecimal [MD5](https://en.wikipedia.org/wiki/MD5) hash of a passcode (your puzzle input) followed by a sequence of uppercase characters representing the *path you have taken so far* (`U` for up, `D` for down, `L` for left, and `R` for right).

Only the first four characters of the hash are used; they represent, respectively, the doors *up, down, left, and right* from your current position. Any `b`, `c`, `d`, `e`, or `f` means that the corresponding door is *open*; any other character (any number or `a`) means that the corresponding door is *closed and locked*.

To access the vault, all you need to do is reach the bottom-right room; reaching this room opens the vault and all doors in the maze.

For example, suppose the passcode is `hijkl`. Initially, you have taken no steps, and so your path is empty: you simply find the MD5 hash of `hijkl` alone. The first four characters of this hash are `ced9`, which indicate that up is open (`c`), down is open (`e`), left is open (`d`), and right is closed and locked (`9`). Because you start in the top-left corner, there are no "up" or "left" doors to be open, so your only choice is *down*.

Next, having gone only one step (down, or `D`), you find the hash of `hijkl*D*`. This produces `f2bc`, which indicates that you can go back up, left (but that's a wall), or right. Going right means hashing `hijkl*DR*` to get `5745` - all doors closed and locked. However, going *up* instead is worthwhile: even though it returns you to the room you started in, your path would then be `DU`, opening a *different set of doors*.

After going `DU` (and then hashing `hijkl*DU*` to get `528e`), only the right door is open; after going `DUR`, all doors lock. (Fortunately, your actual passcode is not `hijkl`).

Passcodes actually used by Easter Bunny Vault Security do allow access to the vault if you know the right path.  For example:


 - If your passcode were `ihgpwlah`, the shortest path would be `DDRRRD`.
 - With `kglvqrro`, the shortest path would be `DDUDRLRRUDRD`.
 - With `ulqzkmiv`, the shortest would be `DRURDRUDDLLDLUURRDULRLDUUDDDRR`.

Given your vault's passcode, *what is the shortest path* (the actual path, not just the length) to reach the vault?


## --- Part Two ---
You're curious how robust this security solution really is, and so you decide to find longer and longer paths which still provide access to the vault. You remember that paths always end the first time they reach the bottom-right room (that is, they can never pass through it, only end in it).

For example:


 - If your passcode were `ihgpwlah`, the longest path would take `370` steps.
 - With `kglvqrro`, the longest path would be `492` steps long.
 - With `ulqzkmiv`, the longest path would be `830` steps long.



What is the *length of the longest path* that reaches the vault?


