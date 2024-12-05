## --- Day 5: Print Queue ---
Satisfied with their search on Ceres, the squadron of scholars suggests subsequently scanning the stationery stacks of sub-basement 17.

The North Pole printing department is busier than ever this close to Christmas, and while The Historians continue their search of this historically significant facility, an Elf operating a **very familiar printer** beckons you over.

The Elf must recognize you, because they waste no time explaining that the new <em>sleigh launch safety manual</em> updates won't print correctly. Failure to update the safety manuals would be dire indeed, so you offer your services.

Read the [full puzzle](https://adventofcode.com/2024/day/5).

The constraints in both my input and the provided sample input define a total ordering of the pages, which I leveraged in my solution. I implemented a custom parser that returns the list of updates to be printed and a page comparison function. That's all we need. In `Part1`, we check which updates are in the correct order, while in `Part2`, we handle the remaining updates by applying .NET's built-in `OrderBy` function with our custom comparer.