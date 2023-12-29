## --- Day 7: Camel Cards ---
Your all-expenses-paid trip turns out to be a one-way, five-minute ride in an [airship](https://en.wikipedia.org/wiki/Airship). (At least it's a <em>cool</em> airship!) It drops you off at the edge of a vast desert and descends back to Island Island.

"Did you bring the parts?"

You turn around to see an Elf completely covered in white clothing, wearing goggles, and riding a large [camel](https://en.wikipedia.org/wiki/Dromedary).

Read the [full puzzle](https://adventofcode.com/2023/day/7).

##  --- Notes ---
We are playing ~poker~ Camel Cards today! In Part 1 we need to evalute hands and put them in order. Each 
hand has what I call a pattern value: five of a kind, poker, full house, three of a kind, double 
pair or pair and some individual card value such as: 1, 2, ..., J, Q, K, or A.

Pattern value becomes one number, card value becomes an other number then let linq do the ordering for me.

Part 2 is not much different, but the individual card value changes, and `J` becomes 
a joker that can replace any other cards. I made a shortcut here and just reused the functions from Part 1.
