## --- Day 19: Linen Layout ---
Today, The Historians take you up to the _hot springs_ on Gear Island! Very [suspiciously](https://www.youtube.com/watch?v=ekL881PJMjI), absolutely nothing goes wrong as they begin their careful search of the vast field of helixes.

Could this <em>finally</em> be your chance to visit the [onsen](https://en.wikipedia.org/wiki/Onsen) next door? Only one way to find out.

_Visit the website for the full story and [full puzzle](https://adventofcode.com/2024/day/19) description._

I initially thought that I would be able to entirely solve today's problem using regular expressions. This worked for Part 1, but unfortunately, the regex library can only determine whether a pattern matches a string — it cannot return the number of ways the match can occur. Not that it’s its job anyway...

I had to implement the search function myself. It’s a simple recursive function that iterates through all towels and tries to match them to the beginning of the pattern. If a match is found, it continues recursively with the remaining pattern until the entire pattern becomes an empty string, which signifies a complete match.

Of course, this approach would never terminate as-is, it needs to be converted into a cached function. Thankfully, this is straightforward to do using a _ConcurrentDictionary_, which I discovered earlier this year. By utilizing the _GetOrAdd_ method of 
the _ConcurrentDictionary_ class, one can simply wrap the uncached logic inside a lambda and call it a day. 

It’s very similar to Python’s cached function decorators, albeit with the caveat of having to pass an additional 'cache' argument at the end of the parameter list. But since we’re in the world of C#, we have to work with what it offers.
