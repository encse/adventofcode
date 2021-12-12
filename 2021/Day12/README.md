original source: [https://adventofcode.com/2021/day/12](https://adventofcode.com/2021/day/12)
## --- Day 12: Passage Pathing ---
With your submarine's subterranean subsystems subsisting suboptimally, the only way you're getting out of this cave anytime soon is by finding a path yourself. Not just <em>a</em> path - the only way to know if you've found the <em>best</em> path is to find <em>all</em> of them.

Fortunately, the sensors are still mostly working, and so you build a rough map of the remaining caves (your puzzle input). For example:

<pre>
<code>start-A
start-b
A-c
A-b
b-d
A-end
b-end
</code>
</pre>

This is a list of how all of the caves are connected. You start in the cave named <code>start</code>, and your destination is the cave named <code>end</code>. An entry like <code>b-d</code> means that cave <code>b</code> is connected to cave <code>d</code> - that is, you can move between them.

So, the above cave system looks roughly like this:

<pre>
<code>    start
    /   \
c--A-----b--d
    \   /
     end
</code>
</pre>

Your goal is to find the number of distinct <em>paths</em> that start at <code>start</code>, end at <code>end</code>, and don't visit small caves more than once. There are two types of caves: <em>big</em> caves (written in uppercase, like <code>A</code>) and <em>small</em> caves (written in lowercase, like <code>b</code>). It would be a waste of time to visit any small cave more than once, but big caves are large enough that it might be worth visiting them multiple times. So, all paths you find should <em>visit small caves at most once</em>, and can <em>visit big caves any number of times</em>.

Given these rules, there are <code><em>10</em></code> paths through this example cave system:

<pre>
<code>start,A,b,A,c,A,end
start,A,b,A,end
start,A,b,end
start,A,c,A,b,A,end
start,A,c,A,b,end
start,A,c,A,end
start,A,end
start,b,A,c,A,end
start,b,A,end
start,b,end
</code>
</pre>

(Each line in the above list corresponds to a single path; the caves visited by that path are listed in the order they are visited and separated by commas.)

Note that in this cave system, cave <code>d</code> is never visited by any path: to do so, cave <code>b</code> would need to be visited twice (once on the way to cave <code>d</code> and a second time when returning from cave <code>d</code>), and since cave <code>b</code> is small, this is not allowed.

Here is a slightly larger example:

<pre>
<code>dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc
</code>
</pre>

The <code>19</code> paths through it are as follows:

<pre>
<code>start,HN,dc,HN,end
start,HN,dc,HN,kj,HN,end
start,HN,dc,end
start,HN,dc,kj,HN,end
start,HN,end
start,HN,kj,HN,dc,HN,end
start,HN,kj,HN,dc,end
start,HN,kj,HN,end
start,HN,kj,dc,HN,end
start,HN,kj,dc,end
start,dc,HN,end
start,dc,HN,kj,HN,end
start,dc,end
start,dc,kj,HN,end
start,kj,HN,dc,HN,end
start,kj,HN,dc,end
start,kj,HN,end
start,kj,dc,HN,end
start,kj,dc,end
</code>
</pre>

Finally, this even larger example has <code>226</code> paths through it:

<pre>
<code>fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW
</code>
</pre>

<em>How many paths through this cave system are there that visit small caves at most once?</em>


## --- Part Two ---
After reviewing the available paths, you realize you might have time to visit a single small cave <em>twice</em>. Specifically, big caves can be visited any number of times, a single small cave can be visited at most twice, and the remaining small caves can be visited at most once. However, the caves named <code>start</code> and <code>end</code> can only be visited <em>exactly once each</em>: once you leave the <code>start</code> cave, you may not return to it, and once you reach the <code>end</code> cave, the path must end immediately.

Now, the <code>36</code> possible paths through the first example above are:

<pre>
<code>start,A,b,A,b,A,c,A,end
start,A,b,A,b,A,end
start,A,b,A,b,end
start,A,b,A,c,A,b,A,end
start,A,b,A,c,A,b,end
start,A,b,A,c,A,c,A,end
start,A,b,A,c,A,end
start,A,b,A,end
start,A,b,d,b,A,c,A,end
start,A,b,d,b,A,end
start,A,b,d,b,end
start,A,b,end
start,A,c,A,b,A,b,A,end
start,A,c,A,b,A,b,end
start,A,c,A,b,A,c,A,end
start,A,c,A,b,A,end
start,A,c,A,b,d,b,A,end
start,A,c,A,b,d,b,end
start,A,c,A,b,end
start,A,c,A,c,A,b,A,end
start,A,c,A,c,A,b,end
start,A,c,A,c,A,end
start,A,c,A,end
start,A,end
start,b,A,b,A,c,A,end
start,b,A,b,A,end
start,b,A,b,end
start,b,A,c,A,b,A,end
start,b,A,c,A,b,end
start,b,A,c,A,c,A,end
start,b,A,c,A,end
start,b,A,end
start,b,d,b,A,c,A,end
start,b,d,b,A,end
start,b,d,b,end
start,b,end
</code>
</pre>

The slightly larger example above now has <code>103</code> paths through it, and the even larger example now has <code>3509</code> paths through it.

Given these new rules, <em>how many paths through this cave system are there?</em>


