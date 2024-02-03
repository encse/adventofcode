## --- Day 20: Pulse Propagation ---
The task description is copyrighted, but it's available [here](https://adventofcode.com/2023/day/20).

I modeled Part 1 following the description closely. I didn't want to introduce separate
classes for the gate types, instead I used a function parameter that maps signals to signals.
I tells what should be emitted when a signal is received by the gate. I know that this is 
Elf logic, but it's 🎄, what did you expect? I have one constructor function for each
gate type (Nand, FlipFlop and Repeater) these declare the necessary state variables
and capture it in the returned lambda. Everything is self contained, I have a single Gate type, 
yet different behavior.

I made a function that triggers the button and executes all the logic until things settle down.
It returns all signals that were emitted, so that I can work with them in both parts.

I think Part 1 doesn't need more explanation. Part 2 however, is a different beast. It's a _reverse 
engineering_ problem.  We need to tell how many times the button is to be pressed until a 
single _high_ value is emitted to the `rx` gate. The catch is that we need to understand a
bit what's happening, because just blindly pressing the button will not terminate in a reasonable time.

I layed out the graph using Graphviz to see what's going on. This immediately showed that 
`broadcaster` feeds four different subgraphs. These work in isolation and a Nand gate
connects their output into `rx`. Further investigation shows that each subgraph runs in a loop 
that has prime length (at least for  my input). We just need to multiply 
them to solve the second half of the problem.

