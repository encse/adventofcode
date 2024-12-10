## --- Day 9: Disk Fragmenter ---
Another push of the button leaves you in the familiar hallways of some friendly [amphipods](/2021/day/23)! Good thing you each somehow got your own personal mini submarine. The Historians jet away in search of the Chief, mostly by driving directly into walls.

While The Historians quickly figure out how to pilot these things, you notice an amphipod in the corner struggling with his computer. He's trying to make more contiguous free space by compacting all of the files, but his program isn't working; you offer to help.

Read the [full puzzle](https://adventofcode.com/2024/day/9).

I'm taking a break from using LINQ today and turning my attention to the low-level world of linked lists instead. I discovered a way to express both paths using a single `CompactFs` function with a `fragmentsEnabled` parameter. `CompactFs` operates with two pointers, `i` and `j`, which define the scan range we’re working on. `i` starts at the beginning of the disk, while `j` starts at the end and moves backward. When `i` points to a free space and `j` points to a used space, we call `RelocateBlock`, which moves `j` (or parts of `j`, depending on whether fragmentation is enabled) to a free space found after `i`.

The rest involves careful pointer arithmetic and linked list management, where I aim to avoid overwriting the data I’ll need in the next line. I find this surprisingly hard to get right when working with linked lists...