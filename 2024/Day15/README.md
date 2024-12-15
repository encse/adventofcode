## --- Day 15: Warehouse Woes ---
You appear back inside your own mini submarine! Each Historian drives their mini submarine in a different direction; maybe the Chief has his own submarine down here somewhere as well?

You look up to see a vast school of [lanternfish](/2021/day/6) swimming past you. On closer inspection, they seem quite anxious, so you drive your mini submarine over to see if you can help.

_Visit the website for the full story and [full puzzle](https://adventofcode.com/2024/day/15) description._

A nice Sokoban-style puzzle for the weekend! The main difference is that in the original Sokoban, the robot could push only a single box, not multiple boxes. This adds complexity to both parts of the puzzle. However, it’s not that difficult to handle... I moved the hard parts into the `TryToStep` function that takes the map, a position, and a direction, then attempts to make a move in that direction.

If the position corresponds to the robot or a box, the function checks whether the neighboring cell is free or can be made free by pushing boxes in the given direction. The .NET API sometimes uses the `TryToDoX` pattern, where a function returns a boolean result and provides an `out` parameter. I reused this pattern here. On success, the updated map is returned via the `ref` parameter. If the move fails, the map remains unchanged. 

The real challenge lies in `part 2`, where recursion needs to branch whenever the robot pushes more than one box at a time. In such cases, we must invoke `TryToStep` for each box. Due to the recursive nature of the algorithm, it’s possible for one branch to succeed while the other fails. When this happens, the entire map must be reset to its original state before we pop from the recursion. This could be quite tricky to manage unless you make copies of the dictionary that holds the state — or, as I did, use an immutable dictionary instead.

I’m not entirely satisfied with the "if-ladder" structure of the `TryToStep` function, but for now, I don’t have a better idea to handle all the cases in a more readable way.
