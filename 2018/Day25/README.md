original source: [https://adventofcode.com//2018/day/25](https://adventofcode.com//2018/day/25)
## --- Day 25: Four-Dimensional Adventure ---
The reindeer's symptoms are getting worse, and neither you nor the white-bearded man have a solution. At least the reindeer has a warm place to rest: a small bed near where you're sitting.

As you reach down, the reindeer looks up at you, accidentally bumping a button on your wrist-mounted device with its nose in the process - a button labeled *"help"*.

"Hello, and welcome to the Time Travel Support Hotline! If you are lost in time and space, press 1. If you are trapped in a time paradox, press 2. If you need help caring for a sick reindeer, press 3. If you--"

*Beep.*

A few seconds later, you hear a new voice. "Hello; please state the nature of your reindeer." You try to describe the situation.

"Just a moment, I think I can remotely run a diagnostic scan." A beam of light projects from the device and sweeps over the reindeer a few times.

"Okay, it looks like your reindeer is very low on magical energy; it should fully recover if we can fix that.  Let me check your timeline for a source.... Got one. There's actually a powerful source of magical energy about 1000 years forward from you, and at roughly your position, too!  It looks like... hot chocolate?  Anyway, you should be able to travel there to pick some up; just don't forget a mug!  Is there anything else I can help you with today?"

You explain that your device isn't capable of going forward in time.  "I... see. That's tricky. Well, according to this information, your device should have the necessary hardware to open a small portal and send some hot chocolate back to you. You'll need a list of *fixed points in spacetime*; I'm transmitting it to you now."

"You just need to align your device to the constellations of fixed points so that it can lock on to the destination and open the portal. Let me look up how much hot chocolate that breed of reindeer needs."

"It says here that your particular reindeer is-- this can't be right, it says there's only one like that in the universe!  But THAT means that you're--" You disconnect the call.

The list of fixed points in spacetime (your puzzle input) is a set of four-dimensional coordinates. To align your device, acquire the hot chocolate, and save the reindeer, you just need to find the *number of constellations* of points in the list.

Two points are in the same *constellation* if their manhattan distance apart is *no more than 3* or if they can form a chain of points, each a manhattan distance no more than 3 from the last, between the two of them. (That is, if a point is close enough to a constellation, it "joins" that constellation.) For example:

```
 0,0,0,0
 3,0,0,0
 0,3,0,0
 0,0,3,0
 0,0,0,3
 0,0,0,6
 9,0,0,0
12,0,0,0
```

In the above list, the first six points form a single constellation: `0,0,0,0` is exactly distance `3` from the next four, and the point at `0,0,0,6` is connected to the others by being `3` away from `0,0,0,3`, which is already in the constellation. The bottom two points, `9,0,0,0` and `12,0,0,0` are in a separate constellation because no point is close enough to connect them to the first constellation.  So, in the above list, the number of constellations is `*2*`.  (If a point at `6,0,0,0` were present, it would connect `3,0,0,0` and `9,0,0,0`, merging all of the points into a single giant constellation instead.)

In this example, the number of constellations is `4`:

```
-1,2,2,0
0,0,2,-2
0,0,0,-2
-1,2,0,0
-2,-2,-2,2
3,0,2,-1
-1,3,2,2
-1,0,-1,0
0,2,1,-2
3,0,0,0
```

In this one, it's `3`:

```
1,-1,0,1
2,0,-1,0
3,2,-1,0
0,0,3,1
0,0,-1,-1
2,3,-2,0
-2,2,0,0
2,-2,0,-1
1,-1,0,-1
3,2,0,2
```

Finally, in this one, it's `8`:

```
1,-1,-1,-2
-2,-2,0,1
0,2,1,3
-2,3,-2,1
0,2,3,-2
-1,-1,1,-2
0,-2,-1,0
-2,2,3,-1
1,2,2,0
-1,-2,0,-2
```

The portly man nervously strokes his white beard. It's time to get that hot chocolate.

*How many constellations are formed by the fixed points in spacetime?*


