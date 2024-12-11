## --- Day 11: Plutonian Pebbles ---
The ancient civilization on _Pluto_ was known for its ability to manipulate spacetime, and while The Historians explore their infinite corridors, you've noticed a strange set of physics-defying stones.

At first glance, they seem like normal stones: they're arranged in a perfectly <em>straight line</em>, and each stone has a <em>number</em> engraved on it.

Read the [full puzzle](https://adventofcode.com/2024/day/11).

Today is all about dynamic programming and cached calculations. Our goal is to determine the number of stones based on specific rules derived from the numbers engraved on them. Without careful optimization, this process can quickly spiral out of control.

To address this, I encoded the stone generation logic inside the `Eval` function and added a cache to prevent exponential growth.

I discovered the `ConcurrentDictionary` class, which includes a convenient `GetOrAdd` method. While this functionality is missing in regular `Dictionary` variants, it allows the caching logic to be neatly encapsulated in a single place.  I decided to "abuse" it a bit here, even though my solution doesn’t involve any concurrency at all.

There is an iterative approach to solving this problem as well, which progresses one blink at a time while keeping track of how many times each number occurs at each step. Working through this approach is left as an exercise for the reader.