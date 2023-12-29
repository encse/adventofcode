## --- Day 4: Scratchcards ---
The gondola takes you up. Strangely, though, the ground doesn't seem to be coming with you; you're not climbing a mountain. As the circle of Snow Island recedes below you, an entire new landmass suddenly appears above you! The gondola carries you to the surface of the new island and lurches into the station.

As you exit the gondola, the first thing you notice is that the air here is much <em>warmer</em> than it was on Snow Island. It's also quite <em>humid</em>. Is this where the water source is?

The next thing you notice is an Elf sitting on the floor across the station in what seems to be a pile of colorful square cards.

Read the [full puzzle](https://adventofcode.com/2023/day/4).

## --- Notes ---
An other day! In Part 1 we need to determine how many _winning_ numbers we have 
in a scratch card, this is simple enough. 

Part 2 can be treated in a very-very bad way if somebody doesn't notice that it's
nothing else but a loop!. We start with a single card #1 and see how many 
cards we are winning, this will generate some new cards of id 2, 3 and so. Then move 
to card(s) #2 and continue this process until the end of the list. Return 
the number of all cards we dealt with.
