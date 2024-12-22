## --- Day 21: Keypad Conundrum ---
As you teleport onto Santa's _Reindeer-class starship_, The Historians begin to panic: someone from their search party is <em>missing</em>. A quick life-form scan by the ship's computer reveals that when the missing Historian teleported, he arrived in another part of the ship.

The door to that area is locked, but the computer can't open it; it can only be opened by <em>physically typing</em> the door codes (your puzzle input) on the numeric keypad on the door.

_Visit the website for the full story and [full puzzle](https://adventofcode.com/2024/day/21) description._

This is the problem that sets casual players apart competitors. I had a hard time solving it. I think I was on track, but 
I just couldn't visualize the whole idea of robots controlling robots controlling robots. I love recursion, but just 
cannot mentally follow multiple layers of indirection. Anyway. I could solve it in the evening hours, so I'm still on track with _Advent of Code 2024_.

It was clear from the exponential growth demonstrated by _part 1_ that I wont be able to generate the final string to
be entered. Luckily the problem asked only for the length of it. (Which makes it really artifical: how would the elves enter
such a long key?)

I created a function called `EncodeKeys` which takes a sequence of keys to be pressed and an array of keypads, and returns the length of the shortest sequence needed to enter the given keys. An empty keypad array means that the sequence is simply entered by a human and no further encoding is needed. Otherwise, the sequence is entered by a robot that needs to be programmed using its keypad. In this case the keys are encoded using the first keypad in the array (the robot's keypad), character by character using _EncodeKey_ which will recursively call back to `EncodeKeys` with the rest of the keypads.

The `_EncodeKey_` helper function is used to move the robot from position _A_ to position _B_ on the keypad and press the button. Usually, _A_ and _B_ are not in the same row and column, so the movement has both a horizontal and a vertical part. We could go both ways: horizontal first then vertical, or the other way around, and we need to check which movement results in fewer key presses. Lastly, we should not forget that the robot's hand should never move over the empty space `' '`.

Since every key sequence we need to enter ends with a button press `'A'`, we can always be sure that the robot will stay over the `'A'` button at the end. Since it initially starts over the `'A'` key as well, we have an invariant: at the beginning and the end of `EncodeKeys`, the robot is over the `'A'` key. It can move around while entering the keys, but the invariant holds when it finishes.

This is great because we don't have to keep track of it! The `Cache` used by `EncodeKey` doesn't need to care about the robot's position recursively. All it depends on is the current robot's position, the position of the next key to be entered, and the number of keypads left in the recursion. This reduces the problem space quite a bit and gives us a well-performing algorithm.

