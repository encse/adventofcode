## --- Day 22: Monkey Market ---
As you're all teleported deep into the jungle, a _monkey_ steals The Historians' device! You'll need get it back while The Historians are looking for the Chief.

The monkey that stole the device seems willing to trade it, but only in exchange for an absurd number of bananas. Your only option is to buy bananas on the Monkey Exchange Market.

_Visit the website for the full story and [full puzzle](https://adventofcode.com/2024/day/22) description._

A refreshing challenge after yesterday's hard one. I created a secret number generator function that returns the 2001 numbers for each seller (the initial one and the 2000 additional). This is enough for _Part 1_. 

For _Part 2_ I maintain the dictionary of buying options for each potential sequence the monkey can recognize with the 
combined amount of bananas this would generate from all sellers. 