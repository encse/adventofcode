## --- Day 7: Internet Protocol Version 7 ---
While snooping around the local network of EBHQ, you compile a list of [IP addresses](https://en.wikipedia.org/wiki/IP_address) (they're IPv7, of course; [IPv6](https://en.wikipedia.org/wiki/IPv6) is much too limited). You'd like to figure out which IPs support *TLS* (transport-layer snooping).

An IP supports TLS if it has an Autonomous Bridge Bypass Annotation, or *ABBA*.  An ABBA is any four-character sequence which consists of a pair of two different characters followed by the reverse of that pair, such as `xyyx` or `abba`.  However, the IP also must not have an ABBA within any hypernet sequences, which are contained by *square brackets*.

Read the [full puzzle](https://adventofcode.com/2016/day/7).