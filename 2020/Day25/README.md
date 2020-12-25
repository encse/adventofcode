original source: [https://adventofcode.com/2020/day/25](https://adventofcode.com/2020/day/25)
## --- Day 25: Combo Breaker ---
You finally reach the check-in desk. Unfortunately, their registration systems are currently offline, and they cannot check you in. Noticing the look on your face, they quickly add that tech support is already on the way! They even created all the room keys this morning; you can take yours now and give them your room deposit once the registration system comes back online.

The room key is a small [RFID](https://en.wikipedia.org/wiki/Radio-frequency_identification) card. Your room is on the 25th floor and the elevators are also temporarily out of service, so it takes what little energy you have left to even climb the stairs and navigate the halls. You finally reach the door to your room, swipe your card, and - <em>beep</em> - the light turns red.

Examining the card more closely, you discover a phone number for tech support.

"Hello! How can we help you today?" You explain the situation.

"Well, it sounds like the card isn't sending the right command to unlock the door. If you go back to the check-in desk, surely someone there can reset it for you." Still catching your breath, you describe the status of the elevator and the exact number of stairs you just had to climb.

"I see! Well, your only other option would be to reverse-engineer the cryptographic handshake the card does with the door and then inject your own commands into the data stream, but that's definitely impossible." You thank them for their time.

Unfortunately for the door, you know a thing or two about cryptographic handshakes.

The handshake used by the card and the door involves an operation that <em>transforms</em> a <em>subject number</em>. To transform a subject number, start with the value <code>1</code>. Then, a number of times called the <em>loop size</em>, perform the following steps:


 - Set the value to itself multiplied by the <em>subject number</em>.
 - Set the value to the remainder after dividing the value by <em><code>20201227</code></em>.

The card always uses a specific, secret <em>loop size</em> when it transforms a subject number. The door always uses a different, secret loop size.

The cryptographic handshake works like this:


 - The <em>card</em> transforms the subject number of <em><code>7</code></em> according to the <em>card's</em> secret loop size. The result is called the <em>card's public key</em>.
 - The <em>door</em> transforms the subject number of <em><code>7</code></em> according to the <em>door's</em> secret loop size. The result is called the <em>door's public key</em>.
 - The card and door use the wireless RFID signal to transmit the two public keys (your puzzle input) to the other device. Now, the <em>card</em> has the <em>door's</em> public key, and the <em>door</em> has the <em>card's</em> public key. Because you can eavesdrop on the signal, you have both public keys, but neither device's loop size.
 - The <em>card</em> transforms the subject number of <em>the door's public key</em> according to the <em>card's</em> loop size. The result is the <em>encryption key</em>.
 - The <em>door</em> transforms the subject number of <em>the card's public key</em> according to the <em>door's</em> loop size. The result is the same <em>encryption key</em> as the <em>card</em> calculated.

If you can use the two public keys to determine each device's loop size, you will have enough information to calculate the secret <em>encryption key</em> that the card and door use to communicate; this would let you send the <code>unlock</code> command directly to the door!

For example, suppose you know that the card's public key is <code>5764801</code>. With a little trial and error, you can work out that the card's loop size must be <em><code>8</code></em>, because transforming the initial subject number of <code>7</code> with a loop size of <code>8</code> produces <code>5764801</code>.

Then, suppose you know that the door's public key is <code>17807724</code>. By the same process, you can determine that the door's loop size is <em><code>11</code></em>, because transforming the initial subject number of <code>7</code> with a loop size of <code>11</code> produces <code>17807724</code>.

At this point, you can use either device's loop size with the other device's public key to calculate the <em>encryption key</em>. Transforming the subject number of <code>17807724</code> (the door's public key) with a loop size of <code>8</code> (the card's loop size) produces the encryption key, <em><code>14897079</code></em>. (Transforming the subject number of <code>5764801</code> (the card's public key) with a loop size of <code>11</code> (the door's loop size) produces the same encryption key: <em><code>14897079</code></em>.)

<em>What encryption key is the handshake trying to establish?</em>


## --- Part Two ---
The light turns green and the door unlocks. As you collapse onto the bed in your room, your pager goes off!

"It's an emergency!" the Elf calling you explains. "The [soft serve](https://en.wikipedia.org/wiki/Soft_serve) machine in the cafeteria on sub-basement 7 just failed and you're the only one that knows how to fix it! We've already dispatched a reindeer to your location to pick you up."

You hear the sound of hooves landing on your balcony.

The reindeer carefully explores the contents of your room while you figure out how you're going to pay the <em>50 stars</em> you owe the resort before you leave. Noticing that you look concerned, the reindeer wanders over to you; you see that it's carrying a small pouch.

"Sorry for the trouble," a note in the pouch reads. Sitting at the bottom of the pouch is a gold coin with a little picture of a starfish on it.

Looks like you only needed <em>49 stars</em> after all.


