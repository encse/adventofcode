## --- Day 8: Haunted Wasteland ---
You're still riding a camel across Desert Island when you spot a sandstorm quickly approaching. When you turn to warn the Elf, she disappears before your eyes! To be fair, she had just finished warning you about <em>ghosts</em> a few minutes ago.

One of the camel's pouches is labeled "maps" - sure enough, it's full of documents (your puzzle input) about how to navigate the desert. At least, you're pretty sure that's what they are; one of the documents contains a list of left/right instructions, and the rest of the documents seem to describe some kind of <em>network</em> of labeled nodes.

It seems like you're meant to use the <em>left/right</em> instructions to <em>navigate the network</em>. Perhaps if you have the camel follow the same instructions, you can escape the haunted wasteland!

Read the [full puzzle](https://adventofcode.com/2023/day/8).

##  --- Notes ---
We need to implement some process that is called _wandering around the desert_ and it's essentially a 
series of dictionary lookups that lead to other dictionary lookups. 

Pretty dry as I'm writing it down, but the point is that after some iterations we get from AAA to ZZZ. 
Part 1 asks for the number of steps needed for that.

Part 2 gives it a spin and is asking us to start from all nodes that end with the letter A at once, 
and continue _wandering around_ in parallel until we reach Z nodes simultanously in every path!

Obviously, this would take ages to wait out, but fortunately the input is specially crafted so that we can take the least common multiplier of the length of the individual loops and return just that.
