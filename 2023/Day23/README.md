## --- Day 23: A Long Walk ---
Visit the Advent of Code website for the problem statement [here](https://adventofcode.com/2023/day/23).

Today's problem looked frightening first, because it's asking for the _longest_
path between two points of a map. Shortest path is a no brainer with _Dijkstra_ 
or whatever _graph search_, but I don't know about an efficient way to calculate 
the longest one. I have a feeling that it is somehow related to the _Hamiltonian path_ 
which is NP-complete, so there might not even exists a super efficient algorithm to 
solve today's problem in the generic case.

But this is a puzzle, so let's get to it. I decided to convert the problem from 
map traversal to graph traversal first. Created _nodes_ from the _crossroads_ of 
the map (those tiles that connect 3 or more `"."` cells). Also assigned a node to 
the entry and one to the exit.
 
Two nodes become _connected_ if there is a _road_ between them. That is, they are 
reachable following the _path_ in the map without visiting other crossroads in 
between.

This reduced the problem quite a bit. In my case it went down to about 30 nodes 
and 120 edges for Part 2. Part 1 is even smaller with 60 edges or so.

This graph is small enough to solve it using _dynamic programming_ with a _cache_.
Since we have just 30+ nodes, I represented them as _powers of 2_ and a set of
these became a _bitset_ stored in a _long_.

