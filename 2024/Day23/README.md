## --- Day 23: LAN Party ---
As The Historians wander around a secure area at Easter Bunny HQ, you come across posters for a [LAN party](https://en.wikipedia.org/wiki/LAN_party) scheduled for today! Maybe you can find it; you connect to a nearby _datalink port_ and download a map of the local network (your puzzle input).

The network map provides a list of every <em>connection between two computers</em>. For example:

_Visit the website for the full story and [full puzzle](https://adventofcode.com/2024/day/23) description._

We tackled a graph algorithm problem today, where we had to find maximal cliques in an undirected graph. The literature provides [efficient algorithms](https://en.wikipedia.org/wiki/Bron%E2%80%93Kerbosch_algorithm) for this problem, but our graph is not too large, so we can use a straightforward "poor man's" strategy as well. 

I started with the _seed_ components, that is single element components for each node that starts with '_t_'. Then, I proceeded to _grow_ these components by adding a single node to them in all possible ways. I can put this in a loop, to get components with size 2, 3, 4, etc. _Part 1_ asks for the number of components that have 3 nodes. _Part 2_ asks for the one that cannot be grown anymore. 