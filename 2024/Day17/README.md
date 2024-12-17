## --- Day 17: Chronospatial Computer ---
The Historians push the button on their strange device, but this time, you all just feel like you're _falling_.

"Situation critical", the device announces in a familiar voice. "Bootstrapping process failed. Initializing debugger...."

_Visit the website for the full story and [full puzzle](https://adventofcode.com/2024/day/17) description._

I'm not a big fan of _disassembly_ tasks, but they come up almost every year. The first part of the problem, which 
I _actually_ liked, was to write an interpreter for an elvish computer (i.e. one with a weird instruction set). This 
is really easy to do with a simple for loop and switching on the different opcodes.

The second half was a bit harder, as I had to understand what's going on in the program. We had to generate inputs that 
would force the program to print out its own source.  Well, a _quine_ is a computer program that takes no input and 
produces a copy of its own source code as its only output. Although our program was not a real quine, I couldn't help 
but smile when I read the problem description in the morning. 

Fortunately, the algorithm was not that complicated once I realized that the _next output number_ depends only on the 
_lowest few bits_ of the input. Then the input gets shifted by a small amount, and this continues until the end. 

There is _some interconnection_ between the consecutive numbers, so one cannot just calculate the result independently 
for each. But I was able to come up with a _recursive solution_ that generates the input _backwards_. Once we know the 
higher bits, we can try all combinations for the next 3 bits, and so on down to the first bit.
