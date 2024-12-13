## --- Day 2: Red-Nosed Reports ---
Fortunately, the first location The Historians want to search isn't a long walk from the Chief Historian's office.

While the _Red-Nosed Reindeer nuclear fusion/fission plant_ appears to contain no sign of the Chief Historian, the engineers there run up to you as soon as they see you. Apparently, they <em>still</em> talk about the time Rudolph was saved through molecular synthesis from a single electron.

_Visit the website for the full story and [puzzle](https://adventofcode.com/2024/day/2) description._

I created a function to check the validity of a single input line. This is achieved using the usual method of _zipping_ the input with itself to generate a list of consecutive pairs. The next step involves checking the monotonicity condition (either increasing or decreasing) for each pair.

The second part of the problem is addressed with another helper function. This function takes an input sequence and generates attenuated versions of it in all possible ways, by omitting _zero_ or _one_ elements from the sample.