## --- Day 2: Red-Nosed Reports ---
Fortunately, the first location The Historians want to search isn't a long walk from the Chief Historian's office.

While the [Red-Nosed Reindeer nuclear fusion/fission plant](/2015/day/19) appears to contain no sign of the Chief Historian, the engineers there run up to you as soon as they see you. Apparently, they <em>still</em> talk about the time Rudolph was saved through molecular synthesis from a single electron.

They're quick to add that - since you're already here - they'd really appreciate your help analyzing some unusual data from the Red-Nosed reactor. You turn to check if The Historians are waiting for you, but they seem to have already divided into groups that are currently searching every corner of the facility. You offer to help with the unusual data.

Read the [full puzzle](https://adventofcode.com/2024/day/2).

I created a function to check the validity of a single input line. This is achieved using the usual method of _zipping_ the input with itself to generate a list of consecutive pairs. The next step involves checking the monotonicity condition (either increasing or decreasing) for each pair.

The second part of the problem is addressed with another helper function. This function takes an input sequence and generates attenuated versions of it in all possible ways, by omitting _zero_ or _one_ elements from the sample.