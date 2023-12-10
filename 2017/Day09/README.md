## --- Day 9: Stream Processing ---
A large stream blocks your path. According to the locals, it's not safe to cross the stream at the moment because it's full of *garbage*. You look down at the stream; rather than water, you discover that it's a *stream of characters*.

You sit for a while and record part of the stream (your puzzle input). The characters represent *groups* - sequences that begin with `{` and end with `}`. Within a group, there are zero or more other things, separated by commas: either another *group* or *garbage*. Since groups can contain other groups, a `}` only closes the *most-recently-opened unclosed group* - that is, they are nestable. Your puzzle input represents a single, large group which itself contains many smaller ones.

Read the [full puzzle](https://adventofcode.com/2017/day/9).