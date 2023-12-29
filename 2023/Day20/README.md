## --- Day 20: Pulse Propagation ---
With your help, the Elves manage to find the right parts and fix all of the machines. Now, they just need to send the command to boot up the machines and get the sand flowing again.

The machines are far apart and wired together with long <em>cables</em>. The cables don't connect to the machines directly, but rather to communication <em>modules</em> attached to the machines that perform various initialization tasks and also act as communication relays.

Modules communicate using <em>pulses</em>. Each pulse is either a <em>high pulse</em> or a <em>low pulse</em>. When a module sends a pulse, it sends that type of pulse to each module in its list of <em>destination modules</em>.

Read the [full puzzle](https://adventofcode.com/2023/day/20).

##  --- Notes ---
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
that has prime length (at least for  my input). We just need to determine and multiply 
them to solve the second half of the problem.

