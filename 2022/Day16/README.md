original source: [https://adventofcode.com/2022/day/16](https://adventofcode.com/2022/day/16)
## --- Day 16: Proboscidea Volcanium ---
The sensors have led you to the origin of the distress signal: yet another handheld device, just like the one the Elves gave you. However, you don't see any Elves around; instead, the device is surrounded by elephants! They must have gotten lost in these tunnels, and one of the elephants apparently figured out how to turn on the distress signal.

The ground rumbles again, much stronger this time. What kind of cave is this, exactly? You scan the cave with your handheld device; it reports mostly igneous rock, some ash, pockets of pressurized gas, magma... this isn't just a cave, it's a volcano!

You need to get the elephants out of here, quickly. Your device estimates that you have <em>30 minutes</em> before the volcano erupts, so you don't have time to go back out the way you came in.

You scan the cave for other options and discover a network of pipes and pressure-release <em>valves</em>. You aren't sure how such a system got into a volcano, but you don't have time to complain; your device produces a report (your puzzle input) of each valve's <em>flow rate</em> if it were opened (in pressure per minute) and the tunnels you could use to move between the valves.

There's even a valve in the room you and the elephants are currently standing in labeled <code>AA</code>. You estimate it will take you one minute to open a single valve and one minute to follow any tunnel from one valve to another. What is the most pressure you could release?

For example, suppose you had the following scan output:

<pre>
<code>Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II
</code>
</pre>

All of the valves begin <em>closed</em>. You start at valve <code>AA</code>, but it must be damaged or jammed or something: its flow rate is <code>0</code>, so there's no point in opening it. However, you could spend one minute moving to valve <code>BB</code> and another minute opening it; doing so would release pressure during the remaining <em>28 minutes</em> at a flow rate of <code>13</code>, a total eventual pressure release of <code>28 * 13 = <em>364</em></code>. Then, you could spend your third minute moving to valve <code>CC</code> and your fourth minute opening it, providing an additional <em>26 minutes</em> of eventual pressure release at a flow rate of <code>2</code>, or <code><em>52</em></code> total pressure released by valve <code>CC</code>.

Making your way through the tunnels like this, you could probably open many or all of the valves by the time 30 minutes have elapsed. However, you need to release as much pressure as possible, so you'll need to be methodical. Instead, consider this approach:

<pre>
<code>== Minute 1 ==
No valves are open.
You move to valve DD.

== Minute 2 ==
No valves are open.
You open valve DD.

== Minute 3 ==
Valve DD is open, releasing <em>20</em> pressure.
You move to valve CC.

== Minute 4 ==
Valve DD is open, releasing <em>20</em> pressure.
You move to valve BB.

== Minute 5 ==
Valve DD is open, releasing <em>20</em> pressure.
You open valve BB.

== Minute 6 ==
Valves BB and DD are open, releasing <em>33</em> pressure.
You move to valve AA.

== Minute 7 ==
Valves BB and DD are open, releasing <em>33</em> pressure.
You move to valve II.

== Minute 8 ==
Valves BB and DD are open, releasing <em>33</em> pressure.
You move to valve JJ.

== Minute 9 ==
Valves BB and DD are open, releasing <em>33</em> pressure.
You open valve JJ.

== Minute 10 ==
Valves BB, DD, and JJ are open, releasing <em>54</em> pressure.
You move to valve II.

== Minute 11 ==
Valves BB, DD, and JJ are open, releasing <em>54</em> pressure.
You move to valve AA.

== Minute 12 ==
Valves BB, DD, and JJ are open, releasing <em>54</em> pressure.
You move to valve DD.

== Minute 13 ==
Valves BB, DD, and JJ are open, releasing <em>54</em> pressure.
You move to valve EE.

== Minute 14 ==
Valves BB, DD, and JJ are open, releasing <em>54</em> pressure.
You move to valve FF.

== Minute 15 ==
Valves BB, DD, and JJ are open, releasing <em>54</em> pressure.
You move to valve GG.

== Minute 16 ==
Valves BB, DD, and JJ are open, releasing <em>54</em> pressure.
You move to valve HH.

== Minute 17 ==
Valves BB, DD, and JJ are open, releasing <em>54</em> pressure.
You open valve HH.

== Minute 18 ==
Valves BB, DD, HH, and JJ are open, releasing <em>76</em> pressure.
You move to valve GG.

== Minute 19 ==
Valves BB, DD, HH, and JJ are open, releasing <em>76</em> pressure.
You move to valve FF.

== Minute 20 ==
Valves BB, DD, HH, and JJ are open, releasing <em>76</em> pressure.
You move to valve EE.

== Minute 21 ==
Valves BB, DD, HH, and JJ are open, releasing <em>76</em> pressure.
You open valve EE.

== Minute 22 ==
Valves BB, DD, EE, HH, and JJ are open, releasing <em>79</em> pressure.
You move to valve DD.

== Minute 23 ==
Valves BB, DD, EE, HH, and JJ are open, releasing <em>79</em> pressure.
You move to valve CC.

== Minute 24 ==
Valves BB, DD, EE, HH, and JJ are open, releasing <em>79</em> pressure.
You open valve CC.

== Minute 25 ==
Valves BB, CC, DD, EE, HH, and JJ are open, releasing <em>81</em> pressure.

== Minute 26 ==
Valves BB, CC, DD, EE, HH, and JJ are open, releasing <em>81</em> pressure.

== Minute 27 ==
Valves BB, CC, DD, EE, HH, and JJ are open, releasing <em>81</em> pressure.

== Minute 28 ==
Valves BB, CC, DD, EE, HH, and JJ are open, releasing <em>81</em> pressure.

== Minute 29 ==
Valves BB, CC, DD, EE, HH, and JJ are open, releasing <em>81</em> pressure.

== Minute 30 ==
Valves BB, CC, DD, EE, HH, and JJ are open, releasing <em>81</em> pressure.
</code>
</pre>

This approach lets you release the most pressure possible in 30 minutes with this valve layout, <code><em>1651</em></code>.

Work out the steps to release the most pressure in 30 minutes. <em>What is the most pressure you can release?</em>


## --- Part Two ---
You're worried that even with an optimal approach, the pressure released won't be enough. What if you got one of the elephants to help you?

It would take you 4 minutes to teach an elephant how to open the right valves in the right order, leaving you with only <em>26 minutes</em> to actually execute your plan. Would having two of you working together be better, even if it means having less time? (Assume that you teach the elephant before opening any valves yourself, giving you both the same full 26 minutes.)

In the example above, you could teach the elephant to help you as follows:

<pre>
<code>== Minute 1 ==
No valves are open.
You move to valve II.
The elephant moves to valve DD.

== Minute 2 ==
No valves are open.
You move to valve JJ.
The elephant opens valve DD.

== Minute 3 ==
Valve DD is open, releasing <em>20</em> pressure.
You open valve JJ.
The elephant moves to valve EE.

== Minute 4 ==
Valves DD and JJ are open, releasing <em>41</em> pressure.
You move to valve II.
The elephant moves to valve FF.

== Minute 5 ==
Valves DD and JJ are open, releasing <em>41</em> pressure.
You move to valve AA.
The elephant moves to valve GG.

== Minute 6 ==
Valves DD and JJ are open, releasing <em>41</em> pressure.
You move to valve BB.
The elephant moves to valve HH.

== Minute 7 ==
Valves DD and JJ are open, releasing <em>41</em> pressure.
You open valve BB.
The elephant opens valve HH.

== Minute 8 ==
Valves BB, DD, HH, and JJ are open, releasing <em>76</em> pressure.
You move to valve CC.
The elephant moves to valve GG.

== Minute 9 ==
Valves BB, DD, HH, and JJ are open, releasing <em>76</em> pressure.
You open valve CC.
The elephant moves to valve FF.

== Minute 10 ==
Valves BB, CC, DD, HH, and JJ are open, releasing <em>78</em> pressure.
The elephant moves to valve EE.

== Minute 11 ==
Valves BB, CC, DD, HH, and JJ are open, releasing <em>78</em> pressure.
The elephant opens valve EE.

(At this point, all valves are open.)

== Minute 12 ==
Valves BB, CC, DD, EE, HH, and JJ are open, releasing <em>81</em> pressure.

...

== Minute 20 ==
Valves BB, CC, DD, EE, HH, and JJ are open, releasing <em>81</em> pressure.

...

== Minute 26 ==
Valves BB, CC, DD, EE, HH, and JJ are open, releasing <em>81</em> pressure.
</code>
</pre>

With the elephant helping, after 26 minutes, the best you could do would release a total of <code><em>1707</em></code> pressure.

<em>With you and an elephant working together for 26 minutes, what is the most pressure you could release?</em>


